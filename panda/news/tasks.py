from __future__ import absolute_import, unicode_literals
from celery import shared_task
import requests
from bs4 import BeautifulSoup
from datetime import datetime
import pytz
from .models import News
import time


@shared_task
def crawNews():
    latest_time = getLatestTime()

    r = requests.get('http://news.ipanda.com/pic/index.shtml')
    r.encoding = 'utf-8'
    soup = BeautifulSoup(r.text, 'html.parser')

    newss = soup.find_all('div', class_='text_lt')
    for news in newss:
        news_time = news.select('div.time')[0].text
        struct_time = datetime.strptime(news_time, '%Y-%m-%d %H:%M:%S')
        struct_time = struct_time.replace(tzinfo=pytz.UTC)

        if latest_time==None or struct_time>latest_time:
            title = news.select('h3')[0].text.strip()
            #abstract = news.select('p')[0].text.strip()
            cover = news.select('img')[0]['src']
            href = news.select('h3 a')[0]['href']

            imgurls = getAblum(href)
            n = News(title=title, 
                    cover=cover, 
                    news_time=struct_time,
                    imgurls=imgurls)
            n.save()


def getLatestTime():
    news = News.objects.all().order_by('-news_time').first()
    if news == None:
        return None
    else:
        return news.news_time


def getAblum(url):
    url = url[:-5] + 'xml'
    r = requests.get(url)
    r.encoding = 'utf-8'
    soup = BeautifulSoup(r.text, 'html.parser')

    images = soup.select('li')
    imgurls = ''
    for image in images:
        bigurl = image['photourl'] + ' '
        imgurls = imgurls + bigurl
    time.sleep(0.1)
    return imgurls
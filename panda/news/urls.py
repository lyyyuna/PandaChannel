from django.conf.urls import url
from .views import NewsViewSet


news_list = NewsViewSet.as_view({
    'get' : 'list',
})


urlpatterns = [
    url(r'^photos', news_list, name='news_photo_api'),
]
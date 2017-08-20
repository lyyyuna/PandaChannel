from django.contrib import admin
from .models import News
# Register your models here.


class NewsModelAdmin(admin.ModelAdmin):
    list_display = ('title', 'news_time')


admin.site.register(News, NewsModelAdmin)

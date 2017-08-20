from django.db import models
from django.utils import timezone

# Create your models here.

class News(models.Model):
    title = models.CharField(max_length=50, default='error')
    cover = models.URLField(default='error')
    news_time = models.DateTimeField(default=timezone.now, db_index=True)
    imgurls = models.TextField(default='error')

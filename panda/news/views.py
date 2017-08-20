from django.shortcuts import render, get_object_or_404

from rest_framework import viewsets
from rest_framework.pagination import PageNumberPagination
from rest_framework.renderers import JSONRenderer
from .models import News
from .serializers import NewsSerializer


class StandardResultsSetPagination(PageNumberPagination):
    page_size = 20
    page_size_query_param = 'page_size'


class NewsViewSet(viewsets.ModelViewSet):
    queryset = News.objects.all().order_by('-news_time')
    pagination_class = StandardResultsSetPagination
    serializer_class = NewsSerializer
    renderer_classes = (JSONRenderer,)

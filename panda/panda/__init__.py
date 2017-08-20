from __future__ import absolute_import, unicode_literals

from .celery_cron import app as celery_app

__all__ = ['celery_app']
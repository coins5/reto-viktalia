import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

import { API_BASE_URL } from '../../app.config';

export const apiBaseUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const baseUrl = inject(API_BASE_URL);

  if (/^https?:\/\//i.test(req.url)) {
    return next(req);
  }

  const normalizedBase = baseUrl.replace(/\/$/, '');
  const normalizedPath = req.url.replace(/^\//, '');

  const apiRequest = req.clone({
    url: `${normalizedBase}/${normalizedPath}`
  });

  return next(apiRequest);
};

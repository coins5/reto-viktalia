import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const snackBar = inject(MatSnackBar);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const apiErrors: string[] | undefined = error.error?.errors?.map((err: { message?: string }) => err.message).filter((msg: string | undefined): msg is string => Boolean(msg));
      const defaultMessage = error.status === 0
        ? 'No fue posible conectarse con el servidor. Verifique su conexión o intente nuevamente.'
        : 'Ocurrió un error inesperado. Intente nuevamente.';
      const message = apiErrors?.length ? apiErrors.join(' \n') : error.error?.message ?? defaultMessage;
      const traceId = error.error?.traceId;

      const fullMessage = traceId ? `${message}\nTraceId: ${traceId}` : message;

      snackBar.open(fullMessage, 'Cerrar', {
        duration: 6000,
        panelClass: ['snackbar-error']
      });

      return throwError(() => error);
    })
  );
};

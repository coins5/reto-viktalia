import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'clientes'
  },
  {
    path: 'clientes',
    loadChildren: () => import('./clientes/clientes.routes').then((m) => m.CLIENTES_ROUTES)
  },
  {
    path: '**',
    redirectTo: 'clientes'
  }
];

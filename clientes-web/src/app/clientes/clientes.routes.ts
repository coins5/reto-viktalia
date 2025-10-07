import { Routes } from '@angular/router';

import { ClientesPageComponent } from './pages/clientes-list/clientes-page.component';
import { ClienteFormPageComponent } from './pages/cliente-form/cliente-form-page.component';
import { pendingChangesGuard } from './guards/pending-changes.guard';

export const CLIENTES_ROUTES: Routes = [
  {
    path: '',
    component: ClientesPageComponent
  },
  {
    path: 'nuevo',
    component: ClienteFormPageComponent,
    canDeactivate: [pendingChangesGuard]
  },
  {
    path: ':id/editar',
    component: ClienteFormPageComponent,
    canDeactivate: [pendingChangesGuard]
  }
];

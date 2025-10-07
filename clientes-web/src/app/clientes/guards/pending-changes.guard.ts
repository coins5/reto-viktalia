import { CanDeactivateFn } from '@angular/router';

export interface PendingChangesComponent {
  hasPendingChanges(): boolean;
}

export const pendingChangesGuard: CanDeactivateFn<PendingChangesComponent> = (component) => {
  if (!component.hasPendingChanges()) {
    return true;
  }

  return confirm('Hay cambios sin guardar. ¿Seguro que desea salir?');
};

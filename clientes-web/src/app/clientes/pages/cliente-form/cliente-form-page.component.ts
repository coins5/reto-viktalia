import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Subject, takeUntil } from 'rxjs';

import { ClientesService } from '../../services/clientes.service';
import { SaveClientePayload } from '../../models/cliente.model';
import { Cliente } from '../../models/cliente.model';
import { PendingChangesComponent } from '../../guards/pending-changes.guard';

@Component({
  selector: 'app-cliente-form-page',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    RouterModule
  ],
  templateUrl: './cliente-form-page.component.html',
  styleUrl: './cliente-form-page.component.css'
})
export class ClienteFormPageComponent implements OnInit, OnDestroy, PendingChangesComponent {
  readonly form: FormGroup;
  readonly isEditMode: boolean;
  private readonly destroy$ = new Subject<void>();

  loading = false;
  saving = false;
  private clienteId?: number;

  constructor(
    private readonly fb: FormBuilder,
    private readonly clientesService: ClientesService,
    private readonly snackBar: MatSnackBar,
    private readonly router: Router,
    route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      ruc: ['', [Validators.required, Validators.pattern(/^[0-9]{11}$/)]],
      razonSocial: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
      telefonoContacto: ['', [Validators.pattern(/^[0-9]{7,15}$/)]],
      correoContacto: ['', [Validators.email]],
      direccion: ['', [Validators.maxLength(200)]]
    });

    const idParam = route.snapshot.paramMap.get('id');
    this.isEditMode = !!idParam;
    if (idParam) {
      this.clienteId = Number(idParam);
    }
  }

  ngOnInit(): void {
    if (this.isEditMode && this.clienteId) {
      this.fetchCliente(this.clienteId);
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  hasPendingChanges(): boolean {
    if (this.saving) {
      return false;
    }
    return this.form.dirty;
  }

  submit(): void {
    if (this.form.invalid || this.saving) {
      this.form.markAllAsTouched();
      return;
    }

    const payload: SaveClientePayload = {
      ruc: this.form.value.ruc,
      razonSocial: this.form.value.razonSocial,
      telefonoContacto: this.form.value.telefonoContacto || null,
      correoContacto: this.form.value.correoContacto || null,
      direccion: this.form.value.direccion || null
    };

    this.saving = true;

    const request$ = this.isEditMode && this.clienteId
      ? this.clientesService.update(this.clienteId, payload)
      : this.clientesService.create(payload);

    request$
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.saving = false;
          this.form.markAsPristine();
          const message = this.isEditMode ? 'Cliente actualizado correctamente.' : 'Cliente creado correctamente.';
          this.snackBar.open(message, 'Cerrar', { duration: 4000 });
          void this.router.navigate(['/clientes']);
        },
        error: () => {
          this.saving = false;
        }
      });
  }

  cancel(): void {
    void this.router.navigate(['/clientes']);
  }

  private fetchCliente(id: number): void {
    this.loading = true;
    this.clientesService
      .getById(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.loading = false;
          this.form.patchValue(response.data);
          this.form.markAsPristine();
        },
        error: () => {
          this.loading = false;
        }
      });
  }
}

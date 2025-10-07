import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { Subject, takeUntil } from 'rxjs';

import { ClientesService } from '../../services/clientes.service';
import { Cliente, ClienteQueryParams } from '../../models/cliente.model';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-clientes-page',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    RouterModule,
    MatCardModule
  ],
  templateUrl: './clientes-page.component.html',
  styleUrl: './clientes-page.component.css'
})
export class ClientesPageComponent implements OnInit, OnDestroy {
  readonly displayedColumns = ['ruc', 'razonSocial', 'telefonoContacto', 'correoContacto', 'direccion', 'actions'];
  readonly dataSource = new MatTableDataSource<Cliente>([]);
  readonly filterForm: FormGroup;
  readonly pageSizeOptions = [5, 10, 20];

  loading = false;
  total = 0;
  pageIndex = 0;
  pageSize = this.pageSizeOptions[1];
  sortBy: string | null = null;
  sortDir: 'asc' | 'desc' | null = null;

  private readonly destroy$ = new Subject<void>();


  constructor(
    private readonly clientesService: ClientesService,
    private readonly fb: FormBuilder,
    private readonly snackBar: MatSnackBar,
    private readonly dialog: MatDialog,
    private readonly router: Router
  ) {
    this.filterForm = this.fb.group({
      ruc: [''],
      razonSocial: ['']
    });
  }

  ngOnInit(): void {
    this.loadClientes();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  applyFilters(): void {
    this.pageIndex = 0;
    this.loadClientes();
  }

  resetFilters(): void {
    this.filterForm.reset();
    this.pageIndex = 0;
    this.loadClientes();
  }

  onPageChange(event: PageEvent): void {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;
    this.loadClientes();
  }

  onSortChange(sort: Sort): void {
    this.sortBy = sort.active || null;
    this.sortDir = (sort.direction as 'asc' | 'desc') || null;
    this.pageIndex = 0;
    this.loadClientes();
  }

  navigateToCreate(): void {
    void this.router.navigate(['/clientes/nuevo']);
  }

  navigateToEdit(cliente: Cliente): void {
    void this.router.navigate(['/clientes', cliente.id, 'editar']);
  }

  deleteCliente(cliente: Cliente): void {
    this.dialog
      .open(ConfirmDialogComponent, {
        width: '360px',
        data: {
          title: 'Eliminar cliente',
          message: `¿Confirma la eliminación de ${cliente.razonSocial}?`,
          confirmText: 'Eliminar'
        }
      })
      .afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe((confirmed: boolean) => {
        if (confirmed) {
          this.executeDelete(cliente.id);
        }
      });
  }

  private executeDelete(id: number): void {
    this.loading = true;
    this.clientesService
      .delete(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.snackBar.open('Cliente eliminado correctamente.', 'Cerrar', { duration: 4000 });
          this.loadClientes();
        },
        error: () => {
          this.loading = false;
        }
      });
  }

  private loadClientes(): void {
    this.loading = true;

    const filters = this.filterForm.value;
    const query: ClienteQueryParams = {
      page: this.pageIndex + 1,
      pageSize: this.pageSize,
      sortBy: this.sortBy,
      sortDir: this.sortDir,
      ruc: filters.ruc,
      razonSocial: filters.razonSocial
    };

    this.clientesService
      .search(query)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.dataSource.data = response.data;
          this.total = response.paging?.total ?? response.data.length;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
        }
      });
  }
}

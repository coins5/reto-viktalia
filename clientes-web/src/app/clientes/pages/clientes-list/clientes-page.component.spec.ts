import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';

import { ClientesPageComponent } from './clientes-page.component';
import { ClientesService } from '../../services/clientes.service';

describe('ClientesPageComponent', () => {
  let component: ClientesPageComponent;
  let fixture: ComponentFixture<ClientesPageComponent>;
  let clientesServiceSpy: jasmine.SpyObj<ClientesService>;

  beforeEach(async () => {
    clientesServiceSpy = jasmine.createSpyObj('ClientesService', ['search', 'delete']);
    clientesServiceSpy.search.and.returnValue(
      of({ data: [], paging: { page: 1, pageSize: 10, total: 0, totalPages: 0 } })
    );
    clientesServiceSpy.delete.and.returnValue(of({ data: null }));

    await TestBed.configureTestingModule({
      imports: [ClientesPageComponent, MatSnackBarModule, MatDialogModule, NoopAnimationsModule],
      providers: [{ provide: ClientesService, useValue: clientesServiceSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(ClientesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
    expect(clientesServiceSpy.search).toHaveBeenCalled();
  });
});

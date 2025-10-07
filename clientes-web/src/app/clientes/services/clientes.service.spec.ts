import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { ClientesService } from './clientes.service';
import { ApiResponse, Cliente } from '../models/cliente.model';

describe('ClientesService', () => {
  let service: ClientesService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ClientesService]
    });

    service = TestBed.inject(ClientesService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch clientes with query params', () => {
    const response: ApiResponse<Cliente[]> = {
      data: [],
      paging: { page: 1, pageSize: 10, total: 0, totalPages: 0 }
    };

    service.search({ page: 1, pageSize: 10, ruc: '201' }).subscribe((res) => {
      expect(res).toEqual(response);
    });

    const req = httpMock.expectOne((r) => r.url === 'clientes');
    expect(req.request.method).toBe('GET');
    expect(req.request.params.get('ruc')).toBe('201');
    req.flush(response);
  });

  it('should create a cliente', () => {
    const payload = { ruc: '20100070970', razonSocial: 'Nuevo Cliente' };
    const response: ApiResponse<Cliente> = {
      data: {
        id: 1,
        ruc: payload.ruc,
        razonSocial: payload.razonSocial,
        createdAt: new Date().toISOString(),
        telefonoContacto: null,
        correoContacto: null,
        direccion: null
      }
    };

    service.create(payload).subscribe((res) => {
      expect(res).toEqual(response);
    });

    const req = httpMock.expectOne('clientes');
    expect(req.request.method).toBe('POST');
    req.flush(response);
  });
});

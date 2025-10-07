import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiResponse, Cliente, ClienteQueryParams, SaveClientePayload } from '../models/cliente.model';

@Injectable({ providedIn: 'root' })
export class ClientesService {
  private readonly resourceUrl = 'clientes';

  constructor(private readonly http: HttpClient) {}

  search(params: ClienteQueryParams): Observable<ApiResponse<Cliente[]>> {
    const httpParams = this.buildQueryParams(params);
    return this.http.get<ApiResponse<Cliente[]>>(this.resourceUrl, { params: httpParams });
  }

  getById(id: number): Observable<ApiResponse<Cliente>> {
    return this.http.get<ApiResponse<Cliente>>(`${this.resourceUrl}/${id}`);
  }

  create(payload: SaveClientePayload): Observable<ApiResponse<Cliente>> {
    return this.http.post<ApiResponse<Cliente>>(this.resourceUrl, payload);
  }

  update(id: number, payload: SaveClientePayload): Observable<ApiResponse<Cliente>> {
    return this.http.put<ApiResponse<Cliente>>(`${this.resourceUrl}/${id}`, payload);
  }

  delete(id: number): Observable<ApiResponse<unknown>> {
    return this.http.delete<ApiResponse<unknown>>(`${this.resourceUrl}/${id}`);
  }

  private buildQueryParams(params: ClienteQueryParams): HttpParams {
    let httpParams = new HttpParams();

    const normalized: ClienteQueryParams = {
      page: params.page ?? 1,
      pageSize: params.pageSize ?? 10,
      sortBy: params.sortBy ?? undefined,
      sortDir: params.sortDir ?? undefined,
      ruc: params.ruc?.trim() ?? undefined,
      razonSocial: params.razonSocial?.trim() ?? undefined
    };

    Object.entries(normalized).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        httpParams = httpParams.set(key, String(value));
      }
    });

    return httpParams;
  }
}

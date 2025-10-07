export interface Cliente {
  id: number;
  ruc: string;
  razonSocial: string;
  telefonoContacto?: string | null;
  correoContacto?: string | null;
  direccion?: string | null;
  createdAt: string;
  updatedAt?: string | null;
}

export interface PagingMetadata {
  page: number;
  pageSize: number;
  total: number;
  totalPages: number;
}

export interface SortMetadata {
  by?: string | null;
  dir?: string | null;
}

export interface ApiResponse<T> {
  data: T;
  paging?: PagingMetadata;
  sort?: SortMetadata;
  errors?: ApiError[];
  traceId?: string;
}

export interface ApiError {
  code: string;
  message: string;
  field?: string | null;
}

export interface ClienteQueryParams {
  page?: number;
  pageSize?: number;
  sortBy?: string | null;
  sortDir?: 'asc' | 'desc' | null;
  ruc?: string | null;
  razonSocial?: string | null;
}

export interface SaveClientePayload {
  ruc: string;
  razonSocial: string;
  telefonoContacto?: string | null;
  correoContacto?: string | null;
  direccion?: string | null;
}

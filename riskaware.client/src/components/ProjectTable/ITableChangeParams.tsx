export interface ITableChangeParams {
  currentPage: number;
  perPage: number;
  sortField: string;
  sortOrder: 'asc' | 'desc';
}

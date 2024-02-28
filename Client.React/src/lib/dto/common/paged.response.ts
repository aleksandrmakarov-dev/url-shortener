export type PagedResponse<T> = {
  items: T[];
  pagination: Pagination;
};

export type Pagination = {
  page: number;
  size: number;
  query?: string;
};

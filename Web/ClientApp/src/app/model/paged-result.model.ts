export class PagedResult<T> {
  items: T[];
  currentPage: number;
  pagesCount: number;
  resultsPerPage: number;
}

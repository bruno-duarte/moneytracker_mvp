namespace MoneyTracker.Api.Responses
{
  public class PagedResponse<T>(
    IEnumerable<T> items,
    int pageNumber,
    int pageSize,
    int totalCount)
  {
    public IEnumerable<T> Items { get; set; } = items;
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = totalCount;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
  }
}

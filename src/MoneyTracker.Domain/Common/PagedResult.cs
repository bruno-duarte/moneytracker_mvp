namespace MoneyTracker.Domain.Common
{
  public class PagedResult<T>(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount)
  {
    public IEnumerable<T> Items { get; } = items;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public int TotalCount { get; } = totalCount;
  }
}

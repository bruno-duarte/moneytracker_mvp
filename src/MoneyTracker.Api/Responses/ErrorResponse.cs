namespace MoneyTracker.Api.Responses
{
  public class ErrorResponse(int statusCode, string? title = null, string? detail = null)
  {
    public int StatusCode { get; set; } = statusCode;
    public string? Title { get; set; } = title;
    public string? Detail { get; set; } = detail;
    public string? Instance { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
  }
}

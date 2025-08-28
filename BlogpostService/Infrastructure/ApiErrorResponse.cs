namespace BlogpostService.Infrastructure;

public class ApiErrorResponse
{
    public int StatusCode { get; set; }
    public string Title { get; set; } = null!;
    public string? Detail { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}
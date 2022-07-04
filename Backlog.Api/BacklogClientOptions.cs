namespace Backlog.Api;

public class BacklogClientOptions
{
    public string BaseUrl { get; set; } = "https://example.backlog.jp";
    public string ApiKey { get; set; } = string.Empty;
    public int ReadRateLimitCount { get; set; } = 80;
    public int ReadRateLimitWindow { get; set; } = 10;
}
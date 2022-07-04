using RestSharp;

namespace Backlog.Api;

public partial class BacklogClient
{
    public async Task<dynamic> GetIssueAsync(string issueIdOrKey, CancellationToken cancellationToken = default)
    {
        const string url = $"/api/v2/issues/{{{nameof(issueIdOrKey)}}}";
        var request = new RestRequest(url)
            .AddUrlSegment(nameof(issueIdOrKey), issueIdOrKey);

        // rate limit control.
        using var lease = await _readRequestLimiter.WaitAsync(cancellationToken: cancellationToken);
        return await _restClient.GetAsync<dynamic>(request, cancellationToken) ?? throw new Exception();
    }
}
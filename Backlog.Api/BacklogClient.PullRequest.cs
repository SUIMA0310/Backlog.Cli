using RestSharp;

namespace Backlog.Api;

public partial class BacklogClient
{
    /// <summary>
    /// プルリクエストコメントの取得
    /// </summary>
    /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
    /// <param name="repoIdOrName">リポジトリのID または リポジトリ名</param>
    /// <param name="number">プルリクエストの番号</param>
    /// <param name="minId">最小ID(オプション)</param>
    /// <param name="maxId">最大ID(オプション)</param>
    /// <param name="count">取得上限(1-100) 指定が無い場合は20(オプション)</param>
    /// <param name="order">“asc"または"desc” 指定が無い場合は"desc"(オプション)</param>
    /// <returns>レスポンスボディ</returns>
    /// <exception cref="Exception"></exception>
    public async Task<dynamic> GetPullRequestCommentsAsync(
        string projectIdOrKey,
        string repoIdOrName,
        int number,
        int minId = default,
        int maxId = default,
        int count = 20,
        string order = "desc",
        CancellationToken cancellationToken = default)
    {
        const string url = $"/api/v2/projects/{{{nameof(projectIdOrKey)}}}/git/repositories/{{{nameof(repoIdOrName)}}}/pullRequests/{{{nameof(number)}}}/comments";
        var request = new RestRequest(url)
            .AddUrlSegment(nameof(projectIdOrKey), projectIdOrKey)
            .AddUrlSegment(nameof(repoIdOrName), repoIdOrName)
            .AddUrlSegment(nameof(number), number);

        if (minId != default)
        {
            request.AddQueryParameter(nameof(minId), minId);
        }

        if (maxId != default)
        {
            request.AddQueryParameter(nameof(maxId), maxId);
        }

        if (count != 20)
        {
            request.AddQueryParameter(nameof(count), count);
        }

        if (order != "desc")
        {
            request.AddQueryParameter(nameof(order), order);
        }

        // rate limit control.
        using var lease = await _readRequestLimiter.WaitAsync(cancellationToken: cancellationToken);
        return await _restClient.GetAsync<dynamic>(request, cancellationToken) ?? throw new Exception();
    }

    /// <summary>
    /// プルリクエストコメント数の取得
    /// </summary>
    /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
    /// <param name="repoIdOrName">リポジトリのID または リポジトリ名</param>
    /// <param name="number">プルリクエストの番号</param>
    /// <returns>レスポンスボディ</returns>
    /// <exception cref="Exception"></exception>
    public async Task<dynamic> GetPullRequestCommentCountAsync(
        string projectIdOrKey,
        string repoIdOrName,
        int number,
        CancellationToken cancellationToken = default)
    {
        const string url = $"/api/v2/projects/{{{nameof(projectIdOrKey)}}}/git/repositories/{{{nameof(repoIdOrName)}}}/pullRequests/{{{nameof(number)}}}/comments/count";
        var request = new RestRequest(url)
            .AddUrlSegment(nameof(projectIdOrKey), projectIdOrKey)
            .AddUrlSegment(nameof(repoIdOrName), repoIdOrName)
            .AddUrlSegment(nameof(number), number);

        // rate limit control.
        using var lease = await _readRequestLimiter.WaitAsync(cancellationToken: cancellationToken);
        return await _restClient.GetAsync<dynamic>(request, cancellationToken) ?? throw new Exception();
    }
}

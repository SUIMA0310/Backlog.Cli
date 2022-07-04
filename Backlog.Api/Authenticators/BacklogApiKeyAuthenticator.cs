using RestSharp;
using RestSharp.Authenticators;

namespace Backlog.Api.Authenticators;

internal class BacklogApiKeyAuthenticator : IAuthenticator
{
    private readonly string _apiKey;

    public BacklogApiKeyAuthenticator(string apiKey)
    {
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
    }

    public ValueTask Authenticate(RestClient client, RestRequest request)
    {
        // Backlog API　認証と認可

        // API Key を用いた認証
        request.AddQueryParameter("apiKey", _apiKey, false);
        return ValueTask.CompletedTask;
    }
}
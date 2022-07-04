using System.Threading.RateLimiting;
using Backlog.Api.Authenticators;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace Backlog.Api;

public partial class BacklogClient : IDisposable
{
    private readonly RateLimiter _readRequestLimiter;
    private readonly RestClient _restClient;

    public BacklogClient(IOptions<BacklogClientOptions> options) : this(options.Value)
    {
        /* do noting. */
    }

    public BacklogClient(BacklogClientOptions options)
    {
        _readRequestLimiter = new FixedWindowRateLimiter(
            new FixedWindowRateLimiterOptions(
                options.ReadRateLimitCount,
                QueueProcessingOrder.OldestFirst,
                int.MaxValue,
                TimeSpan.FromSeconds(options.ReadRateLimitWindow)
            ));

        _restClient = new RestClient(new RestClientOptions(options.BaseUrl))
        {
            Authenticator = new BacklogApiKeyAuthenticator(options.ApiKey)
        };

        // use Newtonsoft.Json.
        _restClient.UseNewtonsoftJson();
    }

    public void Dispose()
    {
        _readRequestLimiter.Dispose();
        _restClient.Dispose();
    }
}
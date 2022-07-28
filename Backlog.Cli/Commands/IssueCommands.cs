using Backlog.Api;
using Microsoft.Extensions.Logging;

namespace Backlog.Cli.Commands;

public class IssueCommands : ConsoleAppBase
{
    private readonly BacklogClient _backlogClient;
    private readonly ILogger<IssueCommands> _logger;

    public IssueCommands(
        ILogger<IssueCommands> logger,
        BacklogClient backlogClient)
    {
        _logger = logger;
        _backlogClient = backlogClient;
    }

    [Command("get-title-file")]
    public async Task GetIssueTitle([Option("i", "input file path")] string inputFilePath,
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(inputFilePath))
        {
            throw new FileNotFoundException("Specified file not found.", inputFilePath);
        }

        IEnumerable<string> GetLines(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                yield return reader.ReadLine() ?? throw new Exception();
            }
        }

        using StreamReader? inputFile = new(inputFilePath);
        var issueIds = GetLines(inputFile).ToArray();

        await GetIssueTitle(issueIds, cancellationToken);
    }

    [Command("get-title")]
    public async Task GetIssueTitle([Option(0)] string[] issueIds, CancellationToken cancellationToken = default)
    {
        foreach (var issueId in issueIds)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation("get Issue info in {issueId}", issueId);
            var response = await _backlogClient.GetIssueAsync(issueId, cancellationToken);
            var summary = (string)response.summary;

            Console.WriteLine($"{issueId} {summary}");
        }
    }
}
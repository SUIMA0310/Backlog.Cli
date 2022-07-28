using Backlog.Api;
using Microsoft.Extensions.Logging;

namespace Backlog.Cli.Commands;

public class PullRequestCommands : ConsoleAppBase
{
    private readonly BacklogClient _backlogClient;
    private readonly ILogger<PullRequestCommands> _logger;

    public PullRequestCommands(BacklogClient backlogClient, ILogger<PullRequestCommands> logger)
    {
        _backlogClient = backlogClient;
        _logger = logger;
    }

    [Command("get-pr-comment")]
    public async Task GetPullRequestComments(
        [Option("p", "project")] string projectIdOrKey,
        [Option("r", "repository")] string repoIdOrName,
        [Option(0, "number")] int number,
        CancellationToken cancellationToken = default)
    {
        var response = await _backlogClient.GetPullRequestCommentsAsync(
                projectIdOrKey: projectIdOrKey,
                repoIdOrName: repoIdOrName,
                number: number,
                count: 100,
                order: "asc",
                cancellationToken: cancellationToken);

        foreach (var comment in response)
        {
            var output = CreateOutput(comment);
            if (output == null)
            {
                continue;
            }

            Console.WriteLine(output);
        }
    }

    private static string? CreateOutput(dynamic comment)
    {
        var content = (string?)comment.content;
        var createdUserName = (string)comment.createdUser.name;

        var filePath = (string?)comment.filePath;
        var position = (string?)comment.position;

        var created = (string?)comment.created;

        string getContent(string content)
        {
            // @ 文字は事故るので回避のため削除
            return content.Replace("@", "");
        }

        string getTime(string? timeStr)
        {
            if(string.IsNullOrEmpty(timeStr))
            {
                return "-";
            }

            return DateTime.Parse(timeStr).ToShortDateString();
        }

        if (content != null
         && filePath != null
         && position != null)
        {
            return $"\"{filePath}/{position}\",\"{getContent(content)}\",\"{createdUserName}\",{getTime(created)}";
        }

        if (!string.IsNullOrWhiteSpace(content))
        {
            return $"-,\"{getContent(content)}\",\"{createdUserName}\",{getTime(created)}";
        }

        return null;
    }
}

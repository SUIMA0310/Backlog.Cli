using Backlog.Api.Extensions;
using Backlog.Cli.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Reflection;

namespace Backlog.Cli;

public class Program
{
    public static async Task Main(string[] args)
    {
        await ConsoleApp.CreateBuilder(args)
            .ConfigureAppConfiguration((_, configure) =>
            {
                configure.SetBasePath(GetConfigFileDirectoryPath());
                configure.AddUserSecrets<Program>();
            })
            .ConfigureLogging((hostContext, logging) =>
            {
                logging.ClearProviders();
                logging.AddNLog(
                    new NLogLoggingConfiguration(
                        hostContext.Configuration.GetSection("NLog")));
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddBacklogApi(
                    hostContext.Configuration.GetSection("Backlog"));
            })
            .Build()
            .AddCommands<IssueCommands>()
            .AddCommands<PullRequestCommands>()
            .RunAsync();
    }

    private static string GetConfigFileDirectoryPath()
    {
        var location = Assembly.GetExecutingAssembly().Location;
        return Path.GetDirectoryName(location) ?? Environment.CurrentDirectory;
    }
}
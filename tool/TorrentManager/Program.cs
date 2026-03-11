using TorrentManager;

try
{
    var exitCode = TorrentApp.Run(args);
    Environment.ExitCode = exitCode;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"执行失败: {ex.Message}");
    Environment.ExitCode = 1;
}

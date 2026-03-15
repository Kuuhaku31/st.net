
using TorrentManager.App;

// 主入口：捕获异常并设置适当的退出代码。
try
{
    var exitCode = TorrentApp.Run(args);
    Environment.ExitCode = exitCode;
}
catch(Exception ex)
{
    Console.Error.WriteLine($"执行失败: {ex.Message}");
    Environment.ExitCode = 1;
}

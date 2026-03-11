namespace TorrentManager.Cli;

/// <summary>
/// 保存命令行解析结果：位置参数、选项字典、数据库路径。
/// </summary>
internal sealed record ParsedArgs(
    IReadOnlyList<string> Positionals,
    IReadOnlyDictionary<string, string> Options,
    string DbPath
);

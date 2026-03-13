
namespace TorrentManager.Cli;

/// <summary>
/// 解析后的命令行参数：包含位置参数列表和选项字典。数据库路径通过 Options["db"] 访问。
/// </summary>
/// <param name="Positionals">位置参数列表</param>
/// <param name="Options">选项字典（含 "db"、"path" 等）</param>
internal sealed record ParsedArgs(
    IReadOnlyList<string>               Positionals,
    IReadOnlyDictionary<string, string> Options
)
{
    public override string ToString()
    {
        var pos  = string.Join(", ", Positionals);
        var opts = string.Join(", ", Options.Select(kv => $"{kv.Key}={kv.Value}"));
        return $"Positionals: [{pos}], Options: {{{opts}}}";
    }
}

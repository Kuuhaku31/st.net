namespace TorrentManager.Cli;

/// <summary>
/// 解析后的命令行参数：包含位置参数列表、选项字典和数据库路径。
/// </summary>
/// <param name="Positionals">位置参数列表</param>
/// <param name="Options">选项字典</param>
/// <param name="DbPath">数据库路径</param>
internal sealed record ParsedArgs(
    IReadOnlyList<string>               Positionals,
    IReadOnlyDictionary<string, string> Options,
    string                              DbPath
)
{
    public void PrintInfo()
    {
        Console.WriteLine("Parsed Arguments:");
        Console.WriteLine($"  DbPath: {DbPath}");
        Console.WriteLine("  Positionals:");
        foreach(var pos in Positionals)
        {
            Console.WriteLine($"    - {pos}");
        }
        Console.WriteLine("  Options:");
        foreach(var kvp in Options)
        {
            Console.WriteLine($"    --{kvp.Key} = {kvp.Value}");
        }
    }
}

namespace TorrentManager.Cli;

/// <summary>
/// 解析命令行参数，并负责输出帮助信息。
/// </summary>
internal static class CommandLineParser
{
    /// <summary>
    /// 默认数据库路径：如果用户未指定，则使用当前目录下的 fastresume.db。
    /// </summary>
    public const string DefaultDbPath = "./fastresume.db";


    /// <summary>
    /// 解析输入参数：支持位置参数和选项（以 -- 开头）。选项必须有对应值，否则抛出异常。返回解析结果对象。
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static ParsedArgs
    Parse(string[] input)
    {
        var positionals = new List<string>();                                               // 位置参数列表
        var options     = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase); // 选项字典，键不区分大小写

        // 遍历输入参数，区分位置参数和选项
        for(var i = 0; i < input.Length; i++)
        {
            var item = input[i];
            if(item.StartsWith("--", StringComparison.Ordinal)) // 如果以 -- 开头，视为选项
            {
                var key = item[2..]; // 去掉 -- 前缀得到选项名称
                if(i + 1 >= input.Length || input[i + 1].StartsWith("--", StringComparison.Ordinal)) // 选项必须有对应值，且下一个参数不能是另一个选项
                    throw new ArgumentException($"选项 '--{key}' 缺少值。");
                options[key] = input[++i]; // 将选项值存入字典，++i 跳过值参数
            }
            else positionals.Add(item); // 否则视为位置参数，添加到列表
        }

        // 如果用户没有提供 --db 选项，使用默认数据库路径
        if(!options.TryGetValue("db", out string? value) || string.IsNullOrWhiteSpace(value))
        {
            value = DefaultDbPath;
            options["db"] = value;
        }

        // 返回解析结果对象
        return new ParsedArgs(positionals, options);
    }

    public static void PrintUsage()
    {
        Console.WriteLine("TorrentManager");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run -- add <path_to_fastresume_file> [--db <database_path>]");
        Console.WriteLine("  dotnet run -- add_all <path_to_fastresume_files_directory> [--db <database_path>]");
        Console.WriteLine("  dotnet run -- export <by_category | by_save_path> <pattern> [--path <export_directory>] [--db <database_path>]");
        Console.WriteLine("  dotnet run -- update <by_category | by_save_path> <pattern> <replace <search_str> <replace_str> | <new_value>> [options]");
    }
}

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
        var options     = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var positionals = new List<string>();

        for(var i = 0; i < input.Length; i++)
        {
            var item = input[i];
            if(item.StartsWith("--", StringComparison.Ordinal))
            {
                var key = item[2..];
                if(i + 1 >= input.Length || input[i + 1].StartsWith("--", StringComparison.Ordinal))
                {
                    throw new ArgumentException($"选项 '--{key}' 缺少值。");
                }

                options[key] = input[i + 1];
                i++;
                continue;
            }

            positionals.Add(item);
        }

        if(!options.ContainsKey("db") || string.IsNullOrWhiteSpace(options["db"]))
        {
            options["db"] = DefaultDbPath;
        }

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
    }
}

namespace TorrentManager.Cli;

/// <summary>
/// 解析命令行参数，并负责输出帮助信息。
/// </summary>
internal static class CommandLineParser
{
    public const string DefaultDbPath = "./fastresume.db";

    public static ParsedArgs Parse(string[] input)
    {
        var options = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var positionals = new List<string>();

        for (var i = 0; i < input.Length; i++)
        {
            var item = input[i];
            if (item.StartsWith("--", StringComparison.Ordinal))
            {
                var key = item[2..];
                if (i + 1 >= input.Length || input[i + 1].StartsWith("--", StringComparison.Ordinal))
                {
                    throw new ArgumentException($"选项 '--{key}' 缺少值。");
                }

                options[key] = input[i + 1];
                i++;
                continue;
            }

            positionals.Add(item);
        }

        var dbPath = options.TryGetValue("db", out var db) && !string.IsNullOrWhiteSpace(db)
            ? db
            : DefaultDbPath;

        return new ParsedArgs(positionals, options, dbPath);
    }

    public static void PrintUsage()
    {
        Console.WriteLine("TorrentManager");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run -- add <path_to_fastresume_file> [--db <database_path>]");
        Console.WriteLine("  dotnet run -- add_all <path_to_fastresume_files_directory> [--db <database_path>]");
        Console.WriteLine("  dotnet run -- export <category> [--path <export_directory>] [--db <database_path>]");
    }
}

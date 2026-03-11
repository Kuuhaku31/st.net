using TorrentManager.Cli;
using TorrentManager.Data;
using TorrentManager.Parsing;

namespace TorrentManager;

/// <summary>
/// 应用主流程：分发命令并协调解析与存储组件。
/// </summary>
internal static class TorrentApp
{
    public static int Run(string[] args)
    {
        if (args.Length == 0)
        {
            CommandLineParser.PrintUsage();
            return 0;
        }

        ParsedArgs parsed;
        try
        {
            parsed = CommandLineParser.Parse(args);
        }
        catch (ArgumentException ex)
        {
            Console.Error.WriteLine(ex.Message);
            CommandLineParser.PrintUsage();
            return 1;
        }

        if (parsed.Positionals.Count == 0)
        {
            CommandLineParser.PrintUsage();
            return 0;
        }

        var repository = new TorrentRepository(parsed.DbPath);
        repository.InitializeDatabase();

        var command = parsed.Positionals[0].ToLowerInvariant();
        return command switch
        {
            "add" => RunAdd(parsed.Positionals, repository),
            "add_all" => RunAddAll(parsed.Positionals, repository),
            "export" => RunExport(parsed.Positionals, parsed.Options, repository),
            _ => RunUnknownCommand(command)
        };
    }

    private static int RunAdd(IReadOnlyList<string> positionals, TorrentRepository repository)
    {
        if (positionals.Count < 2)
        {
            Console.Error.WriteLine("Usage: dotnet run -- add <path_to_fastresume_file> [--db <database_path>]");
            return 1;
        }

        var filePath = positionals[1];
        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"文件不存在: {filePath}");
            return 1;
        }

        try
        {
            var record = FastResumeReader.ReadRecord(filePath);
            repository.Upsert(record);
            Console.WriteLine($"已添加: {record.TorHash} ({Path.GetFileName(filePath)})");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"添加失败 '{filePath}': {ex.Message}");
            return 1;
        }
    }

    private static int RunAddAll(IReadOnlyList<string> positionals, TorrentRepository repository)
    {
        if (positionals.Count < 2)
        {
            Console.Error.WriteLine("Usage: dotnet run -- add_all <path_to_fastresume_files_directory> [--db <database_path>]");
            return 1;
        }

        var directory = positionals[1];
        if (!Directory.Exists(directory))
        {
            Console.Error.WriteLine($"目录不存在: {directory}");
            return 1;
        }

        var files = Directory.EnumerateFiles(directory, "*.fastresume", SearchOption.AllDirectories).ToList();
        if (files.Count == 0)
        {
            Console.WriteLine("未找到 .fastresume 文件。");
            return 0;
        }

        var success = 0;
        var failed = 0;

        foreach (var file in files)
        {
            try
            {
                var record = FastResumeReader.ReadRecord(file);
                repository.Upsert(record);
                success++;
            }
            catch (Exception ex)
            {
                failed++;
                Console.Error.WriteLine($"跳过 '{file}': {ex.Message}");
            }
        }

        Console.WriteLine($"add_all 完成: success={success}, failed={failed}");
        return failed > 0 ? 1 : 0;
    }

    private static int RunExport(
        IReadOnlyList<string> positionals,
        IReadOnlyDictionary<string, string> options,
        TorrentRepository repository)
    {
        if (positionals.Count < 2)
        {
            Console.Error.WriteLine("Usage: dotnet run -- export <category> [--path <export_directory>] [--db <database_path>]");
            return 1;
        }

        var categoryPattern = positionals[1];
        var exportPath = options.TryGetValue("path", out var pathValue) && !string.IsNullOrWhiteSpace(pathValue)
            ? pathValue
            : Environment.CurrentDirectory;

        Directory.CreateDirectory(exportPath);

        var rows = repository.QueryByCategory(categoryPattern);
        foreach (var row in rows)
        {
            var output = Path.Combine(exportPath, $"{row.TorHash}.fastresume");
            File.WriteAllBytes(output, row.Data);
        }

        Console.WriteLine($"已导出 {rows.Count} 个文件到: {Path.GetFullPath(exportPath)}");
        return 0;
    }

    private static int RunUnknownCommand(string command)
    {
        Console.Error.WriteLine($"未知命令: {command}");
        CommandLineParser.PrintUsage();
        return 1;
    }
}

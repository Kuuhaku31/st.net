
namespace TorrentManager;

using TorrentManager.Cli;
using TorrentManager.Data;
using TorrentManager.Parsing;


/// <summary>
/// 应用主流程：分发命令并协调解析与存储组件。
/// </summary>
internal static class
TorrentApp
{
    /// <summary>
    /// 运行应用程序：解析命令行参数，执行对应命令，并返回适当的退出代码。
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static int
    Run(string[] args)
    {
        // 参数解析
        ParsedArgs parsed;
        {
            // 如果没有参数，直接显示用法信息并退出成功。
            if(args.Length == 0)
            {
                CommandLineParser.PrintUsage();
                return 0;
            }

            // 解析参数，捕获任何格式错误并显示用法信息。
            try{ parsed = CommandLineParser.Parse(args); }
            catch(ArgumentException ex)
            {
                Console.Error.WriteLine(ex.Message);
                CommandLineParser.PrintUsage();
                return 1;
            }

            // 如果没有位置参数，显示用法信息并退出成功。
            if(parsed.Positionals.Count == 0)
            {
                CommandLineParser.PrintUsage();
                return 0;
            }
        }

        // 数据库初始化
        var repository = new TorrentRepository(parsed.DbPath);
        repository.InitializeDatabase();

        // 命令分发
        var command = parsed.Positionals[0].ToLowerInvariant();
        return command switch
        {
            "add"     => RunAdd           (parsed.Positionals, repository),
            "add_all" => RunAddAll        (parsed.Positionals, repository),
            "export"  => RunExport        (parsed.Positionals, parsed.Options, repository),
            _         => RunUnknownCommand(command)
        };
    }


    /// <summary>
    /// 处理 "add" 命令：从指定的 .fastresume 文件读取数据并存储到数据库中。
    /// </summary>
    /// <param name="positionals"></param>
    /// <param name="repository"></param>
    /// <returns></returns>
    private static int
    RunAdd(IReadOnlyList<string> positionals, TorrentRepository repository)
    {
        // 参数验证
        if(positionals.Count < 2)
        {
            Console.Error.WriteLine("Usage: dotnet run -- add <path_to_fastresume_file> [--db <database_path>]");
            return 1;
        }

        // 确保提供了文件路径，并且文件存在。
        var filePath = positionals[1];
        if(!File.Exists(filePath))
        {
            Console.Error.WriteLine($"文件不存在: {filePath}");
            return 1;
        }

        // 尝试读取 .fastresume 文件并存储到数据库中，捕获任何错误并报告。
        try
        {
            var record = FastResumeReader.ReadRecord(filePath);
            repository.Upsert(record);
            Console.WriteLine($"已添加: {record.TorHash} ({Path.GetFileName(filePath)})");
            return 0;
        }
        catch(Exception ex)
        {
            Console.Error.WriteLine($"添加失败 '{filePath}': {ex.Message}");
            return 1;
        }
    }

    /// <summary>
    /// 处理 "add_all" 命令：从指定目录递归读取所有 .fastresume 文件，并存储到数据库中。统计成功和失败的文件数量，并在完成后报告结果。
    /// </summary>
    /// <param name="positionals"></param>
    /// <param name="repository"></param>
    /// <returns></returns>
    private static int
    RunAddAll(IReadOnlyList<string> positionals, TorrentRepository repository)
    {
        if(positionals.Count < 2)
        {
            Console.Error.WriteLine("Usage: dotnet run -- add_all <path_to_fastresume_files_directory> [--db <database_path>]");
            return 1;
        }

        var directory = positionals[1];
        if(!Directory.Exists(directory))
        {
            Console.Error.WriteLine($"目录不存在: {directory}");
            return 1;
        }

        var files = Directory.EnumerateFiles(directory, "*.fastresume", SearchOption.AllDirectories).ToList();
        if(files.Count == 0)
        {
            Console.WriteLine("未找到 .fastresume 文件。");
            return 0;
        }

        var success = 0;
        var failed = 0;

        foreach(var file in files)
        {
            try
            {
                var record = FastResumeReader.ReadRecord(file);
                repository.Upsert(record);
                success++;
            }
            catch(Exception ex)
            {
                failed++;
                Console.Error.WriteLine($"跳过 '{file}': {ex.Message}");
            }
        }

        Console.WriteLine($"add_all 完成: success={success}, failed={failed}");
        return failed > 0 ? 1 : 0;
    }

    /// <summary>
    /// 处理 "export" 命令：根据指定的类别模式查询数据库，导出匹配的记录为 .fastresume 文件到指定目录。默认导出路径为当前目录。完成后报告导出的文件数量和目标路径。
    /// </summary>
    /// <param name="positionals"></param>
    /// <param name="options"></param>
    /// <param name="repository"></param>
    /// <returns></returns>
    private static int
    RunExport(
        IReadOnlyList<string>               positionals,
        IReadOnlyDictionary<string, string> options,
        TorrentRepository                   repository
    )
    {
        // 参数验证：确保提供了类别参数。
        if(positionals.Count < 2)
        {
            Console.Error.WriteLine("Usage: dotnet run -- export <category> [--path <export_directory>] [--db <database_path>]");
            return 1;
        }

        // 确定导出路径：如果用户提供了 --path 选项且有效，则使用它；否则使用当前目录。
        var categoryPattern = positionals[1];

        // 确保导出目录存在，如果不存在则创建它。
        var exportPath =
        options.TryGetValue("path", out var pathValue) && !string.IsNullOrWhiteSpace(pathValue) ?
        pathValue : Environment.CurrentDirectory;
        Directory.CreateDirectory(exportPath);

        // 查询数据库并导出匹配的记录为 .fastresume 文件，捕获任何错误并报告。
        var rows = repository.QueryByCategory(categoryPattern);
        foreach(var row in rows)
        {
            var output = Path.Combine(exportPath, $"{row.TorHash}.fastresume");
            File.WriteAllBytes(output, row.Data);
        }

        // 完成后报告导出的文件数量和目标路径。
        Console.WriteLine($"已导出 {rows.Count} 个文件到: {Path.GetFullPath(exportPath)}");
        return 0;
    }

    /// <summary>
    /// 处理未知命令：显示错误信息和用法信息，并返回错误退出代码。
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    private static int RunUnknownCommand(string command)
    {
        Console.Error.WriteLine($"未知命令: {command}");
        CommandLineParser.PrintUsage();
        return 1;
    }
}

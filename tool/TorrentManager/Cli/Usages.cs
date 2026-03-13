
namespace TorrentManager.Cli;

internal static class Usages
{
    public const string AppTitle = "TorrentManager";
    public const string UsageHeader = "Usage:";
    public const string ExportModeHint = "<by_category | by_save_path>";

    public const string AddCommand = "dotnet run -- add <path_to_fastresume_file> [--db <database_path>]";
    public const string AddAllCommand = "dotnet run -- add_all <path_to_fastresume_files_directory> [--db <database_path>]";
    public const string ExportCommand = "dotnet run -- export <by_category | by_save_path> <pattern> [--path <export_directory>] [--db <database_path>]";
    public const string UpdateCommand = "dotnet run -- update <by_category | by_save_path> <pattern> <replace <search_str> <replace_str> | <new_value>> [options]";

    public const string UsageAdd = "Usage: " + AddCommand;
    public const string UsageAddAll = "Usage: " + AddAllCommand;
    public const string UsageExport = "Usage: " + ExportCommand;
    public const string UsageUpdate = "Usage: " + UpdateCommand;

    public const string ExportModeError = "export 参数错误: " + ExportModeHint;
    public const string UpdateModeError = "update 参数错误: " + ExportModeHint;
    public const string UnknownCommandPrefix = "未知命令: ";
    public const string FileNotFoundPrefix = "文件不存在: ";
    public const string DirectoryNotFoundPrefix = "目录不存在: ";
    public const string ExportPathInvalidPrefix = "导出路径无效或不可访问: ";

    public static void PrintUsage()
    {
        Console.WriteLine(AppTitle);
        Console.WriteLine();
        Console.WriteLine(UsageHeader);
        Console.WriteLine($"  {AddCommand}");
        Console.WriteLine($"  {AddAllCommand}");
        Console.WriteLine($"  {ExportCommand}");
        Console.WriteLine($"  {UpdateCommand}");
    }
}

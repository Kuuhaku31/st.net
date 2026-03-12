namespace TorrentManager.Tests.App;

using TorrentManager.Parsing;

public sealed class TorrentAppExportTests
{
    [Fact]
    public void Run_Returns1_WhenExportModeInvalid()
    {
        using var sandbox = new TempSandbox();
        var args = new[] { "export", "bad_mode", "%", "--db", sandbox.DbPath };

        var exitCode = TorrentApp.Run(args);

        Assert.Equal(1, exitCode);
    }

    [Fact]
    public void Run_ExportsFiles_BySavePath()
    {
        using var sandbox = new TempSandbox();

        // 先通过 add 命令写入测试数据，再执行 export 验证导出行为。
        File.WriteAllBytes(sandbox.FastResumePath, CreateFastResume("1234567890123456789012345678901234567890", "Music", @"D:\downloads\music"));
        var addExitCode = TorrentApp.Run(new[] { "add", sandbox.FastResumePath, "--db", sandbox.DbPath });
        Assert.Equal(0, addExitCode);

        var exportExitCode = TorrentApp.Run(new[]
        {
            "export", "by_save_path", "%\\music%", "--path", sandbox.ExportDir, "--db", sandbox.DbPath
        });

        Assert.Equal(0, exportExitCode);
        var exportedFiles = Directory.GetFiles(sandbox.ExportDir, "*.fastresume", SearchOption.TopDirectoryOnly);
        Assert.Single(exportedFiles);
    }

    [Fact]
    public void Run_Returns1_WhenExportPathInvalid()
    {
        using var sandbox = new TempSandbox();

        var exitCode = TorrentApp.Run(new[]
        {
            "export", "by_category", "%", "--path", "bad\0export", "--db", sandbox.DbPath
        });

        Assert.Equal(1, exitCode);
    }

    [Fact]
    public void Run_ReplaceByCategory_UpdatesFastResumeAndDatabase()
    {
        using var sandbox = new TempSandbox();
        File.WriteAllBytes(sandbox.FastResumePath, CreateFastResume("1234567890123456789012345678901234567890", "OldCat", @"D:\downloads\music"));

        var addExitCode = TorrentApp.Run(new[] { "add", sandbox.FastResumePath, "--db", sandbox.DbPath });
        Assert.Equal(0, addExitCode);

        var updateExitCode = TorrentApp.Run(new[] { "update", "by_category", "Old%", "NewCat", "--db", sandbox.DbPath });
        Assert.Equal(0, updateExitCode);

        var exportExitCode = TorrentApp.Run(new[] { "export", "by_category", "NewCat", "--path", sandbox.ExportDir, "--db", sandbox.DbPath });
        Assert.Equal(0, exportExitCode);

        var exportedFiles = Directory.GetFiles(sandbox.ExportDir, "*.fastresume", SearchOption.TopDirectoryOnly);
        Assert.Single(exportedFiles);

        var record = FastResumeReader.ReadRecord(exportedFiles[0]);
        Assert.Equal("NewCat", record.QbtCategory);
    }

    [Fact]
    public void Run_ReplaceBySavePath_UpdatesFastResumeAndDatabase()
    {
        using var sandbox = new TempSandbox();
        File.WriteAllBytes(sandbox.FastResumePath, CreateFastResume("1234567890123456789012345678901234567890", "Music", @"D:\downloads\old"));

        var addExitCode = TorrentApp.Run(new[] { "add", sandbox.FastResumePath, "--db", sandbox.DbPath });
        Assert.Equal(0, addExitCode);

        var updateExitCode = TorrentApp.Run(new[] { "update", "by_save_path", "%\\old", @"D:\downloads\new", "--db", sandbox.DbPath });
        Assert.Equal(0, updateExitCode);

        var exportExitCode = TorrentApp.Run(new[] { "export", "by_save_path", "%\\new", "--path", sandbox.ExportDir, "--db", sandbox.DbPath });
        Assert.Equal(0, exportExitCode);

        var exportedFiles = Directory.GetFiles(sandbox.ExportDir, "*.fastresume", SearchOption.TopDirectoryOnly);
        Assert.Single(exportedFiles);

        var record = FastResumeReader.ReadRecord(exportedFiles[0]);
        Assert.Equal(@"D:\downloads\new", record.SavePath);
    }

    [Fact]
    public void Run_UpdateByCategory_ReplaceMode_UpdatesMatchedSubstring()
    {
        using var sandbox = new TempSandbox();
        File.WriteAllBytes(sandbox.FastResumePath, CreateFastResume("1234567890123456789012345678901234567890", "music/2026-03-07/tmp", @"D:\downloads\music"));

        var addExitCode = TorrentApp.Run(new[] { "add", sandbox.FastResumePath, "--db", sandbox.DbPath });
        Assert.Equal(0, addExitCode);

        var updateExitCode = TorrentApp.Run(new[]
        {
            "update", "by_category", "music/%", "replace", "tmp", "tmp2", "--db", sandbox.DbPath
        });
        Assert.Equal(0, updateExitCode);

        var exportExitCode = TorrentApp.Run(new[] { "export", "by_category", "%tmp2", "--path", sandbox.ExportDir, "--db", sandbox.DbPath });
        Assert.Equal(0, exportExitCode);

        var exportedFiles = Directory.GetFiles(sandbox.ExportDir, "*.fastresume", SearchOption.TopDirectoryOnly);
        Assert.Single(exportedFiles);

        var record = FastResumeReader.ReadRecord(exportedFiles[0]);
        Assert.Equal("music/2026-03-07/tmp2", record.QbtCategory);
    }

    [Fact]
    public void Run_UpdateBySavePath_ReplaceMode_UpdatesMatchedSubstring()
    {
        using var sandbox = new TempSandbox();
        File.WriteAllBytes(sandbox.FastResumePath, CreateFastResume("1234567890123456789012345678901234567890", "Music", @"D:\qb\Downloads\music\2026-03-07\tmp"));

        var addExitCode = TorrentApp.Run(new[] { "add", sandbox.FastResumePath, "--db", sandbox.DbPath });
        Assert.Equal(0, addExitCode);

        var updateExitCode = TorrentApp.Run(new[]
        {
            "update", "by_save_path", @"D:\qb\Downloads\music\2026-03-07\tmp", "replace", "tmp", "tmp2", "--db", sandbox.DbPath
        });
        Assert.Equal(0, updateExitCode);

        var exportExitCode = TorrentApp.Run(new[] { "export", "by_save_path", "%tmp2", "--path", sandbox.ExportDir, "--db", sandbox.DbPath });
        Assert.Equal(0, exportExitCode);

        var exportedFiles = Directory.GetFiles(sandbox.ExportDir, "*.fastresume", SearchOption.TopDirectoryOnly);
        Assert.Single(exportedFiles);

        var record = FastResumeReader.ReadRecord(exportedFiles[0]);
        Assert.Equal(@"D:\qb\Downloads\music\2026-03-07\tmp2", record.SavePath);
    }

    private static byte[] CreateFastResume(string infoHash, string category, string savePath)
    {
        static string EncodeString(string text) => $"{text.Length}:{text}";

        var payload = "d"
            + EncodeString("info-hash") + EncodeString(infoHash)
            + EncodeString("qBt-category") + EncodeString(category)
            + EncodeString("save_path") + EncodeString(savePath)
            + "e";

        return System.Text.Encoding.UTF8.GetBytes(payload);
    }

    private sealed class TempSandbox : IDisposable
    {
        private readonly string _directory = Path.Combine(Path.GetTempPath(), "torrent-app-tests", Guid.NewGuid().ToString("N"));

        public string DbPath => Path.Combine(_directory, "fastresume.db");
        public string ExportDir => Path.Combine(_directory, "out");
        public string FastResumePath => Path.Combine(_directory, "sample.fastresume");

        public TempSandbox()
        {
            Directory.CreateDirectory(_directory);
        }

        public void Dispose()
        {
            if (!Directory.Exists(_directory))
            {
                return;
            }

            for (var i = 0; i < 5; i++)
            {
                try
                {
                    Directory.Delete(_directory, recursive: true);
                    return;
                }
                catch (IOException)
                {
                    if (i == 4)
                    {
                        return;
                    }

                    Thread.Sleep(50);
                }
            }

            // SQLite 连接池在测试结束时可能短暂持有文件句柄，清理失败时忽略即可。
        }
    }
}

namespace TorrentManager.Tests.Data;

using TorrentManager.Data;
using TorrentManager.Models;

public sealed class TorrentRepositoryTests
{
    [Fact]
    public void QueryForExport_ByCategory_ReturnsMatchedRows()
    {
        using var sandbox = new TempSandbox();
        var repository = new TorrentRepository(sandbox.DbPath);
        repository.InitializeDatabase();

        repository.Upsert(new FastResumeRecord("hash1", new byte[] { 0x01 }, "Anime", @"D:\downloads\anime"));
        repository.Upsert(new FastResumeRecord("hash2", new byte[] { 0x02 }, "Music", @"D:\downloads\music"));

        var rows = repository.QueryForExport("by_category", "A%");

        Assert.Single(rows);
        Assert.Equal("hash1", rows[0].TorHash);
    }

    [Fact]
    public void QueryForExport_BySavePath_ReturnsMatchedRows()
    {
        using var sandbox = new TempSandbox();
        var repository = new TorrentRepository(sandbox.DbPath);
        repository.InitializeDatabase();

        repository.Upsert(new FastResumeRecord("hash1", new byte[] { 0x01 }, "Anime", @"D:\downloads\anime"));
        repository.Upsert(new FastResumeRecord("hash2", new byte[] { 0x02 }, "Music", @"D:\downloads\music"));

        var rows = repository.QueryForExport("by_save_path", "%\\music%");

        Assert.Single(rows);
        Assert.Equal("hash2", rows[0].TorHash);
    }

    [Fact]
    public void QueryForExport_Throws_WhenModeUnsupported()
    {
        using var sandbox = new TempSandbox();
        var repository = new TorrentRepository(sandbox.DbPath);
        repository.InitializeDatabase();

        var ex = Assert.Throws<ArgumentException>(() => repository.QueryForExport("invalid", "%"));

        Assert.Contains("不支持的导出方式", ex.Message);
    }

    private sealed class TempSandbox : IDisposable
    {
        private readonly string _directory = Path.Combine(Path.GetTempPath(), "torrent-manager-tests", Guid.NewGuid().ToString("N"));
        public string DbPath => Path.Combine(_directory, "fastresume.db");

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

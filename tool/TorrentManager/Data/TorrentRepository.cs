
namespace TorrentManager.Data;

using Microsoft.Data.Sqlite;
using TorrentManager.Models;


/// <summary>
/// TorrentRepository：负责与 SQLite 数据库交互，提供初始化、插入/更新和查询功能。
/// </summary>
/// <param name="dbPath"></param>
internal sealed class TorrentRepository(string dbPath)
{
    private readonly string _dbPath = dbPath;

    /// <summary>
    /// 初始化数据库：如果数据库文件不存在，则创建它并设置必要的表结构。
    /// </summary>
    public void InitializeDatabase()
    {
        var directory = Path.GetDirectoryName(Path.GetFullPath(_dbPath));
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var connection = CreateConnection();
        using var command = connection.CreateCommand();
        command.CommandText = @"
CREATE TABLE IF NOT EXISTS torrent_fastresume (
    TOR_HASH TEXT PRIMARY KEY,
    fastresume_file BLOB NOT NULL,
    qbt_category TEXT,
    save_path TEXT
);";
        command.ExecuteNonQuery();
    }

    public void Upsert(FastResumeRecord record)
    {
        using var connection = CreateConnection();
        using var command = connection.CreateCommand();
        command.CommandText = @"
INSERT INTO torrent_fastresume (TOR_HASH, fastresume_file, qbt_category, save_path)
VALUES ($hash, $file, $category, $savePath)
ON CONFLICT(TOR_HASH) DO UPDATE SET
    fastresume_file = excluded.fastresume_file,
    qbt_category = excluded.qbt_category,
    save_path = excluded.save_path;";

        command.Parameters.AddWithValue("$hash", record.TorHash);
        command.Parameters.AddWithValue("$file", record.FastResumeFile);
        command.Parameters.AddWithValue("$category", (object?)record.QbtCategory ?? DBNull.Value);
        command.Parameters.AddWithValue("$savePath", (object?)record.SavePath ?? DBNull.Value);
        command.ExecuteNonQuery();
    }

    public IReadOnlyList<(string TorHash, byte[] Data)> QueryForExport(string exportMode, string pattern)
    {
        using var connection = CreateConnection();
        using var command = connection.CreateCommand();
        command.CommandText = exportMode switch
        {
            "by_category" => @"
SELECT TOR_HASH, fastresume_file
FROM torrent_fastresume
WHERE qbt_category LIKE $pattern;",
            "by_save_path" => @"
SELECT TOR_HASH, fastresume_file
FROM torrent_fastresume
WHERE save_path LIKE $pattern;",
            _ => throw new ArgumentException($"不支持的导出方式: {exportMode}")
        };
        command.Parameters.AddWithValue("$pattern", pattern);

        var result = new List<(string TorHash, byte[] Data)>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add((reader.GetString(0), (byte[])reader[1]));
        }

        return result;
    }

    private SqliteConnection CreateConnection()
    {
        var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Open();
        return connection;
    }
}

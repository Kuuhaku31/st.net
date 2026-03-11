using Microsoft.Data.Sqlite;
using TorrentManager.Models;

namespace TorrentManager.Data;

/// <summary>
/// 负责所有 SQLite 读写操作，业务层无需关心 SQL 细节。
/// </summary>
internal sealed class TorrentRepository
{
    private readonly string _dbPath;

    public TorrentRepository(string dbPath)
    {
        _dbPath = dbPath;
    }

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

    public IReadOnlyList<(string TorHash, byte[] Data)> QueryByCategory(string categoryPattern)
    {
        using var connection = CreateConnection();
        using var command = connection.CreateCommand();
        command.CommandText = @"
SELECT TOR_HASH, fastresume_file
FROM torrent_fastresume
WHERE qbt_category LIKE $category;";
        command.Parameters.AddWithValue("$category", categoryPattern);

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


using Microsoft.Data.Sqlite;
using TorrentManager.Models;

namespace TorrentManager.Data;


/// <summary>
/// TorrentRepository：负责与 SQLite 数据库交互，提供初始化、插入/更新和查询功能。
/// </summary>
internal sealed class TorrentRepository
{
    private readonly string _dbPath;

    /// <summary>
    /// 构造函数：接受数据库路径，确保数据库文件所在目录存在，并创建数据表（如果尚未存在）。
    /// </summary>
    /// <param name="dbPath"></param>
    public TorrentRepository(string dbPath)
    {
        // 验证并规范化数据库路径，确保目录存在
        _dbPath = NormalizeAndValidateDbPath(dbPath);

        // 创建数据库表，如果尚未存在
        using var connection = CreateConnection();
        using var command    = connection.CreateCommand();
        command.CommandText  = SqlStr.CreateTableSql;
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// 插入或更新记录：根据 TOR_HASH 主键，如果记录已存在则更新，否则插入新记录。
    /// 使用参数化查询防止 SQL 注入，并正确处理可选字段的 null 值。
    /// </summary>
    /// <param name="record"></param>
    public void Upsert(FastResumeRecord record)
    {
        using var connection = CreateConnection();
        using var command    = connection.CreateCommand();
        command.CommandText  = SqlStr.UpsertSql;

        command.Parameters.AddWithValue("$hash", record.TorHash);
        command.Parameters.AddWithValue("$file", record.FastResumeFile);
        command.Parameters.AddWithValue("$category", (object?)record.QbtCategory ?? DBNull.Value);
        command.Parameters.AddWithValue("$savePath", (object?)record.SavePath ?? DBNull.Value);
        command.ExecuteNonQuery();
    }

    public IReadOnlyList<(string TorHash, byte[] Data)>
    QueryForExport(string exportMode, string pattern)
    {
        using var connection = CreateConnection();
        using var command    = connection.CreateCommand();
        command.CommandText  = exportMode switch
        {
            "by_category"  => SqlStr.QueryByCategorySql,
            "by_save_path" => SqlStr.QueryBySavePathSql,
            _              => throw new ArgumentException($"不支持的导出方式: {exportMode}")
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

    private static string NormalizeAndValidateDbPath(string dbPath)
    {
        if(string.IsNullOrWhiteSpace(dbPath))
        {
            throw new ArgumentException("数据库路径不能为空。", nameof(dbPath));
        }

        string fullPath;
        try
        {
            fullPath = Path.GetFullPath(dbPath);
        }
        catch(Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
        {
            throw new ArgumentException($"数据库路径无效: {dbPath}", nameof(dbPath), ex);
        }

        var directory = Path.GetDirectoryName(fullPath);
        if(!string.IsNullOrWhiteSpace(directory))
        {
            try
            {
                Directory.CreateDirectory(directory);
            }
            catch(Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException or UnauthorizedAccessException)
            {
                throw new ArgumentException($"数据库目录无效或不可访问: {directory}", nameof(dbPath), ex);
            }
        }

        return fullPath;
    }
}

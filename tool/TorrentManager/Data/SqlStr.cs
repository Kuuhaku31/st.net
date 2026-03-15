
namespace TorrentManager.Data;


internal static class SqlStr
{
    public static readonly string CreateTableSql = @"
CREATE TABLE IF NOT EXISTS torrent_fastresume (
    TOR_HASH TEXT PRIMARY KEY,
    fastresume_file BLOB NOT NULL,
    qbt_category TEXT,
    save_path TEXT
);";

    public static readonly string UpsertSql = @"
INSERT INTO torrent_fastresume (TOR_HASH, fastresume_file, qbt_category, save_path)
VALUES ($hash, $file, $category, $savePath)
ON CONFLICT(TOR_HASH) DO UPDATE SET
    fastresume_file = excluded.fastresume_file,
    qbt_category = excluded.qbt_category,
    save_path = excluded.save_path;";

    public static readonly string QueryByCategorySql = @"
SELECT TOR_HASH, fastresume_file
FROM torrent_fastresume
WHERE qbt_category LIKE $pattern;";

    public static readonly string QueryBySavePathSql = @"
SELECT TOR_HASH, fastresume_file
FROM torrent_fastresume
WHERE save_path LIKE $pattern;";
}

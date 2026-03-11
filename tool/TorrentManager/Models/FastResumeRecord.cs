namespace TorrentManager.Models;

/// <summary>
/// 表示一条 fastresume 记录，对应数据库的一行数据。
/// </summary>
internal sealed record FastResumeRecord(
    string TorHash,
    byte[] FastResumeFile,
    string? QbtCategory,
    string? SavePath
);

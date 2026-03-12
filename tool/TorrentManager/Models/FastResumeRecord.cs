
namespace TorrentManager.Models;


/// <summary>
/// 快速恢复记录：包含 torrent hash、fast resume 文件内容、qBittorrent 分类和保存路径。
/// </summary>
/// <param name="TorHash">种子哈希</param>
/// <param name="FastResumeFile">Fast Resume 文件内容</param>
/// <param name="QbtCategory">qBittorrent 分类</param>
/// <param name="SavePath">保存路径</param>
internal sealed record FastResumeRecord(
    string  TorHash,
    byte[]  FastResumeFile,
    string? QbtCategory,
    string? SavePath
);

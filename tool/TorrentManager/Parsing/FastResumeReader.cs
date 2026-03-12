using System.Text;

using TorrentManager.Models;

namespace TorrentManager.Parsing;

/// <summary>
/// 负责从 .fastresume 文件读取并提取业务字段。
/// </summary>
internal static class FastResumeReader
{
    public static FastResumeRecord ReadRecord(string filePath)
    {
        var bytes  = File.ReadAllBytes(filePath);
        var parser = new BencodeParser(bytes);
        var root   = parser.ParseDictionary();

        var infoHashRaw = GetRequiredBytes(root, "info-hash");
        var torHash     = NormalizeInfoHash(infoHashRaw);
        var category    = GetOptionalString(root, "qBt-category");
        var savePath    = GetOptionalString(root, "save_path");

        return new FastResumeRecord(torHash, bytes, category, savePath);
    }

    private static byte[]
    GetRequiredBytes(Dictionary<string, object> dict, string key)
    {
        return !dict.TryGetValue(key, out var value) || value is not byte[] data ?
        throw new InvalidDataException($"缺少或无效字段: {key}") : data;
    }

    private static string?
    GetOptionalString(Dictionary<string, object> dict, string key)
    {
        return !dict.TryGetValue(key, out var value) || value is not byte[] data ?
        null : Encoding.UTF8.GetString(data);
    }

    // info-hash 既可能是 20 字节原始哈希，也可能是 40 位十六进制字符串。
    private static string NormalizeInfoHash(byte[] raw)
    {
        if(raw.Length == 20) return Convert.ToHexString(raw).ToLowerInvariant();

        var text = Encoding.UTF8.GetString(raw).Trim();
        return text.Length == 40 && text.All(IsHexChar) ?
        text.ToLowerInvariant() :
        throw new InvalidDataException("字段 info-hash 既不是 20 字节，也不是 40 位十六进制字符串。");
    }

    private static bool IsHexChar(char c)
    {
        return (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
    }
}

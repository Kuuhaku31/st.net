namespace TorrentManager.Parsing;

using System.Text;

/// <summary>
/// 提供 .fastresume 字段修改能力：解析 bencode 后修改指定键并重新编码。
/// </summary>
internal static class FastResumeEditor
{
    public static byte[] ReplaceField(byte[] originalBytes, string fieldName, string newValue)
    {
        var parser = new BencodeParser(originalBytes);
        var root = parser.ParseDictionary();

        root[fieldName] = Encoding.UTF8.GetBytes(newValue);
        return EncodeDictionary(root);
    }

    private static byte[] EncodeDictionary(Dictionary<string, object> dict)
    {
        using var stream = new MemoryStream();
        WriteDictionary(stream, dict);
        return stream.ToArray();
    }

    private static void WriteValue(Stream stream, object value)
    {
        switch (value)
        {
            case byte[] bytes:
                WriteByteString(stream, bytes);
                break;
            case string text:
                WriteByteString(stream, Encoding.UTF8.GetBytes(text));
                break;
            case int intValue:
                WriteInteger(stream, intValue);
                break;
            case long longValue:
                WriteInteger(stream, longValue);
                break;
            case Dictionary<string, object> map:
                WriteDictionary(stream, map);
                break;
            case List<object> list:
                WriteList(stream, list);
                break;
            default:
                throw new InvalidDataException($"不支持的 bencode 值类型: {value.GetType().Name}");
        }
    }

    private static void WriteDictionary(Stream stream, Dictionary<string, object> dict)
    {
        stream.WriteByte((byte)'d');
        foreach (var key in dict.Keys.OrderBy(k => k, StringComparer.Ordinal))
        {
            WriteByteString(stream, Encoding.UTF8.GetBytes(key));
            WriteValue(stream, dict[key]);
        }
        stream.WriteByte((byte)'e');
    }

    private static void WriteList(Stream stream, List<object> list)
    {
        stream.WriteByte((byte)'l');
        foreach (var item in list)
        {
            WriteValue(stream, item);
        }
        stream.WriteByte((byte)'e');
    }

    private static void WriteInteger(Stream stream, long value)
    {
        stream.WriteByte((byte)'i');
        var textBytes = Encoding.ASCII.GetBytes(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        stream.Write(textBytes, 0, textBytes.Length);
        stream.WriteByte((byte)'e');
    }

    private static void WriteByteString(Stream stream, byte[] bytes)
    {
        var lenBytes = Encoding.ASCII.GetBytes(bytes.Length.ToString(System.Globalization.CultureInfo.InvariantCulture));
        stream.Write(lenBytes, 0, lenBytes.Length);
        stream.WriteByte((byte)':');
        stream.Write(bytes, 0, bytes.Length);
    }
}

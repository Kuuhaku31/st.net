namespace TorrentManager.Parsing;

using System.Text;

/// <summary>
/// 提供 .fastresume 字段修改能力：解析 bencode 后修改指定键并重新编码。
/// </summary>
internal static class FastResumeEditor
{
    /// <summary>
    /// 替换指定字段的值：解析原始字节数组，修改指定字段的值为新的字符串（UTF-8 编码），然后重新编码为字节数组。
    /// </summary>
    /// <param name="originalBytes">原始 .fastresume 文件内容的字节数组</param>
    /// <param name="fieldName">要替换的字段名称（如 "qBt-category" 或 "save_path"）</param>
    /// <param name="newValue">新的字段值（字符串，将被编码为 UTF-8 字节）</param>
    /// <returns>修改后的 .fastresume 文件内容的字节数组</returns>
    public static byte[] ReplaceField(byte[] originalBytes, string fieldName, string newValue)
    {
        var parser = new BencodeParser(originalBytes);
        var root = parser.ParseDictionary();

        root[fieldName] = Encoding.UTF8.GetBytes(newValue);
        return EncodeDictionary(root);
    }

    public static byte[] ReplaceFieldSubstring(byte[] originalBytes, string fieldName, string searchValue, string replaceValue)
    {
        var parser = new BencodeParser(originalBytes);
        var root = parser.ParseDictionary();

        var currentValue = root.TryGetValue(fieldName, out var value) && value is byte[] bytes
            ? Encoding.UTF8.GetString(bytes)
            : string.Empty;

        root[fieldName] = Encoding.UTF8.GetBytes(currentValue.Replace(searchValue, replaceValue, StringComparison.Ordinal));
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

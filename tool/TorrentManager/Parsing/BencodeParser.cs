
namespace TorrentManager.Parsing;

using System.Globalization;
using System.Text;


/// <summary>
/// 最小 bencode 解析器：支持 dictionary/list/integer/string。
/// </summary>
internal sealed class BencodeParser(byte[] data)
{
    private readonly byte[] _data = data;
    private int _position;

    public Dictionary<string, object> ParseDictionary()
    {
        var value = ParseValue();

        return
        value is not Dictionary<string, object> dictionary ?
        throw new InvalidDataException("Bencode 根节点不是字典。") :

        _position != _data.Length ?
        throw new InvalidDataException("Bencode 存在未消费的尾部数据。") : dictionary;
    }

    private object ParseValue()
    {
        EnsureHasData();
        return _data[_position] switch
        {
            (byte)'d' => ParseDictionaryValue(),
            (byte)'l' => ParseListValue(),
            (byte)'i' => ParseIntegerValue(),
            >= (byte)'0' and <= (byte)'9' => ParseByteString(),
            _ => throw new InvalidDataException($"无效的 bencode 标记，偏移 {_position}。")
        };
    }

    private Dictionary<string, object> ParseDictionaryValue()
    {
        _position++;
        var dictionary = new Dictionary<string, object>(StringComparer.Ordinal);

        while (true)
        {
            EnsureHasData();
            if (_data[_position] == (byte)'e')
            {
                _position++;
                break;
            }

            var key = Encoding.UTF8.GetString(ParseByteString());
            var value = ParseValue();
            dictionary[key] = value;
        }

        return dictionary;
    }

    private List<object> ParseListValue()
    {
        _position++;
        var list = new List<object>();

        while (true)
        {
            EnsureHasData();
            if (_data[_position] == (byte)'e')
            {
                _position++;
                break;
            }

            list.Add(ParseValue());
        }

        return list;
    }

    private long ParseIntegerValue()
    {
        _position++;
        var end = Array.IndexOf(_data, (byte)'e', _position);
        if (end < 0)
        {
            throw new InvalidDataException("无效的 bencode 整数格式。");
        }

        var text = Encoding.ASCII.GetString(_data, _position, end - _position);
        if (!long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
        {
            throw new InvalidDataException("bencode 整数值解析失败。");
        }

        _position = end + 1;
        return value;
    }

    private byte[] ParseByteString()
    {
        var colon = Array.IndexOf(_data, (byte)':', _position);
        if (colon < 0)
        {
            throw new InvalidDataException("无效的 bencode 字符串长度。");
        }

        var lenText = Encoding.ASCII.GetString(_data, _position, colon - _position);
        if (!int.TryParse(lenText, NumberStyles.None, CultureInfo.InvariantCulture, out var length) || length < 0)
        {
            throw new InvalidDataException("bencode 字符串长度解析失败。");
        }

        _position = colon + 1;
        if (_position + length > _data.Length)
        {
            throw new InvalidDataException("bencode 字符串长度越界。");
        }

        var buffer = new byte[length];
        Buffer.BlockCopy(_data, _position, buffer, 0, length);
        _position += length;
        return buffer;
    }

    private void EnsureHasData()
    {
        if (_position >= _data.Length)
        {
            throw new InvalidDataException("bencode 数据提前结束。");
        }
    }
}


using System.Text;
using System.Text.Json;

class BencodeParser(byte[] data)
{
    private readonly byte[] data = data; //
    private int             pos  = 0;    // 

    public object
    Parse() { return ParseValue(); }

    private object
    ParseValue()
    {
        byte b = data[pos];

        if(b == 'i')              return ParseInteger();
        if(b == 'l')              return ParseList();
        if(b == 'd')              return ParseDictionary();
        if(char.IsDigit((char)b)) return ParseString();

        throw new Exception("Invalid Bencode format");
    }

    private long
    ParseInteger()
    {
        pos++; // skip 'i'
        int start = pos;

        while(data[pos] != 'e') pos++;

        string num = Encoding.ASCII.GetString(data, start, pos - start);
        pos++; // skip 'e'

        return long.Parse(num);
    }

    private string
    ParseString()
    {
        int start = pos;

        while(data[pos] != ':') pos++;

        string lenStr = Encoding.ASCII.GetString(data, start, pos - start);
        int len = int.Parse(lenStr);

        pos++; // skip ':'

        string str = Encoding.UTF8.GetString(data, pos, len);
        pos += len;

        return str;
    }

    private List<object>
    ParseList()
    {
        pos++; // skip 'l'
        var list = new List<object>();

        while(data[pos] != 'e') list.Add(ParseValue());

        pos++; // skip 'e'
        return list;
    }

    private Dictionary<string, object>
    ParseDictionary()
    {
        pos++; // skip 'd'
        var dict = new Dictionary<string, object>();

        while(data[pos] != 'e')
        {
            string key = ParseString();
            object value = ParseValue();
            dict[key] = value;
        }

        pos++; // skip 'e'
        return dict;
    }
}

class Program
{
    static void Main(string[] args)
    {
        if(args.Length == 0)
        {
            Console.WriteLine("Usage: BencodeToJson <file>");
            return;
        }

        byte[] data = File.ReadAllBytes(args[0]);

        var parser = new BencodeParser(data);
        object result = parser.Parse();

        string json = JsonSerializer.Serialize(
            result,
            new JsonSerializerOptions { WriteIndented = true }
        );

        // Console.WriteLine(json);
        // 输出到文件
        string outputFile = args[0] + ".json";
        File.WriteAllText(outputFile, json);
    }
}
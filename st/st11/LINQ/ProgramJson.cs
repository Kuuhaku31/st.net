using Newtonsoft.Json.Linq;

class ProgramJson
{
    static void Main(string[] args)
    {
        string jsonText = @"{
            'books': [
                {
                    'title': 'Shadows of Tomorrow',
                    'author': 'Elena Smith',
                    'date': '2024-01-05'
                },
                {
                    'title': 'Echoes of the Past',
                    'author': 'Sarah Connor',
                    'date': '2023-11-15'
                },
                {
                    'title': 'Whispers of the Future',
                    'author': 'Sarah Connor',
                    'date': '2024-02-20'
                }
            ]
        }";

        JObject json = JObject.Parse(jsonText);

        // var results = json["books"]
        //     .OrderByDescending(book => book["date"])
        //     .Select(book => book["title"] + ", " + book["date"])
        // ;

        // 先将JToken转换为JArray，再进行排序和选择
        if(json["books"] is JArray books)
        {
            // 对数组进行排序和选择
            var results = books
                .OrderByDescending(book => book["date"])             // 按照日期降序排序
                .Select(book => book["title"] + ", " + book["date"]) // 选择需要的字段并格式化输出
            ;

            // 输出结果
            foreach(var item in results)
            {
                Console.WriteLine(item);
            }
        }
    }

}

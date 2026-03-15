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

        var json = JObject.Parse(jsonText);

        // var results = json["books"]
        //     .OrderByDescending(book => book["date"])
        //     .Select(book => book["title"] + ", " + book["date"])
        // ;

        // 先将JToken转换为JArray，再进行排序和选择
        if(json["books"] is JArray books)
        {
            // 对数组进行排序和选择
            var results = SelectAuthorDescending(books);

            // 输出结果
            foreach(var item in results)
            {
                Console.WriteLine(item);
            }

            // 统计指定作者的书籍数量
            string authorToCount = "Sarah Connor";
            int count = CountBooksByAuthor(books, authorToCount);
            Console.WriteLine($"Number of books by {authorToCount}: {count}");
        }
    }

    /// <summary>
    /// 根据作者名称降序排序，并选择作者名称
    /// </summary>
    /// <param name="books"></param>
    /// <returns></returns>
    static IEnumerable<JToken?> SelectAuthorDescending(JArray books)
    {
        return books.OrderByDescending(b => b["author"]).Select(b => b["author"]);
    }

    /// <summary>
    /// 根据作者名称过滤出指定作者的书籍数量
    /// </summary>
    /// <param name="books"></param>
    /// <param name="author"></param>
    /// <returns></returns>
    static int CountBooksByAuthor(JArray books, string author)
    {
        return books.Count(b => b["author"]?.ToString() == author);
    }
}

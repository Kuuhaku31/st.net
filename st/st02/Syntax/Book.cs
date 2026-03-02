namespace ST02.Syntax
{
    internal class Book
    {
        private readonly string _author;
        private readonly string _title;
        private readonly int    _pages;

        public Book(string author, string title, int pages)
        {
            _author = author;
            _title  = title;
            _pages  = pages;
        }

        public void DisplayBook()
        {
            Console.WriteLine($"Author: {_author}, Title: {_title}, Pages: {_pages}");
        }
    }
}
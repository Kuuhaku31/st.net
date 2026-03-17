using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace ToDoApp
{
    internal class ToDoModel
    {
        public List<ToDo> ToDos { get; set; } = [
            new ToDo(
                id: 0,
                name: "Buy milk",
                deadline: DateTime.Now
                ),
            new ToDo(
                id: 1,
                name: "Buy new PC",
                deadline: new DateTime(2023, 12, 24),
                completed: true
                ),
            new ToDo(
                id: 2,
                name: "Buy chocolate",
                deadline: new DateTime(2024, 2, 14),
                completed: true
                )
        ];
    }

    partial class ToDo(string name, DateTime deadline, bool completed = false, int? id = null) : ObservableObject
    {
        public int? Id { get; set; } = id;
        [ObservableProperty]
        private string _name = name;
        [ObservableProperty]
        private DateTime _deadline = deadline;
        [ObservableProperty]
        private bool _completed = completed;
    }
}
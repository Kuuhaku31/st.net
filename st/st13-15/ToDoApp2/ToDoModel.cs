using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace ToDoApp2
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
                completed: true,
                priority: 2
                ),
            new ToDo(
                id: 2,
                name: "Buy chocolate",
                deadline: new DateTime(2024, 2, 14),
                completed: true,
                priority: 3
                )
        ];

        public void UpdateName(ToDo todo, string name) => Debug.WriteLine($"Name has been updated to {name} in ToDo#{todo.Id}");
        public void UpdateDeadline(ToDo todo, DateTime deadline) => Debug.WriteLine($"Deadline has been updated to {deadline} in ToDo#{todo.Id}");
        public void UpdateCompleted(ToDo todo, bool completed) => Debug.WriteLine($"Completed has been updated to {completed} in ToDo#{todo.Id}");
        public void Add(ToDo todo) => Debug.WriteLine($"ToDo#{todo.Id} has been added");
        public void Delete(ToDo todo) => Debug.WriteLine($"ToDo#{todo.Id} has been deleted");
    }

    partial class ToDo(string name, DateTime deadline, bool completed = false, int priority = 1, int? id = null) : ObservableObject
    {
        public int? Id { get; set; } = id;
        [ObservableProperty]
        private string _name = name;
        [ObservableProperty]
        private DateTime _deadline = deadline;
        [ObservableProperty]
        private bool _completed = completed;
        [ObservableProperty]
        private int _priority = priority;
    }
}

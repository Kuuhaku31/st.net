using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics;

namespace ToDoApp3
{
    internal class ToDoContext : DbContext
    {
        public DbSet<ToDo> ToDos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=testdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }

    internal class ToDoModel
    {
        public List<ToDo> ToDos { get; set; }
            
        public ToDoModel()
        {
            using (var context = new ToDoContext())
            {
                ToDos = context.ToDos.Select(t => new ToDo(t.Name, t.Deadline, t.Completed, t.Id)).ToList();
            }
        }

        public void UpdateName(ToDo todo, string name) {
            using (var context = new ToDoContext())
            {
                var taskToUpdate = context.ToDos.Find(todo.Id);
                if (taskToUpdate != null)
                {
                    taskToUpdate.Name = name;
                    context.SaveChanges();
                    Debug.WriteLine($"Name has been updated to {name} in ToDo#{todo.Id}");
                }
            }
        }
         
        public void UpdateDeadline(ToDo todo, DateTime deadline)
        {
            using (var context = new ToDoContext())
            {
                var taskToUpdate = context.ToDos.Find(todo.Id);
                if (taskToUpdate != null)
                {
                    taskToUpdate.Deadline = deadline;
                    context.SaveChanges();
                    Debug.WriteLine($"Deadline has been updated to {deadline} in ToDo#{todo.Id}");
                }
            }
        }

        public void UpdateCompleted(ToDo todo, bool completed)
        {
            using (var context = new ToDoContext())
            {
                var taskToUpdate = context.ToDos.Find(todo.Id);
                if (taskToUpdate != null)
                {
                    taskToUpdate.Completed = completed;
                    context.SaveChanges();
                    Debug.WriteLine($"Completed has been updated to {completed} in ToDo#{todo.Id}");
                }
            }
        }

        public ToDo Add(ToDo todo)
        {
            ToDo newToDo;
            using (var context = new ToDoContext())
            {
                EntityEntry<ToDo> entry = context.ToDos.Add(todo);
                newToDo = entry.Entity; 
                context.SaveChanges();
                Debug.WriteLine($"ToDo#{newToDo.Id} has been added");
            }
            return newToDo;
        }
        public void Delete(ToDo todo)
        {
            using (var context = new ToDoContext())
            {
                var taskToDelete = context.ToDos.Find(todo.Id);
                if (taskToDelete != null)
                {
                    context.ToDos.Remove(taskToDelete);
                    context.SaveChanges();
                    Debug.WriteLine($"ToDo#{todo.Id} has been deleted");
                }
            }
        } 
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

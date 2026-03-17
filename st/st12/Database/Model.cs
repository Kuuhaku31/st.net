
namespace Database;

using Microsoft.EntityFrameworkCore;


public class TaskContext : DbContext
{
    public DbSet<ToDo> ToDos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog = testdb; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate = False; Application Intent = ReadWrite; Multi Subnet Failover = False");
    }
}

public class ToDo(string name, DateTime deadline, bool completed = false, int? id = null)
{
    public int? Id { get; set; } = id;
    public string Name { get; set; } = name;
    public DateTime Deadline { get; set; } = deadline;
    public bool Completed { get; set; } = completed;
}

using Microsoft.EntityFrameworkCore;
using praktika28_Shein.Classes.Database;
using praktika28_Shein.Models;

public class TaskContext : DbContext
{
    public DbSet<Tasks> Tasks { get; set; }
    public DbSet<Priority> Priorities { get; set; }

    public TaskContext()
    {
        Database.EnsureCreated();
        Tasks.Load();
        Priorities.Load();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tasks>()
            .HasOne(t => t.Priority)
            .WithMany()
            .HasForeignKey(t => t.PriorityId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(Config.connection, Config.version);
    }
}
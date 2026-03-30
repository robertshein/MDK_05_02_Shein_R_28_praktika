using Microsoft.EntityFrameworkCore;
using praktika28_Shein.Classes.Database;
using praktika28_Shein.Models;
using System.Linq;

namespace praktika28_Shein.Context
{
    public class TaskContext : DbContext
    {
        public DbSet<Tasks> Tasks { get; set; }

        public TaskContext()
        {
            Database.EnsureCreated();
            Tasks.Load();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Config.connection, Config.version);
        }
    }
}
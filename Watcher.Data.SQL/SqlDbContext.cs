using Microsoft.EntityFrameworkCore;
using Watcher.Data.Entities;

namespace Watcher.Data.SQL
{
    public class SqlDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<EventLog> Events { get; set; }

    }
}

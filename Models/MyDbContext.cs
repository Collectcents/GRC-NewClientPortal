using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GRC_NewClientPortal.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        // Define your DbSets here, e.g.:
        //public DbSet<SomeEntity> SomeEntities { get; set; }
    }
}

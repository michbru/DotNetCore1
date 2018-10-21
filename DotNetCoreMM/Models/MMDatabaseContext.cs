using Microsoft.EntityFrameworkCore;

namespace DotNetCoreMM.Models
{
    public class MMDatabaseContext : DbContext
    {
        private bool customVar = true;
        public MMDatabaseContext(DbContextOptions options)
            : base(options)
        {
        }
        public MMDatabaseContext(DbContextOptions options, bool custom = true)
          : base(options)
        {
            customVar = false;
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (customVar == false)
            {
                base.OnConfiguring(optionsBuilder);
                // HACK: Requires putting Startup in this project
                optionsBuilder.UseSqlServer("Server=.;Initial Catalog=DotNetCoreMMDatabase;Trusted_Connection=True");
            }
           }
    }



}

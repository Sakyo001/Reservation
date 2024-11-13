using System.Data.Entity;
using LAB_FINAL_ACT.Models;

namespace LAB_FINAL_ACT.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Admin> Admin { get; set; }

        public ApplicationDbContext() 
            : base("name=DefaultConnection")  // This will use the connection string from Web.config
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Admin>()
                .ToTable("dbo.Admin");
        }
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<Employee> Employees { get; set; }

        // Seeding Initial Data for the DB
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* 
               -- We chose to move thsese seeding codes in the ModelBuilderExtension static class --
                  modelBuilder.Entity<Employee>().HasData(new Employee{ Id = 1, Name="Felly KANYIKI", 
                                              Department=Dept.It, Email="fellyka@sollers.co.za"}); */

            base.OnModelCreating(modelBuilder);

            // modelBuilder.Seed();
            ModelBuilderExtension.Seed(modelBuilder);
        }
    }
}

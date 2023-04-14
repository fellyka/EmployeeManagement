using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
               new Employee { Id = 1, Name = "Felly KANYIKI", Department = Dept.It, Email = "fellyka@sollers.co.za"},
               new Employee { Id = 2, Name = "David KUELA", Department = Dept.Hr, Email = "davidku@sollers.co.za"},
               new Employee { Id = 3, Name = "Mary MAEL", Department = Dept.Payroll, Email = "maryma@sollers.co.za"});
        }
    }
}

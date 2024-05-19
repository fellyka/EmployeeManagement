using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
               new Employee { Id = 1, Name = "Felly KANYIKI", Department = Dept.IT, Email = "fellyka@sollers.co.za" },
               new Employee { Id = 2, Name = "David KUELA", Department = Dept.HR, Email = "davidku@sollers.co.za" },
               new Employee { Id = 3, Name = "Mary MAEL", Department = Dept.Payroll, Email = "maryma@sollers.co.za" },
               new Employee { Id = 4, Name = "Gael MBULO", Department = Dept.IT, Email = "gaelmb@sollers.co.za" },
               new Employee { Id = 5, Name = "John RIKOLO", Department = Dept.HR, Email = "johnri@sollers.co.za" });
        }
    }
}
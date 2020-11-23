using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {

        private readonly AppDbContext context;

        public SQLEmployeeRepository(AppDbContext context)
        {
            this.context = context;  
        }


        public Employee Add(Employee employee)
        {
            context.MyEmployees.Add(employee);
            context.SaveChanges();

            return employee;
        }

        public Employee Delete(int id)
        {
           Employee employee =  context.MyEmployees.Find(id);
           if(employee != null)
            {
                context.MyEmployees.Remove(employee);
                context.SaveChanges();
            }
            return employee;        
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return context.MyEmployees;
        }

        public Employee GetEmployee(int id)
        {
            return context.MyEmployees.Find(id);
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = context.MyEmployees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();

            return employeeChanges;
        }
    }
}

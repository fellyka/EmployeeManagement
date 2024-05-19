
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;

        public SQLEmployeeRepository(EmployeeDbContext context)
        {
            _context = context;
        }
        public Employee Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees;
        }

        public Employee GetEmployee(int id)
        {
           return  _context.Employees.Find(id);
           
        }

        public Employee Update(Employee employeeChanges)
        {
           var employee = _context.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return employeeChanges;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> employeeList;

        public MockEmployeeRepository()
        {
            employeeList = new List<Employee>()
            {
                new Employee(){Id = 1, Name = "Francois MUTOTO", Email = "f@fk.co.za", Department = Dept.IT},
                new Employee(){Id = 2, Name = "Malaika KOKO", Email = "mk@fk.co.za", Department = Dept.HR},
                new Employee(){Id = 3, Name = "Munya AMANI", Email = "ma@fk.co.za", Department = Dept.Electronics}
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = employeeList.Max(a => a.Id) + 1;
            employeeList.Add(employee);

            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return employeeList;
        }

        public Employee GetEmployee(int id)
        {
            Employee employee = employeeList.FirstOrDefault(x => x.Id == id);
            return employee;
        }
    }
}

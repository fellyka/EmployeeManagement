using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        List<Employee> employeeList;

        public MockEmployeeRepository()
        {
            employeeList = new List<Employee>()
            {
                new Employee(){Id = 1, Name = "Francois MUTOTO", Email = "f@fk.co.za", Department = "IT"},
                new Employee(){Id = 1, Name = "Malaika KOKO", Email = "mk@fk.co.za", Department = "HR"},
                new Employee(){Id = 1, Name = "Munya AMANI", Email = "ma@fk.co.za", Department = "ELECTRONICS"}
            };
        }

        public IEnumerable<Employee> ListAll()
        {
            return employeeList;
        }
    }
}

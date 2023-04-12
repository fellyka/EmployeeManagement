using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()       
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){Id = 1, Name = "Felly KANYIKI", Department = "IT" , Email = "fellyka@gmail.com" },
                new Employee(){Id = 2, Name = "Remy MUALU", Department = "HR" , Email = "remymua@gmail.com" },
                new Employee(){Id = 3, Name = "Helene MIRA", Department = "ACCOUNTING" , Email = "helenemi@gmail.com" },

            };
        }
        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }
    }
}

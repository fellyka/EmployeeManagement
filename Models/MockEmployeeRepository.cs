﻿using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account;
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
                new Employee(){Id = 1, Name = "Felly KANYIKI", Department = Dept.It , Email = "fellyka@gmail.com" },
                new Employee(){Id = 2, Name = "Remy MUALU", Department = Dept.Hr , Email = "remymua@gmail.com" },
                new Employee(){Id = 3, Name = "Helene MIRA", Department = Dept.Accounting , Email = "helenemi@gmail.com" },

            };
        }
        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        IEnumerable<Employee> IEmployeeRepository.GetAllEmployee()
        {
            return _employeeList;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if(employee != null)
                _employeeList.Remove(employee);
            return employee;
        }
    }
}

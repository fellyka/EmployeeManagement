
namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        /*Let create some Mock data for the Employee class*/
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){ Id = 1, Name = "Mary", Department = Dept.HR, Email = "mary@sollers.co.za"},
                new Employee(){ Id = 2, Name = "Xavier", Department = Dept.IT, Email = "xavier@sollers.co.za"},
                new Employee(){ Id = 3, Name = "John", Department = Dept.HR, Email = "john@sollers.co.za"},
                new Employee(){ Id = 4, Name = "Sara", Department = Dept.Payroll, Email = "sara@sollers.co.za"}
            };
        }

        public Employee Add(Employee employee)
        {
            /*Start by incrementing Id since we are working with an in-Memory data set*/
            employee.Id = _employeeList.Max(e => e.Id) + 1;

            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
            var employee = _employeeList.Where(e => e.Id == id).FirstOrDefault();
           // var employee = _employeeList.FirstOrDefault(e => e.Id == id);
            return employee;
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
    }
}

namespace EmployeeManagement.Models;

public class MockEmployeeRepository : IEmployeeRepository
{
    private List<Employee> _employeeList;

    public MockEmployeeRepository()
    {
        _employeeList = new List<Employee>()
        {
            new Employee() { Id = 1, Name = "Mary", Department = "HR", Email = "mary@pragimtech.com" },
            new Employee() { Id = 2, Name = "John", Department = "IT", Email = "john@pragimtech.com" },
            new Employee() { Id = 3, Name = "Sara", Department = "IT", Email = "sara@pragimtech.com" },
            new Employee() { Id = 4, Name = "Mike", Department = "Finance", Email = "mike@pragimtech.com" },
            new Employee() { Id = 5, Name = "Linda", Department = "Marketing", Email = "linda@pragimtech.com" },
            new Employee() { Id = 6, Name = "David", Department = "Sales", Email = "david@pragimtech.com" },
            new Employee() { Id = 7, Name = "Emily", Department = "HR", Email = "emily@pragimtech.com" },
            new Employee() { Id = 8, Name = "Tom", Department = "IT", Email = "tom@pragimtech.com" },
            new Employee() { Id = 9, Name = "Jane", Department = "Finance", Email = "jane@pragimtech.com" },
            new Employee() { Id = 10, Name = "Chris", Department = "Marketing", Email = "chris@pragimtech.com" }
        };
    }

    public IEnumerable<Employee> GetAllEmployees()
    {
        if (_employeeList == null)
        {
            throw new ArgumentNullException(nameof(_employeeList), "Employee list is null.");
        }
        return _employeeList;
    }//end of GetAllEmployees()

    public Employee GetEmployee(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "ID must be a positive integer.");
        }

        var employeeById = _employeeList.FirstOrDefault(e => e.Id == id);

        if (employeeById == null)
        {
            throw new KeyNotFoundException($"No employee found with ID: {id}");
        }

        return employeeById;
    }//end of GetEmployee(int id)
}
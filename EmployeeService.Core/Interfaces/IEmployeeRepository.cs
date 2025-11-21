using EmployeeService.Core.Models;
using System.Collections.Generic;

namespace EmployeeService.Core.Interfaces
{
    // Repository interface for Employee data operations (added to mock in unit tests)
    public interface IEmployeeRepository
    {
        int UpdateEmployeeEnableStatus(int id, int enable);
        List<Employee> GetAllEmployeesFlat();
    }
}

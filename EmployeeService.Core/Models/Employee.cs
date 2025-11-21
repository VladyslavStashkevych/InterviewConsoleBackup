using System.Collections.Generic;
using Newtonsoft.Json;

namespace EmployeeService.Core.Models
{
    public class Employee
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int? ManagerID { get; set; }

        public int Enable { get; set; }
        
        public List<EmployeeWrapper> Employees { get; set; } = new List<EmployeeWrapper>();
    }

    public class EmployeeWrapper
    {
        public Employee Employee { get; set; }
    }
}
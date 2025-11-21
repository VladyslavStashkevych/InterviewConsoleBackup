using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Web.UI.WebControls;
using EmployeeService.Core.Models;
using EmployeeService.Core.Interfaces;
using EmployeeService.Implementation.Infrastructure;

namespace EmployeeService.Implementation
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [EmployeeServiceBehavior]
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        // Inject the repository via constructor
        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public EmployeeWrapper GetEmployeeById(int id)
        {
            // id validation
            if (id < 0)
            {
                throw new ArgumentException("Invalid ID format.");
            }

            List<Employee> allEmployees = _repository.GetAllEmployeesFlat();
            Dictionary<int, Employee> employeeLookup = allEmployees.ToDictionary(e => e.ID);

            this.BuildEmployeeTree(allEmployees, employeeLookup);

            if (employeeLookup.TryGetValue(id, out Employee rootEmployee))
            {
                return new EmployeeWrapper { Employee = rootEmployee, };
            }

            // Employee not found
            return null;
        }

        public bool EnableEmployee(int id, int enable)
        {
            // Id validation
            if (id < 0)
            {
                throw new ArgumentException("Invalid ID.");
            }

            if (enable != 0 && enable != 1)
            {
                throw new ArgumentException("Invalid enable flag format. Use 0 or 1.");
            }

            int rowsAffected = _repository.UpdateEmployeeEnableStatus(id, enable);

            // Return true if one row was affected
            return rowsAffected == 1;
        }

        private void BuildEmployeeTree(List<Employee> employees, Dictionary<int, Employee> employeeLookup)
        {
            foreach (var employee in employees)
            {
                if (employee.ManagerID.HasValue)
                {
                    int managerId = employee.ManagerID.Value;

                    if (employeeLookup.TryGetValue(managerId, out Employee manager))
                    {
                        manager.Employees.Add(new EmployeeWrapper { Employee = employee });
                    }
                }
            }
        }


        // Extract the ADO.NET string from config file.
        private static string GetConnectionString()
        {
            string entityConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Emploee"].ConnectionString;

            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder(entityConnectionString);

            return entityBuilder.ProviderConnectionString;
        }


    }

      
}
using System.Collections.Generic;
using System.Linq;
using EmployeeService.Core.Interfaces;
using EmployeeService.Core.Models;
using EmployeeService.Implementation;
using EmployeeService.Implementation.Data;
using Moq;
using NUnit.Framework;

namespace EmployeeService.UnitTests
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        [Test]
        public void GetEmployeeById_ShouldCorrectlyBuildTreeStructure()
        {
            // ARRANGE: Set up the mock repository
            var mockRepo = new Mock<IEmployeeRepository>();

            var fakeData = new List<Employee>
            {
                new Employee { ID = 1, Name = "Boss", ManagerID = null, Enable = 1 },
                new Employee { ID = 2, Name = "Emp1", ManagerID = 1, Enable = 1 },
                new Employee { ID = 3, Name = "Emp2", ManagerID = 1, Enable = 1 }
            };

            mockRepo.Setup(r => r.GetAllEmployeesFlat()).Returns(fakeData);

            var service = new Implementation.EmployeeService(mockRepo.Object);

            // ACT
            EmployeeWrapper root = service.GetEmployeeById(1);

            // ASSERT: Check that the tree building logic worked
            Assert.IsNotNull(root);
            Assert.AreEqual(2, root.Employee.Employees.Count, "Tree builder failed to link employees.");
        }
    }
}

using EmployeeService.Core.Interfaces;
using EmployeeService.Core.Models;
using EmployeeService.IntegrationTests.Infrastructure;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace EmployeeService.IntegrationTests
{
    public static class TestSettings
    {
        public const string ServiceBaseUrl = "http://localhost:8000/EmployeeService/";
    }

    [SetUpFixture]
    public class GlobalTestSetup
    {
        private static WebServiceHost _host;
        private Mock<IEmployeeRepository> _mockRepo;

        [OneTimeSetUp]
        public void StartServiceHost()
        {
            // Set up the Mock Repository
            _mockRepo = new Mock<IEmployeeRepository>();

            var mockEmployees = new List<Employee>
            {
                new Employee { ID = 10, Name = "Mock Boss", ManagerID = null, Enable = 1 },
                new Employee { ID = 11, Name = "Mock Employee", ManagerID = 10, Enable = 1 }
            };

            _mockRepo.Setup(r => r.GetAllEmployeesFlat()).Returns(mockEmployees);
            _mockRepo.Setup(r => r.UpdateEmployeeEnableStatus(It.IsAny<int>(), It.IsAny<int>())).Returns(1);

            Uri baseAddress = new Uri(TestSettings.ServiceBaseUrl);

            _host = new WebServiceHost(typeof(Implementation.EmployeeService), baseAddress);
            _host.Description.Behaviors.Add(new MockingServiceBehavior(_mockRepo.Object));

            _host.Open();
        }

        [OneTimeTearDown]
        public void StopServiceHost()
        {
            _host?.Close();
            _host = null;
        }


    }

    [TestFixture]
    public class EndpointTests
    {
        private static readonly HttpClient _client = new HttpClient();

        // Expected URL: http://localhost:8000/EmployeeService/EnableEmployee?id=1&enable=1
        [Test]
        public async Task EnableEmployee_Post_ShouldReturnTrue()
        {
            // ARRANGE
            const int id = 1;
            const int enable = 1;
            string url = $"{TestSettings.ServiceBaseUrl}EnableEmployee?id={id}&enable={enable}";

            // ACT
            HttpResponseMessage response = await _client.PutAsync(url, null);

            // ASSERT
            Assert.IsTrue(response.IsSuccessStatusCode, $"Endpoint call failed. Status: {response.StatusCode}");
        }

        // Expected URL: http://localhost:8000/EmployeeService/GetEmployeeById?id=3
        [Test]
        public async Task GetEmployeeById_Get_ShouldReturnJsonTree()
        {
            // ARRANGE
            const int id = 10;
            string url = $"{TestSettings.ServiceBaseUrl}GetEmployeeById?id={id}";

            // ACT
            HttpResponseMessage response = await _client.GetAsync(url);

            // ASSERT
            Assert.IsTrue(response.IsSuccessStatusCode, $"GET call failed. Status: {response.StatusCode}");
            Assert.IsTrue(response.Content.Headers.ContentType.MediaType.Equals("application/json"), "Response should be JSON.");

            string jsonResult = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(jsonResult.Contains($"\"ID\":{id}"), "Response JSON does not contain the expected employee ID.");
        }


    }


}
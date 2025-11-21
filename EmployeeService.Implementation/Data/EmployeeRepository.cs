using System.Collections.Generic;
using System.Data.SqlClient;
using EmployeeService.Core.Interfaces;
using EmployeeService.Core.Models;
using EmployeeService.Implementation.Infrastructure;

namespace EmployeeService.Implementation.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository()
        {
            // Get the extracted ADO.NET connection string
            _connectionString = ConnectionHelper.GetAdoNetConnectionString();
        }

        public int UpdateEmployeeEnableStatus(int id, int enable)
        {
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Use the parameterized UPDATE query
                string sql = "UPDATE Employee SET Enable = @Enable WHERE ID = @ID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Enable", enable);
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }

        public List<Employee> GetAllEmployeesFlat()
        {
            var employees = new List<Employee>();

            string sql = "SELECT ID, Name, ManagerID, Enable FROM Employee";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var employee = new Employee
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Enable = reader.GetInt32(reader.GetOrdinal("Enable"))
                        };

                        // Handle the nullable ManagerID column
                        if (!reader.IsDBNull(reader.GetOrdinal("ManagerID")))
                        {
                            employee.ManagerID = reader.GetInt32(reader.GetOrdinal("ManagerID"));
                        }

                        employees.Add(employee);
                    }
                }
            }

            return employees;
        }


    }


}
using System;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;

namespace EmployeeService.Implementation.Infrastructure
{
    // Helper class to extract ADO.NET connection string from Entity Framework connection string
    public static class ConnectionHelper
    {
        private const string EntityConnectionStringName = "Emploee";

        public static string GetAdoNetConnectionString()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[EntityConnectionStringName];

            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
            {
                throw new InvalidOperationException($"Connection string '{EntityConnectionStringName}' not found in web.config.");
            }

            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(settings.ConnectionString);

            // Return connection string for ADO.NET
            return entityBuilder.ProviderConnectionString;
        }


    }


}
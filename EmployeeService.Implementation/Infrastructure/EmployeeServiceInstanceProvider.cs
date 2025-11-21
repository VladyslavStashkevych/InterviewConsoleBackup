using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using EmployeeService.Core.Interfaces;
using EmployeeService.Implementation.Data;

namespace EmployeeService.Implementation.Infrastructure
{
    // Custom provider to register EmployeeRepository
    public class EmployeeServiceInstanceProvider : IInstanceProvider
    {
        private readonly Type _serviceType;

        public EmployeeServiceInstanceProvider(Type serviceType)
        {
            _serviceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            IEmployeeRepository repository = new EmployeeRepository();

            return Activator.CreateInstance(_serviceType, repository);
        }

        public object GetInstance(InstanceContext instanceContext, Message message) => GetInstance(instanceContext);
        public void ReleaseInstance(InstanceContext instanceContext, object instance) { }


    }


}
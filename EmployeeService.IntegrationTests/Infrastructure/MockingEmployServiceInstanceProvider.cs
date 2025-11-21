using EmployeeService.Core.Interfaces;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace EmployeeService.IntegrationTests.Infrastructure
{
    public class MockingEmployeeServiceInstanceProvider : IInstanceProvider
    {
        private readonly Type _serviceType;
        private readonly IEmployeeRepository _mockRepository;

        public MockingEmployeeServiceInstanceProvider(Type serviceType, IEmployeeRepository mockRepository)
        {
            _serviceType = serviceType;
            _mockRepository = mockRepository;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return Activator.CreateInstance(_serviceType, _mockRepository);
        }

        public object GetInstance(InstanceContext instanceContext, Message message) => GetInstance(instanceContext);

        public void ReleaseInstance(InstanceContext instanceContext, object instance) { }


    }


}

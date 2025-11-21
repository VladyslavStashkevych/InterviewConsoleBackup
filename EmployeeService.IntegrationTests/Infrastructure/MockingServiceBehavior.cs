using EmployeeService.Core.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace EmployeeService.IntegrationTests.Infrastructure
{
    // Custom behavior for mocking purposes
    public class MockingServiceBehavior : IServiceBehavior
    {
        private readonly IEmployeeRepository _mockRepository;

        public MockingServiceBehavior(IEmployeeRepository mockRepository)
        {
            _mockRepository = mockRepository;
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                {
                    endpointDispatcher.DispatchRuntime.InstanceProvider =
                        new MockingEmployeeServiceInstanceProvider(serviceDescription.ServiceType, _mockRepository);
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }


    }


}

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace EmployeeService.Implementation.Infrastructure
{
    // Custom behavior to add EmployeeServiceInstanceProvider to the service
    [AttributeUsage(AttributeTargets.Class)]
    public class EmployeeServiceBehavior : Attribute, IServiceBehavior
    {
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                {
                    endpointDispatcher.DispatchRuntime.InstanceProvider =
                        new EmployeeServiceInstanceProvider(serviceDescription.ServiceType);
                }
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }


    }


}
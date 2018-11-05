﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JIC.Prosecution.Service.ServiceBus.FaultService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="FaultService.IFaultCourtService")]
    public interface IFaultCourtService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFaultCourtService/AddNewCase", ReplyAction="http://tempuri.org/IFaultCourtService/AddNewCaseResponse")]
        JIC.Base.Views.Services.Response AddNewCase(JIC.Base.Views.Services.NewCase Case);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFaultCourtService/AddNewCase", ReplyAction="http://tempuri.org/IFaultCourtService/AddNewCaseResponse")]
        System.Threading.Tasks.Task<JIC.Base.Views.Services.Response> AddNewCaseAsync(JIC.Base.Views.Services.NewCase Case);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFaultCourtService/AddCompleteCase", ReplyAction="http://tempuri.org/IFaultCourtService/AddCompleteCaseResponse")]
        JIC.Base.Views.Services.Response AddCompleteCase(JIC.Base.Views.Services.CompleteCase Case);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFaultCourtService/AddCompleteCase", ReplyAction="http://tempuri.org/IFaultCourtService/AddCompleteCaseResponse")]
        System.Threading.Tasks.Task<JIC.Base.Views.Services.Response> AddCompleteCaseAsync(JIC.Base.Views.Services.CompleteCase Case);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFaultCourtService/RequestObjection", ReplyAction="http://tempuri.org/IFaultCourtService/RequestObjectionResponse")]
        JIC.Base.Views.Services.Response RequestObjection(JIC.Base.Views.Services.ObjectionRequest objectionRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFaultCourtService/RequestObjection", ReplyAction="http://tempuri.org/IFaultCourtService/RequestObjectionResponse")]
        System.Threading.Tasks.Task<JIC.Base.Views.Services.Response> RequestObjectionAsync(JIC.Base.Views.Services.ObjectionRequest objectionRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFaultCourtService/RequestResumption", ReplyAction="http://tempuri.org/IFaultCourtService/RequestResumptionResponse")]
        JIC.Base.Views.Services.Response RequestResumption(JIC.Base.Views.Services.ResumptionRequest resumptionRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFaultCourtService/RequestResumption", ReplyAction="http://tempuri.org/IFaultCourtService/RequestResumptionResponse")]
        System.Threading.Tasks.Task<JIC.Base.Views.Services.Response> RequestResumptionAsync(JIC.Base.Views.Services.ResumptionRequest resumptionRequest);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFaultCourtServiceChannel : JIC.Prosecution.Service.ServiceBus.FaultService.IFaultCourtService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FaultCourtServiceClient : System.ServiceModel.ClientBase<JIC.Prosecution.Service.ServiceBus.FaultService.IFaultCourtService>, JIC.Prosecution.Service.ServiceBus.FaultService.IFaultCourtService {
        
        public FaultCourtServiceClient() {
        }
        
        public FaultCourtServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FaultCourtServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FaultCourtServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FaultCourtServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public JIC.Base.Views.Services.Response AddNewCase(JIC.Base.Views.Services.NewCase Case) {
            return base.Channel.AddNewCase(Case);
        }
        
        public System.Threading.Tasks.Task<JIC.Base.Views.Services.Response> AddNewCaseAsync(JIC.Base.Views.Services.NewCase Case) {
            return base.Channel.AddNewCaseAsync(Case);
        }
        
        public JIC.Base.Views.Services.Response AddCompleteCase(JIC.Base.Views.Services.CompleteCase Case) {
            return base.Channel.AddCompleteCase(Case);
        }
        
        public System.Threading.Tasks.Task<JIC.Base.Views.Services.Response> AddCompleteCaseAsync(JIC.Base.Views.Services.CompleteCase Case) {
            return base.Channel.AddCompleteCaseAsync(Case);
        }
        
        public JIC.Base.Views.Services.Response RequestObjection(JIC.Base.Views.Services.ObjectionRequest objectionRequest) {
            return base.Channel.RequestObjection(objectionRequest);
        }
        
        public System.Threading.Tasks.Task<JIC.Base.Views.Services.Response> RequestObjectionAsync(JIC.Base.Views.Services.ObjectionRequest objectionRequest) {
            return base.Channel.RequestObjectionAsync(objectionRequest);
        }
        
        public JIC.Base.Views.Services.Response RequestResumption(JIC.Base.Views.Services.ResumptionRequest resumptionRequest) {
            return base.Channel.RequestResumption(resumptionRequest);
        }
        
        public System.Threading.Tasks.Task<JIC.Base.Views.Services.Response> RequestResumptionAsync(JIC.Base.Views.Services.ResumptionRequest resumptionRequest) {
            return base.Channel.RequestResumptionAsync(resumptionRequest);
        }
    }
}
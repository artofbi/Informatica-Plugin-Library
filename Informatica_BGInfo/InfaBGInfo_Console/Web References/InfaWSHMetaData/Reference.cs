﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3603
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.3603.
// 
#pragma warning disable 1591

namespace InfaBGInfo_Console.InfaWSHMetaData {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="MetadataServiceSoapBinding", Namespace="http://www.informatica.com/wsh")]
    public partial class MetadataService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private SessionHeader contextField;
        
        private System.Threading.SendOrPostCallback loginOperationCompleted;
        
        private System.Threading.SendOrPostCallback logoutOperationCompleted;
        
        private System.Threading.SendOrPostCallback getAllFoldersOperationCompleted;
        
        private System.Threading.SendOrPostCallback getAllWorkflowsOperationCompleted;
        
        private System.Threading.SendOrPostCallback getAllTaskInstancesOperationCompleted;
        
        private System.Threading.SendOrPostCallback getAllDIServersOperationCompleted;
        
        private System.Threading.SendOrPostCallback getAllRepositoriesOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public MetadataService() {
            this.Url = global::InfaBGInfo_Console.Properties.Settings.Default.InfaBGInfo_Console_InfaWSHMetaData_MetadataService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public SessionHeader Context {
            get {
                return this.contextField;
            }
            set {
                this.contextField = value;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event loginCompletedEventHandler loginCompleted;
        
        /// <remarks/>
        public event logoutCompletedEventHandler logoutCompleted;
        
        /// <remarks/>
        public event getAllFoldersCompletedEventHandler getAllFoldersCompleted;
        
        /// <remarks/>
        public event getAllWorkflowsCompletedEventHandler getAllWorkflowsCompleted;
        
        /// <remarks/>
        public event getAllTaskInstancesCompletedEventHandler getAllTaskInstancesCompleted;
        
        /// <remarks/>
        public event getAllDIServersCompletedEventHandler getAllDIServersCompleted;
        
        /// <remarks/>
        public event getAllRepositoriesCompletedEventHandler getAllRepositoriesCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("Context", Direction=System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("LoginReturn", Namespace="http://www.informatica.com/wsh")]
        public string login([System.Xml.Serialization.XmlElementAttribute("Login", Namespace="http://www.informatica.com/wsh")] LoginRequest Login1) {
            object[] results = this.Invoke("login", new object[] {
                        Login1});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void loginAsync(LoginRequest Login1) {
            this.loginAsync(Login1, null);
        }
        
        /// <remarks/>
        public void loginAsync(LoginRequest Login1, object userState) {
            if ((this.loginOperationCompleted == null)) {
                this.loginOperationCompleted = new System.Threading.SendOrPostCallback(this.OnloginOperationCompleted);
            }
            this.InvokeAsync("login", new object[] {
                        Login1}, this.loginOperationCompleted, userState);
        }
        
        private void OnloginOperationCompleted(object arg) {
            if ((this.loginCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.loginCompleted(this, new loginCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("Context")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("LogoutReturn", Namespace="http://www.informatica.com/wsh")]
        public VoidResponse logout([System.Xml.Serialization.XmlElementAttribute("Logout", Namespace="http://www.informatica.com/wsh")] VoidRequest Logout1) {
            object[] results = this.Invoke("logout", new object[] {
                        Logout1});
            return ((VoidResponse)(results[0]));
        }
        
        /// <remarks/>
        public void logoutAsync(VoidRequest Logout1) {
            this.logoutAsync(Logout1, null);
        }
        
        /// <remarks/>
        public void logoutAsync(VoidRequest Logout1, object userState) {
            if ((this.logoutOperationCompleted == null)) {
                this.logoutOperationCompleted = new System.Threading.SendOrPostCallback(this.OnlogoutOperationCompleted);
            }
            this.InvokeAsync("logout", new object[] {
                        Logout1}, this.logoutOperationCompleted, userState);
        }
        
        private void OnlogoutOperationCompleted(object arg) {
            if ((this.logoutCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.logoutCompleted(this, new logoutCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("Context")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlArrayAttribute("GetAllFoldersReturn", Namespace="http://www.informatica.com/wsh")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public FolderInfo[] getAllFolders([System.Xml.Serialization.XmlElementAttribute("GetAllFolders", Namespace="http://www.informatica.com/wsh", IsNullable=true)] VoidRequest GetAllFolders1) {
            object[] results = this.Invoke("getAllFolders", new object[] {
                        GetAllFolders1});
            return ((FolderInfo[])(results[0]));
        }
        
        /// <remarks/>
        public void getAllFoldersAsync(VoidRequest GetAllFolders1) {
            this.getAllFoldersAsync(GetAllFolders1, null);
        }
        
        /// <remarks/>
        public void getAllFoldersAsync(VoidRequest GetAllFolders1, object userState) {
            if ((this.getAllFoldersOperationCompleted == null)) {
                this.getAllFoldersOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAllFoldersOperationCompleted);
            }
            this.InvokeAsync("getAllFolders", new object[] {
                        GetAllFolders1}, this.getAllFoldersOperationCompleted, userState);
        }
        
        private void OngetAllFoldersOperationCompleted(object arg) {
            if ((this.getAllFoldersCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAllFoldersCompleted(this, new getAllFoldersCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("Context")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlArrayAttribute("GetAllWorkflowsReturn", Namespace="http://www.informatica.com/wsh")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public WorkflowInfo[] getAllWorkflows([System.Xml.Serialization.XmlElementAttribute("GetAllWorkflows", Namespace="http://www.informatica.com/wsh")] FolderInfo GetAllWorkflows1) {
            object[] results = this.Invoke("getAllWorkflows", new object[] {
                        GetAllWorkflows1});
            return ((WorkflowInfo[])(results[0]));
        }
        
        /// <remarks/>
        public void getAllWorkflowsAsync(FolderInfo GetAllWorkflows1) {
            this.getAllWorkflowsAsync(GetAllWorkflows1, null);
        }
        
        /// <remarks/>
        public void getAllWorkflowsAsync(FolderInfo GetAllWorkflows1, object userState) {
            if ((this.getAllWorkflowsOperationCompleted == null)) {
                this.getAllWorkflowsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAllWorkflowsOperationCompleted);
            }
            this.InvokeAsync("getAllWorkflows", new object[] {
                        GetAllWorkflows1}, this.getAllWorkflowsOperationCompleted, userState);
        }
        
        private void OngetAllWorkflowsOperationCompleted(object arg) {
            if ((this.getAllWorkflowsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAllWorkflowsCompleted(this, new getAllWorkflowsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("Context")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlArrayAttribute("GetAllTaskInstancesReturn", Namespace="http://www.informatica.com/wsh")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public TaskInstanceInfo[] getAllTaskInstances([System.Xml.Serialization.XmlElementAttribute("GetAllTaskInstances", Namespace="http://www.informatica.com/wsh")] GetAllTaskInstancesRequest GetAllTaskInstances1) {
            object[] results = this.Invoke("getAllTaskInstances", new object[] {
                        GetAllTaskInstances1});
            return ((TaskInstanceInfo[])(results[0]));
        }
        
        /// <remarks/>
        public void getAllTaskInstancesAsync(GetAllTaskInstancesRequest GetAllTaskInstances1) {
            this.getAllTaskInstancesAsync(GetAllTaskInstances1, null);
        }
        
        /// <remarks/>
        public void getAllTaskInstancesAsync(GetAllTaskInstancesRequest GetAllTaskInstances1, object userState) {
            if ((this.getAllTaskInstancesOperationCompleted == null)) {
                this.getAllTaskInstancesOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAllTaskInstancesOperationCompleted);
            }
            this.InvokeAsync("getAllTaskInstances", new object[] {
                        GetAllTaskInstances1}, this.getAllTaskInstancesOperationCompleted, userState);
        }
        
        private void OngetAllTaskInstancesOperationCompleted(object arg) {
            if ((this.getAllTaskInstancesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAllTaskInstancesCompleted(this, new getAllTaskInstancesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("Context")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlArrayAttribute("GetAllDIServersReturn", Namespace="http://www.informatica.com/wsh")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public DIServerInfo[] getAllDIServers([System.Xml.Serialization.XmlElementAttribute("GetAllDIServers", Namespace="http://www.informatica.com/wsh", IsNullable=true)] VoidRequest GetAllDIServers1) {
            object[] results = this.Invoke("getAllDIServers", new object[] {
                        GetAllDIServers1});
            return ((DIServerInfo[])(results[0]));
        }
        
        /// <remarks/>
        public void getAllDIServersAsync(VoidRequest GetAllDIServers1) {
            this.getAllDIServersAsync(GetAllDIServers1, null);
        }
        
        /// <remarks/>
        public void getAllDIServersAsync(VoidRequest GetAllDIServers1, object userState) {
            if ((this.getAllDIServersOperationCompleted == null)) {
                this.getAllDIServersOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAllDIServersOperationCompleted);
            }
            this.InvokeAsync("getAllDIServers", new object[] {
                        GetAllDIServers1}, this.getAllDIServersOperationCompleted, userState);
        }
        
        private void OngetAllDIServersOperationCompleted(object arg) {
            if ((this.getAllDIServersCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAllDIServersCompleted(this, new getAllDIServersCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlArrayAttribute("GetAllRepositoriesReturn", Namespace="http://www.informatica.com/wsh")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public RepositoryInfo[] getAllRepositories([System.Xml.Serialization.XmlElementAttribute("GetAllRepositories", Namespace="http://www.informatica.com/wsh", IsNullable=true)] VoidRequest GetAllRepositories1) {
            object[] results = this.Invoke("getAllRepositories", new object[] {
                        GetAllRepositories1});
            return ((RepositoryInfo[])(results[0]));
        }
        
        /// <remarks/>
        public void getAllRepositoriesAsync(VoidRequest GetAllRepositories1) {
            this.getAllRepositoriesAsync(GetAllRepositories1, null);
        }
        
        /// <remarks/>
        public void getAllRepositoriesAsync(VoidRequest GetAllRepositories1, object userState) {
            if ((this.getAllRepositoriesOperationCompleted == null)) {
                this.getAllRepositoriesOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAllRepositoriesOperationCompleted);
            }
            this.InvokeAsync("getAllRepositories", new object[] {
                        GetAllRepositories1}, this.getAllRepositoriesOperationCompleted, userState);
        }
        
        private void OngetAllRepositoriesOperationCompleted(object arg) {
            if ((this.getAllRepositoriesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAllRepositoriesCompleted(this, new getAllRepositoriesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.informatica.com/wsh")]
    [System.Xml.Serialization.XmlRootAttribute("Context", Namespace="http://www.informatica.com/wsh", IsNullable=false)]
    public partial class SessionHeader : System.Web.Services.Protocols.SoapHeader {
        
        private string sessionIdField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SessionId {
            get {
                return this.sessionIdField;
            }
            set {
                this.sessionIdField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.informatica.com/wsh")]
    public partial class RepositoryInfo {
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.informatica.com/wsh")]
    public partial class DIServerInfo {
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.informatica.com/wsh")]
    public partial class TaskInstanceInfo {
        
        private string nameField;
        
        private string typeField;
        
        private TaskInstanceInfo[] childTaskField;
        
        private bool isValidField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ChildTask", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TaskInstanceInfo[] ChildTask {
            get {
                return this.childTaskField;
            }
            set {
                this.childTaskField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool IsValid {
            get {
                return this.isValidField;
            }
            set {
                this.isValidField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.informatica.com/wsh")]
    public partial class GetAllTaskInstancesRequest {
        
        private WorkflowInfo workflowInfoField;
        
        private int depthField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public WorkflowInfo WorkflowInfo {
            get {
                return this.workflowInfoField;
            }
            set {
                this.workflowInfoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int Depth {
            get {
                return this.depthField;
            }
            set {
                this.depthField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.informatica.com/wsh")]
    public partial class WorkflowInfo {
        
        private string nameField;
        
        private bool isValidField;
        
        private string folderNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool IsValid {
            get {
                return this.isValidField;
            }
            set {
                this.isValidField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FolderName {
            get {
                return this.folderNameField;
            }
            set {
                this.folderNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.informatica.com/wsh")]
    public partial class FolderInfo {
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.informatica.com/wsh")]
    public partial class VoidResponse {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.informatica.com/wsh")]
    public partial class VoidRequest {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.informatica.com/wsh")]
    public partial class LoginRequest {
        
        private string repositoryDomainNameField;
        
        private string repositoryNameField;
        
        private string userNameField;
        
        private string passwordField;
        
        private string userNameSpaceField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public string RepositoryDomainName {
            get {
                return this.repositoryDomainNameField;
            }
            set {
                this.repositoryDomainNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string RepositoryName {
            get {
                return this.repositoryNameField;
            }
            set {
                this.repositoryNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UserName {
            get {
                return this.userNameField;
            }
            set {
                this.userNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public string UserNameSpace {
            get {
                return this.userNameSpaceField;
            }
            set {
                this.userNameSpaceField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void loginCompletedEventHandler(object sender, loginCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class loginCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal loginCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void logoutCompletedEventHandler(object sender, logoutCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class logoutCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal logoutCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public VoidResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((VoidResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void getAllFoldersCompletedEventHandler(object sender, getAllFoldersCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAllFoldersCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAllFoldersCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public FolderInfo[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((FolderInfo[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void getAllWorkflowsCompletedEventHandler(object sender, getAllWorkflowsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAllWorkflowsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAllWorkflowsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public WorkflowInfo[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((WorkflowInfo[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void getAllTaskInstancesCompletedEventHandler(object sender, getAllTaskInstancesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAllTaskInstancesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAllTaskInstancesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TaskInstanceInfo[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TaskInstanceInfo[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void getAllDIServersCompletedEventHandler(object sender, getAllDIServersCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAllDIServersCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAllDIServersCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public DIServerInfo[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((DIServerInfo[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void getAllRepositoriesCompletedEventHandler(object sender, getAllRepositoriesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAllRepositoriesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAllRepositoriesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RepositoryInfo[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RepositoryInfo[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591
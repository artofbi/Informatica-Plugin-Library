using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;

using System.IO;
using System.Net;
using System.Web.Services.Protocols;
using System.Xml;

//using InfaBGInfo.InfaWSHDataIntegration;
//using InfaWSHMetaData;

using System.Security.Cryptography.X509Certificates;

namespace InfaBGInfo
{

    // This class is used to indicate not to authenticate the certificate
    // sent by Web Services Hub while using https
    class NoAuthCertPolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint sp, X509Certificate cert,
                            WebRequest request, int problem)
        {
            return true;
        }
    }

    public struct ShowOutputInfoOptions
    {
        public bool showOSInformation;
        public bool showWSHRepositoriesAvailable;
        public bool showAllRepositoryFolders;
        public bool showGlobalRepositoryWorkflowInfo;
        public bool showRepositoryAssociatedIntSvc;
    }

    public class InfaWSHMetaDataInfo
    {
        #region Private Members

        // WSH host name
        public static string WSH_HOST_NAME = null;
        // WSH port number
        public static string WSH_PORT_NUM = null;

        // Service endpoint URL for Metadata web service
        public static string MWS_URL = null;
        // Service endpoint URL for DataIntegration web service
        public static string DIWS_URL = null;

        // Domain name of the repository
        public static string REPO_DOMAIN_NAME = null;
        // Repositry name used for login
        public static string REPO_NAME = null;
        // User name used for login
        public static string USER_NAME = null;
        // Password used for login
        public static string PASSWORD = null;
        // Domain name of the DI Service
        public static string DI_DOMAIN_NAME = null;
        // DI Server name used for DataIntegration Web Service
        public static string DI_SERVICE_NAME = null;

        // ping time out in seconds used in PingDIServer
        public static int PING_TIME_OUT = 30;

        private static string URL_PROTOCOL = "http://";

        // extras
        public static bool isHTTPSMode = false;
        public bool isLoginSuccessful = false;

        public int numOfFoldersInRepository = 0;
        public int numOfRepositoriesAvailableinWSH = 0;
        public int numOfIntSvcAttachedToRepository = 0;

        public string strIntSvcPerRepository = string.Empty;
        public int numOfGlobalWorkflows = 0;
        public int numOfGlobalInvalidWorkflows = 0;
        public int numOfGlobalValidWorkflows = 0;

        public string strDefaultURL_MetaData = "http://<WSH_Server>:7333/wsh/services/BatchServices/Metadata";
        public string strDefaultURL_DataIntegration = "http://<WSH_Server>:7333/wsh/services/BatchServices/DataIntegration";


        // declares proxy object for Meta Data Web Service
        private static InfaWSHMetaData.MetadataService MWSProxy;
        // declares proxy object for Load Manager Web Service
        private static InfaBGInfo.InfaWSHDataIntegration.DataIntegrationService DIWSProxy;

        //public static StringBuilder InfaStringRecordsValues = new StringBuilder();

        #endregion

        // Default Constructor
        public InfaWSHMetaDataInfo() { }
        public InfaWSHMetaDataInfo(string wshHostName, int wshPort, string altFullURL, string repDomain
            , string repName, string repUsername, string repPassword
            , string isvcDomain, string isvcName) {
                WSH_HOST_NAME = wshHostName;
                WSH_PORT_NUM = wshPort.ToString();
                REPO_DOMAIN_NAME = repDomain;
                REPO_NAME = repName;
                USER_NAME = repUsername;
                PASSWORD = repPassword;
                DI_DOMAIN_NAME = isvcDomain;
                DI_SERVICE_NAME = isvcName;

                #region Check HTTPS Mode
                if (isHTTPSMode)
                {
                    URL_PROTOCOL = "https://";
                    // set the certificate policy which does not authenticate
                    //ServicePointManager.CertificatePolicy = new NoAuthCertPolicy();
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return false; };
                }
                #endregion

                MWS_URL = URL_PROTOCOL + WSH_HOST_NAME + ":" + WSH_PORT_NUM + "/wsh/services/BatchServices/Metadata";

                DIWS_URL = URL_PROTOCOL + WSH_HOST_NAME + ":" + WSH_PORT_NUM + "/wsh/services/BatchServices/DataIntegration";


                // run the initials for SDK/API to WSH, login, etc.
                initWSH();
        }


        private void initWSH()
        {
            try
            {
                // create proxy object for Metadata Web Service
                MWSProxy = new InfaWSHMetaData.MetadataService();

                if (MWSProxy == null)
                    throw new NullReferenceException("Initializing : MetaData Service NOT Exploitable");

                MWSProxy.Url = MWS_URL;

                InfaWSHMetaData.LoginRequest loginReq
                    = new InfaWSHMetaData.LoginRequest();

                loginReq.RepositoryDomainName = REPO_DOMAIN_NAME;
                loginReq.RepositoryName = REPO_NAME;
                loginReq.UserName = USER_NAME;
                loginReq.Password = PASSWORD;

                // get all important sessionID
                String sessID = MWSProxy.login(loginReq);

                // set header with sessionID
                MWSProxy.Context = new InfaWSHMetaData.SessionHeader();
                MWSProxy.Context.SessionId = sessID;

                // create proxy object for DataIntegration Web Service
                DIWSProxy = new InfaBGInfo.InfaWSHDataIntegration.DataIntegrationService();
                if (DIWSProxy == null)
                    throw new NullReferenceException("Initializing : DataIntegration Service(WSH) NOT Exploitable");

                // pass in the DI URL and use the same session ID
                DIWSProxy.Url = DIWS_URL;
                DIWSProxy.Context = new InfaBGInfo.InfaWSHDataIntegration.SessionHeader();
                DIWSProxy.Context.SessionId = sessID;

                // set login as successful
                isLoginSuccessful = true;
            }
            catch (SoapException e)
            {
                throw new Exception("SOAP Exception Occurred:\nError - Most likely the incorrect Domain or Repository was entered.\n" 
                    + e.Message.ToString());
            }
            catch (System.Exception e)
            {
                throw e;
            }
            
        }


        public StringBuilder GatherStringDataValues(ShowOutputInfoOptions okDisplay)
        {
            // holds count of invalid workflows when assessing workflows in a single folder
            int invalidWorkflowsInFolder = 0;
            string tmpOutputHeader = "Informatica Repository Information - " + REPO_NAME.ToUpper();

            StringBuilder InfaStringRecordsValues = new StringBuilder();

            // Provide some type of header
            InfaStringRecordsValues.AppendLine(tmpOutputHeader);
            InfaStringRecordsValues.AppendLine("".PadRight(tmpOutputHeader.Length, '#'));
            InfaStringRecordsValues.AppendLine("");


            // List all repositories available to hit from WSH
            if (okDisplay.showWSHRepositoriesAvailable)
            {
                InfaStringRecordsValues.AppendLine("WSH Assigned Repositories:");
                foreach (string s in GetAllRepositoriesAvailableInWSH())
                {
                    InfaStringRecordsValues.AppendLine("\t" + s);
                }
                InfaStringRecordsValues.AppendLine("");
            }

            if (okDisplay.showAllRepositoryFolders)
            {
                InfaStringRecordsValues.AppendLine("Repository Folders:");
                foreach (string s in GetAllFoldersInRepository())
                {
                    int tmpNumWorkflowsInFolder = GetNumberOfWorkflowsInFolder(s, out invalidWorkflowsInFolder);

                    InfaStringRecordsValues.AppendLine("\t" + s
                        + " (Workflows: " + tmpNumWorkflowsInFolder.ToString() + ")");

                    numOfGlobalWorkflows += tmpNumWorkflowsInFolder;

                    if (invalidWorkflowsInFolder > 0)
                    {
                        numOfGlobalInvalidWorkflows++;
                    }

                    // reset value
                    invalidWorkflowsInFolder = 0;
                }
                InfaStringRecordsValues.AppendLine("");
            }


            if (okDisplay.showGlobalRepositoryWorkflowInfo)
            {
                // get total valid, invalid, and global count of workflows for the repository
                numOfGlobalValidWorkflows = numOfGlobalWorkflows - numOfGlobalInvalidWorkflows;
                InfaStringRecordsValues.AppendLine("Repository Workflows:");
                InfaStringRecordsValues.AppendLine("\tTotal Valid: ".PadRight(16, ' ') + numOfGlobalValidWorkflows);
                InfaStringRecordsValues.AppendLine("\tTotal Invalid: ".PadRight(16, ' ') + numOfGlobalInvalidWorkflows);
                InfaStringRecordsValues.AppendLine("");
            }


            if (okDisplay.showRepositoryAssociatedIntSvc)
            {
                InfaStringRecordsValues.AppendLine("Associated Integration Services: ");
                foreach (string s in GetAllIntegrationServicesPerRepository())
                {
                    InfaStringRecordsValues.AppendLine("\t" + s
                        + (GetIntegrationServicePingResult(s) ? " (Enabled)" : " (Disabled)"));
                }
                InfaStringRecordsValues.AppendLine("");
            }

            return InfaStringRecordsValues;
        }


        public void logoutWSH()
        {
            MWSProxy.logout(null);
        }


        public ArrayList GetAllFoldersInRepository()
        {
            ArrayList tmpReturn = new ArrayList();
            
            // create bogus void request variable
            InfaWSHMetaData.VoidRequest voidReq
                    = new InfaWSHMetaData.VoidRequest();
            
            // Hit WSH to pull down the rep folders
            InfaWSHMetaData.FolderInfo[] folders 
                = MWSProxy.getAllFolders(voidReq);

            // print name of repositories availabe
            if (folders != null)
            {
                int i = 0;
                for (i = 0; i < folders.Length; i++)
                {
                    tmpReturn.Add(folders[i].Name);
                }

                numOfFoldersInRepository = i;
            }
            else
            {
                tmpReturn.Add("No Folders are Present in this Repository.");
            }

            return tmpReturn;
        }


        public ArrayList GetAllRepositoriesAvailableInWSH()
        {
            ArrayList tmpReturn = new ArrayList();

            // create bogus void request variable
            InfaWSHMetaData.VoidRequest voidReq
                    = new InfaWSHMetaData.VoidRequest();

            // Hit WSH to pull down the int svc servers
            InfaWSHMetaData.RepositoryInfo[] repInfo
                    = MWSProxy.getAllRepositories(voidReq);

            // print name of repositories availabe
            if (repInfo != null)
            {
                int i = 0;
                for (i = 0; i < repInfo.Length; i++)
                {
                    tmpReturn.Add(repInfo[i].Name);
                }
                numOfRepositoriesAvailableinWSH = i;
            }
            else
            {
                tmpReturn.Add("No Repositories Configured in Web Service Hub.");
            }

            return tmpReturn;
        }


        public ArrayList GetAllIntegrationServicesPerRepository()
        {
            ArrayList tmpReturn = new ArrayList();

            // create bogus void request variable
            InfaWSHMetaData.VoidRequest voidReq
                    = new InfaWSHMetaData.VoidRequest();

            // Hit WSH to pull down the int svc servers
            InfaWSHMetaData.DIServerInfo[] servers
                    = MWSProxy.getAllDIServers(voidReq);

            // print names of DI servers
            if (servers != null)
            {
                int i = 0;
                for (i = 0; i < servers.Length; i++)
                {
                    tmpReturn.Add(servers[i].Name);
                }
                numOfIntSvcAttachedToRepository = i;
            }
            else
            {
                tmpReturn.Add("No Integration Services Registered with this Repository.");
            }

            return tmpReturn;
        }


        public bool GetIntegrationServicePingResult(string intsvc)
        {
            bool tmpReturn = false;

            // instantiate pinger object
            InfaWSHDataIntegration.PingDIServerRequest pingReq 
                = new InfaWSHDataIntegration.PingDIServerRequest();

            // set timeout
            pingReq.TimeOut = (PING_TIME_OUT);

            // provide DI WSH Service connection info.  
            // We should already be logged but now provide domain and service name info
            InfaWSHDataIntegration.DIServiceInfo diInfo = new InfaWSHDataIntegration.DIServiceInfo();
            diInfo.DomainName = DI_DOMAIN_NAME;
            diInfo.ServiceName = intsvc;

            pingReq.DIServiceInfo = diInfo;

            // Conduct the actual ping based on the pingRequest using the WSDL Proxy Object
            InfaWSHDataIntegration.EPingState pingResult = DIWSProxy.pingDIServer(pingReq);

            // determine if ping provided success or not
            if (pingResult == InfaWSHDataIntegration.EPingState.ALIVE)
                tmpReturn = true;


            return tmpReturn;
        }


        public string[] GetIntegrationServiceProperties(string intsvc)
        {
            string[] tmpReturn = new string[6];

            InfaWSHDataIntegration.DIServiceInfo diInfo 
                = new InfaWSHDataIntegration.DIServiceInfo();
            diInfo.DomainName = DI_DOMAIN_NAME;
            diInfo.ServiceName = intsvc;

            InfaWSHDataIntegration.DIServerProperties serverProp 
                = DIWSProxy.getDIServerProperties(diInfo);

            tmpReturn[0] = intsvc;
            tmpReturn[1] = serverProp.DIServerName;
            tmpReturn[2] = serverProp.DIServerVersion;
            tmpReturn[3] = serverProp.ProductName;
            tmpReturn[4] = serverProp.DIServerMode.Value.ToString();
            tmpReturn[5] = serverProp.CanInfaServerDebugMapping.ToString();
            tmpReturn[6] = serverProp.CurrentTime.ToString();

            #region Detail to what the server properties mean

            //Console.WriteLine("[Output] : DI Server properties are as follows...");
            //Console.WriteLine("[Output] : DI Server name : " + serverProp.DIServerName);
            //Console.WriteLine("[Output] : DI Server version : " + serverProp.DIServerVersion);
            //Console.WriteLine("[Output] : DI Server product name : " + serverProp.ProductName);
            //Console.WriteLine("[Output] : DI Server mode : " + serverProp.DIServerMode);
            //Console.WriteLine("[Output] : Can DI Server debug mapping : " + serverProp.CanInfaServerDebugMapping);
            //Console.WriteLine("[Output] : DI Server time");
            //InfaWSHDataIntegration.DIServerDate date = serverProp.CurrentTime;
            //Console.WriteLine("[Output] : date : " + date.Date);
            //Console.WriteLine("[Output] : hours : " + date.Hours);
            //Console.WriteLine("[Output] : minutes : " + date.Minutes);
            //Console.WriteLine("[Output] : month : " + date.Month);
            //Console.WriteLine("[Output] : nano seconds : " + date.NanoSeconds);
            //Console.WriteLine("[Output] : seconds : " + date.Seconds);
            //Console.WriteLine("[Output] : UTC time : " + date.UTCTime);
            //Console.WriteLine("[Output] : year : " + date.Year); 
            #endregion

            return tmpReturn;
        }


        public ArrayList GetWorkflowsInFolder(string folderName, out int invalidCount)
        {
            ArrayList tmpReturn = new ArrayList();

            // create bogus void request variable
            InfaWSHMetaData.VoidRequest voidReq
                    = new InfaWSHMetaData.VoidRequest();

            // Hit WSH to pull down the int svc servers
            InfaWSHMetaData.FolderInfo oFI = new InfaBGInfo.InfaWSHMetaData.FolderInfo();
            oFI.Name = folderName;

            // we will choose first workflow and folder pair that we find
            // one can similary browse repository for any condition
            InfaWSHMetaData.WorkflowInfo[] wfArr 
                = MWSProxy.getAllWorkflows(oFI);
 

            // assign a value to the output parameter
            invalidCount = 0;


            // print name of repositories availabe
            if (wfArr != null)
            {
                
                for (int i = 0; i < wfArr.Length; i++)
                {
                    tmpReturn.Add(wfArr[i].Name);

                    if (!wfArr[i].IsValid)
                        invalidCount++;
                }
            }
            else
            {
                tmpReturn.Add("No Workflows in folder " + folderName + ".");
            }

            return tmpReturn;
        }


        public int GetNumberOfWorkflowsInFolder(string folderName, out int invalidCount)
        {
            int tmpReturn = 0;

            // create bogus void request variable
            InfaWSHMetaData.VoidRequest voidReq
                    = new InfaWSHMetaData.VoidRequest();

            // Hit WSH to pull down the int svc servers
            InfaWSHMetaData.FolderInfo oFI = new InfaBGInfo.InfaWSHMetaData.FolderInfo();
            oFI.Name = folderName;

            try
            {
                // we will choose first workflow and folder pair that we find
                // one can similary browse repository for any condition
                InfaWSHMetaData.WorkflowInfo[] wfArr
                    = MWSProxy.getAllWorkflows(oFI);


                // assign a value to the output parameter
                invalidCount = 0;


                // print name of repositories availabe
                if (wfArr != null)
                {

                    for (int i = 0; i < wfArr.Length; i++)
                    {
                        tmpReturn++;

                        if (!wfArr[i].IsValid)
                            invalidCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tmpReturn;
        }

    }
}

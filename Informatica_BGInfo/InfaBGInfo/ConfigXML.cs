using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;


namespace InfaBGInfo.Config
{
    public class ConfigXML
    {
        #region Private Members and Properties

        private bool isDecrypted = false;
        private bool isEncrypted = false;
        private bool isConfigXMLOkayToRead = false;
        private string encryptedData;
        private XmlDocument ConfigXMLDoc;   //Core document used for processing

        // Settings
        private string _WSHServerName;
        private int _WSHServerPort;
        private bool _WSHUsesSSL;
        private string _DomainName;
        private string _RepositoryName;
        private string _RepositoryUsername;
        private string _RepositoryPassword;

        // Output
        private int _BkgCustomOption; //copy or custom
        private Color _BkgColor; // could this hold System.Color?
        private int _BkgPosition;
        private string _LayerOnImage;
        private Color _ForegroundColor;
        private int _FontSize;
        private int _ScreenRegion;

        // Advanced
        private bool _ShowAvailableWSHRepositories;
        private bool _ShowAllRepositoryFolders;
        private bool _ShowGlobalRepositoryWorkflows;
        private bool _ShowAssociatedIntegrationServices;
        private bool _ShowSystemInformationBlock;
        private bool _TimerEnabled;
        private int _TimerIntervalSeconds;

        #endregion


        private ConfigXML()
        {
        }

        public ConfigXML(XmlDocument xmlDocToRead)
        {
            try
            {
                ConfigXMLDoc = xmlDocToRead;

                //extract data from XML
                extractConfigXML();

                isConfigXMLOkayToRead = true;
            }
            catch(Exception ex)
            {
                throw new InfaBGInfo.Security.ConfigXMLParseException("Unable to read configuration data.\n" 
                    + ex.InnerException.Message.ToString());
            }
        }

        public ConfigXML(string xmlDocToRead, bool extractXMLValues)
        {
            try
            {
                ConfigXMLDoc = new XmlDocument();
                ConfigXMLDoc.Load(xmlDocToRead);

                //extract data from XML
                extractConfigXML();

                isConfigXMLOkayToRead = true;
            }
            catch
            {
                throw new InfaBGInfo.Security.ConfigXMLParseException("Unable to read configuration data.");
            }
        }

        public ConfigXML(string newEncryptedData)
        {
            //constructor for use with encrypted data
            encryptedData = newEncryptedData;
            decryptConfigXMLData();
        }


        public ConfigXML(string WSHServerName, int WSHServerPort, bool WSHUsesSSL
            , string DomainName
            , string RepositoryName, string RepositoryUsername, string RepositoryPassword
            , int BkgCustomOption, Color BkgColor, int BkgPosition
            , string LayerOnImage, Color ForegroundColor, int FontSize, int ScreenRegion
            , bool showAvailableWSHRepositories, bool showAllRepositoryFolders, bool showGlobalRepositoryWorkflows
            , bool showAssociatedIntegrationServices, bool showSystemInformationBlock
            , bool TimerEnabled, int TimerIntervalSeconds)
        {
            try
            {
                //constructor for ue with decrypted data
                _WSHServerName = WSHServerName;
                _WSHServerPort = WSHServerPort;
                _WSHUsesSSL = WSHUsesSSL;
                _DomainName = DomainName;
                _RepositoryName = RepositoryName;
                _RepositoryUsername = RepositoryUsername;
                _RepositoryPassword = RepositoryPassword;
                _BkgCustomOption = BkgCustomOption;
                _BkgColor = BkgColor;
                _BkgPosition = BkgPosition;
                _LayerOnImage = LayerOnImage;
                _ForegroundColor = ForegroundColor;
                _FontSize = FontSize;
                _ScreenRegion = ScreenRegion;
                _ShowAvailableWSHRepositories = showAvailableWSHRepositories;
                _ShowAllRepositoryFolders = showAllRepositoryFolders;
                _ShowGlobalRepositoryWorkflows = showGlobalRepositoryWorkflows;
                _ShowAssociatedIntegrationServices = showAssociatedIntegrationServices;
                _ShowSystemInformationBlock = showSystemInformationBlock;
                _TimerEnabled = TimerEnabled;
                _TimerIntervalSeconds = TimerIntervalSeconds;

                // Encrypt the data
                encryptConfigXMLData();
            }
            catch (Exception ex)
            {
                throw new Exception("Instantiating Main Constructor Error:\n" + ex.Message.ToString());
            }
        }


        public void createConfigXML()
        {
            //encode card details as XML Document 
            ConfigXMLDoc = new XmlDocument();
            XmlElement documentRoot = ConfigXMLDoc.CreateElement("InfaBGInfo");



            ////let's add the XML declaration section
            XmlNode xmlnode;
            xmlnode = ConfigXMLDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            ConfigXMLDoc.AppendChild(xmlnode);

            // declar each core (parent) section's elements
            XmlElement child;
            XmlElement eSettings;
            XmlElement eOutput;
            XmlElement eAdvanced;
            
            // add each parent section to document root
            eSettings = ConfigXMLDoc.CreateElement("Settings");
            documentRoot.AppendChild(eSettings);

            eOutput = ConfigXMLDoc.CreateElement("Output");
            documentRoot.AppendChild(eOutput);

            eAdvanced = ConfigXMLDoc.CreateElement("Advanced");
            documentRoot.AppendChild(eAdvanced);

            // Begin adding any children of major elements with element inner test, attribute, etc. values
            // Advanced
            child = ConfigXMLDoc.CreateElement("WSHServerName");
            child.InnerText = _WSHServerName;
            eSettings.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("WSHServerPort");
            child.InnerText = _WSHServerPort.ToString();
            eSettings.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("WSHUsesSSL");
            child.InnerText = _WSHUsesSSL.ToString();
            eSettings.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("DomainName");
            child.InnerText = _DomainName;
            eSettings.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("RepositoryName");
            child.InnerText = _RepositoryName;
            eSettings.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("RepositoryUsername");
            child.InnerText = _RepositoryUsername;
            eSettings.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("RepositoryPassword");
            child.InnerText = _RepositoryPassword;
            eSettings.AppendChild(child);

            // Output
            child = ConfigXMLDoc.CreateElement("BkgCustomOption");
            child.InnerText = _BkgCustomOption.ToString();
            eOutput.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("BkgColor");
            child.InnerText = _BkgColor.ToArgb().ToString();
            eOutput.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("BkgPosition");
            child.InnerText = _BkgPosition.ToString();
            eOutput.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("LayerOnImage");
            child.InnerText = _LayerOnImage;
            eOutput.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("ForegroundColor");
            child.InnerText = _ForegroundColor.ToArgb().ToString();
            eOutput.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("FontSize");
            child.InnerText = _FontSize.ToString();
            eOutput.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("ScreenRegion");
            child.InnerText = _ScreenRegion.ToString();
            eOutput.AppendChild(child);


            // Advanced
            child = ConfigXMLDoc.CreateElement("ShowAvailableWSHRepositories");
            child.InnerText = _ShowAvailableWSHRepositories.ToString();
            eAdvanced.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("ShowAllRepositoryFolders");
            child.InnerText = _ShowAllRepositoryFolders.ToString();
            eAdvanced.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("ShowGlobalRepositoryWorkflows");
            child.InnerText = _ShowGlobalRepositoryWorkflows.ToString();
            eAdvanced.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("ShowAssociatedIntegrationServices");
            child.InnerText = _ShowAssociatedIntegrationServices.ToString();
            eAdvanced.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("ShowSystemInformationBlock");
            child.InnerText = _ShowSystemInformationBlock.ToString();
            eAdvanced.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("TimerEnabled");
            child.InnerText = _TimerEnabled.ToString();
            eAdvanced.AppendChild(child);

            child = ConfigXMLDoc.CreateElement("TimerIntervalSeconds");
            child.InnerText = _TimerIntervalSeconds.ToString();
            eAdvanced.AppendChild(child);


            // Finish By Apending the main document root which should contain all elements above
            ConfigXMLDoc.AppendChild(documentRoot);
        }

        private void extractConfigXML()
        {
            try
            {
                // Settings
                _WSHServerName = ConfigXMLDoc.GetElementsByTagName("WSHServerName").Item(0).InnerXml;
                _WSHServerPort = int.Parse(ConfigXMLDoc.GetElementsByTagName("WSHServerPort").Item(0).InnerXml);
                _WSHUsesSSL = bool.Parse(ConfigXMLDoc.GetElementsByTagName("WSHUsesSSL").Item(0).InnerXml);
                _DomainName = ConfigXMLDoc.GetElementsByTagName("DomainName").Item(0).InnerXml;
                _RepositoryName = ConfigXMLDoc.GetElementsByTagName("RepositoryName").Item(0).InnerXml;
                _RepositoryUsername = ConfigXMLDoc.GetElementsByTagName("RepositoryUsername").Item(0).InnerXml;
                _RepositoryPassword = ConfigXMLDoc.GetElementsByTagName("RepositoryPassword").Item(0).InnerXml;

                // Output
                _BkgCustomOption = int.Parse(ConfigXMLDoc.GetElementsByTagName("BkgCustomOption").Item(0).InnerXml);
                _BkgColor = Color.FromArgb(int.Parse(ConfigXMLDoc.GetElementsByTagName("BkgColor").Item(0).InnerXml));
                _BkgPosition = int.Parse(ConfigXMLDoc.GetElementsByTagName("BkgPosition").Item(0).InnerXml);
                _LayerOnImage = ConfigXMLDoc.GetElementsByTagName("LayerOnImage").Item(0).InnerXml;
                _ForegroundColor = Color.FromArgb(int.Parse(ConfigXMLDoc.GetElementsByTagName("ForegroundColor").Item(0).InnerXml));
                _FontSize = int.Parse(ConfigXMLDoc.GetElementsByTagName("FontSize").Item(0).InnerXml);
                _ScreenRegion = int.Parse(ConfigXMLDoc.GetElementsByTagName("ScreenRegion").Item(0).InnerXml);

                // Advanced
                _ShowAvailableWSHRepositories = bool.Parse(ConfigXMLDoc.GetElementsByTagName("ShowAvailableWSHRepositories").Item(0).InnerXml);
                _ShowAllRepositoryFolders = bool.Parse(ConfigXMLDoc.GetElementsByTagName("ShowAllRepositoryFolders").Item(0).InnerXml);
                _ShowGlobalRepositoryWorkflows = bool.Parse(ConfigXMLDoc.GetElementsByTagName("ShowGlobalRepositoryWorkflows").Item(0).InnerXml);
                _ShowAssociatedIntegrationServices = bool.Parse(ConfigXMLDoc.GetElementsByTagName("ShowAssociatedIntegrationServices").Item(0).InnerXml);
                _ShowSystemInformationBlock = bool.Parse(ConfigXMLDoc.GetElementsByTagName("ShowSystemInformationBlock").Item(0).InnerXml);
                _TimerEnabled = bool.Parse(ConfigXMLDoc.GetElementsByTagName("TimerEnabled").Item(0).InnerXml);
                _TimerIntervalSeconds = int.Parse(ConfigXMLDoc.GetElementsByTagName("TimerIntervalSeconds").Item(0).InnerXml);
            }
            catch (NullReferenceException)
            {
            }
            catch (Exception ex)
            {
                throw new InfaBGInfo.Security.ConfigXMLParseException("Issue Parsing XML Configuration: "
                    + ex.InnerException.Message.ToString());
            }
        }

        private void encryptConfigXMLData()
        {
            try
            {
                createConfigXML();

                encryptedData = InfaBGInfo.Security.symmetricHash.encryptWithDES(ConfigXMLDoc.OuterXml);

                isEncrypted = true;
            }
            catch
            {
                throw new InfaBGInfo.Security.ConfigXMLParseException("Unable to encrypt configuration data.");
            }
        }

        private void decryptConfigXMLData()
        {
            try
            {
                ConfigXMLDoc = new XmlDocument();
                ConfigXMLDoc.InnerXml = InfaBGInfo.Security.symmetricHash.decryptWithDES(encryptedData);

                //extrct data from XML
                extractConfigXML();

                isDecrypted = true;
            }
            catch
            {
                throw new InfaBGInfo.Security.ConfigXMLParseException("Unable to decrypt configuration data.");
            }
        }

        public void outputXMLToFile(string fullFilePath)
        {
            ConfigXMLDoc.Save(fullFilePath);
        }


        #region ACCESSORS FOR THE CLASS

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public string WSHServerName
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _WSHServerName;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public int WSHServerPort
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _WSHServerPort;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public bool WSHUsesSSL
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _WSHUsesSSL;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public string DomainName
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _DomainName;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public string RepositoryName
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _RepositoryName;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public string RepositoryUsername
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _RepositoryUsername;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public string RepositoryPassword
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _RepositoryPassword;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public int BkgCustomOption
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _BkgCustomOption;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public Color BkgColor
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _BkgColor;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public int BkgPosition
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _BkgPosition;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public string LayerOnImage
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _LayerOnImage.Replace("&#46;", ".");
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public Color ForegroundColor
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _ForegroundColor;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public int FontSize
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _FontSize;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public int ScreenRegion
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _ScreenRegion;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public bool ShowAvailableWSHRepositories
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _ShowAvailableWSHRepositories;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public bool ShowAllRepositoryFolders
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _ShowAllRepositoryFolders;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public bool ShowGlobalRepositoryWorkflows
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _ShowGlobalRepositoryWorkflows;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public bool ShowAssociatedIntegrationServices
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _ShowAssociatedIntegrationServices;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }
        
        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public bool ShowSystemInformationBlock
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _ShowSystemInformationBlock;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public bool TimerEnabled
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _TimerEnabled;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }

        /// <summary>
        /// Gets value of this name after being read/decrypted from configuration file
        /// </summary>
        public int TimerIntervalSeconds
        {
            get
            {
                if (isDecrypted || isConfigXMLOkayToRead)
                    return _TimerIntervalSeconds;
                else
                {
                    throw new InfaBGInfo.Security.ConfigXMLParseException("Data not decrypted or not ready!");
                }
            }
        }
        
        #endregion




    } //end secureCreditConfigXML class
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.IO;

namespace InfaBGInfo_Console
{
    class GenerateConfigXML
    {
        
        //private void createCardXML()
        //{
        //    //encode card details as XML Document 
        //    xmlCardData = new XmlDocument();
        //    XmlElement documentRoot = xmlCardData.CreateElement("card");

        //    XmlElement child;
        //    child = xmlCardData.CreateElement("gatewayName");
        //    child.InnerXml = gatewayName;
        //    documentRoot.AppendChild(child);

        //    child = xmlCardData.CreateElement("cardHolder");
        //    child.InnerXml = cardHolder;
        //    documentRoot.AppendChild(child);

        //    child = xmlCardData.CreateElement("cardNumber");
        //    child.InnerXml = cardNumber;
        //    documentRoot.AppendChild(child);

        //    child = xmlCardData.CreateElement("issueDate");
        //    child.InnerXml = issueDate;
        //    documentRoot.AppendChild(child);

        //    child = xmlCardData.CreateElement("expDate");
        //    child.InnerXml = expDate;
        //    documentRoot.AppendChild(child);

        //    child = xmlCardData.CreateElement("expMonth");
        //    child.InnerXml = expDate;
        //    documentRoot.AppendChild(child);

        //    child = xmlCardData.CreateElement("expYear");
        //    child.InnerXml = expDate;
        //    documentRoot.AppendChild(child);

        //    child = xmlCardData.CreateElement("issueNumber");
        //    child.InnerXml = String.IsNullOrEmpty(issueNumber) ? "" : issueNumber;
        //    documentRoot.AppendChild(child);

        //    child = xmlCardData.CreateElement("securityCode");
        //    child.InnerXml = securityCode;
        //    documentRoot.AppendChild(child);

        //    child = xmlCardData.CreateElement("cardType");
        //    child.InnerXml = cardType;
        //    documentRoot.AppendChild(child);

        //    xmlCardData.AppendChild(documentRoot);
        //}

    }
}

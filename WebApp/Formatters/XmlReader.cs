using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.AspNetCore.Http;

namespace WebApp.Formatters
{
    public class XmlReader
    {
        public static XDocument ReadXmlDocument(string virtualFilePath)
        {
			return null;
            //return XDocument.Load(Server.MapPath(virtualFilePath));
        }

        public static string GetNodeValue(ref XDocument xDocument, string xPath)
        {
            return xDocument.XPathSelectElement(xPath).Value;
        }
    }
}
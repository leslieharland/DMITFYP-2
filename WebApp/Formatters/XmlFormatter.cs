using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;

namespace WebApp.Formatters
{
    public class XmlFormatter
    {
        public static void CreatePersistentDataXmlDocument(string virtualFilePath)
        {
            // The C# team had made a good choice to use LINQ as one of the methods in programmatic XML creation.
            // This results in programmatic XML creation being simplified and better code readability (when creating XML programmatically).

            /*
            XDocument persistentDataXmlDocument = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("PersistentData",
                    new XElement("ProjectChoiceWindowPeriod",
                        new XElement("StartDate", ""),
                        new XElement("EndDate", "")
                        ),
                    new XElement("GroupFormationWindowPeriod",
                        new XElement("StartDate", ""),
                        new XElement("EndDate", "")
                        )
                    )
                );*/
            
            /*
            XDocument persistentDataXmlDocument = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("PersistentData",
                    new XElement("XmlFileCreatedDate", DateTime.Now),
                    new XElement("Course", new XAttribute("courseid", 1),
                        new XElement("))
                );*/
            //persistentDataXmlDocument.Save(System.Web.HttpContext.Current.Server.MapPath(virtualFilePath));
        }

        public static void UpdateNodeValue(ref XDocument xDocument, string xPath, string updatedValue)
        {
            xDocument.XPathSelectElement(xPath).Value = updatedValue;
        }

        public static void UpdateNodesValues(ref XDocument xDocument, string xPath, string updatedValue)
        {
            IEnumerable<XElement> selectedElements = xDocument.XPathSelectElements(xPath);
            foreach (XElement xe in selectedElements)
            {
                xe.Value = updatedValue;
            }
        }

        public static void FinalizeXmlWriting(ref XDocument xDocument, string virtualFilePath)
        {
            //xDocument.Save(System.Web.HttpContext.Current.Server.MapPath(virtualFilePath));
        }

        /*
        public static void FinalizeXmlWriting(ref XDocument xDocument)
        {
            xDocument
        }*/
    }
}
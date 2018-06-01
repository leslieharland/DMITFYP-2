using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.DAL
{
    public class SerializationUtilities
    {
        public static void serialize<T>(object objectToSerialize, string filePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
            FileStream file = System.IO.File.Create(filePath);
            xmlSerializer.Serialize(file, objectToSerialize);
            file.Close();
        }

        public static void deserialize(object o)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MadGains.Data
{
    class Files
    {
        string pathToDirectory;
        string pathToArchive;
        string pathToUser;

        string today;


        public string PathToDirectory { get => pathToDirectory; set => pathToDirectory = value; }
        public string PathToArchive { get => pathToArchive; set => pathToArchive = value; }
        public string PathToUser { get => pathToUser; set => pathToUser = value; }
        public string Today { get => today; set => today = value; }

        public Files()
        {
            PathToDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +"\\" + "\\MadGains";

            PathToArchive = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MadGains\\Archive";

            PathToUser = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MadGains\\User";

            Today = DateTime.Now.ToString("yyyy-dd-MM-HH");

            createDirectorie(PathToDirectory);
            createDirectorie(PathToArchive);
            createDirectorie(PathToUser);
        }

        private void createDirectorie(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void createDirectoryForTodayArchive()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Stats\\Archive";

            createDirectorie(path + "\\" + Today);
        }

        public void createDirectoryForUser()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Stats\\User";
        }



        public T DeserializeToObject<T>(string filepath) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StreamReader sr = new StreamReader(filepath))
            {
                return (T)ser.Deserialize(sr);
            }
        }



        public static void SerializeToXml<T>(T anyobject, string xmlFilePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(anyobject.GetType());

            using (StreamWriter writer = new StreamWriter(xmlFilePath))
            {
                xmlSerializer.Serialize(writer, anyobject);
            }
        }


    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace LogFileCreation
{
    public struct xmlSettings
    {
        public string serverName;
        public string dbName;
        public string dbPort;
        public string userName;
        public string password;
        public bool success;
        public string errorMessage;
        public string timer;
    }
    public class clsXMLData1
    {
        //public DataSet ds = new DataSet();
        private string xmlPath, xmlSettingsFile;
        //string edition = "\\ePlus Professional Edition";
        public void getconnectionstring(string edition)
        {
            edition = "\\" + edition;
            xmlPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //xmlPath1 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + Application.ProductName;
            //MessageBox.Show(xmlPath1);
            //MessageBox.Show(Path.GetDirectoryName(Application.ExecutablePath).Replace(@"bin\debug\", ""));
            //MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            //xmlPath = "C:\\Users\\Muhsina\\AppData\\Roaming\\ePlus Professional Edition";
            xmlSettingsFile = xmlPath + edition + "\\Settings.xml";

        }
        public xmlSettings ReadSettings()
        {

            xmlSettings settings = new xmlSettings();
            //MessageBox.Show(xmlSettingsFile);
            if (!File.Exists(xmlSettingsFile))
            {
                settings.success = false;
                settings.errorMessage = "File not found (" + xmlSettingsFile + ")";
                return settings;
            }
            string xmlElement = "";
            XmlTextReader reader = new XmlTextReader(xmlSettingsFile);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        xmlElement = reader.Name;
                        //MessageBox.Show(objXmlTextReader.Name);
                        break;
                    case XmlNodeType.Text:
                        switch (xmlElement)
                        {
                            case "SERVER":
                                settings.serverName = reader.Value;
                                break;
                            case "DATABASE":
                                settings.dbName = reader.Value;
                                break;
                            case "PORT":
                                settings.dbPort = reader.Value;
                                break;
                            case "USER":
                                settings.userName = reader.Value;
                                break;
                            case "PWD":
                                settings.password = reader.Value;
                                break;
                            case "TIMER":
                                settings.timer = reader.Value;
                                break;
                        }
                        break;
                }
            }
            reader.Close();
            settings.success = true;
            settings.errorMessage = "";
            return settings;
        }

        public string getServerName(DataRow currentRow)
        {
            return currentRow["SERVERNAME"].ToString();
        }
        public string getDBName(DataRow currentRow)
        {
            return currentRow["DBNAME"].ToString();
        }
        public string getDBPort(DataRow currentRow)
        {
            return currentRow["PORT"].ToString();
        }
        public string getCompanyCode(DataRow currentRow)
        {
            return currentRow["COCODE"].ToString();
        }
        public string getBranchCode(DataRow currentRow)
        {
            return currentRow["BRCODE"].ToString();
        }
        public string getFiscalYearCode(DataRow currentRow)
        {
            return currentRow["FYCODE"].ToString();
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Xml;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;


namespace LogFileCreation
{
    public struct DBinfo
    {
        public string DBCode;
        public string ServerName;
        public string DBName;
        public string DBPort;
        public string DBUser;
        public string DBPWD;
        public string ConnectionString;
        public Boolean Success;
    }
    public class Dbconnection
    {

        clsXMLData1 settings1 = new clsXMLData1();
        //xmlSettings settings = new xmlSettings();
        //string connectionString = null;
        MySqlConnection cnnLog;
        MySqlCommand cmd = new MySqlCommand();
        public string getConnectionString(DBinfo dbconnstring)
        {
            return "Data Source=" + dbconnstring.ServerName + ";Initial Catalog=" + dbconnstring.DBName + ";User ID=root" + ";Password=metalic";
        }

        public void createLogFile(DBinfo dbconnstring)
        {
            string connString = @"server=" + dbconnstring.ServerName + ";userid=root" + ";password=metalic";
            cnnLog = new MySqlConnection(connString); 
            string db;
            try
            {
                cnnLog.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Can not open connection ! \n" + ex.ToString());
            }
            cmd.Connection = cnnLog;
            /*if (db == null)
            {
                db = dbconnstring.DBName.Replace("_data", "");
            }
            else
            {*/
            db = dbconnstring.DBName;
            
            db = db.Replace("_data", "");
            //}
            cmd.CommandText = "CREATE DATABASE IF NOT EXISTS " + db + "_log";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show("\n" + ex.ToString());
            }
            cmd.CommandText = "USE " + db + "_log;" ;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("\n" + ex.ToString());
            }
            string path = Path.GetDirectoryName(Application.ExecutablePath);           
            //path = path.Replace(@"bin\Debug", "");
            MySqlScript script = new MySqlScript(cnnLog, File.ReadAllText(path + "\\SQLLOG2203.sql"));
            script.Delimiter = "***";
            script.Execute();           
           
        }

    }
}


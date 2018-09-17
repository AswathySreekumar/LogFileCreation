using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Xml;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace LogFileCreation
{
    public partial class LogCreation : Form
    {
        
        clsXMLData1 settings1 = new clsXMLData1();
        xmlSettings settings = new xmlSettings();
        string connectionString = null;
        MySqlConnection cnn,cnnLog;
        Dbconnection dbinfo = new Dbconnection();
        DBinfo dbinfo1 = new DBinfo();
        DBinfo dbinfoLog = new DBinfo();
        DataSet ds1 = new DataSet();
        DataTable dt = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        MySqlDataAdapter adapterNew = new MySqlDataAdapter();
        MySqlCommand cmd = new MySqlCommand();
        MySqlCommandBuilder cmb;
        DataGridView Dgv;

       
        string code;
        //int update = 0;
        //int found = 0;
        //string dbname;
        int i;
        string Edition,br;
        string Repcode;
        string type;
        string[] branch;

        public LogCreation()
        {
            InitializeComponent();
        }

        private void LogCreation_Load(object sender, EventArgs e)
        {
            Dgv = new DataGridView();
            this.Gb.Controls.Add(Dgv);
            //this.Controls.Add(Dgv);
            //Dgv.AutoSize = true;
            Dgv.Size = new System.Drawing.Size(0, 0);
            Dgv.Location = new System.Drawing.Point(10, 60);
            //cmbEdition.Items.Add("aaa");
            cmbEdition.SelectedIndex = 0;
            ToolTip toolTip = new ToolTip();
            toolTip.ShowAlways = true;
            toolTip.SetToolTip(this.cmbEdition, "Select Edition");
            toolTip.SetToolTip(this.cmbBranch, "Select your branch");
        }
       

        private void cmbEdition_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbBranch.Enabled = true;        
            Edition = cmbEdition.SelectedItem.ToString();
            //MessageBox.Show(Edition);
            settings1.getconnectionstring(Edition);
            settings = settings1.ReadSettings();
            dbinfo1.ServerName = settings.serverName;
            dbinfo1.DBName = settings.dbName;
            dbinfo1.DBPort = settings.dbPort;
            dbinfo1.DBUser = settings.userName;
            dbinfo1.DBPWD = settings.password;
            connectionString = dbinfo.getConnectionString(dbinfo1);
            //Dgv.DataSource = null;
            // Dgv.Rows.Clear();
            cnn = new MySqlConnection(connectionString);
            openconnection(cnn);

        }

        void openconnection(MySqlConnection conn)
        {
            try
            {
                conn.Open();
                MessageBox.Show("Connection Open!\n", "Connection", MessageBoxButtons.OK, MessageBoxIcon.None);
                
                Dgv.DataSource = null;
                Dgv.Rows.Clear();
                Dgv.Refresh();
                //dt.Rows.Clear();
               
               
                FillBranch(Edition);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Can not open connection ! \n" + ex.ToString());
            }
        }

        void FillBranch(String Edition)
        {
           adapter.SelectCommand = new MySqlCommand("SELECT code from branchmaster", cnn);          
            DataSet ds = new DataSet();
            try
            {
                adapter.Fill(ds, "branchmaster");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("No Branches Available!\n" + ex.ToString());
            }
            this.cmbBranch.SelectedIndexChanged -= new System.EventHandler(this.cmbBranch_SelectedIndexChanged);
            cmbBranch.DataSource = null;
            if (Edition == "ePlus Professional Edition")
            {
                branch = new string[]{ "HO"};
                cmbBranch.DataSource = branch;
                //cmbBranch.Items.Add("HO");
            }
            else
            {
                cmbBranch.DataSource = ds.Tables["branchmaster"];
                cmbBranch.DisplayMember = "code";
                cmbBranch.ValueMember = "code";
            }     
            this.cmbBranch.SelectedIndexChanged += new System.EventHandler(this.cmbBranch_SelectedIndexChanged);
            
        }

        private void cmbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddRepDetails();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }


        void AddRepDetails()
        {

            if (cmbBranch.SelectedIndex > -1)
            {             
                code = null;
                br = this.cmbBranch.GetItemText(this.cmbBranch.SelectedItem);
                /*
                this.cmbBranch.SelectedIndexChanged -= new System.EventHandler(this.cmbBranch_SelectedIndexChanged);
                for (int i = cmbBranch.Items.Count - 1; i >= 0; i--)
                {
                    if (cmbBranch.Items[i] != this.cmbBranch.SelectedItem)
                       //cmbBranch.Items.RemoveAt(i);
                       cmbBranch.Items[i]
                }
                this.cmbBranch.SelectedIndexChanged += new System.EventHandler(this.cmbBranch_SelectedIndexChanged);
                */
                cmbBranch.Enabled = false;  
                //MessageBox.Show(br);
                cmd.Connection = cnn;
                cmd.CommandText = "SELECT CODE FROM DATABASEMASTER WHERE CODE IN (SELECT DBCODE FROM DATABASEMAPPING WHERE BRCODE = '" + br + "')";
                cmd.ExecuteNonQuery();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    code = rdr.GetValue(0).ToString(); //MessageBox.Show(code);
                }
                rdr.Close();
                if (code != null)
                {
                    dbinfo.createLogFile(dbinfo1);
                    //cmd.Connection = cnn;
                    if (br == "HO")
                    {
                        type = "DistributedServer";
                    }
                    else
                    {
                        type = "DistributedClient";
                        cmd.CommandText = "delete from branchuser where usercode = 'admin' and brcode <> '" + br + "'";
                        cmd.ExecuteNonQuery();
                    }
                    cmd.CommandText = "UPDATE " + dbinfo1.DBName + ".setup SET `DBTYPE`='" + type + "' WHERE  `CODE`='01'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "UPDATE SETUP SET DEFAULTBRANCH = '" + br + "' WHERE CODE = '01'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "update databasemaster set logdbname = '" + dbinfo1.DBName.Replace("_data", "_log") + "' , logservername = '" + dbinfo1.ServerName + "' , logport = '" + dbinfo1.DBPort + "' where code = '" + code + "'";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Created Log Successfully");
                    dbinfoLog.ServerName = settings.serverName;
                    dbinfoLog.DBName = dbinfo1.DBName.Replace("_data", "_log");
                    dbinfoLog.DBPort = settings.dbPort;
                    dbinfoLog.DBUser = settings.userName;
                    dbinfoLog.DBPWD = settings.password;
                    connectionString = dbinfo.getConnectionString(dbinfoLog);
                    cnnLog = new MySqlConnection(connectionString);
                    openconnection(cnnLog);

                    cmd.CommandText = "select code from databasemaster where code not in ('" + code + "')";
                    cmd.ExecuteNonQuery();
                    rdr = cmd.ExecuteReader();
                    i = 0;
                    while (rdr.Read())
                    {
                        Repcode = rdr.GetValue(0).ToString();
                        //RepBrcode = rdr.GetValue(1).ToString();
                       // MessageBox.Show(Repcode.ToString());
                        //MessageBox.Show(RepBrcode.ToString());
                        i++;
                        cmd.Connection = cnnLog;
                        cmd.CommandText = "insert ignore into replicationinfo (code,firstid,minimumtimegap) values ('" + Repcode + "','0','60')";
                        try
                        {
                            
                            cmd.ExecuteNonQuery();
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show("\n" + ex.ToString());

                        }
                    }
                    rdr.Close();        
                    DataColumnCollection columns = dt.Columns;
                    if (columns.Contains("servername"))
                    {
                        dt.Columns.Remove("servername");
                    }      
                    dt.Clear();
                    //adapterNew.Dispose();
                    DataColumn col = dt.Columns.Add("servername", typeof(string));
                    col.SetOrdinal(0);
                    //dt.Columns.Add(new DataColumn("ServerName", typeof(string))

                    adapterNew.SelectCommand = new MySqlCommand("select code,firstid,minimumtimegap from replicationinfo ", cnnLog);    
                    try
                    {
                        adapterNew.Fill(dt);
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("\n" + ex.ToString());
                    }  
                    cmb = new MySqlCommandBuilder(adapterNew);
                    Dgv.AutoSize = true;
                    //Size startingSize = new Size(20, 20);
                    //Dgv.Size = startingSize;
                    Dgv.DataSource = dt;
                    cmd.Connection = cnn;
                    for (int rows = 0; rows < (Dgv.Rows.Count - 1); rows++)
                    {
                        string value = Dgv.Rows[rows].Cells["code"].Value.ToString();
                        cmd.CommandText = "select servername from databasemaster where code = '" + value + "'";
                        cmd.ExecuteNonQuery();
                        rdr = cmd.ExecuteReader();
                        if(rdr.Read())
                        { //MessageBox.Show(rdr.GetValue(0).ToString());
                            Dgv.Rows[rows].Cells["servername"].Value = rdr.GetValue(0).ToString();
                        }
                        rdr.Close();
                    }
                    Dgv.Show();
                }
            }     
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
           
            DialogResult dr = MessageBox.Show("Are you sure to save Changes", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    adapterNew.Update(dt);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("\n" + ex.ToString());
                }
                
                //Dgv.Refresh();
                //Dgv.Columns.Clear();
                MessageBox.Show("Record Updated");
            }
        }


    }
}

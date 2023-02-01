/*
*    FILE            : Admin.xaml.cs
*    PROJECT         : SENG2020 - TMS Project
*    PROGRAMMER      : Hyuk jin Kwon
*    FIRST VERSION   : 2022-12-04
*    DESCRIPTION     :
*        Responsible for the functions of the admin account.
*        Admin can modify the carrier data.
*        Admin can modify IP, Port, and log file directories.
*        Displays the saved log contents in the application
*        Admin can backup data.
*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;

namespace TMS
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        // Properties
        MySqlConnection conn = null;
        MySqlCommand cmd = null;
        MySqlDataAdapter da = null;
        DataTable dt = null;
        MySqlCommandBuilder cb = null;

        // Constructor
        public Admin()
        {
            // Initialize ip, port path on admin windows
            InitializeComponent();
            txtIP.Text = ConfigurationManager.AppSettings["ipAddress"];
            txtPort.Text = ConfigurationManager.AppSettings["port"];
            txtLogDir.Text = ConfigurationManager.AppSettings["path"];

        }

        /*
         * Method       : Window_Loaded()
         * Description  : Load carriers table of database into DataGrid
         * Parameters   : object sender, RoutedEventArgs e
         * Return       : void
         */
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Loaded Carriers Data
            try
            {
                conn = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");

                conn.Open();

                string query = "select * from carriers;";

                cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();

                da.Fill(dt);
                DataGridView.ItemsSource = dt.DefaultView;

                cb = new MySqlCommandBuilder(da);

                da.InsertCommand = cb.GetInsertCommand();
                da.UpdateCommand = cb.GetUpdateCommand();
                da.DeleteCommand = cb.GetDeleteCommand();

                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                WriteLog("Exception: " + ex.ToString());
            }

        }

        /*
         * Method       : Btn_LoadLogFile_Click()
         * Description  : Display the contents of the Log file
         * Parameters   : object sender, RoutedEventArgs e
         * Return       : void
         */
        private void Btn_LoadLogFile_Click(object sender, RoutedEventArgs e)
        {
            txtLogFile.Text = File.ReadAllText(ConfigurationManager.AppSettings["path"]);
            MessageBox.Show("Load log");
            WriteLog("admin - Load log");
        }

        /*
         * Method       : WriteLog()
         * Description  : Create a Log file or add log contents to the specified path
         * Parameters   : string log
         * Return       : void
         */
        public void WriteLog(string log)
        {
            string path = ConfigurationManager.AppSettings["path"];
            string directory = path.Remove(path.LastIndexOf("\\"));

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + log);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + log);
                }
            }
        }

        /*
         * Method       : SelectLogFileDir_Click()
         * Description  : Change the path of the log file
         * Parameters   : object sender, RoutedEventArgs e
         * Return       : void
         */
        private void SelectLogFileDir_Click(object sender, RoutedEventArgs e)
        {
            string newPath = txtLogDir.Text;

            var changeLogConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            changeLogConfig.AppSettings.Settings["path"].Value = newPath;

            changeLogConfig.Save(ConfigurationSaveMode.Modified);
            MessageBox.Show("Log file path changed");
            WriteLog("Log File path changed");

            ConfigurationManager.RefreshSection("appSettings");
        }


        /*
         * Method       : ChangeIPAndPort_Click()
         * Description  : Change IP and Port to config file and save
         * Parameters   : object sender, RoutedEventArgs e
         * Return       : void
         */
        private void ChangeIPAndPort_Click(object sender, RoutedEventArgs e)
        {
            string newIP = txtIP.Text;
            string newPort = txtPort.Text;

            var changeIPAndPortConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            changeIPAndPortConfig.AppSettings.Settings["ipAddress"].Value = newIP;
            changeIPAndPortConfig.AppSettings.Settings["port"].Value = newPort;

            changeIPAndPortConfig.Save(ConfigurationSaveMode.Modified);
            MessageBox.Show("IP and Port Saved");
            WriteLog("IP and Port Saved");

            ConfigurationManager.RefreshSection("appSettings");
        }

        /*
         * Method       : Btn_Backup_Click()
         * Description  : Create a database backup file
         * Parameters   : object sender, RoutedEventArgs e
         * Return       : void
         * Reference    : https://stackoverflow.com/questions/40083207/mysql-backup-and-restore-c-sharp
         */
        private void Btn_Backup_Click(object sender, RoutedEventArgs e)
        {
            string constring = "Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;";
            string file = "c:/temp/backup.sql";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(file);
                        MessageBox.Show("Backup TMS database (c:/temp/backup.sql)");
                        WriteLog("Backup TMS database (c:/temp/backup.sql)");
                        conn.Close();
                    }
                }
            }
        }

        /*
         * Method       : DataSave_Click()
         * Description  : Save changes to the carriers table
         * Parameters   : object sender, RoutedEventArgs e
         * Return       : void
         */
        private void DataSave_Click(object sender, RoutedEventArgs e)
        {   
            try
            {
                da.Update(dt);
                WriteLog("Save Carriers Data");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                WriteLog("Exception: " + ex.ToString());
            }           
            
        }

        /*
         * Method       : Logout_Click()
         * Description  : admin logout
         * Parameters   : object sender, RoutedEventArgs e
         * Return       : void
         */
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

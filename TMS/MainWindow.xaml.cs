/*
*    FILE            : MainWindow.xaml.cs
*    PROJECT         : SENG2020 - TMS Project
*    PROGRAMMER      : Group 10 members (Amandeep Nath, Eunyoung Kim, Hyuk Jin Kwon, Yujung Park)
*    FIRST VERSION   : 2022-12-04
*    DESCRIPTION     :
*        Connect the application to the database.
*        Depending on the user, the admin, buyer, and planner windows are displayed.
*        Log contextual log files.
*/

using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.IO;
using MySql.Data.MySqlClient;

namespace TMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string name { get; set; }
        public string password { get; set; }
        MySqlConnection conn = null;
        MySqlCommand cmd = null;
        MySqlDataReader rdr = null;
        public MainWindow()
        {
            // InitializeComponent();
        }

        /*
        * Method       : Btn_LoginSubmit_Click()
        * Description  : Based on the account information entered by the user, 
        *              : the user type is separated and the window is displayed
        * Parameters   : object sender, RoutedEventArgs e
        * Return       : void
        */
        public void Btn_LoginSubmit_Click(object sender, RoutedEventArgs e)
        {
            name = txtUsername.Text.Trim();
            password = txtPassword.Password.Trim();

            if (name.Equals("") || password.Equals(""))
            {
                MessageBox.Show("Please enter a username or password");
            }
            else
            {
                // set up mysql connection
                conn = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");

                // check user data
                string sqlCmd = "select * from users where name = '" + name + "' and password = '" + password + "'";
                cmd = new MySqlCommand(sqlCmd, conn);

                conn.Open();

                rdr = cmd.ExecuteReader();
                try
                {
                    bool entryCheck = false;

                    while (rdr.Read())
                    {
                        if (rdr.HasRows)
                        {
                            // check user account admin
                            if (rdr["userType"].ToString() == "admin")
                            {
                                MessageBox.Show("admin login");
                                WriteLog("admin login");
                                Admin admin = new Admin();
                                admin.ShowDialog();
                                entryCheck = true;
                            }
                            // check user account buyer
                            else if (rdr["userType"].ToString() == "buyer")
                            {
                                MessageBox.Show("buyer login");
                                WriteLog("buyer login");
                                Buyer buyer = new Buyer();
                                buyer.ShowDialog();
                                entryCheck = true;
                            }
                            // check user account planner
                            else if (rdr["userType"].ToString() == "planner")
                            {
                                MessageBox.Show("planner login");
                                WriteLog("planner login");
                                Planner planner = new Planner();
                                planner.ShowDialog();
                                entryCheck = true;
                            }
                        }
                    }

                    if (entryCheck == false)
                    {
                        MessageBox.Show("Invalid login");
                        WriteLog("Invalid login");
                        rdr.Close();
                        conn.Close();
                    }
                    else
                    {
                        rdr.Close();
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    WriteLog("Failed connect");
                }
            }
        }

        /*
        * Method       : WriteLog()
        * Description  : Create a log file or add log content
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
    }
}
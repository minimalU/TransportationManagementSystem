//FILE          : Planner.xaml.cs
//PROJECT       : SENG2020 - TMS Project
//PROGRAMMER    : Yujung Park
//FIRST VERSION : 2022.11.15
//DESCRIPTION   : This file includes the functions for the Planner windows of TMS system.
//                The application of the planner receives the data from tms database of mySQL and
//                diplays the data on the window and allow user to manipulate the data contents of the window
//                and update or create the user input data to the mySQL database.
//                https://dev.mysql.com/doc/connector-net/en/connector-net-tutorials.html

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace TMS
{

    public partial class Planner : Window
    {
        MySqlConnection connection;
        MySqlCommand command;
        MySqlDataAdapter dataAdapter;
        MySqlCommandBuilder commandBuilder;
        DataTable dataTable = new DataTable();
        DataSet ds = new DataSet();    // database
        DataTable dtTrip = new DataTable();


        string sql;

        private string clientName { get; set; }
        private int jobType { get; set; }
        private int qty { get; set; }
        private int vanType { get; set; }

        public Planner()
        {
            InitializeComponent();

        }

        // FUNCTION    : RecOrder_Click
        // DESCRIPTION : This function is to receive the data of order table from tms database
        // PARAMETERS  : sender object
        //               e      RoutedEvenArgs
        // RETURNS     : nothing
        private void RecOrder_Click(object sender, RoutedEventArgs e)
        {
            // Planner receives Orders from the Buyer.
            try
            {
                connection = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");
                connection.Open();

                string query = "select * from Orders;";

                command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                dataAdapter = new MySqlDataAdapter(command);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                OrderDGV.ItemsSource = dataTable.DefaultView;
                commandBuilder = new MySqlCommandBuilder(dataAdapter);
                dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                dataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //WriteLog("Exception: " + ex.ToString());
            }
        }

        // FUNCTION    : Update_Click
        // DESCRIPTION : This function is to update the data of order table from tms database
        // PARAMETERS  : sender object
        //               e      RoutedEvenArgs
        // RETURNS     : nothing
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            dataAdapter.Update(dataTable);
        }


        // FUNCTION    : SelectCarr_Click
        // DESCRIPTION : This function is to load the data of carrier table from tms database
        // PARAMETERS  : sender object
        //               e      RoutedEvenArgs
        // RETURNS     : nothing
        private void SelectCarr_Click(object sender, RoutedEventArgs e)
        {
            //	Planner selects Carriers from the targeted cities to complete the Order,
            //  which adds a ‘Trip’ to the Order for each Carrier selected
            try
            {
                connection = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");
                connection.Open();

                string query = "select * from carrier;";

                command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                dataAdapter = new MySqlDataAdapter(command);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                OrderDGV.ItemsSource = dataTable.DefaultView;
                commandBuilder = new MySqlCommandBuilder(dataAdapter);
                dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                dataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //WriteLog("Exception: " + ex.ToString());
            }

        }

        // FUNCTION    : Add_Click
        // DESCRIPTION : This function is to load the data of trip table from tms database 
        //               and to allow user to change data
        // PARAMETERS  : sender object
        //               e      RoutedEvenArgs
        // RETURNS     : nothing
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string orderNo = OrderNo.Text;

            try
            {
                connection = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");
                connection.Open();

                string query = "select * from trip where OrderID=" + orderNo + ";";

                command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                dataAdapter = new MySqlDataAdapter(command);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                workScreen.ItemsSource = dataTable.DefaultView;
                commandBuilder = new MySqlCommandBuilder(dataAdapter);
                dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                dataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
                connection.Close();
            }
            catch (Exception ex)
            {
                string tripTableException = ex.Message;
                //MessageBox.Show(ex.Message);
                //WriteLog("Exception: " + ex.ToString());
            }
        }

        // FUNCTION    : SaveTrip_Click
        // DESCRIPTION : This function is to update the data of trip table from tms database
        // PARAMETERS  : sender object
        //               e      RoutedEvenArgs
        // RETURNS     : nothing
        private void SaveTrip_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataAdapter.Update(dataTable);
            }
            catch (Exception tripAdtException)
            {
                string tripAdtExp = tripAdtException.ToString();
            }
        }

        // FUNCTION    : OpenOrder_Click
        // DESCRIPTION : This function is to load the data of order table from tms database 
        // PARAMETERS  : sender object
        //               e      RoutedEvenArgs
        // RETURNS     : nothing
        private void OpenOrder_Click(object sender, RoutedEventArgs e)
        {
            // Planner may see a summary of all active Orders in a status screen.
            try
            {
                connection = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");
                connection.Open();

                string query = "select * from orders where OrderStatus = 0;";

                command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                dataAdapter = new MySqlDataAdapter(command);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                OrderDGV.ItemsSource = dataTable.DefaultView;
                commandBuilder = new MySqlCommandBuilder(dataAdapter);
                dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                dataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
                connection.Close();
            }
            catch (Exception ex)
            {
                string tripTableException = ex.Message;
                //MessageBox.Show(ex.Message);
                //WriteLog("Exception: " + ex.ToString());
            }
        }

        // FUNCTION    : Report_Click
        // DESCRIPTION : This function is to load the data of invoice table from tms database according to the range user chosen
        // PARAMETERS  : sender object
        //               e      RoutedEvenArgs
        // RETURNS     : nothing
        private void Report_Click(object sender, RoutedEventArgs e)
        {
            //	Planner may generate a summary report of all Invoice data
            //  for a) all time, and b) The ‘past 2 weeks’ of simulated time
            string period = ReportPeriod.Text;

            DateTime date = DateTime.Today.AddDays(-14);
            string dt = date.ToShortDateString();
            string prd = dt.Replace("-", "");
            connection = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");
            connection.Open();

            if (period == "2 weeks")
            {
                sql = "select * from invoice where InvoiceDate >" + prd + ";";
            }
            else
            {
                sql = "select * from invoice;";

            }

            command = new MySqlCommand(sql, connection);

            command.ExecuteNonQuery();

            dataAdapter = new MySqlDataAdapter(command);
            dataTable = new DataTable();

            dataAdapter.Fill(dataTable);
            workScreen.ItemsSource = dataTable.DefaultView;

            commandBuilder = new MySqlCommandBuilder(dataAdapter);

            dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
            dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
            dataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();

            connection.Close();
        }

        // simulate the passage of time
        //private void Simulate_Click(object sender, RoutedEventArgs e)
        //{
        //}
    }
}

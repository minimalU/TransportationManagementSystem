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
//using System.Web.Mvc;
using System.Data.SqlClient;
using K4os.Compression.LZ4.Internal;
using System.Collections;

namespace TMS
{
    /// <summary>
    /// Interaction logic for Buyer.xaml
    /// </summary>
    public partial class Buyer : Window
    {

        // Properties
        MySqlConnection conn = null;
        MySqlCommand cmd = null;
        MySqlDataAdapter da = null;
        DataTable dt = null;
        MySqlCommandBuilder cb = null;
        int number = 0;

        public Buyer()
        {
            InitializeComponent();
            getInvoice.Click += new RoutedEventHandler(invoice_Click);
            listGrid.SelectionChanged += new SelectionChangedEventHandler(listGrid_SelectionChanged);
        }



        /*
       * Method       : contract_Click()
       * Description  : Show the contract list
       * Parameters   : object sender, RoutedEventArgs e
       * Return       : void
       */
        private void contract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conn = new MySqlConnection("Server=159.89.117.198;Database=cmp;Uid=DevOSHT;Pwd=Snodgr4ss!;");

                conn.Open();

                string query = "select * from Contract order by Client_Name asc;";

                cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();

                da.Fill(dt);
                listGrid.ItemsSource = dt.DefaultView;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        /*
       * Method       : customer_Click()
       * Description  : Show the customer list
       * Parameters   : object sender, RoutedEventArgs e
       * Return       : void
       */
        private void customer_Click(object sender, RoutedEventArgs e)
        {

            string name = searchName.Text.Trim();
            string query;

            if (name.Equals(""))
            {

                try
                {
                    conn = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");

                    conn.Open();



                    query = "select * from Customer order by CustomerID asc;";

                    cmd = new MySqlCommand(query, conn);

                    cmd.ExecuteNonQuery();

                    da = new MySqlDataAdapter(cmd);
                    dt = new DataTable();

                    da.Fill(dt);
                    listGrid.ItemsSource = dt.DefaultView;

                    cb = new MySqlCommandBuilder(da);

                    da.InsertCommand = cb.GetInsertCommand();
                    da.UpdateCommand = cb.GetUpdateCommand();
                    da.DeleteCommand = cb.GetDeleteCommand();

                    conn.Close();




                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {

                try
                {
                    conn = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");

                    conn.Open();

                    query = "select * from Customer where CustomerName = '" + name + "'";

                    cmd = new MySqlCommand(query, conn);

                    cmd.ExecuteNonQuery();

                    da = new MySqlDataAdapter(cmd);
                    dt = new DataTable();

                    da.Fill(dt);
                    listGrid.ItemsSource = dt.DefaultView;

                    cb = new MySqlCommandBuilder(da);

                    da.InsertCommand = cb.GetInsertCommand();
                    da.UpdateCommand = cb.GetUpdateCommand();
                    da.DeleteCommand = cb.GetDeleteCommand();

                    conn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }



        /*
       * Method       : order_Click()
       * Description  : Show the order list
       * Parameters   : object sender, RoutedEventArgs e
       * Return       : void
       */
        private void order_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                conn = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");

                conn.Open();

                string query = "select * from Orders where OrderStatus = false order by OrderDate asc;";

                cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();

                da.Fill(dt);
                listGrid.ItemsSource = dt.DefaultView;

                cb = new MySqlCommandBuilder(da);

                da.InsertCommand = cb.GetInsertCommand();
                da.UpdateCommand = cb.GetUpdateCommand();
                da.DeleteCommand = cb.GetDeleteCommand();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /*
       * Method       : completed_Click()
       * Description  : Show the completed order list
       * Parameters   : object sender, RoutedEventArgs e
       * Return       : void
       */
        private void completed_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                conn = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");

                conn.Open();

                string query = "select * from Orders where OrderStatus = true order by OrderDate asc;";

                cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();

                da.Fill(dt);
                listGrid.ItemsSource = dt.DefaultView;

                cb = new MySqlCommandBuilder(da);

                da.InsertCommand = cb.GetInsertCommand();
                da.UpdateCommand = cb.GetUpdateCommand();
                da.DeleteCommand = cb.GetDeleteCommand();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            number = 3;
        }


        /*
       * Method       : Save_Click()
       * Description  : Save the database when the buyer modify
       * Parameters   : object sender, RoutedEventArgs e
       * Return       : void
       */
        private void Save_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                da.Update(dt);
                MessageBox.Show("Completed add customer");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }

        /*
        * Method       : listGrid_SelectionChanged()
        * Description  : Put the data value into the invoice database
        * Parameters   : object sender, SelectionChangedEventArgs e
        * Return       : void
        */
        private void listGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (number == 3)
            {

                var result = MessageBox.Show("Do you want to create invoice?", "Invoice", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {

                    DataRowView row = (DataRowView)listGrid.CurrentCell.Item;

                    if (row != null)
                    {

                        string orID = row.Row.ItemArray[0].ToString();
                        string cuID = row.Row.ItemArray[2].ToString();
                        string tripID = row.Row.ItemArray[3].ToString();
                        string tripName = row.Row.ItemArray[4].ToString();

                        try
                        {
                            conn = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");

                            conn.Open();

                            string query = "insert into Invoice(OrderID,CustomerID, TripID, TripName, InvoiceID,InvoiceDate,Qty,  Unit, Currency, Amount  ) VALUES('" + orID + "', '" + cuID + "', '" + tripID + "', '" + tripName + "', 0, CURRENT_TIMESTAMP, null,null,'default',0 ); ";

                            cmd = new MySqlCommand(query, conn);

                            cmd.ExecuteNonQuery();

                            da = new MySqlDataAdapter(cmd);
                            dt = new DataTable();

                            da.Fill(dt);


                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        number = 0;

                        return;
                    }
                    else
                    {
                        number = 0;
                        return;
                    }


                }
                else if (result == MessageBoxResult.No)
                {
                    number = 0;
                    return;
                }


            }
            else
            {
                number = 0;
                return;
            }

        }



        /*
        * Method       : invoice_Click()
        * Description  : Show the invoice list
        * Parameters   : object sender, RoutedEventArgs e
        * Return       : void
        */
        private void invoice_Click(object sender, RoutedEventArgs e)
        {

            number = 4;

            try
            {
                conn = new MySqlConnection("Server=127.0.0.1;Database=tms;Uid=SETUser;Pwd=Conestoga1;");

                conn.Open();

                string query = "select * from Invoice order by InvoiceDate asc;";

                cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();

                da.Fill(dt);
                invoiceList.ItemsSource = dt.DefaultView;

                cb = new MySqlCommandBuilder(da);

                da.InsertCommand = cb.GetInsertCommand();
                da.UpdateCommand = cb.GetUpdateCommand();
                da.DeleteCommand = cb.GetDeleteCommand();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        /*
        * Method       : searchName_TextChanged()
        * Description  : Show the grid list
        * Parameters   : object sender, RoutedEventArgs e
        * Return       : void
        */
        private void searchName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        /*
        * Method       : invoiceUpdate_Click()
        * Description  : Update the invoice value
        * Parameters   : object sender, RoutedEventArgs e
        * Return       : void
        */
        private void invoiceUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (number == 4)
            {
                try
                {
                    da.Update(dt);
                    MessageBox.Show("Completed Invoice");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

            }

        }



        /*
       * Method       : invoiceList_SelectionChanged()
       * Description  : Save the invoice in the txt file
       * Parameters   : object sender, SelectionChangedEventArgs e
       * Return       : void
       */
        private void invoiceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            var result = MessageBox.Show("Do you want to save Invoice", "SAVE", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                DataRowView row = (DataRowView)invoiceList.CurrentCell.Item;

                if (row != null)
                {

                    string orID = row.Row.ItemArray[0].ToString();
                    string cuID = row.Row.ItemArray[1].ToString();
                    string tripID = row.Row.ItemArray[2].ToString();
                    string tripName = row.Row.ItemArray[3].ToString();
                    string inID = row.Row.ItemArray[4].ToString();
                    string inDate = row.Row.ItemArray[5].ToString();
                    string qty = row.Row.ItemArray[6].ToString();
                    string unit = row.Row.ItemArray[7].ToString();
                    string currency = row.Row.ItemArray[8].ToString();
                    string amount = row.Row.ItemArray[9].ToString();

                    string path = @"C:\Users\yujun\source\repos\tms\invoice.txt";
                    string textValue = "********** Invoice **********\n" +
                                       "Order number: " + orID + "\n" +
                                       "Customer ID: " + cuID + "\n" +
                                       "Trip: " + tripName + "\n" +
                                       "Date: " + inDate + "\n" +
                                       "Qty: " + qty + "\n" +
                                       "Unit: " + unit + "\n" +
                                       "Currency: " + currency + "\n" +
                                       "Amount: " + amount + "\n\n";
                    FileInfo fi = new FileInfo(path);

                    if (fi.Exists == false)
                    {

                        System.IO.File.WriteAllText(path, textValue);

                        return;

                    }

                    System.IO.File.AppendAllText(path, textValue);


                    //System.IO.File.AppendAllText(path, textValue);
                    number = 0;

                    return;
                }
            }
            else if (result == MessageBoxResult.No)
            {
                return;
            }



        }



    }
}
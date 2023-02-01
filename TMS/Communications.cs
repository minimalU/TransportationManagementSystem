using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS
{
    internal class Communications : MainWindow
    {
        private string username { get; set; }
        private string cPassword { get; set; }
        private string ipAddress
        {
            set { ipAddress = "159.89.117.198"; }
        } 
        private int port
        {
            set { port = 3306; }
        }
        private string databaseName
        {
            set { databaseName = "cmp"; }
        }

        private void ConnectMySQL(string conn)
        {

        }
    }
}

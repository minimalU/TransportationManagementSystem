using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS
{
    internal class Invoice : MainWindow
    {
        private string invoiceID { get; set; }
        private string invoiceDate { get; set; }
        private string orderID { get; set; }
        private string customerID { get; set; }
        private string shipFrom { get; set; }
        private string shipTo { get; set; }
        private int itemNo { get; set; }
        private string itemDescription { get; set; }
        private string shipmentID { get; set; }
        private int qty { get; set; }
        private string unit { get; set; }
        private decimal currency { get; set; }
        private int amount { get; set; }

        public void BillingDetail(object sender)
        {

        }
    }
}

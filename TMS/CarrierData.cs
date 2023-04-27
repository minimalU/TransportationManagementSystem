using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS
{
    internal class CarrierData : MainWindow
    {
        public string CarrierName { get; set; }
        public string depotCity { get; set; }
        public double FTLAvailability { get; set; }
        public double LTLAvailability { get; set; }
        public double FTLRate { get; set; }
        public double LTLRate { get; set; }
        public double ReeferCharge { get; set; }

        public List<CarrierData> CarrierTable()
        {
            List<CarrierData> list = new List<CarrierData>();
            return list;
        }
    }
}

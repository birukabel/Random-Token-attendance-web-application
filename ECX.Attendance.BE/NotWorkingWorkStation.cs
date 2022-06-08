using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Attendance.BE
{
    public class NotWorkingWorkStation
    {
        public Guid Id { get; set; }
        public string TradingCenter { get; set; }
        public DateTime TradeDate { get; set; }
        public string WorkinStationNumber { get; set; }
        public bool Status { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

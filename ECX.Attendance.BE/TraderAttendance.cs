using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECX.Attendance.BE
{
    public class TraderAttendance
    {
        public string TradeDate { get; set; }
         public string TradingPlatform { get; set; }
         public string SessionName { get; set; }
         public string TokenNo { get; set; }
         public string MemberName { get; set; }
         public string MemberId { get; set; }
         public string RepName { get; set; }
         public string RepId { get; set; }
         public string IsBuySell { get; set; }
         public string AttendedWithTrade { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECX.Attendance.BE
{
    public class SellerBuyerPerSession
    {
        public string TradeDate { get; set; }
        public string TradingPlatform { get; set; }
        public string SessionName { get; set; }
        public Int32 BuyerNo { get; set; }
        public Int32 SellerNo { get; set; }
    }
}
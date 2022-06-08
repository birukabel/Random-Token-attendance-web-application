using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Attendance.BE
{
    public class MemberAllowedTrade
    {
        public Guid Id { get; set; }
        public DateTime TradeDate { get; set; }
        public Guid MemberId { get; set; }
        public Guid RepId { get; set; }
        public Guid SessionId { get; set; }
        public string Token { get; set; }
        public int EnteredAs { get; set; }
        public bool IsOnline { get; set; }
        public bool Status { get; set; }
        public Guid CreatedBy { get; set; }

        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Additional { get; set; }
        public string TradingCenter { get; set; }
    }

    
}

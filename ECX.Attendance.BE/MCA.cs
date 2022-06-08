using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECX.Attendance.BE
{
    public class MCA
    {
        public Guid MemberId { get; set; }
        public Guid ClientId { get; set; }
        public Guid CommodityId { get; set; }
        public bool Active { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECX.Attendance.BE
{
    public class SessionCommodityClass
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public Guid CommodityClassId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECX.Attendance.BE
{
    public class CommodityClass
    {
        public Guid Id { get; set; }
        public Guid CommodityGuid { get; set; }
        public string Name { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECX.Attendance.BE
{
    public class Session
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CommodityClassId{ get; set; }
        public Guid CommodityGuid{ get; set; }
        public int LicenseTypeID{ get; set; }
        public string LicenseName { get; set; }
    }
}
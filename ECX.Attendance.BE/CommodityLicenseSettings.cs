using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECX.Attendance.BE
{
    public class CommodityLicenseSettings
    {
        public Guid CommodityGuid { get; set; }
        public bool Exportable { get; set; }
        public bool IsSellOrder { get; set; }
        public Int16 LicenseId { get; set; }
    }
}
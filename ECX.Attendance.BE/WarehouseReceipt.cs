using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECX.Attendance.BE
{
    public class WarehouseReceipt
    {
        public Guid Id { get; set; }
        public Guid CommodityGrade { get; set; }
        public Guid ClientId { get; set; }
    }
}
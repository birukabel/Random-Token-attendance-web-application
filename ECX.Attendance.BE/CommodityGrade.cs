using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECX.Attendance.BE
{
    public class CommodityGrade
    {
        public Guid Id { get; set; }
        public Guid CommodityClassGuid { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public bool Exportable { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECX.Attendance.BE
{
    public class License
    {
        public int LicenseId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Active { get; set; }
    }
}
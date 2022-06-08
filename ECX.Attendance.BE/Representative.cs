using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECX.Attendance.BE
{
    public class Representative
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ECXNewId { get; set; }
        public int ECXOldId { get; set; }
        public Guid MemberId { get; set; }
        public bool OnlineCertified { get; set; }
        public DateTime OnlineCertificationExpiry { get; set; }
        //Entered as values 1 = Seller, 2 = Buyer, 3 = Both
        public Int16 EnteredAs { get; set; }
        //public bool Active { get; set; }
    }
    public class RepAllowed
    {
        public DateTime TradeDate { get; set; }
        public Guid MemberId { get; set; }
        public Guid RepId { get; set; }
        public Guid SessionId { get; set; }
        public bool IsOnline { get; set; }
        //1 = Seller, 2 = Buyer, 3 = Both
        public Int16 EnteredAs { get; set; }
        public string Token { get; set; }
        public Guid CreatedBy { get; set; }
    }

    [Serializable]
    public class RepData
    {
        public Guid RepId { get; set; }
        public string ECXNewId { get; set; }
        public string RepName { get; set; }
        //1 = Seller, 2 = Buyer, 3 = Both
        public Int16 EnteredAs { get; set; }
    }

    public class RepInfo
    {
        public string RepId { get; set; }
        public string RepName { get; set; }

    }
}
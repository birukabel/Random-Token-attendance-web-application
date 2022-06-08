using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Attendance.BE
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public Guid OwnerGuid { get; set; }
        public EAccountType AccountType { get; set; }
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
    }
    public enum EAccountType
    {
        MemberPayIn, MemberPayOut, ClientPayIn, ClientPayOut
    }
}

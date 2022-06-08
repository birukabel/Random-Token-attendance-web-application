using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Attendance.BE
{
    public class Member
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ECXNewId { get; set; }
        public int ECXOldId { get; set; }
        //public bool Active { get; set; }
    }

    public class MemberInfo
    {
        public string MemberId { get; set; }
        public string MemberName { get; set; }

    }

    [Serializable]
    public class MemberData
    {
        public Guid MemberId { get; set; }
        public string ECXNewId { get; set; }
        public string MemberName { get; set; }
        public List<RepData> RepList { get; set; }
    }
}

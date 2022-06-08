using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Attendance.BE
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ECXNewId { get; set; }
        public int ECXOldId { get; set; }
        //public bool Active { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.BE;
using System.Data;
using System.Configuration;
using ECX.Attendance.DAL;

namespace ECX.Attendance.BLL
{
    public class CNSBLL
    {
        public void Load(List<Guid> clientList)
        {
            new DAL.CNSDAL().Load(clientList);
        }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.DAL;
using ECX.Attendance.BE;
using System.Data;
using System.Configuration;

namespace ECX.Attendance.BLL
{
    public class CDBLL
    {
        public void LoadCD(List<Guid> clientList, List<Guid> commodityGrades)
        {
            new DAL.CDDAL().Load(clientList, commodityGrades);
        }

       
    }
}

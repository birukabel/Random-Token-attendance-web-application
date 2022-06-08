using ECX.Attendance.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Attendance.BLL
{
    public class LookUpBLL
    {
        public void Load()
        {
            new DAL.LookUpDAL().Load();
        }
        public DataTable GetMaximumAllowedReps()
        {
            return new LookUpDAL().GetMaximumAllowedReps();
        }

        public DataTable GetCommodity()
        {
            return new LookUpDAL().GetCommodity();
        }

        public void SaveSetting(string tradingcenter, Guid commodity, int reps,Guid createdBy)
        {
            new LookUpDAL().SaveSetting(tradingcenter,commodity,reps,createdBy);
        }
    }
}

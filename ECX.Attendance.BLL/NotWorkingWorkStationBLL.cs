using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.BE;

namespace ECX.Attendance.BLL
{
    public class NotWorkingWorkStationBLL
    {
        public DataTable GetNotWorkingStationsById(Guid id)
        {
            return new  DAL.NotWorkingWorkStationDAL().GetNotWorkingStationsById(id);
        }

        public bool DeleteNotWorkingStations(Guid Id)
        {
            return new DAL.NotWorkingWorkStationDAL().DeleteNotWorkingStations(Id);
        }
        public DataTable GetWorkStationsByTradingCennterNDate(string TradingCenter, DateTime From, DateTime To)
        {
            return new DAL.NotWorkingWorkStationDAL().GetWorkStationsByTradingCennterNDate(TradingCenter, From, To);
        }

        public DataSet GetWorkStationsByDateAsDataSet(DateTime From, DateTime To)
        {
            return new DAL.NotWorkingWorkStationDAL().GetWorkStationsByDateAsDataSet(From, To);
        }

        public List<NotWorkingWorkStation> GetActiveNOTWorkingWorkStationsByTradingCenterNDate(string TradingCenter, DateTime From, DateTime To)
        {
            return new DAL.NotWorkingWorkStationDAL().GetActiveNOTWorkingWorkStationsByTradingCenterNDate(TradingCenter, From, To); 
        }

        

        public DataTable GetWorkingStationsByDate(DateTime From, DateTime To)
        {
            return new DAL.NotWorkingWorkStationDAL().GetWorkingStationsByDate(From, To);
        }

        public bool SaveNotWorkingWorkStation(string TradingCenter, DateTime TradeDate, string WorkinStationNumber, bool Status, Guid CreatedBy )
        {
            return new DAL.NotWorkingWorkStationDAL().SaveNotWorkingWorkStation(TradingCenter, TradeDate, WorkinStationNumber, Status, CreatedBy);
        }

        public DataTable GetAllWorkingStations()
        {
            return new DAL.NotWorkingWorkStationDAL().GetAllWorkingStations();
        }
    }
}

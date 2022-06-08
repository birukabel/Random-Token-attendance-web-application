using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.BE;

namespace ECX.Attendance.DAL
{
    public class NotWorkingWorkStationDAL
    {
        #region memberVariables

        #endregion

        #region memberMethods

        public DataTable GetNotWorkingStationsById(Guid Id)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@Id");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(Id);

                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetNotWorkingStationsById", paramName, paramVal, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public bool DeleteNotWorkingStations(Guid Id)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@Id");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(Id);

                string errormesg = "";
                return DataAccessProvider.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spDeleteNotWorkingStations", paramName, paramVal, ref errormesg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }


        public DataSet GetWorkStationsByDateAsDataSet(DateTime From, DateTime To)
        {
            try
            {

                ArrayList paramName = new ArrayList();

                
                paramName.Add("@From");
                paramName.Add("@To");

                ArrayList paramVal = new ArrayList();

               
                paramVal.Add(From);
                paramVal.Add(To);

                string errormesg = "";
                DataSet ds = DataAccessProvider.ExecuteDataSet(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetWorkingStationsByDateForReport", paramName, paramVal, ref errormesg);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public DataTable GetWorkStationsByTradingCennterNDate(string TradingCenter, DateTime From, DateTime To)
        {
            try
            {

                ArrayList paramName = new ArrayList();

                paramName.Add("@TradingCenter");
                paramName.Add("@From");
                paramName.Add("@To");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(TradingCenter);
                paramVal.Add(From);
                paramVal.Add(To);

                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetWorkStationsByTradingCennterNDate", paramName, paramVal, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }




        public List<NotWorkingWorkStation> GetActiveNOTWorkingWorkStationsByTradingCenterNDate(string TradingCenter, DateTime From, DateTime To)
        {
            try
            {

                ArrayList paramName = new ArrayList();

                paramName.Add("@TradingCenter");
                paramName.Add("@From");
                paramName.Add("@To");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(TradingCenter);
                paramVal.Add(From);
                paramVal.Add(To);

                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetActiveNOTWorkingWorkStationsByTradingCenterNDate", paramName, paramVal, ref errormesg);
                return DataAccessProvider.ConvertDataTable<NotWorkingWorkStation>(dt);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        
        

        public DataTable GetWorkingStationsByDate(DateTime From, DateTime To)
        {
            try
            {

                ArrayList paramName = new ArrayList();


                paramName.Add("@From");
                paramName.Add("@To");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(From);
                paramVal.Add(To);

                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetWorkingStationsByDate", paramName, paramVal, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public bool SaveNotWorkingWorkStation(string TradingCenter, DateTime TradeDate, string WorkinStationNumber, bool Status, Guid CreatedBy)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@TradingCenter");
                paramName.Add("@TradeDate");
                paramName.Add("@WorkinStationNumber");
                paramName.Add("@Status");
                paramName.Add("@CreatedBy");
                

                ArrayList paramVal = new ArrayList();
               
                paramVal.Add(TradingCenter);
                paramVal.Add(TradeDate);
                paramVal.Add(WorkinStationNumber);
                paramVal.Add(Status);
                paramVal.Add(CreatedBy);
               


                string errormesg = "";
                return DataAccessProvider.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spSaveNotWorkingWorkStation", paramName, paramVal, ref errormesg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAllWorkingStations()
        {
            try
            {

                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetAllWorkingStations", ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        #endregion
    }
}

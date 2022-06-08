using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.BE;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;

namespace ECX.Attendance.DAL
{
    public class MemberAllowedTradeDAL
    {
        public List<MemberAllowedTrade> LoadMemberAllowedToTrade(Guid MemberId, Guid SessionId)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@MemberId");
                paramName.Add("@SessionId");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(MemberId);
                paramVal.Add(SessionId);

                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetMemberAllowedToTrade", paramName, paramVal, ref errormesg);
                return DataAccessProvider.ConvertDataTable<MemberAllowedTrade>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        
        public List<MemberAllowedTrade> LoadMemberAllowedToTradeForManual(Guid MemberId, Guid SessionId)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@MemberId");
                paramName.Add("@SessionId");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(MemberId);
                paramVal.Add(SessionId);

                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetMemberAllowedToTradeForManual", paramName, paramVal, ref errormesg);
                return DataAccessProvider.ConvertDataTable<MemberAllowedTrade>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public bool DeleteAttendance(Guid repId, Guid SessionId, string TradingCenter, Guid updatedBy)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@repId");
                paramName.Add("@SessionId");
                paramName.Add("@TradingCenter");
                paramName.Add("@UpdatedBy");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(repId);
                paramVal.Add(SessionId);
                paramVal.Add(TradingCenter);
                paramVal.Add(updatedBy);

                string errormesg = "";
                return DataAccessProvider.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spDeleteAttendance", paramName, paramVal, ref errormesg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public bool CheckRepHasAttendanceRecord(Guid repId, Guid SessionId)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@RepId");
                paramName.Add("@SessionId");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(repId);
                paramVal.Add(SessionId);

                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spCheckRepHasAttendanceRecord", paramName, paramVal, ref errormesg);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public bool CheckIfAllTokensAreAssigned(int NoOfTokens, string TradingCenter, Guid SessionId)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@NoOfTokens");
                paramName.Add("@TradingCenter");
                paramName.Add("@SessionId");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(NoOfTokens);
                paramVal.Add(TradingCenter);
                paramVal.Add(SessionId);

                string errormesg = "";
                bool returnedValue =Convert.ToBoolean(DataAccessProvider.ExecuteScalar(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "fCheckIfAllTokensAreAssigned", paramName, paramVal, ref errormesg));
                return returnedValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public bool CheckTokenIsAssigned(string Token, Guid SessionId, string TradingCenter)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@Token");
                paramName.Add("@SessionId");
                paramName.Add("@TradingCenter");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(Token);
                paramVal.Add(SessionId);
                paramVal.Add(TradingCenter);

                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spCheckTokenIsAssigned", paramName, paramVal, ref errormesg);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public bool CheckMemberHasExceededMaxNoOfReps(Guid memberId, Guid SessionId,int noOfRepsAllowedForMember)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@memberId");
                paramName.Add("@SessionId");
                //paramName.Add("@noOfRepsAllowedForMember");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(memberId);
                paramVal.Add(SessionId);
                //paramVal.Add(noOfRepsAllowedForMember);

                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spCheckMemberHasExceededMaxNoOfReps", paramName, paramVal, ref errormesg);
                if (dt != null)
                {
                    if (dt.Rows.Count >= noOfRepsAllowedForMember)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public bool CheckRepIsAssigned(Guid RepId, Guid memberId, Guid SessionId)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@RepId");
                paramName.Add("@memberId");
                paramName.Add("@SessionId");

                //paramName.Add("@noOfRepsAllowedForMember");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(RepId);
                paramVal.Add(memberId);
                paramVal.Add(SessionId);

                //paramVal.Add(noOfRepsAllowedForMember);

                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spCheckRepIsAssigned", paramName, paramVal, ref errormesg);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        public bool SaveAttendance(Guid MemberId, Guid RepId, Guid SessionId, string Token, int EnteredAs,
            bool IsOnline, bool Status, Guid CreatedBy, bool Additional, string TradingCenter)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@MemberId");
                paramName.Add("@RepId");
                paramName.Add("@SessionId");
                paramName.Add("@Token");
                paramName.Add("@EnteredAs");
                paramName.Add("@IsOnline");
                paramName.Add("@Status");
                paramName.Add("@CreatedBy");
                paramName.Add("@Additional");
                paramName.Add("@TradingCenter");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(MemberId);
                paramVal.Add(RepId);
                paramVal.Add(SessionId);
                paramVal.Add(Token);
                paramVal.Add(EnteredAs);
                paramVal.Add(IsOnline);
                paramVal.Add(Status);
                paramVal.Add(CreatedBy);
                paramVal.Add(Additional);
                paramVal.Add(TradingCenter);

                string errormesg = "";
                return DataAccessProvider.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spSaveAttendance", paramName, paramVal, ref errormesg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool BulkInsertAttendanceTVP(DataTable dt)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString);
            SqlCommand command = new SqlCommand("spBulkInsertAttendanceTVP", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter sqlParam = command.Parameters.AddWithValue("@AttendanceTableTypeTVP", dt);
            sqlParam.SqlDbType = SqlDbType.Structured;

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
                command.Dispose();
                connection.Dispose();
            }
        }

        public object BulkInsertAttendanceTVPNew(Guid MemberId, Guid RepId, Guid SessionId, int EnteredAs, Guid CreatedBy, string TradingCenter, int noOfWorkStations,
            int result, Guid commdoityId)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@MemberId");
                paramName.Add("@RepId");
                paramName.Add("@SessionId");
                paramName.Add("@EnteredAs");
                paramName.Add("@CreatedBy");
                paramName.Add("@TradingCenter");
                paramName.Add("@noOfWorkStations");
                paramName.Add("@commdoityId");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(MemberId);
                paramVal.Add(RepId);
                paramVal.Add(SessionId);
                paramVal.Add(EnteredAs);
                paramVal.Add(CreatedBy);
                paramVal.Add(TradingCenter);
                paramVal.Add(noOfWorkStations);
                paramVal.Add(commdoityId);

                string errormesg = "";
                return DataAccessProvider.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spBulkInsertAttendanceTVPNew", paramName, paramVal, "@result", 0, ref errormesg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRepAssignedWorkstations(string repIdNo)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@RepIdNo");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(repIdNo);

                string errormesg = "";
                DataSet ds = DataAccessProvider.ExecuteDataSet(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetRepAssignedWorkstations", paramName, paramVal, ref errormesg);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetMemberAllowedToTradeForMember(Guid memberId, int commodityType)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@MemberId");
                paramName.Add("@CommodityType");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(memberId);
                paramVal.Add(commodityType);

                string errormesg = "";
                return DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetMemberAllowedToTradeForMember", paramName, paramVal, ref errormesg);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetAssignedRepsForMember(Guid memberId,Guid sessionId,string tradingCenter)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@MemberId");
                paramName.Add("@SessionId");
                paramName.Add("@TradingCenter");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(memberId);
                paramVal.Add(sessionId);
                paramVal.Add(tradingCenter);

                string errormesg = "";
                return (int)DataAccessProvider.ExecuteScalar(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetAssignedRepsForMember", paramName, paramVal, ref errormesg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetAssignedRepsForMemberWithOutRTC(Guid memberId, Guid sessionId)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@MemberId");
                paramName.Add("@SessionId");
               
                ArrayList paramVal = new ArrayList();

                paramVal.Add(memberId);
                paramVal.Add(sessionId);
               
                string errormesg = "";
                return (int)DataAccessProvider.ExecuteScalar(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetAssignedRepsForMemberWithOutRTC", paramName, paramVal, ref errormesg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetAssignedTokens(Guid SessionId, string TradingCenter)
        {
            try
            {

                ArrayList paramName = new ArrayList();

                paramName.Add("@SessionId");
                paramName.Add("@TradingCenter");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(SessionId);
                paramVal.Add(TradingCenter);

                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetAssignedTokens", paramName, paramVal, ref errormesg);
                return DataAccessProvider.ConvertDataTable<string>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        
    }
}

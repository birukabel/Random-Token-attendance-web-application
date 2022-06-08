using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.BE;
using ECX.Attendance.DAL;
using System.Data;
using System.Configuration;

namespace ECX.Attendance.BLL
{
    public class MemberAllowedTradeBLL
    {
        #region memberVariables

        public static ConcurrentDictionary<Guid, ConcurrentDictionary<string, List<int>>> dicAssigendTokens ;

        #endregion

        #region memberMethods

        public List<MemberAllowedTrade> LoadMemberAllowedToTrade(Guid MemberId, Guid SessionId)
        {
            return new MemberAllowedTradeDAL().LoadMemberAllowedToTrade(MemberId, SessionId);
        }

        public List<MemberAllowedTrade> LoadMemberAllowedToTradeForManual(Guid MemberId, Guid SessionId)
        {
            return new MemberAllowedTradeDAL().LoadMemberAllowedToTradeForManual(MemberId, SessionId);
        }

        public bool DeleteAttendance(Guid repId, Guid SessionId, string TradingCenter, Guid updatedBy)
        {

            return new MemberAllowedTradeDAL().DeleteAttendance(repId, SessionId, TradingCenter, updatedBy);
        }

        public bool AssigenRandomToken(MemberAllowedTrade memberAllowedTrade, int tokenNumber, string tradingCenter, int noOfWorkStations, bool isAdditional)
        {
            if (CheckRepHasAttendanceRecord(memberAllowedTrade.RepId, memberAllowedTrade.SessionId))
            {
                //int tokenNumber = GenerateRandomToken(noOfWorkStations, memberAllowedTrade.SessionId);
                if (SaveAttendance(memberAllowedTrade.MemberId, memberAllowedTrade.RepId, memberAllowedTrade.SessionId, memberAllowedTrade.Token, memberAllowedTrade.EnteredAs,
                        memberAllowedTrade.IsOnline, memberAllowedTrade.Status, memberAllowedTrade.CreatedBy, memberAllowedTrade.Additional, memberAllowedTrade.TradingCenter))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckMemberHasExceededMaxNoOfReps(Guid memberId, Guid SessionId, int noOfRepsAllowedForMember)
        {
            return new MemberAllowedTradeDAL().CheckMemberHasExceededMaxNoOfReps(memberId, SessionId, noOfRepsAllowedForMember);
        }

        public int GenerateRandomToken(int noOfWorkStations, Guid SessionId, ConcurrentDictionary<Guid,string> dic, string tradingCente)
        {
            Random _random = new Random();
            int _token = _random.Next(1, noOfWorkStations);
            List<NotWorkingWorkStation> data = new NotWorkingWorkStationBLL().GetActiveNOTWorkingWorkStationsByTradingCenterNDate(tradingCente, DateTime.Now, DateTime.Now);
            if(data.Exists(x=>Convert.ToInt32( x.WorkinStationNumber) == _token)){
                GenerateRandomToken(noOfWorkStations, SessionId,dic, tradingCente);
            }
            if (CheckTokenIsAssigned(_token.ToString(), SessionId,tradingCente) && dic.Values.Contains(_token.ToString()))
            {
                GenerateRandomToken(noOfWorkStations, SessionId, dic, tradingCente);
            }           
            return _token;
        }

        public int GenerateRandomToken(int noOfWorkStations, Guid SessionId, string tradingCenter)
        {
            Random _random = new Random();
            List<NotWorkingWorkStation> data = new NotWorkingWorkStationBLL().GetActiveNOTWorkingWorkStationsByTradingCenterNDate(tradingCenter, DateTime.Now, DateTime.Now);
            

            int _token = _random.Next(1, noOfWorkStations);
            List<int> _tokenTemp = new List<int>();
            ConcurrentDictionary<string,List<int>> dicTemp = new ConcurrentDictionary<string,List<int>>();
            //List<int> lstTokens = new List<int>();
            Dictionary<string, List<int>> dicTemp1 = new Dictionary<string, List<int>>();
            if (dicAssigendTokens == null)
                dicAssigendTokens = new ConcurrentDictionary<Guid, ConcurrentDictionary<string, List<int>>>();
            if (data.Exists(x => Convert.ToInt32(x.WorkinStationNumber) == _token))
            {
                GenerateRandomToken(noOfWorkStations, SessionId, tradingCenter);
            }
            if (CheckTokenIsAssigned(_token.ToString(), SessionId, tradingCenter))
            {
                GenerateRandomToken(noOfWorkStations, SessionId, tradingCenter);
            }

            if (dicAssigendTokens.TryGetValue(SessionId, out dicTemp))
            {
                if (dicTemp.TryGetValue(tradingCenter, out _tokenTemp))
                {
                    if (_tokenTemp.Contains(_token))
                    {
                        GenerateRandomToken(noOfWorkStations, SessionId, tradingCenter);
                    }
                    else
                    {//Proceed with other validations
                        //if (data.Exists(x => Convert.ToInt32(x.WorkinStationNumber) == _token))
                        //{
                        //    GenerateRandomToken(noOfWorkStations, SessionId, tradingCenter);
                        //}
                        //if (CheckTokenIsAssigned(_token.ToString(), SessionId, tradingCenter))
                        //{
                        //    GenerateRandomToken(noOfWorkStations, SessionId, tradingCenter);
                        //}
                        dicAssigendTokens[SessionId][tradingCenter].Add(_token);
                    }
                }
                else
                {
                    _tokenTemp.Add(_token);
                    dicAssigendTokens[SessionId].TryAdd(tradingCenter, _tokenTemp);
                }
            }
            else
            {
                //if (data.Exists(x => Convert.ToInt32(x.WorkinStationNumber) == _token))
                //{
                //    GenerateRandomToken(noOfWorkStations, SessionId, tradingCenter);
                //}
                //if (CheckTokenIsAssigned(_token.ToString(), SessionId, tradingCenter))
                //{
                //    GenerateRandomToken(noOfWorkStations, SessionId, tradingCenter);
                //}
                _tokenTemp.Add(_token);
                dicTemp = new ConcurrentDictionary<string, List<int>>();
                if(dicTemp.TryAdd(tradingCenter, _tokenTemp))
                    dicAssigendTokens.TryAdd(SessionId, dicTemp);
            }
            return _token;
        }

        public bool CheckIfAllTokensAreAssigned(int NoOfTokens, string TradingCenter, Guid SessionId)
        {
            return new MemberAllowedTradeDAL().CheckIfAllTokensAreAssigned(NoOfTokens, TradingCenter, SessionId);
        }

        public List<string> GetAssignedTokens(Guid SessionId, string TradingCenter)
        {
            return new DAL.MemberAllowedTradeDAL().GetAssignedTokens(SessionId, TradingCenter);
        }

        public bool CheckRepHasAttendanceRecord(Guid repId, Guid SessionId)
        {
            return new MemberAllowedTradeDAL().CheckRepHasAttendanceRecord(repId, SessionId);
        }


        public bool CheckRepIsAssigned(Guid RepId, Guid memberId, Guid SessionId)
        {
            return new MemberAllowedTradeDAL().CheckRepIsAssigned(RepId, memberId, SessionId);
        }
        public bool CheckTokenIsAssigned(String Token, Guid SessionId,string TradingCenter)
        {
            return new MemberAllowedTradeDAL().CheckTokenIsAssigned(Token, SessionId,TradingCenter);
        }

        public bool SaveAttendance(Guid MemberId, Guid RepId, Guid SessionId, string Token, int EnteredAs,
           bool IsOnline, bool Status, Guid CreatedBy, bool Additional, string TradingCenter)
        {
            return new MemberAllowedTradeDAL().SaveAttendance(MemberId, RepId, SessionId, Token, EnteredAs,
                IsOnline, Status, CreatedBy, Additional, TradingCenter);
        }

        public object BulkInsertAttendanceTVPNew(Guid MemberId, Guid RepId, Guid SessionId, int EnteredAs, Guid CreatedBy, string TradingCenter, int noOfWorkStations,
           int result, Guid commdoityId)
        {
            return new MemberAllowedTradeDAL().BulkInsertAttendanceTVPNew(MemberId, RepId, SessionId, EnteredAs, CreatedBy, TradingCenter, noOfWorkStations, result, commdoityId);
        }

        public DataTable PrepareAttendanceDataTable(List<MemberAllowedTrade> lst)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(Guid)));
            dt.Columns.Add(new DataColumn("TradeDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("MemberId", typeof(Guid)));
            dt.Columns.Add(new DataColumn("RepId", typeof(Guid)));
            dt.Columns.Add(new DataColumn("SessionId", typeof(Guid)));
            dt.Columns.Add(new DataColumn("Token", typeof(string)));
            dt.Columns.Add(new DataColumn("EnteredAs", typeof(int)));
            dt.Columns.Add(new DataColumn("IsOnline", typeof(bool)));
            dt.Columns.Add(new DataColumn("Status", typeof(bool)));
            dt.Columns.Add(new DataColumn("CreatedBy", typeof(Guid)));
            dt.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("UpdatedBy", typeof(Guid)));
            dt.Columns.Add(new DataColumn("UpdatedDate", typeof(DateTime)));        
            dt.Columns.Add(new DataColumn("TradingCenter", typeof(string)));
            dt.Columns.Add(new DataColumn("Additional", typeof(bool)));
            
            foreach (MemberAllowedTrade obj in lst)
            {
                DataRow row = dt.NewRow();
                row["Id"] = obj.Id;
                row["TradeDate"] = obj.TradeDate;
                row["MemberId"] = obj.MemberId;
                row["RepId"] = obj.RepId;
                row["SessionId"] =obj.SessionId;
                row["Token"] = obj.Token;
                row["EnteredAs"] = obj.EnteredAs;
                row["IsOnline"] = obj.IsOnline;
                row["Status"] = obj.Status;
                row["CreatedBy"] = obj.CreatedBy;
                row["CreatedDate"] = obj.CreatedDate;
                row["UpdatedBy"] = obj.UpdatedBy;
                row["UpdatedDate"] = obj.UpdatedDate;
                row["TradingCenter"] = obj.TradingCenter;
                row["Additional"] = obj.Additional;                

                dt.Rows.Add(row);
            }

            return dt;
        }

        public bool BulkInsertAttendanceTVP(List<MemberAllowedTrade> lst)
        {
            return new MemberAllowedTradeDAL().BulkInsertAttendanceTVP(PrepareAttendanceDataTable(lst));
        }

        public DataSet GetRepAssignedWorkstations(string repIdNo)
        {
            return new MemberAllowedTradeDAL().GetRepAssignedWorkstations(repIdNo);
        }

        public DataTable GetMemberAllowedToTradeForMember(Guid memberId, int commodityType)
        {
            return new MemberAllowedTradeDAL().GetMemberAllowedToTradeForMember(memberId,commodityType);
        }
        public int GetAssignedRepsForMember(Guid memberId, Guid sessionId, string tradingCenter)
        { 
            return new MemberAllowedTradeDAL().GetAssignedRepsForMember( memberId, sessionId, tradingCenter);
        
        }

        public int GetAssignedRepsForMemberWithOutRTC(Guid memberId, Guid sessionId)
        {
            return new MemberAllowedTradeDAL().GetAssignedRepsForMemberWithOutRTC(memberId, sessionId);
        }
      
        #endregion

    }
}

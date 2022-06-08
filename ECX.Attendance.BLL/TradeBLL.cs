using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.DAL;
using ECX.Attendance.BE;

namespace ECX.Attendance.BLL
{
    public class TradeBLL
    {
        public TradeBLL()
        {

        }
        public void Load()
        {
            new DAL.TradeDAL().Load();
        }
        public DataTable GetCurrentSession()
        {
            return new DAL.TradeDAL().GetCurrentSession();
        }
        public DataTable GetCurrentSession(DateTime tradeDateFrom, DateTime tradeDateTo)
        {
            return new DAL.TradeDAL().GetCurrentSession(tradeDateFrom, tradeDateTo);
        }
        public DataTable GetSessionData(Guid MemberId, Guid SessionId, bool IsOnline)
        {
            return new DAL.TradeDAL().GetSessionData(MemberId, SessionId, IsOnline);
        }
        public List<TraderAttendance> AttendanceData(DateTime FromDate, DateTime ToDate, string platform, string session, string MID, string RID, string TradeAttend)
        {
            return new DAL.TradeDAL().AttendanceData(FromDate, ToDate, platform, session, MID, RID, TradeAttend);
        }

        internal List<Session> LoadSession()
        {
            return new DAL.TradeDAL().LoadSession();
        }
        
        public DataTable GetTodaySession()
        {
            return new DAL.TradeDAL().GetTodaySession();
        }

        public DataTable GetAttendanceReport(DateTime From, DateTime To, Guid SessionId, Guid MemberId, Guid RepId, int AttenedWith)
        {
            return new DAL.TradeDAL().GetAttendanceReport(From, To, SessionId, MemberId, RepId, AttenedWith);
        }
        public DataSet GetAttendanceReport(DateTime From, DateTime To, int AttenedWith)
        {
            return new DAL.TradeDAL().GetAttendanceReport(From, To, AttenedWith);
        }

        public DataTable GetBuyerSellerPerSession(DateTime From, DateTime To, string SessionName)
        {
            return new DAL.TradeDAL().GetBuyerSellerPerSession(From, To, SessionName);
        }

        public DataTable GetAttendanceWithNoOrderReport(DateTime From, DateTime To)
        {
            return new DAL.TradeDAL().GetAttendanceWithNoOrderReport(From, To);
        }

        public DataTable GetOrderWithNoAttendanceReport(DateTime From, DateTime To)
        {
            return new DAL.TradeDAL().GetOrderWithNoAttendanceReport(From, To);
        }

        public DataSet GetTradesByDateRange(DateTime From, DateTime To)
        {
            return new DAL.TradeDAL().GetTradesByDateRange(From, To);
        }

        public DataTable GetOrdersWithNoAttendance(DateTime From, DateTime To)
        {
            return new DAL.TradeDAL().GetOrdersWithNoAttendance(From, To);
        }

        public DataTable GetAuctionSellOrdersWithNoAttendance(DateTime From, DateTime To)
        {
            return new DAL.TradeDAL().GetAuctionSellOrdersWithNoAttendance(From, To);
        }

        public DataTable GetAuctionBuyOrdersWithNoAttendance(DateTime From, DateTime To)
        {
            return new DAL.TradeDAL().GetAuctionBuyOrdersWithNoAttendance(From, To);
        }
        public bool SaveNMDTRegionalRTC(Guid MemberID, string MemberIdNo, string MemberName, string RegionalRTC, Guid CreatedBy)
        {
            return new DAL.TradeDAL().SaveNMDTRegionalRTC(MemberID, MemberIdNo, MemberName, RegionalRTC, CreatedBy);
        }
        public bool UpdateNMDTRegionalRTC(Guid ID, Guid MemberID, string MemberIdNo, string MemberName, string RegionalRTC, Guid UpdatedBy)
        {
            return new DAL.TradeDAL().UpdateNMDTRegionalRTC(ID, MemberID, MemberIdNo, MemberName, RegionalRTC, UpdatedBy);
        }
        public DataTable GetRegionalTradingCenter()
        {
            return new DAL.TradeDAL().GetRegionalTradingCenter();
        }

        public DataTable GetAllNMDTAssigenedToRTC()
        {
            return new DAL.TradeDAL().GetAllNMDTAssigenedToRTC();
        }

        public DataTable GetNMDTRtc(string strRTC)
        {
            return new DAL.TradeDAL().GetNMDTRtc(strRTC);
        }

        public DataTable GetAssignedRTc(string memberOldId)
        {
            return new DAL.TradeDAL().GetNMDTAssignedRtc(memberOldId);
        }

        public bool SaveExceptionLog(string ExceptionType, string ShortMessage, string FullMessage, string UserName, Guid UserGuid, string Remark, string ExceptionSource)
        {
            return new DAL.TradeDAL().SaveExceptionLog(ExceptionType, ShortMessage, FullMessage, UserName, UserGuid, Remark, ExceptionSource);
        }
    }
}

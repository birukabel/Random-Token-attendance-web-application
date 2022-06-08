using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.BE;
using System.Collections;

namespace ECX.Attendance.DAL
{
    public class TradeDAL
    {
        public Dictionary<Guid, Session> Sessions = new Dictionary<Guid, Session>();
        public Dictionary<Guid, Dictionary<Guid, SessionCommodityClass>> SessionCommodityClasses = new Dictionary<Guid, Dictionary<Guid, SessionCommodityClass>>();
        public Dictionary<string, CommodityLicenseSettings> CommodityLicenseSettings = new Dictionary<string, CommodityLicenseSettings>();

        public Dictionary<Guid, string> SessionDic = new Dictionary<Guid, string>();
        public List<TraderAttendance> TraderAttList = new List<TraderAttendance>();
        public List<SellerBuyerPerSession> PerSessionList = new List<SellerBuyerPerSession>();

        public MembershipDAL membershipDAL = new MembershipDAL();
        public TradeDAL()
        { }
        public void Load()
        {
            try
            {
                string errormesg = "";
                DataSet dsTrade = DataAccessProvider.ExecuteDataSet(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spTradeLoadAttendance", null, null, ref errormesg);

                if (dsTrade.Tables.Count > 0)
                {
                    DataTable tblSession = dsTrade.Tables[0];
                    foreach (DataRow r in tblSession.Rows)
                    {
                        Session s = new Session();
                        s.Id = new Guid(r["Id"].ToString());
                        s.Name = r["Name"].ToString();
                        Sessions.Add(s.Id, s);
                    }
                }

                if (dsTrade.Tables.Count > 1)
                {
                    DataTable tblSessionCommodityClass = dsTrade.Tables[1];
                    foreach (DataRow r in tblSessionCommodityClass.Rows)
                    {
                        SessionCommodityClass scc = new SessionCommodityClass();
                        scc.Id = new Guid(r["Id"].ToString());
                        scc.SessionId = new Guid(r["SessionId"].ToString());
                        scc.CommodityClassId = new Guid(r["CommodityClassId"].ToString());
                        Dictionary<Guid, SessionCommodityClass> temp = null;
                        if (!SessionCommodityClasses.TryGetValue(scc.SessionId, out temp))
                        {
                            temp = new Dictionary<Guid, SessionCommodityClass>();
                            SessionCommodityClasses.Add(scc.SessionId, temp);
                        }
                        if (!temp.ContainsKey(scc.CommodityClassId))
                        {
                            temp.Add(scc.CommodityClassId, scc);
                        }
                    }
                }
                if (dsTrade.Tables.Count > 2)
                {
                    DataTable tblCLS = dsTrade.Tables[2];
                    foreach (DataRow r in tblCLS.Rows)
                    {
                        try
                        {
                            CommodityLicenseSettings cls = new CommodityLicenseSettings();
                            cls.CommodityGuid = new Guid(r["CommodityGuid"].ToString());
                            cls.Exportable = Convert.ToBoolean(r["Exportable"]);
                            cls.IsSellOrder = Convert.ToBoolean(r["IsSellOrder"]);
                            cls.LicenseId = Convert.ToInt16(r["LicenseId"]);
                            if (!CommodityLicenseSettings.ContainsKey(cls.CommodityGuid.ToString() + "-" + cls.Exportable.ToString() + "-" + cls.IsSellOrder.ToString()))
                            {
                                CommodityLicenseSettings.Add(cls.CommodityGuid.ToString() + "-" + cls.Exportable.ToString() + "-" + cls.IsSellOrder.ToString(), cls);
                            }
                        }
                        catch (Exception ex)
                        {

                            //throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public DataTable GetCurrentSession()
        {
            try
            {
                string errormesg = "";
                var dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spCurrentSessionLoadAttendance", ref errormesg); //.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spCurrentSessionLoadAttendance", null, null, ref errormesg);
                return dt;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataTable GetCurrentSession(DateTime tradeDateFrom, DateTime tradeDateTo)
        {
            try
            {
                string errormesg = "";
                ArrayList paramArrayList = new ArrayList()
            {
                "@tradeDateFrom","@tradeDateTo"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                tradeDateFrom,tradeDateTo
            };

                return DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spCurrentSessionLoadAttendanceByDate", paramArrayList, paramValArrayList, ref errormesg);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataTable GetSessionData(Guid MemberId, Guid SessionId, bool IsOnline)
        {
            try
            {
                string errormesg = "";
                ArrayList paramArrayList = new ArrayList()
            {
                "@MemberId","@SessionId","@IsOnline"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                MemberId,SessionId,IsOnline
            };
                return DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spSessionDataLoadAttendance", paramArrayList, paramValArrayList, ref errormesg);
            }
            catch (Exception ex) 
            { 
                throw; 
            }
        }
        public List<TraderAttendance> AttendanceData(DateTime FromDate, DateTime ToDate, string platform, string session, string MID, string RID, string TradeAttend)
        {
            try
            {
                membershipDAL.LoadMembership(MID, RID);

                string errormesg = "";
                ArrayList paramArrayList = new ArrayList()
            {
                "@FromDate","@ToDate","@platform","@session","@MID","@RID","@TradeAttend"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                FromDate,ToDate,platform,session,MID,RID,TradeAttend
            };
                DataSet dset = DataAccessProvider.ExecuteDataSet(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spMembershipDataForAttendanceReport", paramArrayList, paramValArrayList, ref errormesg);


                if (dset.Tables.Count > 0)
                {
                    DataTable tblSession = dset.Tables[0];
                    foreach (DataRow r in tblSession.Rows)
                    {
                        if (!SessionDic.ContainsKey(new Guid(r["SessionId"].ToString())))
                        {
                            SessionDic.Add(new Guid(r["SessionId"].ToString()), r["Name"].ToString());
                        }
                    }
                }


                if (dset.Tables.Count > 1)
                {
                    DataTable tblAll = dset.Tables[1];
                    foreach (DataRow r in tblAll.Rows)
                    {
                        TraderAttendance tad = new TraderAttendance();
                        tad.TradeDate = r["TradeDate"].ToString();
                        if (membershipDAL.MemberInfoDic.ContainsKey(new Guid(r["MemberId"].ToString())))
                        {
                            tad.MemberId = membershipDAL.MemberInfoDic[new Guid(r["MemberId"].ToString())].MemberId;
                            tad.MemberName = membershipDAL.MemberInfoDic[new Guid(r["MemberId"].ToString())].MemberName;
                        }
                        if (membershipDAL.RepInfoDic.ContainsKey(new Guid(r["RepId"].ToString())))
                        {
                            tad.RepId = membershipDAL.RepInfoDic[new Guid(r["RepId"].ToString())].RepId;
                            tad.RepName = membershipDAL.RepInfoDic[new Guid(r["RepId"].ToString())].RepName;
                        }

                        tad.TradingPlatform = r["Platfrm"].ToString();
                        if (SessionDic.ContainsKey(new Guid(r["SessionId"].ToString())))
                        {
                            tad.SessionName = SessionDic[new Guid(r["SessionId"].ToString())];
                        }

                        tad.TokenNo = r["Token"].ToString();
                        tad.IsBuySell = r["IsBuySell"].ToString();
                        tad.AttendedWithTrade = r["AttendTrade"].ToString();

                        TraderAttList.Add(tad);
                    }
                }
                return TraderAttList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Session> LoadSession()
        {
            try
            {
                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetSessionDataForAttendance",  ref errormesg);
                return DataAccessProvider.ConvertDataTable<Session>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetTodaySession()
        {
            try
            {
                string errormesg = "";
                return DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetTodaySessionforReport", ref errormesg);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAttendanceReport(DateTime From, DateTime To, Guid SessionId, Guid MemberId, Guid RepId, int AttenedWith)
        {
            string errormesg = "";
            try
            {

                ArrayList paramArrayList = new ArrayList()
            {
                "@From","@To","@SessionId","@MemberId","@RepId","@AttenedWith"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                From,To,SessionId,MemberId,RepId,AttenedWith
            };

                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetAttendanceReport", paramArrayList, paramValArrayList, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAttendanceReport(DateTime From, DateTime To, int AttenedWith)
        {
            string errormesg = "";
            try
            {

                ArrayList paramArrayList = new ArrayList()
            {
                "@From","@To","@AttenedWith"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                From,To,AttenedWith
            };

                DataSet dt = DataAccessProvider.ExecuteDataSet(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetAttendanceReport", paramArrayList, paramValArrayList, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBuyerSellerPerSession(DateTime From, DateTime To, string SessionName)
        {
            string errormesg = "";
            try
            {

                ArrayList paramArrayList = new ArrayList()
            {
                "@From","@To","@SessionName"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                From,To,SessionName
            };

                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetBuyerSellerPerSession", paramArrayList, paramValArrayList, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetAttendanceWithNoOrderReport(DateTime From, DateTime To)
        {
            string errormesg = "";
            try
            {

                ArrayList paramArrayList = new ArrayList()
            {
                "@StartDate","@EndDate"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                From,To
            };

                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetAttendanceWithNoOrderReport", paramArrayList, paramValArrayList, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetOrderWithNoAttendanceReport(DateTime From, DateTime To)
        {
            string errormesg = "";
            try
            {

                ArrayList paramArrayList = new ArrayList()
            {
                "@StartDate","@EndDate"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                From,To
            };

                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetOrderWithNoAttendanceReport", paramArrayList, paramValArrayList, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTradesByDateRange(DateTime From, DateTime To)
        {
            string errormesg = "";
            try
            {

                ArrayList paramArrayList = new ArrayList()
            {
                "@StartDate","@EndDate"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                From,To
            };

                DataSet dt = DataAccessProvider.ExecuteDataSet(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetTradesByDateRange", paramArrayList, paramValArrayList, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable GetOrdersWithNoAttendance(DateTime From, DateTime To)
        {
            string errormesg = "";
            try
            {

                ArrayList paramArrayList = new ArrayList()
            {
                "@StartDate","@EndDate"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                From,To
            };

                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetOrdersWithNoAttendance", paramArrayList, paramValArrayList, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable GetAuctionSellOrdersWithNoAttendance(DateTime From, DateTime To)
        {
            string errormesg = "";
            try
            {

                ArrayList paramArrayList = new ArrayList()
            {
                "@StartDate","@EndDate"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                From,To
            };

                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetAuctionSellOrdersWithNoAttendance", paramArrayList, paramValArrayList, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetAuctionBuyOrdersWithNoAttendance(DateTime From, DateTime To)
        {
            string errormesg = "";
            try
            {

                ArrayList paramArrayList = new ArrayList()
            {
                "@StartDate","@EndDate"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                From,To
            };

                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetAuctionBuyOrdersWithNoAttendance", paramArrayList, paramValArrayList, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveNMDTRegionalRTC(Guid MemberID, string MemberIdNo, string MemberName, string RegionalRTC, Guid CreatedBy)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@MemberID");
                paramName.Add("@MemberIdNo");
                paramName.Add("@MemberName");
                paramName.Add("@RegionalRTC");
                paramName.Add("@CreatedBy");
               

                ArrayList paramVal = new ArrayList();

                paramVal.Add(MemberID);
                paramVal.Add(MemberIdNo);
                paramVal.Add(MemberName);
                paramVal.Add(RegionalRTC);
                paramVal.Add(CreatedBy);
                

                string errormesg = "";
                return DataAccessProvider.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spSaveNMDTRegionalRTC", paramName, paramVal, ref errormesg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateNMDTRegionalRTC(Guid ID,Guid MemberID, string MemberIdNo, string MemberName, string RegionalRTC, Guid UpdatedBy)
        {
        
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@ID");
                paramName.Add("@MemberID");
                paramName.Add("@MemberIdNo");
                paramName.Add("@MemberName");
                paramName.Add("@RegionalRTC");
                paramName.Add("@UpdatedBy");
               

                ArrayList paramVal = new ArrayList();
                paramVal.Add(ID);
                paramVal.Add(MemberID);
                paramVal.Add(MemberIdNo);
                paramVal.Add(MemberName);
                paramVal.Add(RegionalRTC);
                paramVal.Add(UpdatedBy);
                

                string errormesg = "";
                return DataAccessProvider.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spUpdateNMDTRegionalRTC", paramName, paramVal, ref errormesg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetRegionalTradingCenter()
        {
            string errormesg = "";
            try
            {
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetRegionalTradingCenter", ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAllNMDTAssigenedToRTC()
        {
            string errormesg = "";
            try
            {
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetAllNMDTAssigenedToRTC", ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetNMDTRtc(string strRTC)
        {
            string errormesg = "";
            try
            {

                ArrayList paramArrayList = new ArrayList()
            {
                "@RegionalRTC"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                strRTC
            };

                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetNMDTRtc", paramArrayList, paramValArrayList, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetNMDTAssignedRtc(string memberOldId)
        {
            string errormesg = "";
            try
            {

                ArrayList paramArrayList = new ArrayList()
            {
                "@MemberId"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                memberOldId
            };

                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetAssignedNMDTRTc", paramArrayList, paramValArrayList, ref errormesg);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveExceptionLog(string ExceptionType, string ShortMessage, string FullMessage, string UserName, Guid UserGuid, string Remark, string ExceptionSource)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@ExceptionType");
                paramName.Add("@ShortMessage");
                paramName.Add("@FullMessage");
                paramName.Add("@UserName");
                paramName.Add("@UserGuid");
                paramName.Add("@Remark");
                paramName.Add("@ExceptionSource");


                ArrayList paramVal = new ArrayList();

                paramVal.Add(ExceptionType);
                paramVal.Add(ShortMessage);
                paramVal.Add(FullMessage);
                paramVal.Add(UserName);
                paramVal.Add(UserGuid);
                paramVal.Add(Remark);
                paramVal.Add(ExceptionSource);

                string errormesg = "";
                return DataAccessProvider.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spSaveExceptionLog", paramName, paramVal, ref errormesg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

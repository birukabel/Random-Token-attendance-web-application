using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.BE;
using ECX.Attendance.DAL;
using System.Configuration;

namespace ECX.Attendance.BLL
{
    public class InitBLL
    {
        #region member Variables

        static List<Member> _memberList;
        static List<Client> _client;
        static List<Representative> _representative;
        static List<MCA> _mca;
        static List<License> _clientLicense;
        static List<License> _memberLicense;
        static List<BankAccount> _bankAccount;
        static List<WarehouseReceipt> _warehouseReceipt;
        static List<Session> _session;


        static DateTime _updatedTimeData=DateTime.Now;
        static bool firstTimeUpdateData = true;

        static DateTime _clientupdatedTimeData = DateTime.Now;
        static bool clientfirstTimeUpdateData = true;

        static DateTime _memberLicenseupdatedTimeData = DateTime.Now;
        static bool memberLicensefirstTimeUpdateData = true;

        static DateTime _mcaupdatedTimeData = DateTime.Now;
        static bool mcafirstTimeUpdateData = true;

        static DateTime _clientlicenseupdatedTimeData = DateTime.Now;
        static bool clientlicensefirstTimeUpdateData = true;

        static DateTime _representativeupdatedTimeData = DateTime.Now;
        static bool representativefirstTimeUpdateData = true;

        static DateTime _bankaccountupdatedTimeData = DateTime.Now;
        static bool bankaccountfirstTimeUpdateData = true;

        static DateTime _whrupdatedTimeData = DateTime.Now;
        static bool whrfirstTimeUpdateData = true;

        static DateTime _sessionupdatedTimeData=DateTime.Now;
        static bool sessionfirstTimeUpdateData = true;

        private static List<Member> MemberData
        {
            get
            {
                if (_memberList == null)
                {
                    _memberList = new List<Member>();
                }
                if (DateTime.Now.Subtract(_updatedTimeData).TotalMinutes > Convert.ToInt32(ConfigurationManager.AppSettings["CacheRefreshingRateInMinute"].ToString()))
                {
                    _memberList = new List<Member>();
                    _memberList = new MembershipDAL().LoadMember();
                    _updatedTimeData = DateTime.Now;
                }
                if (firstTimeUpdateData)
                {
                    _memberList = new List<Member>();
                    _memberList = new MembershipDAL().LoadMember();
                    _updatedTimeData = DateTime.Now;
                    firstTimeUpdateData = false;
                }
                return _memberList;
            }
        }

        private static List<Client> ClientData
        {
            get
            {
                if (_client == null)
                {
                    _client = new List<Client>();
                }
                if (DateTime.Now.Subtract(_clientupdatedTimeData).TotalMinutes > Convert.ToInt32(ConfigurationManager.AppSettings["CacheRefreshingRateInMinute"].ToString()))
                {
                    _client = new List<Client>();
                    _client = new MembershipDAL().LoadClient();
                    _clientupdatedTimeData = DateTime.Now;
                }
                if (clientfirstTimeUpdateData)
                {
                    _client = new List<Client>();
                    _client = new MembershipDAL().LoadClient();
                    _clientupdatedTimeData = DateTime.Now;
                    clientfirstTimeUpdateData = false;
                }
                return _client;
            }
        }

        private static List<Representative> RepresentativeData
        {
            get
            {
                if (_representative == null)
                {
                    _representative = new List<Representative>();
                }
                if (DateTime.Now.Subtract(_representativeupdatedTimeData).TotalMinutes > Convert.ToInt32(ConfigurationManager.AppSettings["CacheRefreshingRateInMinute"].ToString()))
                {
                    _representative = new List<Representative>();
                    _representative = new MembershipDAL().LoadRepresentative();
                    _representativeupdatedTimeData = DateTime.Now;
                }
                if (representativefirstTimeUpdateData)
                {
                    _representative = new List<Representative>();
                    _representative = new MembershipDAL().LoadRepresentative();
                    _representativeupdatedTimeData = DateTime.Now;
                    representativefirstTimeUpdateData = false;
                }
                return _representative;
            }
        }

        private static List<MCA> MCAData
        {
            get
            {
                if (_mca == null)
                {
                    _mca = new List<MCA>();
                }
                if (DateTime.Now.Subtract(_mcaupdatedTimeData).TotalMinutes > Convert.ToInt32(ConfigurationManager.AppSettings["CacheRefreshingRateInMinute"].ToString()))
                {
                    _mca = new List<MCA>();
                    _mca = new MembershipDAL().LoadMCA();
                    _mcaupdatedTimeData = DateTime.Now;
                }
                if (mcafirstTimeUpdateData)
                {
                    _mca = new List<MCA>();
                    _mca = new MembershipDAL().LoadMCA();
                    _mcaupdatedTimeData = DateTime.Now;
                    mcafirstTimeUpdateData = false;
                }
                return _mca;
            }
        }

        private static List<License> ClientLicenseData
        {
            get
            {
                if (_clientLicense == null)
                {
                    _clientLicense = new List<License>();
                }
                if (DateTime.Now.Subtract(_clientlicenseupdatedTimeData).TotalMinutes > Convert.ToInt32(ConfigurationManager.AppSettings["CacheRefreshingRateInMinute"].ToString()))
                {
                    _clientLicense = new List<License>();
                    _clientLicense = new MembershipDAL().LoadClientLicense();
                    _clientlicenseupdatedTimeData = DateTime.Now;
                }
                if (clientlicensefirstTimeUpdateData)
                {
                    _clientLicense = new List<License>();
                    _clientLicense = new MembershipDAL().LoadClientLicense();
                    _clientlicenseupdatedTimeData = DateTime.Now;
                    clientfirstTimeUpdateData = false;
                }
                return _clientLicense;
            }
        }

        private static List<License> MemberLicenseData
        {
            get
            {
                if (_memberLicense == null)
                {
                    _memberLicense = new List<License>();
                }
                if (DateTime.Now.Subtract(_memberLicenseupdatedTimeData).TotalMinutes > Convert.ToInt32(ConfigurationManager.AppSettings["CacheRefreshingRateInMinute"].ToString()))
                {
                    _memberLicense = new List<License>();
                    _memberLicense = new MembershipDAL().LoadMemberLicense();
                    _memberLicenseupdatedTimeData = DateTime.Now;
                }
                if (memberLicensefirstTimeUpdateData)
                {
                    _memberLicense = new List<License>();
                    _memberLicense = new MembershipDAL().LoadMemberLicense();
                    _memberLicenseupdatedTimeData = DateTime.Now;
                    memberLicensefirstTimeUpdateData = false;
                }
                return _memberLicense;
            }
        }

        private static List<BankAccount> BankAccountData
        {
            get
            {
                if (_bankAccount == null)
                {
                    _bankAccount = new List<BankAccount>();
                }
                if (DateTime.Now.Subtract(_bankaccountupdatedTimeData).TotalMinutes > Convert.ToInt32(ConfigurationManager.AppSettings["CacheRefreshingRateInMinute"].ToString()))
                {
                    _bankAccount = new List<BankAccount>();
                    _bankAccount = new CNSDAL().LoadBankAccount();
                    _bankaccountupdatedTimeData = DateTime.Now;
                }
                if (bankaccountfirstTimeUpdateData)
                {
                    _bankAccount = new List<BankAccount>();
                    _bankAccount = new CNSDAL().LoadBankAccount();
                    _bankaccountupdatedTimeData = DateTime.Now;
                    bankaccountfirstTimeUpdateData = false;
                }
                return _bankAccount;
            }
        }

        private static List<WarehouseReceipt> WHRData
        {
            get
            {
                if (_warehouseReceipt == null)
                {
                    _warehouseReceipt = new List<WarehouseReceipt>();
                }
                if (DateTime.Now.Subtract(_whrupdatedTimeData).TotalMinutes > Convert.ToInt32(ConfigurationManager.AppSettings["CacheRefreshingRateInMinute"].ToString()))
                {
                    _warehouseReceipt = new List<WarehouseReceipt>();
                    _warehouseReceipt = new CDDAL().LoadWHR();
                    _whrupdatedTimeData = DateTime.Now;
                }
                if (whrfirstTimeUpdateData)
                {
                    _warehouseReceipt = new List<WarehouseReceipt>();
                    _warehouseReceipt = new CDDAL().LoadWHR();
                    _whrupdatedTimeData = DateTime.Now;
                    whrfirstTimeUpdateData = false;
                }
                return _warehouseReceipt;
            }
        }
        private static List<Session> SessionData
        {
            get
            {
                if (_session == null)
                {
                    _session = new List<Session>();
                }
                if (DateTime.Now.Subtract(_sessionupdatedTimeData).TotalMinutes > Convert.ToInt32(ConfigurationManager.AppSettings["SessionCacheRefreshingRateInMinute"].ToString()))
                {
                    _session = new List<Session>();
                    _session = new TradeBLL().LoadSession();
                    _sessionupdatedTimeData = DateTime.Now;
                }
                if (sessionfirstTimeUpdateData)
                {
                    _session = new List<Session>();
                    _session = new TradeBLL().LoadSession();
                    _sessionupdatedTimeData = DateTime.Now;
                    sessionfirstTimeUpdateData = false;
                }
                return _session;
            }
        }

        #endregion
        
        
        #region member_Method

        public static List<Member> GetAllMemberData()
        {
            return MemberData;
        }

        public static List<Client> GetAllClientData()
        {
            return ClientData;
        }

        public static List<Representative> GetAllRepresentativeData()
        {
            return RepresentativeData;
        }

        public static List<MCA> GetAllMCAData()
        {
            return MCAData;
        }

        public static List<License> GetAllMemberLicense()
        {
            return MemberLicenseData;
        }

        public static List<License> GetAllClientLicense()
        {
            return ClientLicenseData;
        }

        public static List<BankAccount> GetAllBankAccount()
        {
            return BankAccountData;
        }

        public static List<WarehouseReceipt> GetAllWHR()
        {
            return WHRData;
        }
        public static List<Session> GetAllSession()
        {
            return SessionData;
        }

        public static void InitAllCache()
        {
            GetAllMemberData();
            GetAllClientData();
            GetAllMCAData();
            GetAllRepresentativeData();
            GetAllMemberLicense();
            GetAllClientLicense();
            GetAllBankAccount();
            GetAllWHR();
            GetAllSession();
        }

        public static Member GetEligibleMemeber(string mid, Guid sessionId)
        {
            Member data = GetAllMemberData().Where(x => x.ECXOldId == Convert.ToInt32(mid)).FirstOrDefault();
            if (data != null)
            {
                //List<Session> lstse = GetAllSession().Where(s => s.Id == sessionId).ToList();
                //List<License> lstlice = GetAllMemberLicense().Where(x => x.ClientId == data.Id).ToList();
                if (GetAllSession().Exists(s=>s.Id==sessionId && GetAllMemberLicense().Exists(x=>x.LicenseId==s.LicenseTypeID)))
                {
                    //if(GetAll)
                    if (GetAllBankAccount().Exists(x => x.Balance > 0 && x.OwnerGuid == data.Id))
                    {
                        return data;
                    }
                    else
                    {
                        if (GetAllWHR().Exists(x => x.ClientId == data.Id))
                        {
                            return data;
                        }
                        else
                        {
                            var mca = GetAllMCAData().Where(x => x.MemberId == data.Id).ToList();
                            if (mca != null)
                            {
                                if (GetAllSession().Exists(s => s.Id == sessionId && GetAllClientLicense().Exists(x => x.LicenseId == s.LicenseTypeID && mca.Exists(y => y.ClientId == x.ClientId))))
                                {
                                    if (GetAllBankAccount().Exists(x => mca.Exists(y => y.ClientId == x.OwnerGuid) && x.Balance > 0))
                                    {
                                        return data;
                                    }
                                    else
                                    {
                                        if (GetAllWHR().Exists(x => mca.Exists(y => y.ClientId == x.ClientId)))
                                        {
                                            return data;
                                        }
                                        else
                                            return null;
                                    }
                                }
                            }
                            else
                                return null;
                        }
                    }
                }
                else
                {
                    var mca = GetAllMCAData().Where(x => x.MemberId == data.Id).ToList();
                    if (mca != null)
                    {
                        if (GetAllSession().Exists(s => s.Id == sessionId && GetAllClientLicense().Exists(x => x.LicenseId == s.LicenseTypeID && mca.Exists(y => y.ClientId == x.ClientId))))
                        {
                            if (GetAllBankAccount().Exists(x => mca.Exists(y => y.ClientId == x.OwnerGuid) && x.Balance > 0))
                            {
                                return data;
                            }
                            else
                            {
                                if (GetAllWHR().Exists(x => mca.Exists(y => y.ClientId == x.ClientId)))
                                {
                                    return data;
                                }
                                else
                                    return null;
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }


        public static List<Guid> GetEligibleMemeber(string mid, List<Guid> sessionId)
        {
            List<Guid> dic = new List<Guid>();
            foreach (Guid sesId in sessionId)
            {
                Member member = GetEligibleMemeber(mid, sesId);
                if (member != null)
                {
                    dic.Add(sesId);
                }
            }

            return dic;
        } 
        #endregion
        


        /// <summary>
        /// retreives all active banks from cache
        /// </summary>
        /// <returns>list of banks</returns>
        

      
        //public InitBLL(Guid MemberId, Guid SessionId, bool IsOnline)
        //{
        //    this.MemberId = MemberId;
        //    this.SessionId = SessionId;
        //    this.IsOnline = IsOnline;
        //}
//        public static MemberData Load()
//        {
//            DAL.TradeDAL trade = new DAL.TradeDAL();
//            trade.Load();
//            Dictionary<Guid, SessionCommodityClass> sccList = null;
//            if (trade.SessionCommodityClasses.TryGetValue(SessionId, out sccList))
//            {
//                DAL.LookUpDAL lookup = new DAL.LookUpDAL();
//                DAL.MembershipDAL dMem = new DAL.MembershipDAL(lookup);
//                dMem.Load(MemberId);
//                Member TheMember = null;
//                if (dMem.Members.TryGetValue(MemberId, out TheMember) && dMem.Representatives.Count > 0 && TheMember.Active)
//                {
//                    List<Guid> ClientList = new List<Guid>();
//                    foreach (Guid g in dMem.Members.Keys)
//                    {
//                        Member mem = dMem.Members[g];
//                        if (mem.Active)
//                        {
//                            ClientList.Add(g);
//                        }
//                    }
//                    foreach (Guid g in dMem.Clients.Keys)
//                    {
//                        Client cli = dMem.Clients[g];
//                        if (cli.Active)
//                        {
//                            ClientList.Add(g);
//                        }
//                    }
//                    DAL.CNSDAL cns = new DAL.CNSDAL();
//                    cns.Load(ClientList);
//                    List<Guid> CommodityGrades = new List<Guid>();

//                    foreach (Guid g in sccList.Keys)
//                    {
//                        Dictionary<Guid, CommodityGrade> tempCG = null;
//                        if (lookup.CommodityClassGrades.TryGetValue(g, out tempCG))
//                        {
//                            foreach (Guid gCG in tempCG.Keys)
//                            {
//                                CommodityGrades.Add(gCG);
//                            }
//                        }
//                    }

//                    DAL.CDDAL cd = new DAL.CDDAL();
//                    cd.Load(ClientList, CommodityGrades);
//                    MemberData memV = null;
//                    Dictionary<Guid, RepData> RepEntered = new Dictionary<Guid, RepData>();
//                    if (cd.OwnerWarehouseReceipts.Count > 0)
//                    {
//                        #region If WHR Exists
//                        foreach (Guid g in cd.OwnerWarehouseReceipts.Keys)
//                        {
//                            if(dMem.Members.ContainsKey(g))
//                            {
//                                foreach(Guid gWHR in cd.OwnerWarehouseReceipts[g].Keys)
//                                {
//                                    WarehouseReceipt whr = cd.OwnerWarehouseReceipts[g][gWHR];
//                                    Guid? Commodity = FindCommodityGuid(lookup, whr.CommodityGrade);
//                                    CommodityGrade cg = null;
//                                    CommodityLicenseSettings cls = null;
//                                    if(Commodity != null && lookup.CommodityGrades.TryGetValue(whr.CommodityGrade, out cg) &&
//                                        trade.CommodityLicenseSettings.TryGetValue(Commodity.Value.ToString() + "-" + cg.Exportable.ToString() + "-" + Convert.ToBoolean(true).ToString(), out cls))
//                                    {
//                                        Dictionary<int, License> tempLic = null;
//                                        if(dMem.MemberLicenses.TryGetValue(g, out tempLic))
//                                        {
//                                            if(tempLic.ContainsKey(cls.LicenseId))
//                                            {
//                                                if (memV == null)
//                                                {
//                                                    memV = new MemberData();
//                                                    memV.MemberId = TheMember.Id;
//                                                    memV.ECXNewId = TheMember.ECXNewId;
//                                                    memV.MemberName = TheMember.Name;
//                                                    memV.RepList = new List<RepData>();
//                                                }
                                                
                                                
//                                                foreach (Guid gRep in dMem.Representatives.Keys)
//                                                {
//                                                    Representative TheRep = dMem.Representatives[gRep];
//                                                    if (!IsOnline && TheRep.Active)
//                                                    {
//                                                        RepData rep = null;
//                                                        if (!RepEntered.TryGetValue(TheRep.Id, out rep))
//                                                        {
//                                                            rep = new RepData();
//                                                            rep.RepId = TheRep.Id;
//                                                            rep.ECXNewId = TheRep.ECXNewId;
//                                                            rep.RepName = TheRep.Name;
//                                                            RepEntered.Add(rep.RepId, rep);
//                                                            memV.RepList.Add(rep);
//                                                        }
//                                                        rep.EnteredAs = 1;
//                                                    }
//                                                    else if (IsOnline && TheRep.Active && TheRep.OnlineCertified && TheRep.OnlineCertificationExpiry.Date >= DateTime.Now.Date)
//                                                    {
//                                                        RepData rep = null;
//                                                        if (!RepEntered.TryGetValue(TheRep.Id, out rep))
//                                                        {
//                                                            rep = new RepData();
//                                                            rep.RepId = TheRep.Id;
//                                                            rep.ECXNewId = TheRep.ECXNewId;
//                                                            rep.RepName = TheRep.Name;
//                                                            RepEntered.Add(rep.RepId, rep);
//                                                            memV.RepList.Add(rep);
//                                                        }
//                                                        rep.EnteredAs = 1;
//                                                    }
//                                                }
//                                                break;
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                            else if(dMem.Clients.ContainsKey(g))
//                            {
//                                foreach (Guid gWHR in cd.OwnerWarehouseReceipts[g].Keys)
//                                {
//                                    WarehouseReceipt whr = cd.OwnerWarehouseReceipts[g][gWHR];
//                                    Guid? Commodity = FindCommodityGuid(lookup, whr.CommodityGrade);
//                                    CommodityGrade cg = null;
//                                    CommodityLicenseSettings cls = null;
//                                    if (Commodity != null && lookup.CommodityGrades.TryGetValue(whr.CommodityGrade, out cg) &&
//                                        trade.CommodityLicenseSettings.TryGetValue(Commodity.Value.ToString() + "-" + cg.Exportable.ToString() + "-" + Convert.ToBoolean(true).ToString(), out cls))
//                                    {
//                                        Dictionary<int, License> tempLic = null;
//                                        if (dMem.ClientLicenses.TryGetValue(g, out tempLic))
//                                        {
//                                            if (tempLic.ContainsKey(cls.LicenseId))
//                                            {
//                                                if (memV == null)
//                                                {
//                                                    memV = new MemberData();
//                                                    memV.MemberId = TheMember.Id;
//                                                    memV.ECXNewId = TheMember.ECXNewId;
//                                                    memV.MemberName = TheMember.Name;
//                                                    memV.RepList = new List<RepData>();
//                                                }
                                                
                                                
//                                                foreach (Guid gRep in dMem.Representatives.Keys)
//                                                {
//                                                    Representative TheRep = dMem.Representatives[gRep];
//                                                    if (!IsOnline && TheRep.Active)
//                                                    {
//                                                        RepData rep = null;
//                                                        if (!RepEntered.TryGetValue(TheRep.Id, out rep))
//                                                        {
//                                                            rep = new RepData();
//                                                            rep.RepId = TheRep.Id;
//                                                            rep.ECXNewId = TheRep.ECXNewId;
//                                                            rep.RepName = TheRep.Name;
//                                                            RepEntered.Add(rep.RepId, rep);
//                                                            memV.RepList.Add(rep);
//                                                        }
//                                                        rep.EnteredAs = 1;
//                                                    }
//                                                    else if (IsOnline && TheRep.Active && TheRep.OnlineCertified && TheRep.OnlineCertificationExpiry.Date >= DateTime.Now.Date)
//                                                    {
//                                                        RepData rep = null;
//                                                        if (!RepEntered.TryGetValue(TheRep.Id, out rep))
//                                                        {
//                                                            rep = new RepData();
//                                                            rep.RepId = TheRep.Id;
//                                                            rep.ECXNewId = TheRep.ECXNewId;
//                                                            rep.RepName = TheRep.Name;
//                                                            RepEntered.Add(rep.RepId, rep);
//                                                            memV.RepList.Add(rep);
//                                                        }
//                                                        rep.EnteredAs = 1;
//                                                    }
//                                                }
//                                                break;
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                            if (memV != null && memV.RepList.Count > 0)
//                            {
//                                break;
//                            }
//                        }
                        
//#endregion
//                    }
//                    if(cns.BankMemberPayinAccounts.Count > 0)
//                    {
//                        #region If Member Can Buy
//                        foreach(Guid cgGuid in CommodityGrades)
//                        {
//                            Guid? Commodity = FindCommodityGuid(lookup, cgGuid);
//                            CommodityGrade cg = null;
//                            CommodityLicenseSettings cls = null;
//                            if (Commodity != null && lookup.CommodityGrades.TryGetValue(cgGuid, out cg) &&
//                                 trade.CommodityLicenseSettings.TryGetValue(Commodity.Value.ToString() + "-" + cg.Exportable.ToString() + "-" + Convert.ToBoolean(false).ToString(), out cls))
//                            {
//                                Dictionary<int, License> tempLic = null;
//                                if (dMem.MemberLicenses.TryGetValue(TheMember.Id, out tempLic))
//                                {
//                                    if (tempLic.ContainsKey(cls.LicenseId))
//                                    {
//                                        if (memV == null)
//                                        {
//                                            memV = new MemberData();
//                                            memV.MemberId = TheMember.Id;
//                                            memV.ECXNewId = TheMember.ECXNewId;
//                                            memV.MemberName = TheMember.Name;
//                                            memV.RepList = new List<RepData>();
//                                        }
                                        
//                                        foreach (Guid gRep in dMem.Representatives.Keys)
//                                        {
//                                            Representative TheRep = dMem.Representatives[gRep];
//                                            if (!IsOnline && TheRep.Active)
//                                            {
//                                                RepData rep = null;
//                                                if (!RepEntered.TryGetValue(TheRep.Id, out rep))
//                                                {
//                                                    rep = new RepData();
//                                                    rep.RepId = TheRep.Id;
//                                                    rep.ECXNewId = TheRep.ECXNewId;
//                                                    rep.RepName = TheRep.Name;
//                                                    RepEntered.Add(rep.RepId, rep);
//                                                    memV.RepList.Add(rep);
//                                                }
//                                                if (rep.EnteredAs == 1)
//                                                {
//                                                    rep.EnteredAs = 3;
//                                                }
//                                                else
//                                                {
//                                                    rep.EnteredAs = 2;
//                                                }
//                                            }
//                                            else if (IsOnline && TheRep.Active && TheRep.OnlineCertified && TheRep.OnlineCertificationExpiry.Date >= DateTime.Now.Date)
//                                            {
//                                                RepData rep = null;
//                                                if (!RepEntered.TryGetValue(TheRep.Id, out rep))
//                                                {
//                                                    rep = new RepData();
//                                                    rep.RepId = TheRep.Id;
//                                                    rep.ECXNewId = TheRep.ECXNewId;
//                                                    rep.RepName = TheRep.Name;
//                                                    RepEntered.Add(rep.RepId, rep);
//                                                    memV.RepList.Add(rep);
//                                                }
//                                                if (rep.EnteredAs == 1)
//                                                {
//                                                    rep.EnteredAs = 3;
//                                                }
//                                                else
//                                                {
//                                                    rep.EnteredAs = 2;
//                                                }
//                                            }
//                                        }
//                                        break;
//                                    }
//                                }
//                            }
//                        }
                        
//                        #endregion
//                    }
//                    if(cns.BankClientPayinAccounts.Count > 0)
//                    {
//                        #region if client can buy
//                        foreach (Guid cgGuid in CommodityGrades)
//                        {
                            
//                            Guid? Commodity = FindCommodityGuid(lookup, cgGuid);
//                            CommodityGrade cg = null;
//                            CommodityLicenseSettings cls = null;
//                            if (Commodity != null && lookup.CommodityGrades.TryGetValue(cgGuid, out cg) &&
//                                        trade.CommodityLicenseSettings.TryGetValue(Commodity.Value.ToString() + "-" + cg.Exportable.ToString() + "-" + Convert.ToBoolean(false).ToString(), out cls))
//                            {
//                                foreach (Guid gCli in dMem.Clients.Keys)
//                                {
//                                    Dictionary<int, License> tempLic = null;
//                                    if (dMem.ClientLicenses.TryGetValue(gCli, out tempLic))
//                                    {
//                                        if (tempLic.ContainsKey(cls.LicenseId))
//                                        {
//                                            if (memV == null)
//                                            {
//                                                memV = new MemberData();
//                                                memV.MemberId = TheMember.Id;
//                                                memV.ECXNewId = TheMember.ECXNewId;
//                                                memV.MemberName = TheMember.Name;
//                                                memV.RepList = new List<RepData>();
//                                            }
                                            
//                                            foreach (Guid gRep in dMem.Representatives.Keys)
//                                            {
//                                                Representative TheRep = dMem.Representatives[gRep];
//                                                if (!IsOnline && TheRep.Active)
//                                                {
//                                                    RepData rep = null;
//                                                    if (!RepEntered.TryGetValue(TheRep.Id, out rep))
//                                                    {
//                                                        rep = new RepData();
//                                                        rep.RepId = TheRep.Id;
//                                                        rep.ECXNewId = TheRep.ECXNewId;
//                                                        rep.RepName = TheRep.Name;
//                                                        RepEntered.Add(rep.RepId, rep);
//                                                        memV.RepList.Add(rep);
//                                                    }
//                                                    if (rep.EnteredAs == 1 || rep.EnteredAs == 3)
//                                                    {
//                                                        rep.EnteredAs = 3;
//                                                    }
//                                                    else
//                                                    {
//                                                        rep.EnteredAs = 2;
//                                                    }
//                                                }
//                                                else if (IsOnline && TheRep.Active && TheRep.OnlineCertified && TheRep.OnlineCertificationExpiry.Date >= DateTime.Now.Date)
//                                                {
//                                                    RepData rep = null;
//                                                    if (!RepEntered.TryGetValue(TheRep.Id, out rep))
//                                                    {
//                                                        rep = new RepData();
//                                                        rep.RepId = TheRep.Id;
//                                                        rep.ECXNewId = TheRep.ECXNewId;
//                                                        rep.RepName = TheRep.Name;
//                                                        RepEntered.Add(rep.RepId, rep);
//                                                        memV.RepList.Add(rep);
//                                                    }
//                                                    if (rep.EnteredAs == 1 || rep.EnteredAs == 3)
//                                                    {
//                                                        rep.EnteredAs = 3;
//                                                    }
//                                                    else
//                                                    {
//                                                        rep.EnteredAs = 2;
//                                                    }
//                                                }
//                                            }
//                                            break;
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                        #endregion
//                    }
//                    if (memV != null && memV.RepList.Count > 0)
//                    {
//                        return memV;
//                    }
//                    else
//                    {
//                        return null;
//                    }
//                }
//                else
//                {
//                    return null;
//                }
//            }
//            else
//            {
//                return null;
//            }
//        }
        private Guid? FindCommodityGuid(DAL.LookUpDAL lookup, Guid CommodityGrade)
        {
            CommodityGrade cg = null;
            if(lookup.CommodityGrades.TryGetValue(CommodityGrade, out cg))
            {
                CommodityClass cc = null;
                if(lookup.CommodityClasses.TryGetValue(cg.CommodityClassGuid, out cc))
                {
                    return cc.CommodityGuid;
                }
            }
            return null;
        }
    }
}

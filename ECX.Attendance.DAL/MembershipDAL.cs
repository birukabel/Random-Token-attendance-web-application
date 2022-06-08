using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.BE;
using System.Data;
using System.Collections;
using System.Configuration;

namespace ECX.Attendance.DAL
{
    public class MembershipDAL
    {
        public Dictionary<Guid, Member> Members = new Dictionary<Guid, Member>();
        public Dictionary<Guid, Client> Clients = new Dictionary<Guid, Client>();
        public Dictionary<Guid, Representative> Representatives = new Dictionary<Guid, Representative>();
        public Dictionary<Guid, Dictionary<Guid, Representative>> MemberRepresentatives = new Dictionary<Guid, Dictionary<Guid, Representative>>();
        public Dictionary<Guid, Dictionary<int, License>> MemberLicenses = new Dictionary<Guid, Dictionary<int, License>>();
        public Dictionary<Guid, Dictionary<int, License>> ClientLicenses = new Dictionary<Guid, Dictionary<int, License>>();
        public Dictionary<Guid, Dictionary<Guid, MCA>> MemberMCA = new Dictionary<Guid, Dictionary<Guid, MCA>>();
        public Dictionary<Guid, Dictionary<Guid, MCA>> ClientMCA = new Dictionary<Guid, Dictionary<Guid, MCA>>();

        public Dictionary<Guid, MemberInfo> MemberInfoDic = new Dictionary<Guid, MemberInfo>();
        public Dictionary<Guid, RepInfo> RepInfoDic = new Dictionary<Guid, RepInfo>();
        public Dictionary<Guid, string> SessionDic = new Dictionary<Guid, string>();
        public List<SellerBuyerPerSession> PerSessionList = new List<SellerBuyerPerSession>();
        DAL.LookUpDAL lookup;
        public MembershipDAL(LookUpDAL lookup)
        {
            this.lookup = lookup;
        }
        public MembershipDAL()
        {
            
        }
        public void Load(Guid MemberId)
        {
            string errormesg = "";
            ArrayList paramArrayList = new ArrayList()
            {
                "@MemberId"
            };
            ArrayList paramValArrayList = new ArrayList()
            {
                MemberId
            };
            try
            {
                DataSet dset = DataAccessProvider.ExecuteDataSet(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spMembershipLoadAttendance", paramArrayList, paramValArrayList, ref errormesg);
                if (dset.Tables.Count > 0)
                {
                    DataTable tblMember = dset.Tables[0];
                    foreach (DataRow r in tblMember.Rows)
                    {
                        Member m = new Member();
                        m.Id = new Guid(r["Id"].ToString());
                        m.Name = r["Name"].ToString();
                        m.ECXNewId = r["ECXNewId"].ToString();
                        m.ECXOldId =(int) r["ECXOldId"];
                        if (Convert.ToInt32(r["Status"]) == 5)
                        {
                            //m.Active = true;
                        }
                        else
                        {
                            //m.Active = false;
                        }
                        if (!Members.ContainsKey(m.Id))
                        {
                            Members.Add(m.Id, m);
                        }
                    }
                }

                if (dset.Tables.Count > 1)
                {
                    DataTable tblClient = dset.Tables[1];
                    foreach (DataRow r in tblClient.Rows)
                    {
                        Client c = new Client();
                        c.Id = new Guid(r["Id"].ToString());
                        c.Name = r["Name"].ToString();
                        c.ECXNewId = r["ECXNewId"].ToString();
                        c.ECXOldId = (int)r["ECXOldId"];
                        if (Convert.ToInt32(r["Status"]) == 9)
                        {
                            //c.Active = true;
                        }
                        else
                        {
                            //c.Active = false;
                        }
                        if (!Clients.ContainsKey(c.Id))
                        {
                            Clients.Add(c.Id, c);
                        }
                    }
                }

                if (dset.Tables.Count > 2)
                {
                    DataTable tblRep = dset.Tables[2];
                    foreach (DataRow r in tblRep.Rows)
                    {
                        Representative rep = new Representative();
                        rep.Id = new Guid(r["Id"].ToString());
                        rep.Name = r["Name"].ToString();
                        rep.ECXNewId = r["ECXNewId"].ToString();
                        rep.ECXOldId = (int)r["ECXOldId"];
                        rep.MemberId = new Guid(r["MemberId"].ToString());
                        //rep.Active = true;
                        if (r["OnlineCertified"] != DBNull.Value)
                        {
                            rep.OnlineCertified = Convert.ToBoolean(r["OnlineCertified"]);
                        }
                        else
                        {
                            rep.OnlineCertified = false;
                        }
                        if (r["OnlineCertificationExpiry"] != DBNull.Value)
                        {
                            rep.OnlineCertificationExpiry = Convert.ToDateTime(r["OnlineCertificationExpiry"]);
                        }
                        Representatives.Add(rep.Id, rep);
                        Dictionary<Guid, Representative> temp = null;

                        if (!MemberRepresentatives.TryGetValue(rep.MemberId, out temp))
                        {
                            temp = new Dictionary<Guid, Representative>();
                            MemberRepresentatives.Add(rep.MemberId, temp);
                        }
                        if (!temp.ContainsKey(rep.Id))
                        {
                            temp.Add(rep.Id, rep);
                        }
                    }
                }

                if (dset.Tables.Count > 3)
                {
                    DataTable tblMemLic = dset.Tables[3];
                    foreach (DataRow r in tblMemLic.Rows)
                    {
                        License lic = new License();
                        lic.LicenseId = Convert.ToInt32(r["BusinessLicenseId"]);
                        lic.ClientId = new Guid(r["Id"].ToString());
                        lic.ExpiryDate = Convert.ToDateTime(r["ExpirationDate"]);
                        lic.Active = true;
                        Dictionary<int, License> temp = null;
                        if (!MemberLicenses.TryGetValue(lic.ClientId, out temp))
                        {
                            temp = new Dictionary<int, License>();
                            MemberLicenses.Add(lic.ClientId, temp);
                        }
                        if (!temp.ContainsKey(lic.LicenseId))
                        {
                            temp.Add(lic.LicenseId, lic);
                        }
                    }
                }

                if (dset.Tables.Count > 4)
                {
                    DataTable tblCliLic = dset.Tables[4];
                    foreach (DataRow r in tblCliLic.Rows)
                    {
                        License lic = new License();
                        lic.LicenseId = Convert.ToInt32(r["BusinessLicenseId"]);
                        lic.ClientId = new Guid(r["Id"].ToString());
                        lic.ExpiryDate = Convert.ToDateTime(r["ExpirationDate"]);
                        lic.Active = true;
                        Dictionary<int, License> temp = null;
                        if (!ClientLicenses.TryGetValue(lic.ClientId, out temp))
                        {
                            temp = new Dictionary<int, License>();
                            ClientLicenses.Add(lic.ClientId, temp);
                        }
                        if (!temp.ContainsKey(lic.LicenseId))
                        {
                            temp.Add(lic.LicenseId, lic);
                        }
                    }
                }

                if (dset.Tables.Count > 5)
                {
                    DataTable tblAgr = dset.Tables[5];
                    foreach (DataRow r in tblAgr.Rows)
                    {
                       MCA mca = new MCA();
                        mca.MemberId = new Guid(r["MemberId"].ToString());
                        mca.ClientId = new Guid(r["ClientId"].ToString());

                        if (lookup.Commodities.ContainsKey(new Guid(r["CommodityId"].ToString())))
                        {
                            mca.CommodityId = new Guid(r["CommodityId"].ToString());
                        }
                        else if (lookup.CommodityClasses.ContainsKey(new Guid(r["CommodityId"].ToString())))
                        {
                            mca.CommodityId = lookup.CommodityClasses[new Guid(r["CommodityId"].ToString())].CommodityGuid;
                        }
                        else if (lookup.CommodityGrades.ContainsKey(new Guid(r["CommodityId"].ToString())))
                        {
                            mca.CommodityId = lookup.CommodityClasses[lookup.CommodityGrades[new Guid(r["CommodityId"].ToString())].CommodityClassGuid].CommodityGuid;
                        }
                        mca.Active = true;
                        Dictionary<Guid, MCA> tempM = null;
                        Dictionary<Guid, MCA> tempC = null;

                        if (!MemberMCA.TryGetValue(mca.MemberId, out tempM))
                        {
                            tempM = new Dictionary<Guid, MCA>();
                            MemberMCA.Add(mca.MemberId, tempM);
                        }
                        if (!tempM.ContainsKey(mca.CommodityId))
                        {
                            tempM.Add(mca.CommodityId, mca);
                        }

                        if (!ClientMCA.TryGetValue(mca.ClientId, out tempC))
                        {
                            tempC = new Dictionary<Guid,MCA>();
                            ClientMCA.Add(mca.ClientId, tempC);
                        }
                        if (!tempC.ContainsKey(mca.CommodityId))
                        {
                            tempC.Add(mca.CommodityId, mca);
                        }
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }
        public DataTable MemberData()
        {
            try
            {
                
                string errormesg = "";
                DataTable dt= DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spMemberDataAttendance", ref errormesg);
                return dt;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public DataTable ReperesentativeData(Guid MemberID)
        {
            try
            {
                string errormesg = "";
                ArrayList paramArrayList = new ArrayList()
            {
                "@MemberID"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                MemberID
            };
                return DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spReperesentativeDataAttendance", paramArrayList, paramValArrayList, ref errormesg);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<SellerBuyerPerSession> SellerBuyerPerSession(DateTime FromDate, DateTime ToDate, string platform, string session)
        {
            try
            {
                string errormesg = "";
                ArrayList paramArrayList = new ArrayList()
            {
                "@FromDate","@ToDate","@platform","@session"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                FromDate,ToDate,platform,session
            };
                DataSet dset = DataAccessProvider.ExecuteDataSet(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spSellerBuyerPerSessionDataAttendance", paramArrayList, paramValArrayList, ref errormesg);

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
                    DataTable tblAllData = dset.Tables[1];
                    foreach (DataRow r in tblAllData.Rows)
                    {
                        SellerBuyerPerSession sb = new SellerBuyerPerSession();

                        sb.TradeDate = r["TradeDate"].ToString();
                        sb.TradingPlatform = r["Platfrm"].ToString();
                        if (SessionDic.ContainsKey(new Guid(r["SessionId"].ToString())))
                        {
                            sb.SessionName = SessionDic[new Guid(r["SessionId"].ToString())];
                        }
                        if (r["Buyer"].ToString() != "")
                        {
                            sb.BuyerNo = Convert.ToInt32(r["Buyer"].ToString());
                        }
                        else
                        {
                            sb.BuyerNo = 0;
                        }
                        if (r["Seller"].ToString() != "")
                        {
                            sb.SellerNo = Convert.ToInt32(r["Seller"].ToString());
                        }
                        else
                        {
                            sb.SellerNo = 0;
                        }

                        PerSessionList.Add(sb);

                    }
                }
                return PerSessionList;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }
        public void LoadMembership(string MID, string RID)
        {
            try
            {
                string errormesg = "";
                ArrayList paramArrayList = new ArrayList()
            {
                "@MID","@RID"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                MID,RID
            };
                DataSet dset = DataAccessProvider.ExecuteDataSet(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spMembershipDataForAttendanceReport", paramArrayList, paramValArrayList, ref errormesg);

                if (dset.Tables.Count > 0)
                {
                    DataTable tblRep = dset.Tables[0];
                    Dictionary<string, string> dic1 = new Dictionary<string, string>();
                    foreach (DataRow r in tblRep.Rows)
                    {
                        if (!RepInfoDic.ContainsKey(new Guid(r["RepresentativeId"].ToString())))
                        {
                            RepInfo rf = new RepInfo();

                            rf.RepId = r["IDNO"].ToString();
                            rf.RepName = r["FullName"].ToString();


                            RepInfoDic.Add(new Guid(r["RepresentativeId"].ToString()), rf);
                        }

                    }
                }         

                if (dset.Tables.Count > 1)
                {
                    DataTable tblMember = dset.Tables[1];
                    Dictionary<string, string> dic2 = new Dictionary<string, string>();
                    foreach (DataRow r in tblMember.Rows)
                    {
                        if (!MemberInfoDic.ContainsKey(new Guid(r["MemberId"].ToString())))
                        {
                            MemberInfo mf = new MemberInfo();

                            mf.MemberId = r["IdNo"].ToString();
                            mf.MemberName = r["Member Name"].ToString();


                            MemberInfoDic.Add(new Guid(r["MemberId"].ToString()), mf);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public List<Member> LoadMember()
        {
            try
            {
                string errormesg = "";
                DataTable dt= DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spMemberDataAttendance",  ref errormesg);
                return DataAccessProvider.ConvertDataTable<Member>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        public List<Client> LoadClient()
        {
            try
            {
                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spClientDataAttendance", ref errormesg);
                return DataAccessProvider.ConvertDataTable<Client>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Representative> LoadRepresentative()
        {
            try
            {
                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spRepresentativeData",  ref errormesg);
                return DataAccessProvider.ConvertDataTable<Representative>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
        List<Representative> MapRepresentativeDataTableToList(DataTable dt)
        { 
           List<Representative> lstRepresentative = new List<Representative>();
           if (dt != null && dt.Rows.Count>0)
           {

               foreach (DataRow dr in dt.Rows)
               {
                   Representative _representative = new Representative();

                   _representative.Id = new Guid(dr["Id"].ToString());
                   _representative.Name = dr["Name"].ToString();
                   _representative.ECXNewId = dr["ECXNewId"].ToString();
                   _representative.ECXOldId = (int)dr["ECXOldId"];
                   _representative.MemberId = new Guid(dr["MemberId"].ToString());
                   _representative.OnlineCertified = Convert.ToBoolean(dr["OnlineCertified"]);
                   _representative.OnlineCertificationExpiry = Convert.ToDateTime(dr["OnlineCertificationExpiry"]);
                   _representative.EnteredAs = 1;
               }
           }
           return lstRepresentative;
        }
        */


        public List<MCA> LoadMCA()
        {
            try
            {
                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spMCAData",  ref errormesg);
                return DataAccessProvider.ConvertDataTable<MCA>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<License> LoadClientLicense()
        {
            try
            {
                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spClientLicenseData", ref errormesg);
                return DataAccessProvider.ConvertDataTable<License>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<License> LoadMemberLicense()
        {
            try
            {
                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spMemberLicenseData",  ref errormesg);
                return DataAccessProvider.ConvertDataTable<License>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetRepByMemberByIdNo(string memberId)
        {
            try
            {
                string errormesg = "";
                ArrayList paramArrayList = new ArrayList()
            {
                "@MemberIDNO"
            };
                ArrayList paramValArrayList = new ArrayList()
            {
                memberId
            };
                return DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetReperesentativeByMemberId", paramArrayList, paramValArrayList, ref errormesg);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int ChekNMDTIsValidForRTC(string MemberIDNo)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@MemberIDNo");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(MemberIDNo);

                string errormesg = "";
                int returnedValue = Convert.ToInt32(DataAccessProvider.ExecuteScalar(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spChekNMDTIsValidForRTC", paramName, paramVal, ref errormesg));
                return returnedValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckNMDTIsValid(string MemberIdNo)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@MemberIdNo");
               
                ArrayList paramVal = new ArrayList();

                paramVal.Add(MemberIdNo);
              

                string errormesg = "";
                 return DataAccessProvider.ExecuteScalerQuery("SELECT dbo.fCheckNMDTIsValid('" + MemberIdNo + "')" ,ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        

        
    }
}

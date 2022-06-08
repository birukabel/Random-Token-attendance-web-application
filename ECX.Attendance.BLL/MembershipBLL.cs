using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.BE;

namespace ECX.Attendance.BLL
{
    public class MembershipBLL
    {
        //1 = Seller, 2 = Buyer, 3 = Both
        public int GetEnteredAs(Guid repId, Guid sessionId, Guid memberID)
        {
            int retrunedValue = -1;
            if (InitBLL.GetAllMemberData().Exists(x => x.Id == memberID) && InitBLL.GetAllRepresentativeData().Exists(x => x.MemberId == memberID && x.Id == repId))
            {
                if (InitBLL.GetAllSession().Exists(x => x.Id == sessionId && InitBLL.GetAllMemberLicense().Exists(y => y.ClientId == memberID && y.LicenseId == x.LicenseTypeID)))
                {

                    if (InitBLL.GetAllBankAccount().Exists(x => x.OwnerGuid == memberID && x.Balance > 0))
                    {
                        retrunedValue = 2;
                        if (InitBLL.GetAllWHR().Exists(x => x.ClientId == memberID))
                        {
                            retrunedValue = 3;
                        }
                    }
                    else
                    {
                        if (InitBLL.GetAllWHR().Exists(x => x.ClientId == memberID))
                        {
                            retrunedValue = 1;
                        }
                        else
                        {
                            List<MCA> mca = InitBLL.GetAllMCAData().Where(x => x.MemberId == memberID).ToList();
                            if (mca != null)
                            {
                                if (InitBLL.GetAllBankAccount().Exists(x => mca.Exists(y => (y.ClientId == x.OwnerGuid || x.OwnerGuid==memberID)) && x.Balance > 0))
                                {
                                    retrunedValue = 2;
                                    if (InitBLL.GetAllWHR().Exists(x => mca.Exists(y => y.ClientId == x.ClientId)))
                                    {
                                        retrunedValue = 3;
                                    }
                                }
                                else
                                {
                                    if (InitBLL.GetAllWHR().Exists(x => mca.Exists(y => y.ClientId == x.ClientId)))
                                    {
                                        retrunedValue = 1;
                                    }
                                }
                            }
                            else
                            {
                                retrunedValue = -1;
                            }
                        }
                    }
                }
                else
                {
                    List<MCA> mca = InitBLL.GetAllMCAData().Where(x => x.MemberId == memberID).ToList();
                    if (mca != null)
                    {
                        List<Guid> cId = mca.Select(x => x.ClientId).Distinct().ToList<Guid>();
                        //var sessionData = InitBLL.GetAllSession().Exists(x => x.Id == sessionId) ;

                        // var clientLice= InitBLL.GetAllClientLicense().Exists(y => cId.Contains(y.ClientId) && y.LicenseId == x.LicenseTypeID);

                        cId = InitBLL.GetAllClientLicense().Where(x => cId.Contains(x.ClientId) && InitBLL.GetAllSession().Exists(y => y.LicenseTypeID == x.LicenseId)).Select(z => z.ClientId).ToList<Guid>();

                        if (cId != null)
                        {
                            if (InitBLL.GetAllBankAccount().Exists(x => (cId.Contains(x.OwnerGuid) || memberID == x.OwnerGuid) && x.Balance > 0))
                            {
                                retrunedValue = 2;
                                if (InitBLL.GetAllWHR().Exists(x => cId.Contains(x.ClientId)))
                                {
                                    retrunedValue = 3;
                                }
                            }
                            else
                            {
                                if (InitBLL.GetAllWHR().Exists(x => cId.Contains(x.ClientId)))
                                {
                                    retrunedValue = 1;
                                }
                            }
                        }
                    }
                }
            }
            return retrunedValue;
        }

        public void Load(Guid memberId)
        {
            new DAL.MembershipDAL().Load(memberId);
        }
        public DataTable MemberData()
        {
            return new DAL.MembershipDAL().MemberData();
        }
        public DataTable ReperesentativeData(Guid memberID)
        {
            return new DAL.MembershipDAL().ReperesentativeData(memberID);
        }
        public List<SellerBuyerPerSession> SellerBuyerPerSession(DateTime FromDate, DateTime ToDate, string platform, string session)
        {
            return new DAL.MembershipDAL().SellerBuyerPerSession(FromDate, ToDate, platform, session);
        }
        public void LoadMembership(string MID, string RID)
        {
            new DAL.MembershipDAL().LoadMembership(MID, RID);
        }

        public DataTable GetRepByMemberByIdNo(string memberId)
        {
            return new DAL.MembershipDAL().GetRepByMemberByIdNo(memberId);
        }

        public bool CheckNMDTIsValid(string MemberIdNo)
        {
            return new DAL.MembershipDAL().CheckNMDTIsValid(MemberIdNo);
        }

        public int ChekNMDTIsValidForRTC(string MemberIDNo)
        {
            return new DAL.MembershipDAL().ChekNMDTIsValidForRTC(MemberIDNo);
        }
    }
}

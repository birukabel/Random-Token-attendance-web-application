

using ECX.Attendance.BE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Attendance.DAL
{
    public class CNSDAL
    {
        public Dictionary<Guid, Dictionary<Guid, BankAccount>> BankMemberPayinAccounts = new Dictionary<Guid, Dictionary<Guid, BankAccount>>();
        public Dictionary<Guid, Dictionary<Guid, BankAccount>> BankMemberPayoutAccounts = new Dictionary<Guid, Dictionary<Guid, BankAccount>>();
        public Dictionary<Guid, Dictionary<Guid, BankAccount>> BankClientPayinAccounts = new Dictionary<Guid, Dictionary<Guid, BankAccount>>();
        public Dictionary<Guid, Dictionary<Guid, BankAccount>> BankClientPayoutAccounts = new Dictionary<Guid, Dictionary<Guid, BankAccount>>();

        public CNSDAL()
        {

        }
        public void Load(List<Guid> ClientList)
        {
            DataTable dtClient = DataAccessProvider.ConstructTVP(ClientList);
            List<DataTable> tblValueParamlst = new List<DataTable>()
            {
                dtClient
            };
            string errormesg = "";
            ArrayList paramArrayList = new ArrayList()
            {
                "@client"
            };
            ArrayList paramTypeArrayList = new ArrayList()
            {
                "dbo.GuidList"
            };
            try
            {
                DataTable tblBankAccount = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spCNSLoadAttendance", null, null, paramArrayList, tblValueParamlst, paramTypeArrayList, ref errormesg);

                foreach (DataRow r in tblBankAccount.Rows)
                {
                    BankAccount acc = new BankAccount();
                    acc.Id = new Guid(r["Guid"].ToString());
                    acc.OwnerGuid = new Guid(r["OwnerGuid"].ToString());
                    if (r["AccountTypeGuid"].ToString() == "1f537d3f-0645-4ae9-8e46-54cbf585f4c8")
                    {
                        acc.AccountType = EAccountType.MemberPayOut;
                    }
                    else if (r["AccountTypeGuid"].ToString() == "92835d96-f18c-441e-b188-563eb17ca618")
                    {
                        acc.AccountType = EAccountType.MemberPayIn;
                    }
                    else if (r["AccountTypeGuid"].ToString() == "2d6cf95f-f357-4bd7-975d-95d4ae4f084c")
                    {
                        acc.AccountType = EAccountType.ClientPayOut;
                    }
                    else if (r["AccountTypeGuid"].ToString() == "feb6798f-eeab-4f6f-9b28-a0f8dbdf67e8")
                    {
                        acc.AccountType = EAccountType.ClientPayIn;
                    }
                    acc.AccountNumber = r["AccountNumber"].ToString();
                    acc.Balance = Convert.ToDouble(r["Balance"]);
                    Dictionary<Guid, BankAccount> temp = null;
                    if (acc.AccountType == EAccountType.MemberPayIn)
                    {
                        if (!BankMemberPayinAccounts.TryGetValue(acc.OwnerGuid, out temp))
                        {
                            temp = new Dictionary<Guid, BankAccount>();
                            BankMemberPayinAccounts.Add(acc.OwnerGuid, temp);
                        }
                        if (!temp.ContainsKey(acc.Id))
                        {
                            temp.Add(acc.Id, acc);
                        }
                    }
                    else if (acc.AccountType == EAccountType.MemberPayOut)
                    {
                        if (!BankMemberPayoutAccounts.TryGetValue(acc.OwnerGuid, out temp))
                        {
                            temp = new Dictionary<Guid, BankAccount>();
                            BankMemberPayoutAccounts.Add(acc.OwnerGuid, temp);
                        }
                        if (!temp.ContainsKey(acc.Id))
                        {
                            temp.Add(acc.Id, acc);
                        }
                    }
                    else if (acc.AccountType == EAccountType.ClientPayIn)
                    {
                        if (!BankClientPayinAccounts.TryGetValue(acc.OwnerGuid, out temp))
                        {
                            temp = new Dictionary<Guid, BankAccount>();
                            BankClientPayinAccounts.Add(acc.OwnerGuid, temp);
                        }
                        if (!temp.ContainsKey(acc.Id))
                        {
                            temp.Add(acc.Id, acc);
                        }
                    }
                    else if (acc.AccountType == EAccountType.ClientPayOut)
                    {
                        if (!BankClientPayoutAccounts.TryGetValue(acc.OwnerGuid, out temp))
                        {
                            temp = new Dictionary<Guid, BankAccount>();
                            BankClientPayoutAccounts.Add(acc.OwnerGuid, temp);
                        }
                        if (!temp.ContainsKey(acc.Id))
                        {
                            temp.Add(acc.Id, acc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<BankAccount> LoadBankAccount()
        {
            try
            {
                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spBankAccountData",  ref errormesg);
                return DataAccessProvider.ConvertDataTable<BankAccount>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

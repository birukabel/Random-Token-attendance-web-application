using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.BE;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;

namespace ECX.Attendance.DAL
{
    public class CDDAL
    {
        public Dictionary<Guid, WarehouseReceipt> WarehouseReceipts = new Dictionary<Guid, WarehouseReceipt>();
        public Dictionary<Guid, Dictionary<Guid, WarehouseReceipt>> OwnerWarehouseReceipts = new Dictionary<Guid, Dictionary<Guid, WarehouseReceipt>>();
        public CDDAL()
        {

        }
        public void Load(List<Guid> ClientList, List<Guid> CommodityGrades)
        {
            DataTable dtClient = DataAccessProvider.ConstructTVP(ClientList);
            DataTable dtCommodity = DataAccessProvider.ConstructTVP(CommodityGrades);
            List<DataTable> tblValueParamlst = new List<DataTable>()
            {
                dtClient,dtCommodity
            };
            string errormesg = "";
            ArrayList paramArrayList = new ArrayList()
            {
                "@client","@commodity"
            };
            ArrayList paramTypeArrayList = new ArrayList()
            {
                "dbo.GuidList","dbo.GuidList"
            };
            try
            {
                DataTable tblWHR = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spCDLoadAttendance", null, null,paramArrayList, tblValueParamlst,paramTypeArrayList, ref errormesg);

                foreach (DataRow r in tblWHR.Rows)
                {
                    WarehouseReceipt whr = new WarehouseReceipt();
                    whr.Id = new Guid(r["Id"].ToString());
                    whr.CommodityGrade = new Guid(r["CommodityGradeId"].ToString());
                    whr.ClientId = new Guid(r["ClientId"].ToString());
                    WarehouseReceipts.Add(whr.Id, whr);
                    Dictionary<Guid, WarehouseReceipt> temp = null;
                    if (!OwnerWarehouseReceipts.TryGetValue(whr.ClientId, out temp))
                    {
                        temp = new Dictionary<Guid, WarehouseReceipt>();
                        OwnerWarehouseReceipts.Add(whr.ClientId, temp);
                    }
                    if (!temp.ContainsKey(whr.Id))
                    {
                        temp.Add(whr.Id, whr);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<WarehouseReceipt> LoadWHR()
        {
            try
            {
                string errormesg = "";
                DataTable dt = DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spWHRData", ref errormesg);
                return DataAccessProvider.ConvertDataTable<WarehouseReceipt>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

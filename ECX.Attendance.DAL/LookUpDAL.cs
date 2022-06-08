using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECX.Attendance.BE;
using System.Data;
using System.Configuration;
using System.Collections;

namespace ECX.Attendance.DAL
{
    public class LookUpDAL
    {
        public Dictionary<Guid, Commodity> Commodities = new Dictionary<Guid, Commodity>();
        public Dictionary<Guid, CommodityClass> CommodityClasses = new Dictionary<Guid, CommodityClass>();
        public Dictionary<Guid, Dictionary<Guid, CommodityGrade>> CommodityClassGrades = new Dictionary<Guid, Dictionary<Guid, CommodityGrade>>();
        public Dictionary<Guid, CommodityGrade> CommodityGrades = new Dictionary<Guid, CommodityGrade>();
        public Dictionary<Guid, Warehouse> Warehouses = new Dictionary<Guid, Warehouse>();
        public LookUpDAL()
        {

        }
        public void Load()
        {
            try
            {
                string errormesg = "";
                DataSet dsLookup = DataAccessProvider.ExecuteDataSet(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spCNSLoadAttendance", null, null, ref errormesg);


                if (dsLookup.Tables.Count > 0)
                {
                    DataTable tblCommodity = dsLookup.Tables[0];
                    foreach (DataRow r in tblCommodity.Rows)
                    {
                        Commodity c = new Commodity();
                        c.Id = new Guid(r["Guid"].ToString());
                        c.Name = r["Description"].ToString();
                        Commodities.Add(c.Id, c);
                    }
                }

                if (dsLookup.Tables.Count > 1)
                {
                    DataTable tblCommodityClass = dsLookup.Tables[1];
                    foreach (DataRow r in tblCommodityClass.Rows)
                    {
                        try
                        {
                            CommodityClass c = new CommodityClass();
                            c.Id = new Guid(r["Guid"].ToString());
                            c.CommodityGuid = new Guid(r["CommodityGuid"].ToString());
                            c.Name = r["Description"].ToString();
                            CommodityClasses.Add(c.Id, c);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }

                if (dsLookup.Tables.Count > 2)
                {
                    DataTable tblCommodityGrade = dsLookup.Tables[2];
                    foreach (DataRow r in tblCommodityGrade.Rows)
                    {
                        CommodityGrade c = new CommodityGrade();
                        c.Id = new Guid(r["Guid"].ToString());
                        c.CommodityClassGuid = new Guid(r["CommodityClassGuid"].ToString());
                        c.Name = r["Description"].ToString();
                        c.Symbol = r["Symbol"].ToString();
                        c.Exportable = Convert.ToBoolean(r["Exportable"]);
                        CommodityGrades.Add(c.Id, c);
                        Dictionary<Guid, CommodityGrade> temp = null;
                        if (!CommodityClassGrades.TryGetValue(c.CommodityClassGuid, out temp))
                        {
                            temp = new Dictionary<Guid, CommodityGrade>();
                            CommodityClassGrades.Add(c.CommodityClassGuid, temp);
                        }
                        if (!temp.ContainsKey(c.Id))
                        {
                            temp.Add(c.Id, c);
                        }
                    }
                }

                if (dsLookup.Tables.Count > 3)
                {
                    DataTable tblWarehouse = dsLookup.Tables[3];
                    foreach (DataRow r in tblWarehouse.Rows)
                    {
                        Warehouse w = new Warehouse();
                        w.Id = new Guid(r["Guid"].ToString());
                        w.Name = r["Description"].ToString();
                        Warehouses.Add(w.Id, w);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public DataTable GetMaximumAllowedReps()
        {
            try
            {
                
                string errormesg = "";
                return DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spGetMaximumAllowedReps", ref errormesg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCommodity()
        {
            try
            {

                string errormesg = "";
                return DataAccessProvider.ExecuteDataTable(ConfigurationManager.ConnectionStrings["LookUpConnectionString"].ConnectionString, "dbo", "tblCommodity_GetRecords", ref errormesg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveSetting(string tradingcenter, Guid commodity, int reps,Guid createdBy)
        {
            try
            {
                ArrayList paramName = new ArrayList();

                paramName.Add("@TradingCenter");
                paramName.Add("@Commodity");
                paramName.Add("@NoReps");
                paramName.Add("@CreatedBy");

                ArrayList paramVal = new ArrayList();

                paramVal.Add(tradingcenter);
                paramVal.Add(commodity);
                paramVal.Add(reps);
                paramVal.Add(createdBy);
                string errormesg = "";
                DataAccessProvider.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString, "dbo", "spSaveRepSetting",paramName ,paramVal,ref errormesg );

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

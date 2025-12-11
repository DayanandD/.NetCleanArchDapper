using NETCleanArch.Application.Dtos.Common;

namespace NETCleanArchInfrastructure.CommonQueries
{
    public class CommonQuery
    {
        #region Application Service Registry
        public static string CheckApplicationServiceRegistry(long serviceregistryid, long serviceid, string baseurl)
        {
            return $"SELECT serviceregistryid FROM Vms360.serviceregistry WHERE isactive = TRUE AND serviceid = {serviceid} AND baseurl = '{baseurl}' AND serviceregistryid != {serviceregistryid}";
        }

        public static string ApplicationServiceRegistryGetF(long serviceregistryid, int p_start, int page_size)
        {
            return $"SELECT * FROM Vms360.applicationserviceregistrygetf({serviceregistryid},{p_start},{page_size})";
        }

        public static string ApplicationServiceRegistryDropDownList()
        {
            return $"SELECT serviceregistryid AS value, servicename AS text FROM Vms360.serviceregistry WHERE isactive=true";
        }

        public static string DeleteApplicationServiceRegistry(long serviceregistryid)
        {
            return $"DELETE FROM Vms360.serviceregistry WHERE serviceregistryid = {serviceregistryid}";
        }

        public string InActiveApplicationServiceRegistry(long serviceregistryid)
        {
            return $"UPDATE Vms360.serviceregistry SET isactive = FALSE, updatedat = '{AppCommon.GetEntryOn()}' WHERE serviceregistryid = {serviceregistryid} AND isactive = TRUE";
        }
        #endregion

        #region Application Service Registry Schema
        public static string CheckApplicationServiceRegistrySchema(long schemasid, long applicationid, long serviceid, string endpoint)
        {
            return $"SELECT schemasid FROM Vms360.service_schema WHERE isactive = TRUE AND applicationid = {applicationid} AND serviceid = {serviceid} AND endpoint = '{endpoint}' AND schemasid != {schemasid}";
        }

        public static string ApplicationServiceRegistrySchemaGetF(long schemasid, long applicationid, long serviceid, int p_start, int page_size)
        {
            string whereClause = "";
            if (schemasid > 0)
                whereClause = $" WHERE schemasid = {schemasid}";
            else if (applicationid > 0 && serviceid > 0)
                whereClause = $" WHERE applicationid = {applicationid} AND serviceid = {serviceid}";

            return $"SELECT * FROM Vms360.applicationserviceregistryschemagetf({schemasid},{applicationid},{serviceid},{p_start},{page_size})";
        }

        public static string DeleteApplicationServiceRegistrySchema(long schemasid)
        {
            return $"DELETE FROM Vms360.service_schema WHERE schemasid = {schemasid}";
        }

        public string InActiveApplicationServiceRegistrySchema(long schemasid)
        {
            return $"UPDATE Vms360.service_schema SET isactive = FALSE, updatedby = createdby'{AppCommon.GetEmployeeCode}', updatedon = '{AppCommon.GetEntryOn()}' WHERE schemasid = {schemasid} AND isactive = TRUE";
        }
        #endregion
    }
}

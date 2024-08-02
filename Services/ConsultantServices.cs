using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class ConsultantServices : IConsultantServices
    {
        private readonly IDapperService _dapper;

        public ConsultantServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public ConsultantDashboardModel GetDashboardAvrageCount(int UserId)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@userId", UserId, DbType.Int32);

            return _dapper.Get<ConsultantDashboardModel>("usp_Consultant_Dashboard_AvrageCount", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<ConsultantDashboardModel> GetDashboardChartCount(int UserId, int Selectedyear)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@userId", UserId, DbType.Int32);
            dbparams.Add("@customYear", Selectedyear, DbType.Int32);

            return _dapper.GetAll<ConsultantDashboardModel>("usp_Consultant_Dashboard_ChartCount", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public ConsultantDashboardModel GetDashboardCount(int UserId)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@UserId", UserId, DbType.Int32);
         
            return _dapper.Get<ConsultantDashboardModel>("usp_Consultant_Dashboard_Count", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }    
        public List<ActiveClient> ConsultantActiveClient(int UserId,DateTime? FDate,DateTime? TDate, bool AllBitCount)
            {
            try
            {
                //if (AllBitCount == true)
                //{
                //    FDate = null;
                //    TDate = null;
                //}
                var dbparams = new DynamicParameters();
                dbparams.Add("@UserId", UserId, DbType.Int32);
                dbparams.Add("@FDate", FDate, DbType.DateTime);
                dbparams.Add("@TDate", TDate, DbType.DateTime);
                dbparams.Add("@AllBitCount", AllBitCount, DbType.Boolean);
                //return _dapper.GetAll<ActiveClient>("usp_ConsultantActiveClient", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
                var data= _dapper.GetAll<ActiveClient>("usp_ConsultantActiveClient", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
                return data;

            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public List<ReceptionWiseCountModel> GetReceptionWiseCounts(int ClinicId, int UserId)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@clinicId", ClinicId, DbType.Int32);
            dbparams.Add("@userId", UserId, DbType.Int32);
            return _dapper.GetAll<ReceptionWiseCountModel>("usp_Consultant_Dashboard_Receptionist_Count", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<TotalRevenue> ConsultantTotalRevenue(int UserId, DateTime? FDate, DateTime? TDate, bool AllBitCount)
        {
            try
            {
                //if (AllBitCount == true)
                //{
                //    FDate = null;
                //    TDate = null;
                //}
                var dbparams = new DynamicParameters();
                dbparams.Add("@UserId", UserId, DbType.Int32);
                dbparams.Add("@FDate", FDate, DbType.DateTime);
                dbparams.Add("@TDate", TDate, DbType.DateTime);
                dbparams.Add("@AllBitCount", AllBitCount, DbType.Boolean);
                var data = _dapper.GetAll<TotalRevenue>("usp_ConsultantTotalRevenue", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
                return data;

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public List<TotalPatientPending> ConsultantTotalPatientPending(int UserId, DateTime? FDate, DateTime? TDate,bool AllBitCount)
        {
            try
            {                
                var dbparams = new DynamicParameters();
                dbparams.Add("@UserId", UserId, DbType.Int32);
                dbparams.Add("@FDate", FDate, DbType.DateTime);
                dbparams.Add("@TDate", TDate, DbType.DateTime);
                dbparams.Add("@AllBitCount", AllBitCount, DbType.Boolean);
                var data = _dapper.GetAll<TotalPatientPending>("usp_ConsultantTotalPatientPending", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
                return data;

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}

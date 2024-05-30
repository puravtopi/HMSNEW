using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class ReceptionistServices : IReceptionistMasterServices
    {
        private readonly IDapperService _dapper;

        public ReceptionistServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public ReceptionistDashboardModel GetDashboardAvrageCount(int UserId)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@userId", UserId, DbType.Int32);

            return _dapper.Get<ReceptionistDashboardModel>("usp_Consultant_Dashboard_AvrageCount", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<ReceptionistDashboardModel> GetDashboardChartCount(int UserId, int Selectedyear)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@userId", UserId, DbType.Int32);
            dbparams.Add("@customYear", Selectedyear, DbType.Int32);

            return _dapper.GetAll<ReceptionistDashboardModel>("usp_Consultant_Dashboard_ChartCount", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public ReceptionistDashboardModel GetDashboardCount(int UserId)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@UserId", UserId, DbType.Int32);
         
            return _dapper.Get<ReceptionistDashboardModel>("usp_Consultant_Dashboard_Count", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
    }
}

using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
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

        public ConsultantDashboardModel GetDashboardCount(int UserId)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@UserId", UserId, DbType.Int32);
         
            return _dapper.Get<ConsultantDashboardModel>("usp_Consultant_Dashboard_Count", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
    }
}

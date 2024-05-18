using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class ServiceHeadMasterService : IServiceHeadMasterService
    {
        private readonly IDapperService _dapper;

        public ServiceHeadMasterService(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public ServiceHeadMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<ServiceHeadMasterModel>(CommonSp.deleteByIdServiceHeadMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<ServiceHeadMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@currentPage", currentPage, DbType.String);
            //searchString = "%" + searchString + "%";
            dbparams.Add("@searchString", searchString, DbType.String);
            dbparams.Add("@pageSize", pageSize, DbType.String);

            sortOrder = sortCol + " " + sortOrder;
            dbparams.Add("@qcnd", qcnd, DbType.String);

            dbparams.Add("@sortOrder", sortOrder, DbType.String);
            dbparams.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //dbparams.Add("@Date", Date, DbType.String);

            var res = _dapper.GetAll<ServiceHeadMasterModel>(CommonSp.getAllServiceHeadMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public List<ServiceHeadMasterModel> GetByDepartmentIdWiseServiceList(int Department_Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Deparment_Id", Department_Id, DbType.Int32);
            return _dapper.GetAll<ServiceHeadMasterModel>(CommonSp.getByDepartmentWise_ServiceHead, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public ServiceHeadMasterModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<ServiceHeadMasterModel>(CommonSp.getByIdServiceHeadMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        

        public ServiceHeadMasterModel Insert(ServiceHeadMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@ServiceHeadName", model.ServiceHeadName, DbType.String);
            dbparams.Add("@Department_Id", model.Department_Id, DbType.Int32);
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            
            return _dapper.Update<ServiceHeadMasterModel>(CommonSp.saveServiceHeadMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public ServiceHeadMasterModel Update(ServiceHeadMasterModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@ServiceHeadName", model.ServiceHeadName, DbType.String);
            dbparams.Add("@Department_Id", model.Department_Id, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            dbparams.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);

            return _dapper.Insert<ServiceHeadMasterModel>(CommonSp.updateServiceHeadMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
    }
}

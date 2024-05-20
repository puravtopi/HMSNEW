using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class DepartmentMasterServices : IDepartmentMasterServices
    {        
        private readonly IDapperService _dapper;

        public DepartmentMasterServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public DepartmentMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<DepartmentMasterModel>(CommonSp.deleteByIdDepartmentMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<DepartmentMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<DepartmentMasterModel>(CommonSp.getAllDepartmentMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public List<DepartmentMasterModel> GetByClinicIdWiseDept(ref int TotalCount, int ClinicId, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Clinic_Id", ClinicId, DbType.Int32);
            dbparams.Add("@currentPage", currentPage, DbType.String);
            //searchString = "%" + searchString + "%";
            dbparams.Add("@searchString", searchString, DbType.String);
            dbparams.Add("@pageSize", pageSize, DbType.String);

            sortOrder = sortCol + " " + sortOrder;
            dbparams.Add("@qcnd", qcnd, DbType.String);

            dbparams.Add("@sortOrder", sortOrder, DbType.String);
            dbparams.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //dbparams.Add("@Date", Date, DbType.String);
            var res = _dapper.GetAll<DepartmentMasterModel>(CommonSp.getByClinicIdWiseDEPT, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public List<DepartmentMasterModel> GetByClinicIdWiseDeptList(int ClinicId)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Clinic_Id", ClinicId, DbType.Int32);
            var res = _dapper.GetAll<DepartmentMasterModel>(CommonSp.getByClinicIdWiseDEPTList, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
            return res;
        }

        //public List<DepartmentMasterModel> GetAllClinicDepartment()
        //{
        //    var dbparams = new DynamicParameters();
        //    var res = _dapper.GetAll<DepartmentMasterModel>(CommonSp.getdepatAll, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
        //    return res;
        //}

        public DepartmentMasterModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<DepartmentMasterModel>(CommonSp.getByIdDepartmentMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public DepartmentMasterModel Insert(DepartmentMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@DepartmentName ", model.DepartmentName, DbType.String);
            dbparams.Add("@Clinic_ID", model.Clinic_Id, DbType.Int32);
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
           

            return _dapper.Update<DepartmentMasterModel>(CommonSp.saveDepartmentMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public DepartmentMasterModel Update(DepartmentMasterModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@DepartmentName ", model.DepartmentName, DbType.String);
            dbparams.Add("@Clinic_ID", model.Clinic_Id, DbType.Int32);
            dbparams.Add("@UpdatedBy", model.CreatedBy, DbType.Int32);

            return _dapper.Insert<DepartmentMasterModel>(CommonSp.updateDepartmentMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
    }
}

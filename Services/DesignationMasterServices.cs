using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class DesignationMasterServices : IDesignationMasterServices
    {
        private readonly IDapperService _dapper;

        public DesignationMasterServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public DesignationMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<DesignationMasterModel>(CommonSp.deleteByIdDesignationMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<DesignationMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<DesignationMasterModel>(CommonSp.getAllDesignationMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

       

        public List<DesignationMasterModel> GetByClinicIdWiseDesig(int ClinicId, int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<DesignationMasterModel>(CommonSp.getByClinicIdWiseDESIG, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public List<DesignationMasterModel> GetByClinicIdWiseDesigList(int ClinicId)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Clinic_Id", ClinicId, DbType.Int32);
            var res = _dapper.GetAll<DesignationMasterModel>(CommonSp.getByClinicIdWiseDESIGList, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
            return res;
        }

        public DesignationMasterModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<DesignationMasterModel>(CommonSp.getByIdDesignationMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public DesignationMasterModel Insert(DesignationMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@DesignationName ", model.DesignationName, DbType.String);
            dbparams.Add("@Clinic_ID", model.Clinic_Id, DbType.Int32);
            dbparams.Add("@DesignationCode ", model.DesignationCode, DbType.String);
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);


            return _dapper.Update<DesignationMasterModel>(CommonSp.saveDesignationMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public DesignationMasterModel Update(DesignationMasterModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@DesignationCode ", model.DesignationCode, DbType.String);
            dbparams.Add("@DesignationName ", model.DesignationName, DbType.String);
            dbparams.Add("@Clinic_ID", model.Clinic_Id, DbType.Int32);
            dbparams.Add("@UpdatedBy", model.CreatedBy, DbType.Int32);

            return _dapper.Insert<DesignationMasterModel>(CommonSp.updateDesignationMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
        //public List<DesignationMasterModel> GetAllClinicDesignationt()
        //{
        //    var dbparams = new DynamicParameters();
        //    var res = _dapper.GetAll<DesignationMasterModel>(CommonSp.getdesigtAll, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
        //    return res;
        //}
    }
}

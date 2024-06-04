using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class ActivityMasterServices:IActivityMasterServices
    {
        private readonly IDapperService _dapper;

        public ActivityMasterServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public ActivityMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<ActivityMasterModel>(CommonSp.deleteByIdActivityMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<ActivityMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<ActivityMasterModel>(CommonSp.getAllActivityMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public ActivityMasterModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<ActivityMasterModel>(CommonSp.getByIdActivityMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }       

        public ActivityMasterModel Insert(ActivityMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@ActivityName", model.ActivityName, DbType.String);
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            return _dapper.Update<ActivityMasterModel>(CommonSp.saveActivityMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public ActivityMasterModel Update(ActivityMasterModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@ActivityName", model.ActivityName, DbType.String);
            
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            dbparams.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);
            return _dapper.Insert<ActivityMasterModel>(CommonSp.updateActivityMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

    }
}

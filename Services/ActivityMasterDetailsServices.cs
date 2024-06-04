using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class ActivityMasterDetailsServices : IActivityMasterDetailsServices
    {
        private readonly IDapperService _dapper;

        public ActivityMasterDetailsServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public ActivityMasterDetailsModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<ActivityMasterDetailsModel>(CommonSp.deleteByIdActivityMasterDetails, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<ActivityMasterDetailsModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<ActivityMasterDetailsModel>(CommonSp.getAllActivityMasterDetails, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public ActivityMasterDetailsModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<ActivityMasterDetailsModel>(CommonSp.getAllActivityMasterDetails, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
        

        public ActivityMasterDetailsModel Insert(ActivityMasterDetailsModel model)
        {
            var dbparams = new DynamicParameters();
            
            dbparams.Add("@ActivityTypeId", model.ActivityTypeId, DbType.Int32);
            dbparams.Add("@ActivityBy", model.ActivityBy, DbType.Int32);
            dbparams.Add("@ActivityFor", model.ActivityFor, DbType.Int32);
            dbparams.Add("@ActivityDate", model.ActivityDate, DbType.String);
            dbparams.Add("@Description", model.Description, DbType.String);
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            return _dapper.Update<ActivityMasterDetailsModel>(CommonSp.saveActivityMasterDetails, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
        }

        public ActivityMasterDetailsModel Update(ActivityMasterDetailsModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@ActivityTypeId", model.ActivityTypeId, DbType.Int32);
            dbparams.Add("@ActivityBy", model.ActivityBy, DbType.Int32);
            dbparams.Add("@ActivityFor", model.ActivityFor, DbType.Int32);
            dbparams.Add("@ActivityDate", model.ActivityDate, DbType.String);
            dbparams.Add("@Description", model.Description, DbType.String); 
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            dbparams.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);
            return _dapper.Insert<ActivityMasterDetailsModel>(CommonSp.updateActivityMasterDetails, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
        }

    }
}

using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace HMS.Services
{
    public class ClinicMasterServices : IClinicMasterServices
    {

        private readonly IDapperService _dapper;

        public ClinicMasterServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public ClinicMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<ClinicMasterModel>(CommonSp.deleteByIdClinicMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<ClinicMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<ClinicMasterModel>(CommonSp.getAllClinicMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public ClinicMasterModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<ClinicMasterModel>(CommonSp.getByIdClinicMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public int GetConsultantIdById(int Id)
        {
            string query = "Select * from DesignationMaster where DesignationName='consultant' and Clinic_Id=" + Id;
            var data = _dapper.Get<DesignationMasterModel>(query, null, commandType: CommandType.Text, ConnStrings.HMSConnectionstring);

            if (data != null)
                return data.Id;
            else
                return 0;

        }

        public ClinicMasterModel Insert(ClinicMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@ClinicName ", model.ClinicName, DbType.String);
            dbparams.Add("@OwnerName", model.OwnerName, DbType.String);
            dbparams.Add("@Address", model.Address, DbType.String);
            dbparams.Add("@Mobile ", model.Mobile, DbType.String);
            dbparams.Add("@EmailId", model.EmailId, DbType.String);
            dbparams.Add("@Logo", model.Logo, DbType.String);
            dbparams.Add("@Code", model.Code, DbType.String);
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            dbparams.Add("@CityName", model.CityName, DbType.String);
            dbparams.Add("@StateName", model.StateName, DbType.String);
            dbparams.Add("@Pincode", model.Pincode, DbType.String);
            dbparams.Add("@password", model.Password, DbType.String);
            return _dapper.Update<ClinicMasterModel>(CommonSp.saveClinicMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public ClinicMasterModel Update(ClinicMasterModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@ClinicName ", model.ClinicName, DbType.String);
            dbparams.Add("@OwnerName", model.OwnerName, DbType.String);
            dbparams.Add("@Address", model.Address, DbType.String);
            dbparams.Add("@Mobile ", model.Mobile, DbType.String);
            dbparams.Add("@EmailId", model.EmailId, DbType.String);
            dbparams.Add("@UpdatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@CityName", model.CityName, DbType.String);
            dbparams.Add("@StateName", model.StateName, DbType.String);
            dbparams.Add("@Pincode", model.Pincode, DbType.String);

            return _dapper.Insert<ClinicMasterModel>(CommonSp.updateClinicMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public ClinicMasterModel UpdateCode(ClinicMasterModel model)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("@Id ", model.Id, DbType.Int32);
                dbparams.Add("@Code ", model.Code, DbType.String);
                return _dapper.Insert<ClinicMasterModel>(CommonSp.updateClinicMasterCode, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}

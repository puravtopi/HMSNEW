using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class PatientGeneralDetailMasterServices : IPatientGeneralDetailMasterServices
    {
        private readonly IDapperService _dapper;

        public PatientGeneralDetailMasterServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public PatientGeneralDetailMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<PatientGeneralDetailMasterModel>(CommonSp.deleteByIdPatientGeneralDetailMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring); ;

        }

        public List<PatientGeneralDetailMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<PatientGeneralDetailMasterModel>(CommonSp.getAllPatientGeneralDetailMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }


        public PatientGeneralDetailMasterModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<PatientGeneralDetailMasterModel>(CommonSp.getByIdPatientGeneralDetailMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public PatientGeneralDetailMasterModel GetByPatientIdWise(int Patient_Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Patient_Id", Patient_Id, DbType.Int32);
            return _dapper.Get<PatientGeneralDetailMasterModel>(CommonSp.getByIdPatientIdWise_GeneralDetailMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

     

        public PatientGeneralDetailMasterModel Insert(PatientGeneralDetailMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Patient_Id  ", model.Patient_Id, DbType.String);
            dbparams.Add("@Temperature ", model.Temperature, DbType.String);
            dbparams.Add("@AdharCard ", model.AdharCard, DbType.String);
            dbparams.Add("@Weight ", model.Weight, DbType.String);
            dbparams.Add("@Bloodpressure ", model.Bloodpressure, DbType.String);
            dbparams.Add("@Remark ", model.Remark, DbType.String);            
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);


            return _dapper.Update<PatientGeneralDetailMasterModel>(CommonSp.savePatientGeneralDetailMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
            

        }

        public PatientGeneralDetailMasterModel Update(PatientGeneralDetailMasterModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@Patient_Id  ", model.Patient_Id, DbType.String);
            dbparams.Add("@Temperature ", model.Temperature, DbType.String);
            dbparams.Add("@AdharCard ", model.AdharCard, DbType.String);
            dbparams.Add("@Weight ", model.Weight, DbType.String);
            dbparams.Add("@Bloodpressure ", model.Bloodpressure, DbType.String);
            dbparams.Add("@Remark ", model.Remark, DbType.String);
            dbparams.Add("@Active ", model.Active, DbType.String);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            dbparams.Add("@UpdatedBy", model.CreatedBy, DbType.Int32);

            return _dapper.Insert<PatientGeneralDetailMasterModel>(CommonSp.updatePatientGeneralDetailMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
    }
}

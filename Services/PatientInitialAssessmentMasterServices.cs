using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class PatientInitialAssessmentMasterServices : IPatientInitialAssessmentMasterServices
    {

        private readonly IDapperService _dapper;

        public PatientInitialAssessmentMasterServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public PatientInitialAssessmentMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<PatientInitialAssessmentMasterModel>(CommonSp.deleteByIdPatientInitialAssessmentMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<PatientInitialAssessmentMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<PatientInitialAssessmentMasterModel>(CommonSp.getAllPatientInitialAssessmentMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public PatientInitialAssessmentMasterModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<PatientInitialAssessmentMasterModel>(CommonSp.getByIdPatientInitialAssessmentMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
        public PatientInitialAssessmentMasterModel GetByPatientId(int Patient_Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Patient_Id", Patient_Id, DbType.Int32);
            return _dapper.Get<PatientInitialAssessmentMasterModel>(CommonSp.getByPatientIdWisePatientInitialAssessmentMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
        public PatientInitialAssessmentMasterModel Insert(PatientInitialAssessmentMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Patient_Id", model.Patient_Id, DbType.Int32);
            dbparams.Add("@ChiefComplaints", model.ChiefComplaints, DbType.String);
            dbparams.Add("@PresentIllness", model.PresentIllness, DbType.String);
            dbparams.Add("@PastHistory", model.PastHistory, DbType.String);
            dbparams.Add("@FutureHistory", model.FutureHistory, DbType.String);
            dbparams.Add("@PersonalHistory", model.PersonalHistory, DbType.String);
            dbparams.Add("@TravelHistory", model.TravelHistory, DbType.String);
            dbparams.Add("@Remake", model.Remake, DbType.String);
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            return _dapper.Update<PatientInitialAssessmentMasterModel>(CommonSp.savePatientInitialAssessmentMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public PatientInitialAssessmentMasterModel Update(PatientInitialAssessmentMasterModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@Patient_Id", model.Patient_Id, DbType.Int32);
            dbparams.Add("@ChiefComplaints", model.ChiefComplaints, DbType.String);
            dbparams.Add("@PresentIllness", model.PresentIllness, DbType.String);
            dbparams.Add("@PastHistory", model.PastHistory, DbType.String);
            dbparams.Add("@FutureHistory", model.FutureHistory, DbType.String);
            dbparams.Add("@PersonalHistory", model.PersonalHistory, DbType.String);
            dbparams.Add("@TravelHistory", model.TravelHistory, DbType.String);
            dbparams.Add("@Remake", model.Remake, DbType.String);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            dbparams.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);
            return _dapper.Insert<PatientInitialAssessmentMasterModel>(CommonSp.updatePatientInitialAssessmentMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
    }
}

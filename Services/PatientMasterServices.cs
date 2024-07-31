using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class PatientMasterServices : IPatientMasterServices
    {
        private readonly IDapperService _dapper;

        public PatientMasterServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public PatientMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<PatientMasterModel>(CommonSp.deleteByIdPatientMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring); ;

        }

        public List<PatientMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<PatientMasterModel>(CommonSp.getAllPatientMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public List<PatientMasterModel> GetByClinicIdWisePatient(ref int TotalCount,int UserId, int ClinicId, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
        {

            if (qcnd == null)
                qcnd = "";

            var dbparams = new DynamicParameters();
            dbparams.Add("@Clinic_Id", ClinicId, DbType.Int32);
            dbparams.Add("@CreatedBy", UserId, DbType.Int32);
            dbparams.Add("@currentPage", currentPage, DbType.String);
            //searchString = "%" + searchString + "%";
            dbparams.Add("@searchString", searchString, DbType.String);
            dbparams.Add("@pageSize", pageSize, DbType.String);

            sortOrder = sortCol + " " + sortOrder;
            dbparams.Add("@qcnd", qcnd, DbType.String);

            dbparams.Add("@sortOrder", sortOrder, DbType.String);
            dbparams.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //dbparams.Add("@Date", Date, DbType.String);

            var res = _dapper.GetAll<PatientMasterModel>(CommonSp.GetAllPatientLoginUserWise, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public PatientMasterModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<PatientMasterModel>(CommonSp.getByIdPatientMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public PatientMasterModel Insert(PatientMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Fname ", model.Fname, DbType.String);
            dbparams.Add("@Lname ", model.Lname, DbType.String);
            dbparams.Add("@Address ", model.Address, DbType.String);
            dbparams.Add("@City ", model.City, DbType.String);
            dbparams.Add("@State ", model.State, DbType.String);
            dbparams.Add("@Pincode ", model.Pincode, DbType.String);
            dbparams.Add("@Email", model.Email, DbType.String);
            dbparams.Add("@ContactNo", model.ContactNo, DbType.String);
            dbparams.Add("@DOB", model.DOB, DbType.Date);
            dbparams.Add("@Age", model.Age, DbType.String);
            dbparams.Add("@Gender", model.Gender, DbType.String);
            dbparams.Add("@Clinic_Id", model.Clinic_Id, DbType.Int32);
            dbparams.Add("@Father_HusbandName ", model.Father_HusbandName, DbType.String);
            dbparams.Add("@MotherName ", model.MotherName, DbType.String);
            dbparams.Add("@BloodGroup ", model.BloodGroup, DbType.String);
            dbparams.Add("@MaritalStatus ", model.MaritalStatus, DbType.String);
            dbparams.Add("@Nationality ", model.Nationality, DbType.String);
            dbparams.Add("@Occupation ", model.Occupation, DbType.String);
            dbparams.Add("@Houseno  ", model.Houseno, DbType.String);
            dbparams.Add("@District  ", model.District, DbType.String);
            dbparams.Add("@Area   ", model.Area, DbType.String);
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            dbparams.Add("@EntryDateTime", model.EntryDateTime, DbType.String);
            dbparams.Add("@ReceiptNo", model.ReceiptNo, DbType.String);
            dbparams.Add("@UHID", model.UHID, DbType.String);

            dbparams.Add("@Temperature ", model.Temperature, DbType.Decimal);
            dbparams.Add("@AdharCard ", model.AdharCard, DbType.String);
            dbparams.Add("@Weight ", model.Weight, DbType.String);
            dbparams.Add("@Bloodpressure ", model.Bloodpressure, DbType.String);
            dbparams.Add("@Remark ", model.Remark, DbType.String);

            dbparams.Add("@User_Id  ", model.User_id, DbType.Int32);
            dbparams.Add("@Consultant_Charges ", model.Consultant_Charges, DbType.String);
            dbparams.Add("@Discount ", model.Discount, DbType.String);
            dbparams.Add("@RefundAmount ", model.RefundAmount, DbType.String);
            dbparams.Add("@TotalAmount ", model.TotalAmount, DbType.String);
            dbparams.Add("@RefBy_Name ", model.RefBy_Name, DbType.String);
            dbparams.Add("@RefBy_Address ", model.RefBy_Address, DbType.String);
            dbparams.Add("@PaymentMode ", model.PaymentMode, DbType.String);
            dbparams.Add("@EmergencyAdminssion ", model.EmergencyAdminssion, DbType.Boolean);
            dbparams.Add("@FileChargesApplicable ", model.FileChargesApplicable, DbType.Boolean);

            dbparams.Add("@ActivityTypeId", model.ActivityTypeId, DbType.Int32);
            //dbparams.Add("@ActivityName ", model.ActivityName, DbType.String);
            dbparams.Add("@ActivityBy ", model.ActivityBy, DbType.Int32);
            dbparams.Add("@Description ", model.Description, DbType.String);
            dbparams.Add("@currentUser ", model.CreatedBy, DbType.Int32);

            return _dapper.Update<PatientMasterModel>(CommonSp.savePatientMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public PatientMasterModel Update(PatientMasterModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@Fname ", model.Fname, DbType.String);
            dbparams.Add("@Lname ", model.Lname, DbType.String);
            dbparams.Add("@Address ", model.Address, DbType.String);
            dbparams.Add("@City ", model.City, DbType.String);
            dbparams.Add("@State ", model.State, DbType.String);
            dbparams.Add("@Pincode ", model.Pincode, DbType.String);
            dbparams.Add("@Email", model.Email, DbType.String);
            dbparams.Add("@ContactNo", model.ContactNo, DbType.String);
            dbparams.Add("@DOB", model.DOB, DbType.Date);
            dbparams.Add("@Age", model.Age, DbType.String);
            dbparams.Add("@Gender", model.Gender, DbType.String);
            dbparams.Add("@Clinic_Id", model.Clinic_Id, DbType.Int32);
            dbparams.Add("@Father_HusbandName ", model.Father_HusbandName, DbType.String);
            dbparams.Add("@MotherName ", model.MotherName, DbType.String);
            dbparams.Add("@BloodGroup ", model.BloodGroup, DbType.String);
            dbparams.Add("@MaritalStatus ", model.MaritalStatus, DbType.String);
            dbparams.Add("@Nationality ", model.Nationality, DbType.String);
            dbparams.Add("@Occupation ", model.Occupation, DbType.String);
            dbparams.Add("@Houseno  ", model.Houseno, DbType.String);
            dbparams.Add("@District  ", model.District, DbType.String);
            dbparams.Add("@Area   ", model.Area, DbType.String);
            dbparams.Add("@Active ", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete ", model.IsDelete, DbType.Boolean);
            dbparams.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);           

            return _dapper.Insert<PatientMasterModel>(CommonSp.updatePatientMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public PatientConsultantMasterModel GetConsultantByPatientId(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Patient_Id", Id, DbType.Int32);
            return _dapper.Get<PatientConsultantMasterModel>(CommonSp.getByIdPatientIdWise_Consultant, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<PatientMasterModel> GetPatientData()
        {
            var dbparams = new DynamicParameters();

            return _dapper.GetAll<PatientMasterModel>(CommonSp.GetpatientData, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<PatientMasterModel> GetUserWisePatientData(ref int TotalCount, int UserId, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
        {
            if (qcnd == null)
                qcnd = "";

            var dbparams = new DynamicParameters();
            dbparams.Add("@User_Id", UserId, DbType.Int32);
            dbparams.Add("@currentPage", currentPage, DbType.String);
            //searchString = "%" + searchString + "%";
            dbparams.Add("@searchString", searchString, DbType.String);
            dbparams.Add("@pageSize", pageSize, DbType.String);

            sortOrder = sortCol + " " + sortOrder;
            dbparams.Add("@qcnd", qcnd, DbType.String);

            dbparams.Add("@sortOrder", sortOrder, DbType.String);
            dbparams.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //dbparams.Add("@Date", Date, DbType.String);

            var res = _dapper.GetAll<PatientMasterModel>(CommonSp.GetUserWisePatient, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public PatientMasterModel PatientStatusUpdate(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<PatientMasterModel>(CommonSp.GetPatientStatusUpdate, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public PatientMasterModel GetConsultantPatient(int User_Id, string currentDate)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@userId", User_Id, DbType.Int32);
            dbparams.Add("@entrydatetime", currentDate, DbType.String);
            return _dapper.Get<PatientMasterModel>("usp_Consultant_Dashboard_Patient", dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
    }
}

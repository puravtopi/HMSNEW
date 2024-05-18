using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class PatientConsultantMasterServices : IPatientConsultantMasterServices
    {
        private readonly IDapperService _dapper;

        public PatientConsultantMasterServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public PatientConsultantMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<PatientConsultantMasterModel>(CommonSp.deleteByIdPatientConsultantMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring); ;

        }

        public List<PatientConsultantMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<PatientConsultantMasterModel>(CommonSp.getAllPatientConsultantMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public PatientConsultantMasterModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<PatientConsultantMasterModel>(CommonSp.getByIdPatientConsultantMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public PatientConsultantMasterModel GetByPatientIdWise(int Patient_Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Patient_Id", Patient_Id, DbType.Int32);
            return _dapper.Get<PatientConsultantMasterModel>(CommonSp.getByPatientIdWise_Consultant, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public PatientConsultantMasterModel Insert(PatientConsultantMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@User_Id  ", model.User_Id, DbType.Int32);
            dbparams.Add("@Consultant_Charges ", model.Consultant_Charges, DbType.String);
            dbparams.Add("@Discount ", model.Discount, DbType.String);
            dbparams.Add("@RefundAmount ", model.RefundAmount, DbType.String);
            dbparams.Add("@TotalAmount ", model.TotalAmount, DbType.String);
            dbparams.Add("@RefBy_Name ", model.RefBy_Name, DbType.String);
            dbparams.Add("@RefBy_Address ", model.RefBy_Address, DbType.String);
            //dbparams.Add("@EmergencyAdminssion ", model.EmergencyAdminssion, DbType.Boolean);
            //dbparams.Add("@FileChargesApplicable ", model.FileChargesApplicable, DbType.Boolean);
            dbparams.Add("@PaymentMode ", model.PaymentMode, DbType.String);
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);


            return _dapper.Update<PatientConsultantMasterModel>(CommonSp.savePatientConsultantMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);


        }

        public PatientConsultantMasterModel Update(PatientConsultantMasterModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@User_Id  ", model.User_Id, DbType.Int32);
            dbparams.Add("@Consultant_Charges ", model.Consultant_Charges, DbType.String);
            dbparams.Add("@Discount ", model.Discount, DbType.String);
            dbparams.Add("@RefundAmount ", model.RefundAmount, DbType.String);
            dbparams.Add("@TotalAmount ", model.TotalAmount, DbType.String);
            dbparams.Add("@RefBy_Name ", model.RefBy_Name, DbType.String);
            dbparams.Add("@RefBy_Address ", model.RefBy_Address, DbType.String);
            //dbparams.Add("@EmergencyAdminssion ", model.EmergencyAdminssion, DbType.Boolean);
            //dbparams.Add("@FileChargesApplicable ", model.FileChargesApplicable, DbType.Boolean);
            dbparams.Add("@PaymentMode ", model.PaymentMode, DbType.String);
            dbparams.Add("@Active ", model.Active, DbType.String);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            dbparams.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);

            return _dapper.Insert<PatientConsultantMasterModel>(CommonSp.updatePatientConsultantMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
    }
}

using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace HMS.Services
{
    public class RevisitDetailMasterServices: IRevisitDetailMasterServices
    {
        private readonly IDapperService _dapper;

        public RevisitDetailMasterServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public RevisitDetailModel Insert(RevisitDetailModel model)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("@Patient_Id  ", model.Patient_Id, DbType.Int32);
                dbparams.Add("@UHID ", model.UHID, DbType.Int32);
                dbparams.Add("@ConsultantId ", model.ConsultantId, DbType.Int32);
                dbparams.Add("@Discount ", model.Discount, DbType.Decimal);
                dbparams.Add("@GenRemarks ", model.GenRemarks, DbType.String);
                dbparams.Add("@ClinicalDiagnosis ", model.ClinicalDiagnosis, DbType.String);
                dbparams.Add("@PatientName  ", model.PatientName, DbType.String);
                dbparams.Add("@ConsultantName ", model.ConsultantName, DbType.String);
                dbparams.Add("@NetAmount ", model.NetAmount, DbType.Decimal);
                dbparams.Add("@GeneralAdvice ", model.GeneralAdvice, DbType.String);
                dbparams.Add("@RevisitDate ", model.RevisitDate, DbType.DateTime);
                dbparams.Add("@RevisitCharges ", model.RevisitCharges, DbType.Decimal);
                dbparams.Add("@ReceiptNo  ", model.ReceiptNo, DbType.String);
                dbparams.Add("@RefReceiptNo ", model.RefReceiptNo, DbType.String);
                dbparams.Add("@RefRemarks ", model.RefRemarks, DbType.String);
                dbparams.Add("@AddToTemplate ", model.AddToTemplate, DbType.Boolean);
                dbparams.Add("@UNPAID ", model.UNPAID, DbType.Int32);
                dbparams.Add("@Complimentary ", model.Complimentary, DbType.Boolean);
                dbparams.Add("@CreatedDate", model.CreatedDate, DbType.DateTime);
                dbparams.Add("@CreatedBy  ", model.CreatedBy, DbType.String);
                dbparams.Add("@UpdatedDate ", model.UpdatedDate, DbType.DateTime);
                dbparams.Add("@UpdatedBy ", model.UpdatedBy, DbType.String);
                dbparams.Add("@Active ", model.Active, DbType.Boolean);
                dbparams.Add("@IsDelete ", model.IsDelete, DbType.Boolean);
                dbparams.Add("@DeletedBy ", model.DeletedBy, DbType.String);                
                dbparams.Add("@DeletedDate", model.DeletedDate, DbType.DateTime);
                dbparams.Add("@SpecifyRevisitDay", model.SpecifyRevisitDay, DbType.Int32);
                dbparams.Add("@OPDCharges", model.OPDCharges, DbType.Decimal);
                dbparams.Add("@Patient_Emergency", model.Patient_Emergency, DbType.Boolean);
                return _dapper.Update<RevisitDetailModel>(CommonSp.saveRevisitDetail, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public RevisitDetailModel Update(RevisitDetailModel model)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("@Id  ", model.Id, DbType.Int32);
                dbparams.Add("@Patient_Id  ", model.Patient_Id, DbType.Int32);
                dbparams.Add("@UHID ", model.UHID, DbType.Int32);
                dbparams.Add("@ConsultantId ", model.ConsultantId, DbType.Int32);
                dbparams.Add("@Discount ", model.Discount, DbType.Decimal);
                dbparams.Add("@GenRemarks ", model.GenRemarks, DbType.String);
                dbparams.Add("@ClinicalDiagnosis ", model.ClinicalDiagnosis, DbType.String);
                dbparams.Add("@PatientName  ", model.PatientName, DbType.String);
                dbparams.Add("@ConsultantName ", model.ConsultantName, DbType.String);
                dbparams.Add("@NetAmount ", model.NetAmount, DbType.Decimal);
                dbparams.Add("@GeneralAdvice ", model.GeneralAdvice, DbType.String);
                dbparams.Add("@RevisitDate ", model.RevisitDate, DbType.Date);
                dbparams.Add("@RevisitCharges ", model.RevisitCharges, DbType.Decimal);
                dbparams.Add("@ReceiptNo  ", model.ReceiptNo, DbType.String);
                dbparams.Add("@RefReceiptNo ", model.RefReceiptNo, DbType.String);
                dbparams.Add("@RefRemarks ", model.RefRemarks, DbType.String);
                dbparams.Add("@AddToTemplate ", model.AddToTemplate, DbType.Boolean);
                dbparams.Add("@UNPAID ", model.UNPAID, DbType.Int32);
                dbparams.Add("@Complimentary ", model.Complimentary, DbType.Boolean);
                dbparams.Add("@UpdatedDate ", model.UpdatedDate, DbType.Date);
                dbparams.Add("@UpdatedBy ", model.UpdatedBy, DbType.String);
                dbparams.Add("@Active ", model.Active, DbType.Boolean);
                dbparams.Add("@IsDelete ", model.IsDelete, DbType.Boolean);
                dbparams.Add("@DeletedBy ", model.DeletedBy, DbType.String);
                dbparams.Add("@DeletedDate", model.DeletedDate, DbType.Date);
                dbparams.Add("@SpecifyRevisitDay", model.SpecifyRevisitDay, DbType.Int32);
                dbparams.Add("@OPDCharges", model.OPDCharges, DbType.Decimal);
                dbparams.Add("@Patient_Emergency", model.Patient_Emergency, DbType.Boolean);
                return _dapper.Update<RevisitDetailModel>(CommonSp.updateRevisitDetail, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public RevisitDetailModel GetTopOneRevisitDetail()
        {
            var dbparams = new DynamicParameters();
            return _dapper.Get<RevisitDetailModel>(CommonSp.GetTopOneRevisitDetail, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
        }

        public List<RevisitDetailModel> GetAll(int Patient_Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Patient_Id  ", Patient_Id, DbType.Int32);
            var res = _dapper.GetAll<RevisitDetailModel>(CommonSp.getAllRevisitDetail, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            return res;
        }

        public RevisitDetailModel GetAllById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id  ", Id, DbType.Int32);
            var res = _dapper.Get<RevisitDetailModel>(CommonSp.getAllById, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            return res;
        }

        public RevisitDetailModel DeteleRevisitDetail(int Id,int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Get<RevisitDetailModel>(CommonSp.deleteByIdRevisitDetail, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

    }
}

using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

namespace HMS.Services
{
    public class PatientServiceMasterServices : IPatientServiceMasterServices
    {
        private readonly IDapperService _dapper;
        public PatientServiceMasterServices(IDapperService dapper)
        {
            _dapper = dapper;
        }
        public PatientServiceMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<PatientServiceMasterModel>(CommonSp.deleteByIdPatientServiceMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring); ;

        }

        public List<PatientServiceMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<PatientServiceMasterModel>(CommonSp.getAllPatientServiceMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public List<PatientServiceMasterModel> GetAll(int Patient_Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Patient_Id", Patient_Id, DbType.Int32);
            return _dapper.GetAll<PatientServiceMasterModel>(CommonSp.GetPatientWisePatientServiceData, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public PatientServiceMasterModel GetById(int id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", id, DbType.Int32);
            return _dapper.Get<PatientServiceMasterModel>(CommonSp.getByIdPatientServiceMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public PatientServiceMasterModel Insert(PatientServiceMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Patient_Id  ", model.Patient_Id, DbType.Int32);
            dbparams.Add("@Department_Id ", model.Department_Id, DbType.Int32);
            dbparams.Add("@Consultant_Id ", model.Consultant_Id, DbType.Int32);
            dbparams.Add("@ServiceHead_Id ", model.ServiceHead_Id, DbType.Int32);
            dbparams.Add("@Service_Id ", model.Service_Id, DbType.Int32);
            dbparams.Add("@Revisit_Id ", model.Revisit_Id, DbType.Int32);
            dbparams.Add("@Charges ", model.Charges, DbType.String);
            dbparams.Add("@Discount", model.Discount, DbType.String);
            dbparams.Add("@NetAmount", model.NetAmount, DbType.String);
            dbparams.Add("@ReceiptNo", model.ReceiptNo, DbType.String);
            dbparams.Add("@ServiceDate", model.ServiceDate, DbType.String);
            dbparams.Add("@RefundDate", model.RefundDate, DbType.String);
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            
            return _dapper.Update<PatientServiceMasterModel>(CommonSp.savePatientServiceMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public PatientServiceMasterModel Update(PatientServiceMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@Patient_Id  ", model.Patient_Id, DbType.Int32);
            dbparams.Add("@Department_Id ", model.Department_Id, DbType.Int32);
            dbparams.Add("@Consultant_Id ", model.Consultant_Id, DbType.Int32);
            dbparams.Add("@ServiceHead_Id ", model.ServiceHead_Id, DbType.Int32);
            dbparams.Add("@Service_Id ", model.Service_Id, DbType.Int32);
            dbparams.Add("@Revisit_Id ", model.Revisit_Id, DbType.Int32);
            dbparams.Add("@Charges ", model.Charges, DbType.String);
            dbparams.Add("@Discount", model.Discount, DbType.String);
            dbparams.Add("@NetAmount", model.NetAmount, DbType.String);
            dbparams.Add("@ReceiptNo", model.ReceiptNo, DbType.String);
            dbparams.Add("@ServiceDate", model.ServiceDate, DbType.String);
            dbparams.Add("@RefundDate", model.RefundDate, DbType.String);            
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            dbparams.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);
            
            return _dapper.Insert<PatientServiceMasterModel>(CommonSp.updatePatientServiceMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
        public List<PatientServiceMasterModel> GetPatienServiceData()
        {
            var dbparams = new DynamicParameters();
            return _dapper.GetAll<PatientServiceMasterModel>(CommonSp.GetPatientService, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }
    }
}
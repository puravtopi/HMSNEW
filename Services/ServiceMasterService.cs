using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class ServiceMasterService : IServiceMasterService
    {
        private readonly IDapperService _dapper;

        public ServiceMasterService(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public ServiceMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<ServiceMasterModel>(CommonSp.deleteByIdServiceMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<ServiceMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<ServiceMasterModel>(CommonSp.getAllServiceMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public ServiceMasterModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<ServiceMasterModel>(CommonSp.getByIdServiceMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<ServiceMasterModel> GetByServiceHeadIdWiseServiceList(int ServiceHead_Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@ServiceHead_Id", ServiceHead_Id, DbType.Int32);
            return _dapper.GetAll<ServiceMasterModel>(CommonSp.GetAllService_ServiceHeadWise, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
        }

        public ServiceMasterModel Insert(ServiceMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@ServiceName", model.ServiceName, DbType.String);
            dbparams.Add("@Department_Id", model.Department_Id, DbType.Int32);
            dbparams.Add("@ServiceHead_Id", model.ServiceHead_Id, DbType.Int32);
            dbparams.Add("@Charges", model.Charges, DbType.String);
            dbparams.Add("@Lab_Legend", model.Lab_Legend, DbType.String);           
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);            
            //dbparams.Add("@Clinic_Id", model.Clinic_Id, DbType.Int32);            
            return _dapper.Update<ServiceMasterModel>(CommonSp.saveServiceMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public ServiceMasterModel Update(ServiceMasterModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@ServiceName", model.ServiceName, DbType.String);
            dbparams.Add("@ServiceHead_Id", model.ServiceHead_Id, DbType.Int32);
            dbparams.Add("@Department_Id", model.Department_Id, DbType.Int32);            
            dbparams.Add("@Charges", model.Charges, DbType.String);
            dbparams.Add("@Lab_Legend ", model.Lab_Legend, DbType.String);            
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            dbparams.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);
            return _dapper.Insert<ServiceMasterModel>(CommonSp.updateServiceMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public ServiceMasterModel InsertPatientServiceMasterDetails(ServiceMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@PatientServiceMasterId", model.PatientServiceMasterId, DbType.Int32);
            dbparams.Add("@ServiceId", model.ServiceId, DbType.Int32);
            dbparams.Add("@Charges", model.Charges, DbType.Decimal);
            dbparams.Add("@Discount", model.Discount, DbType.Decimal);
            dbparams.Add("@NetAmount", model.NetAmount, DbType.String);
            dbparams.Add("@Active", model.Active, DbType.Int32);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            dbparams.Add("@CurrentUser", model.CreatedBy, DbType.Int32);
            return _dapper.Update<ServiceMasterModel>(CommonSp.savePatientServiceMasterDetails, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
        }

        public List<PatientServiceMasterModel> GetAllPatientServiceMasterDetails(int Patient_Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Patient_Id", Patient_Id, DbType.Int32);
            return _dapper.GetAll<PatientServiceMasterModel>(CommonSp.GetAllPatientServiceMasterDetails, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
        }
    }
}

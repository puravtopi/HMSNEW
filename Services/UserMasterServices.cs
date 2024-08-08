using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace HMS.Services
{
    public class UserMasterServices : IUserMasterServices
    {
        private readonly IDapperService _dapper;

        public UserMasterServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public UserMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<UserMasterModel>(CommonSp.deleteByIdUserMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<UserMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<UserMasterModel>(CommonSp.getAllUserMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public List<UserMasterModel> GetByClinicIdWiseUser(ref int TotalCount, int ClinicId, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@ClinicId", ClinicId, DbType.Int32);
            dbparams.Add("@currentPage", currentPage, DbType.String);
            //searchString = "%" + searchString + "%";
            dbparams.Add("@searchString", searchString, DbType.String);
            dbparams.Add("@pageSize", pageSize, DbType.String);

            sortOrder = sortCol + " " + sortOrder;
            dbparams.Add("@qcnd", qcnd, DbType.String);

            dbparams.Add("@sortOrder", sortOrder, DbType.String);
            dbparams.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //dbparams.Add("@Date", Date, DbType.String);
            var res = _dapper.GetAll<UserMasterModel>(CommonSp.getByClinicIdWiseUSER, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public List<UserMasterModel> GetByClinicIdWiseUserList(int ClinicId)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Clinic_Id", ClinicId, DbType.Int32);
            return _dapper.GetAll<UserMasterModel>(CommonSp.getByClinicIdWiseUserList, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
        }

        public List<UserMasterModel> GetByDepartmentIdWiseUserList(int Department_Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Dept_Id", Department_Id, DbType.Int32);
            return _dapper.GetAll<UserMasterModel>(CommonSp.getByDepartmentIdWise_User, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
        }

        public UserMasterModel GetByEmail(string email)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("@Email", email, DbType.String);
                return _dapper.Get<UserMasterModel>(CommonSp.getByEmailUserMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            
        }

        public UserMasterModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<UserMasterModel>(CommonSp.getByIdUserMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public UserMasterModel Insert(UserMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@FirstName ", model.FirstName, DbType.String);
            dbparams.Add("@LastName ", model.LastName, DbType.String);
            dbparams.Add("@Clinic_Id", model.Clinic_Id, DbType.Int32);
            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Email", model.Email, DbType.String);
            dbparams.Add("@Password", model.Password, DbType.String);
            dbparams.Add("@Dept_Id", model.Dept_id, DbType.Int16);
            dbparams.Add("@Desig_Id", model.Desig_Id, DbType.Int16);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            dbparams.Add("@OPD_Charge", model.OPD_Charge, DbType.String);
            dbparams.Add("@Night_Charge", model.Night_Charge, DbType.String);
            dbparams.Add("@Emergency_Charge", model.Emergency_Charge, DbType.String);
            dbparams.Add("@SpecifyRevisit", model.SpecifyRevisit, DbType.String);
            dbparams.Add("@Revisit_Charge",model.Revisit_Charge, DbType.String);

            return _dapper.Update<UserMasterModel>(CommonSp.saveUserMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public UserMasterModel Update(UserMasterModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@FirstName ", model.FirstName, DbType.String);
            dbparams.Add("@LastName ", model.LastName, DbType.String);
            dbparams.Add("@Email ", model.Email, DbType.String);
            dbparams.Add("@Clinic_Id", model.Clinic_Id, DbType.Int32);
            dbparams.Add("@Active ", model.Active, DbType.String);
            dbparams.Add("@UpdatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@OPD_Charge", model.OPD_Charge, DbType.String);
            dbparams.Add("@Night_Charge", model.Night_Charge, DbType.String);
            dbparams.Add("@Emergency_Charge", model.Emergency_Charge, DbType.String);
            dbparams.Add("@SpecifyRevisit", model.SpecifyRevisit, DbType.String);
            dbparams.Add("@Dept_Id", model.Dept_id, DbType.Int32);
            dbparams.Add("@Desig_Id", model.Desig_Id, DbType.Int32);
            dbparams.Add("@Revisit_Charge", model.Revisit_Charge, DbType.String);

            return _dapper.Insert<UserMasterModel>(CommonSp.updateUserMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public UserMasterModel UpdatePassword(UserMasterModel model)
        {
            var dbparams = new DynamicParameters();
            //GEt Id used Session ID
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@Password ", model.Password ,DbType.String);            
            dbparams.Add("@UpdatedBy ", model.UpdatedBy ,DbType.String);            
            
            return _dapper.Insert<UserMasterModel>(CommonSp.updateChangePassword, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<UserMasterModel> GetByDesignationIdWiseUserList(int Desig_Id)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("@Desig_Id", Desig_Id, DbType.Int32);
                return _dapper.GetAll<UserMasterModel>(CommonSp.getByDesignationIdWise_User, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
}
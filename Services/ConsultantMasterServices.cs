using Dapper;
using HMS.Common;
using HMS.Interface;
using HMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;

namespace HMS.Services
{
    public class ConsultantMasterServices : IConsultantMasterServices
    {
        private readonly IDapperService _dapper;

        public ConsultantMasterServices(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public ConsultantMasterModel DeleteById(int Id, int DeletedBy)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            dbparams.Add("@Deleted_By", DeletedBy, DbType.Int32);
            return _dapper.Update<ConsultantMasterModel>(CommonSp.deleteByIdConsultantMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<ConsultantMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
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

            var res = _dapper.GetAll<ConsultantMasterModel>(CommonSp.getAllConsultantMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public List<ConsultantMasterModel> GetByClinicIdWiseConsultant(ref int TotalCount, int ClinicId, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Clinic_Id", ClinicId, DbType.Int32);
            dbparams.Add("@currentPage", currentPage, DbType.String);
            //searchString = "%" + searchString + "%";
            dbparams.Add("@searchString", searchString, DbType.String);
            dbparams.Add("@pageSize", pageSize, DbType.String);

            sortOrder = sortCol + " " + sortOrder;
            dbparams.Add("@qcnd", qcnd, DbType.String);

            dbparams.Add("@sortOrder", sortOrder, DbType.String);
            dbparams.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //dbparams.Add("@Date", Date, DbType.String);

            var res = _dapper.GetAll<ConsultantMasterModel>(CommonSp.getByClinicIdWiseCONSULTANT, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            TotalCount = dbparams.Get<int>("TotalCount");
            return res;
        }

        public List<ConsultantMasterModel> GetBYDepartmentIdWiseConsultant(int Department_Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Dept_Id", Department_Id, DbType.Int32);
            return _dapper.GetAll<ConsultantMasterModel>(CommonSp.getByDepartmentIdWise_Consultant, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public ConsultantMasterModel GetById(int Id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Id", Id, DbType.Int32);
            return _dapper.Get<ConsultantMasterModel>(CommonSp.getByIdConsultantMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }



        public ConsultantMasterModel Insert(ConsultantMasterModel model)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@Dept_id ", model.Department_Id, DbType.String);
            dbparams.Add("@Clinic_Id", model.Clinic_Id, DbType.String);
            dbparams.Add("@ConsultantName", model.ConsultantName, DbType.String);

            dbparams.Add("@Consultant_Code ", model.Consultant_Code, DbType.String);
            dbparams.Add("@Speciality_in", model.Speciality_in, DbType.String);
            dbparams.Add("@ConsultantName_in", model.ConsultantName_in, DbType.String);
            dbparams.Add("@Sub_Deptt", model.Sub_Deptt, DbType.String);
            dbparams.Add("@HouseNo", model.HouseNo, DbType.String);
            dbparams.Add("@City", model.City, DbType.String);
            dbparams.Add("@State", model.State, DbType.String);
            dbparams.Add("@Pincode", model.Pincode, DbType.String);
            dbparams.Add("@Phone", model.Phone, DbType.String);
            dbparams.Add("@Mobile", model.Mobile, DbType.String);

            dbparams.Add("@Consultant_OPD_Charge", model.Consultant_OPD_Charge, DbType.String);
            dbparams.Add("@Consultant_Night_Charge", model.Consultant_Night_Charge, DbType.String);
            dbparams.Add("@Consultant_Emergency_Charge", model.Consultant_Emergency_Charge, DbType.String);
            dbparams.Add("@Consultant_Revisit_ChargesForOPD", model.Consultant_Revisit_ChargesForOPD, DbType.String);
            dbparams.Add("@Consultant_Revisit_ChargesForIP", model.Consultant_Revisit_ChargesForIP, DbType.String);
            dbparams.Add("@Hospital_OPD_Charge", model.Hospital_OPD_Charge, DbType.String);
            dbparams.Add("@Hospital_Night_Charge", model.Hospital_Night_Charge, DbType.String);
            dbparams.Add("@Hospital_Emergency_Charge", model.Hospital_Emergency_Charge, DbType.String);
            dbparams.Add("@Hospital_Revisit_ChargesForOPD", model.Hospital_Revisit_ChargesForOPD, DbType.String);
            dbparams.Add("@Hospital_Revisit_ChargesForIP", model.Hospital_Revisit_ChargesForIP, DbType.String);
            dbparams.Add("@SpecifyRevisit", model.SpecifyRevisit, DbType.String);
            dbparams.Add("@DifferentReceiptForHospitalCharges", model.DifferentReceiptForHospitalCharges, DbType.Boolean);

            dbparams.Add("@Consultant_Deluxe_Charges", model.Consultant_Deluxe_Charges, DbType.String);
            dbparams.Add("@Consultant_Others_Charges", model.Consultant_Others_Charges, DbType.String);
            dbparams.Add("@Consultant_Private_Charge", model.Consultant_Private_Charge, DbType.String);
            dbparams.Add("@Consultant_Semi_Private_Charge", model.Consultant_Semi_Private_Charge, DbType.String);
            dbparams.Add("@Hospital_Deluxe_Charges", model.Hospital_Deluxe_Charges, DbType.String);
            dbparams.Add("@Hospital_Others_Charges", model.Hospital_Others_Charges, DbType.String);
            dbparams.Add("@Hospital_Private_Charge", model.Hospital_Private_Charge, DbType.String);
            dbparams.Add("@Hospital_Semi_Private_Charge", model.Hospital_Semi_Private_Charge, DbType.String);

            dbparams.Add("@FormelInfo", model.FormelInfo, DbType.String);
            dbparams.Add("@Specifications", model.Specifications, DbType.String);

            dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            dbparams.Add("@Active", model.Active, DbType.Boolean);
            dbparams.Add("@IsDelete", model.IsDelete, DbType.Boolean);

            return _dapper.Update<ConsultantMasterModel>(CommonSp.saveConsultantMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public ConsultantMasterModel Update(ConsultantMasterModel model)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("@Id ", model.Id, DbType.Int32);
            dbparams.Add("@Dept_id ", model.Department_Id, DbType.String);
            dbparams.Add("@Clinic_Id", model.Clinic_Id, DbType.String);
            dbparams.Add("@ConsultantName", model.ConsultantName, DbType.String);
            dbparams.Add("@Consultant_Code ", model.Consultant_Code, DbType.String);
            dbparams.Add("@Speciality_in", model.Speciality_in, DbType.String);
            dbparams.Add("@ConsultantName_in", model.ConsultantName_in, DbType.String);
            dbparams.Add("@Sub_Deptt", model.Sub_Deptt, DbType.String);
            dbparams.Add("@HouseNo", model.HouseNo, DbType.String);
            dbparams.Add("@City", model.City, DbType.String);
            dbparams.Add("@State", model.State, DbType.String);
            dbparams.Add("@Pincode", model.Pincode, DbType.String);
            dbparams.Add("@Phone", model.Phone, DbType.String);
            dbparams.Add("@Mobile", model.Mobile, DbType.String);

            dbparams.Add("@Consultant_OPD_Charge", model.Consultant_OPD_Charge, DbType.String);
            dbparams.Add("@Consultant_Night_Charge", model.Consultant_Night_Charge, DbType.String);
            dbparams.Add("@Consultant_Emergency_Charge", model.Consultant_Emergency_Charge, DbType.String);
            dbparams.Add("@Consultant_Revisit_ChargesForOPD", model.Consultant_Revisit_ChargesForOPD, DbType.String);
            dbparams.Add("@Consultant_Revisit_ChargesForIP", model.Consultant_Revisit_ChargesForIP, DbType.String);
            dbparams.Add("@Hospital_OPD_Charge", model.Hospital_OPD_Charge, DbType.String);
            dbparams.Add("@Hospital_Night_Charge", model.Hospital_Night_Charge, DbType.String);
            dbparams.Add("@Hospital_Emergency_Charge", model.Hospital_Emergency_Charge, DbType.String);
            dbparams.Add("@Hospital_Revisit_ChargesForOPD", model.Hospital_Revisit_ChargesForOPD, DbType.String);
            dbparams.Add("@Hospital_Revisit_ChargesForIP", model.Hospital_Revisit_ChargesForIP, DbType.String);
            dbparams.Add("@SpecifyRevisit", model.SpecifyRevisit, DbType.String);
            dbparams.Add("@DifferentReceiptForHospitalCharges", model.DifferentReceiptForHospitalCharges, DbType.Boolean);

            dbparams.Add("@Consultant_Deluxe_Charges", model.Consultant_Deluxe_Charges, DbType.String);
            dbparams.Add("@Consultant_Others_Charges", model.Consultant_Others_Charges, DbType.String);
            dbparams.Add("@Consultant_Private_Charge", model.Consultant_Private_Charge, DbType.String);
            dbparams.Add("@Consultant_Semi_Private_Charge", model.Consultant_Semi_Private_Charge, DbType.String);
            dbparams.Add("@Hospital_Deluxe_Charges", model.Hospital_Deluxe_Charges, DbType.String);
            dbparams.Add("@Hospital_Others_Charges", model.Hospital_Others_Charges, DbType.String);
            dbparams.Add("@Hospital_Private_Charge", model.Hospital_Private_Charge, DbType.String);
            dbparams.Add("@Hospital_Semi_Private_Charge", model.Hospital_Semi_Private_Charge, DbType.String);

            dbparams.Add("@FormelInfo", model.FormelInfo, DbType.String);
            dbparams.Add("@Specifications", model.Specifications, DbType.String);
            dbparams.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);


            return _dapper.Insert<ConsultantMasterModel>(CommonSp.updateConsultantMaster, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

        }

        public List<SelectListItem> GetConsultantList()
        {

            var res = _dapper.GetAll<ConsultantMasterModel>(CommonSp.getAllConsultantMasterList, null, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);
            var designationlist = new List<SelectListItem>();

            foreach (var item in res)
            {
                designationlist.Add(new SelectListItem
                {
                    Text = item.ConsultantName,
                    Value = item.Id.ToString()
                });
            }
            return designationlist;
        }



        public ClinicMasterModel UpdateConsultant_Code(ConsultantMasterModel model)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("@Id ", model.Id, DbType.Int32);
                dbparams.Add("@Code ", model.Consultant_Code, DbType.String);
                return _dapper.Insert<ClinicMasterModel>(CommonSp.updateConsultantMasterCode, dbparams, commandType: CommandType.StoredProcedure, ConnStrings.HMSConnectionstring);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}

using HMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IConsultantMasterServices
    {
        public List<ConsultantMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public ConsultantMasterModel GetById(int id);
        public ConsultantMasterModel Insert(ConsultantMasterModel model);
        public ConsultantMasterModel Update(ConsultantMasterModel model);
        public ConsultantMasterModel DeleteById(int Id, int DeletedBy);
        public List<ConsultantMasterModel> GetByClinicIdWiseConsultant(int ClinicId, int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);

        public List<ConsultantMasterModel> GetBYDepartmentIdWiseConsultant(int Department_Id);
        public ClinicMasterModel UpdateConsultant_Code(ConsultantMasterModel model);
        public List<SelectListItem> GetConsultantList();
    }
}

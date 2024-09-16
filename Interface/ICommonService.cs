using HMS.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface ICommonService
    {
        public List<PageSizeDdl> GetPageSizeDDL();

        public List<SelectListItem> GetStatusList();
        public List<SelectListItem> GetDesignationList(int ClinicId);
        public List<SelectListItem> GetDepartmentList(int ClinicId);
        public List<SelectListItem> GetMaritalStatusList();
        public List<SelectListItem> GetGenderList();
        public List<SelectListItem> GetBloodgroupList();
        public string GenerateReciptNo();

        public string GetMd5HashNewMethod(string password);
        public string GetPrefix(int IdString);

        public List<SelectListItem> GetConsultantList(int Department_Id);
        public List<SelectListItem> GetUserDepartmentList(int Department_Id);
        public List<SelectListItem> GetPaymentModeList();

        public int GetConsultantId(int ClinicId);
        public int GetReceptionistId(int ClinicId);
        public List<SelectListItem> GetDesignationListById(int Desig_Id);
        public List<SelectListItem> GetDepartmentwiseServiceheadList(int Department_Id);
        public List<SelectListItem> GetServiceHeadwiseServiceList(int ServiceHead_Id);
        public List<SelectListItem> GetConsultantDept(int ClinicId);

    }
}

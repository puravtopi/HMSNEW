using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IDepartmentMasterServices
    {
        public List<DepartmentMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public DepartmentMasterModel GetById(int id);
        public DepartmentMasterModel Insert(DepartmentMasterModel model);
        public DepartmentMasterModel Update(DepartmentMasterModel model);
        public DepartmentMasterModel DeleteById(int Id, int DeletedBy);
        public List<DepartmentMasterModel> GetByClinicIdWiseDept(ref int TotalCount, int ClinicId, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public List<DepartmentMasterModel> GetByClinicIdWiseDeptList(int ClinicId);
        public List<DepartmentMasterModel> GetConsultantWiseDept(int ClinicId);
        //public List<DepartmentMasterModel> GetDepatmentByUser(int DeptId,int ClinicId);

    }
}

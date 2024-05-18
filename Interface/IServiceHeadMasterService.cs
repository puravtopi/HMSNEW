using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IServiceHeadMasterService
    {
        public List<ServiceHeadMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public ServiceHeadMasterModel GetById(int id);
        public ServiceHeadMasterModel Insert(ServiceHeadMasterModel model);
        public ServiceHeadMasterModel Update(ServiceHeadMasterModel model);
        public ServiceHeadMasterModel DeleteById(int Id, int DeletedBy);
        public List<ServiceHeadMasterModel> GetByDepartmentIdWiseServiceList(int Department_Id);
    }
}

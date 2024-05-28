using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IServiceMasterService
    {
        public List<ServiceMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public ServiceMasterModel GetById(int id);
        public ServiceMasterModel Insert(ServiceMasterModel model);
        public ServiceMasterModel Update(ServiceMasterModel model);
        public ServiceMasterModel DeleteById(int Id, int DeletedBy);
        public List<ServiceMasterModel> GetByServiceHeadIdWiseServiceList(int ServiceHead_Id);
        public ServiceMasterModel InsertPatientServiceMasterDetails(ServiceMasterModel model);
        public List<PatientServiceMasterModel> GetAllPatientServiceMasterDetails(int Patient_Id);
    }
}

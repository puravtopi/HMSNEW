using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IPatientServiceMasterServices
    {
        public List<PatientServiceMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public PatientServiceMasterModel GetById(int id);
        public PatientServiceMasterModel Insert(PatientServiceMasterModel model);
        public PatientServiceMasterModel Update(PatientServiceMasterModel model);
        public PatientServiceMasterModel DeleteById(int Id, int DeletedBy);
        public List<PatientServiceMasterModel> GetAll(int Patient_Id);
    }
}

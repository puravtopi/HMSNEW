using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IPatientMasterServices
    {
        public List<PatientMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public PatientMasterModel GetById(int id);
        public PatientMasterModel Insert(PatientMasterModel model);
        public PatientMasterModel Update(PatientMasterModel model);
        public PatientMasterModel DeleteById(int Id, int DeletedBy);
        public List<PatientMasterModel> GetByClinicIdWisePatient(int ClinicId, int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);

        public PatientConsultantMasterModel GetConsultantByPatientId(int Id);
        public List<PatientMasterModel> GetPatientData();
        public List<PatientMasterModel> GetUserWisePatientData(int UserId, int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
    }
}

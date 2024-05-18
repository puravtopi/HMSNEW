using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IPatientConsultantMasterServices
    {
        public List<PatientConsultantMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public PatientConsultantMasterModel GetById(int id);
        public PatientConsultantMasterModel Insert(PatientConsultantMasterModel model);
        public PatientConsultantMasterModel Update(PatientConsultantMasterModel model);
        public PatientConsultantMasterModel DeleteById(int Id, int DeletedBy);
        public PatientConsultantMasterModel GetByPatientIdWise(int Patient_Id);
    }
}

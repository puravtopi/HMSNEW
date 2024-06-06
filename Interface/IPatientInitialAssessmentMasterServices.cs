using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IPatientInitialAssessmentMasterServices
    {

        public List<PatientInitialAssessmentMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public PatientInitialAssessmentMasterModel GetById(int id);
        public PatientInitialAssessmentMasterModel GetByPatientId(int Patient_Id);
        public PatientInitialAssessmentMasterModel Insert(PatientInitialAssessmentMasterModel model);
        public PatientInitialAssessmentMasterModel Update(PatientInitialAssessmentMasterModel model);
        public PatientInitialAssessmentMasterModel DeleteById(int Id, int DeletedBy);
    }
}

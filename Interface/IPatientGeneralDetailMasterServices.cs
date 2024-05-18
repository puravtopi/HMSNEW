using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IPatientGeneralDetailMasterServices
    {
        public List<PatientGeneralDetailMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public PatientGeneralDetailMasterModel GetById(int id);
        public PatientGeneralDetailMasterModel Insert(PatientGeneralDetailMasterModel model);
        public PatientGeneralDetailMasterModel Update(PatientGeneralDetailMasterModel model);
        public PatientGeneralDetailMasterModel DeleteById(int Id, int DeletedBy);
        public PatientGeneralDetailMasterModel GetByPatientIdWise(int Patient_Id);
        

    }
}

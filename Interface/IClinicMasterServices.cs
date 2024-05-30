using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IClinicMasterServices
    {
        public List<ClinicMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public ClinicMasterModel GetById(int id);
        public ClinicMasterModel Insert(ClinicMasterModel model);
        public ClinicMasterModel Update(ClinicMasterModel model);
        public ClinicMasterModel DeleteById(int Id, int DeletedBy);
        public ClinicMasterModel UpdateCode(ClinicMasterModel model);
        public int GetConsultantIdById(int Id);
        public int GetReceptionistIdById(int Id);
    }
}

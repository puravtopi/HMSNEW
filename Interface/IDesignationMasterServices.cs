using HMS.Models;
using System.Collections.Generic;
namespace HMS.Interface
{
    public interface IDesignationMasterServices
    {
        public List<DesignationMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public DesignationMasterModel GetById(int id);
        public DesignationMasterModel Insert(DesignationMasterModel model);
        public DesignationMasterModel Update(DesignationMasterModel model);
        public DesignationMasterModel DeleteById(int Id, int DeletedBy);
        public List<DesignationMasterModel> GetByClinicIdWiseDesig(ref int TotalCount, int ClinicId, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public List<DesignationMasterModel> GetByClinicIdWiseDesigList(int ClinicId);
    }
}

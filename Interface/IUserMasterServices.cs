using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IUserMasterServices
    {
        public List<UserMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public UserMasterModel GetById(int id);
        public UserMasterModel Insert(UserMasterModel model);
        public UserMasterModel Update(UserMasterModel model);
        public UserMasterModel DeleteById(int Id, int DeletedBy);
        public UserMasterModel GetByEmail(string email);
        public UserMasterModel UpdatePassword(UserMasterModel model);
        public List<UserMasterModel> GetByClinicIdWiseUser(ref int TotalCount, int ClinicId, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public List<UserMasterModel> GetByClinicIdWiseUserList(int ClinicId);
        public List<UserMasterModel> GetByDepartmentIdWiseUserList(int Department_Id);
        public List<UserMasterModel> GetByDesignationIdWiseUserList(int Desig_Id);
    }
}

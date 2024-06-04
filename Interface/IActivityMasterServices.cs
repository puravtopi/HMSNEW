using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IActivityMasterServices
    {

        public List<ActivityMasterModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public ActivityMasterModel GetById(int id);
        public ActivityMasterModel Insert(ActivityMasterModel model);
        public ActivityMasterModel Update(ActivityMasterModel model);
        public ActivityMasterModel DeleteById(int Id, int DeletedBy);
    }
}

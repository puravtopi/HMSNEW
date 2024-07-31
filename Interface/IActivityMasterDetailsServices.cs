using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IActivityMasterDetailsServices
    {

        public List<ActivityMasterDetailsModel> GetAll(ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string qcnd = null);
        public ActivityMasterDetailsModel GetById(int id);
        public ActivityMasterDetailsModel Insert(ActivityMasterDetailsModel model);
        public ActivityMasterDetailsModel Update(ActivityMasterDetailsModel model);
        public ActivityMasterDetailsModel DeleteById(int Id, int DeletedBy);
        public List<ActivityMasterDetailsModel> GetByActivityTypeIdWiseActivityList(int ActivityTypeId);
    }
}

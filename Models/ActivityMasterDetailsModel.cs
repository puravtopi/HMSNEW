using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;

namespace HMS.Models
{
    public class ActivityMasterDetailsModel:BaseResponseModel
    {
        public int Id { get; set; }
        public int ActivityTypeId {  get; set; }
        public int ActivityBy {  get; set; }
        public int ActivityFor { get; set; }
        public DateTime? ActivityDate { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public List<SelectListItem> lstStatus { get; set; }

        public List<ActivityMasterDetailsModel> activityMasterDetailsList { get; set; }
        public string ActivityName { get; set; }
        public string FirstName { get; set; }
        public string Fname {  get; set; }
    }
}

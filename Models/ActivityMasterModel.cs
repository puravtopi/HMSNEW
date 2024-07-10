using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class ActivityMasterModel:BaseResponseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string ActivityName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public List<SelectListItem> lstStatus { get; set; }

        public List<ActivityMasterModel>  activityMastersList { get; set; }
    }
}

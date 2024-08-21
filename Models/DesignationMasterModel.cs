using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class DesignationMasterModel : BaseResponseModel
    {
        public int Id { get; set; }
        [Display(Name = "DesignationCode")]
        [Required(ErrorMessage = "This field is required.")]
        public string DesignationCode { get; set; }
        [Display(Name = "DesignationName")]
        [Required(ErrorMessage = "This field is required.")]
        public string DesignationName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsChecked { get; set; }
        public int? Clinic_Id { get; set; }
        public int DesigWiseUserCount { get; set; }
        public List<SelectListItem> lstStatus { get; set; }

        public List<DesignationMasterModel> designationMasterList { get; set; }
    }
}

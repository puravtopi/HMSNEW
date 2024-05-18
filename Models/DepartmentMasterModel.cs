using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace HMS.Models
{
    public class DepartmentMasterModel :BaseResponseModel
    {
        public int Id { get; set; }
        [Display(Name = "DepartmentName")]
        [Required(ErrorMessage = "This field is required.")]
        public string DepartmentName { get; set; }
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
        
        public List<SelectListItem> lstStatus { get; set; }

        public List<DepartmentMasterModel> departmentMasterList { get; set; }
    }
}

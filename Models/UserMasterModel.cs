using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class UserMasterModel:BaseResponseModel
    {
        public int Id { get; set; }
        [Display(Name = "FirstName")]
        [Required(ErrorMessage = "This field is required.")]
        public string FirstName { get; set; }
        [Display(Name = "LastName")]
        [Required(ErrorMessage = "This field is required.")]
        public string LastName { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$")]

        public string Email { get; set; }
        [Display(Name = "Password")]
        //[Required(ErrorMessage = "This field is required.")]
        public string Password { get; set; }
        public int? Clinic_Id { get; set; }
        public int? Desig_Id { get; set; }
        public int? Dept_id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public List<SelectListItem> lstStatus { get; set; }

        public List<UserMasterModel> userMasterList { get; set; }
            
        public string NewPassowrd {  get; set; }

        public List<SelectListItem> departmentList { get; set; }
        public List<SelectListItem> designationList { get; set; }
        public string Designations { get; set; } 
        public string Departments { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public decimal OPD_Charge { get; set; }
        public string Night_Charge { get; set; }
        public string Emergency_Charge { get; set; }
        public float SpecifyRevisit { get; set; }
    }
}

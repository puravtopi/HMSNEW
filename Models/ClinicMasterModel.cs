using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class ClinicMasterModel : BaseResponseModel
    {
        public int Id { get; set; }
        [Display(Name = "Clinic Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string ClinicName { get; set; }

        [Display(Name = "Owner Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string OwnerName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }       
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$")]
        public string EmailId { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }        

        public List<SelectListItem> lstStatus { get; set; }

        public List<ClinicMasterModel> clinicMasterList { get; set; }        

        [Display(Name = "City Name")]
        public string CityName { get; set; }
        [Display(Name = "State Name")]
        public string StateName { get; set; }
        [Display(Name = "Pincode")]
        public string Pincode { get; set; }
        public string Logo { get; set; }
        public string Password { get; set; }
        public List<DepartmentMasterModel> departmentList { get; set; }
        public List<DesignationMasterModel> designationList { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public List<SelectListItem> CityList { get; set; }
    }
}

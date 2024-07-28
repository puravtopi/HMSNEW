using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class ServiceHeadMasterModel:BaseResponseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string ServiceHeadName { get; set; }
        public int Department_Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public int? DeletedBy { get; set; }
        public int? Clinic_Id { get; set; }
        public DateTime? DeletedDate { get; set; }
        public List<SelectListItem> lstStatus { get; set; }
        public List<ServiceHeadMasterModel> serviceHeadsList { get; set; }
        public List<SelectListItem> departmentList { get; set; }
        public string Departments { get; set; }
        public string DepartmentName { get; set; }

    }
}

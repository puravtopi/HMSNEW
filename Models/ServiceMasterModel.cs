using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class ServiceMasterModel :BaseResponseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string ServiceName { get; set; }
        public int ServiceHead_Id { get; set; }
        public int Department_Id { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Charges { get; set; }        
        public string Lab_Legend { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public List<SelectListItem> lstStatus { get; set; }
        public List<ServiceMasterModel> serviceMasterList { get; set; }
        public List<SelectListItem> departmentList { get; set; }
        public string Departments { get; set; }
        public string DepartmentName { get; set; }


        public List<SelectListItem> ServiceHeadList { get; set; }
        public string ServiceHead { get; set; }


    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class PatientServiceMasterModel : BaseResponseModel
    {
        public int Id { get; set; }
        public int Patient_Id { get; set; }
        public int Department_Id { get; set; }
        public int Consultant_Id { get; set; }
        public int ServiceHead_Id { get; set; }
        public int Service_Id { get; set; }
        public int Revisit_Id { get; set; }
        public string Charges { get; set; }
        public string Discount { get; set; }
        public string NetAmount { get; set; }
        public string ReceiptNo { get; set; }
        public string ServiceDate { get; set; }
        public string RefundDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public DateTime DeletedDate { get; set; }
        public int DeletedBy { get; set; }
        public List<SelectListItem> lstStatus { get; set; }
        public List<PatientServiceMasterModel> patientServiceMastersList { get; set; }

        public string PatientName { get; set; }
        public string UHID { get; set; }
        public string ConsultantName { get; set; }
        public string RevisitDetail { get; set; }
        public string Department { get; set; }
        public string DepartmentName { get; set; }
        public List<SelectListItem> departmentList { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string ServiceHead { get; set; }
        public string ServiceHeadName { get; set; }
        public List<SelectListItem> serviceHeadList { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Service { get; set; }
        public string ServiceName { get; set; }
        public List<SelectListItem> serviceList { get; set; }
        public string Revisit { get; set; }
        public List<SelectListItem> revisitList { get; set; }
    }
}

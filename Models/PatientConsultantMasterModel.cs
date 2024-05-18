using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace HMS.Models
{
    public class PatientConsultantMasterModel:BaseResponseModel
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string Consultant_Charges { get; set; }
        public string Discount { get; set; }
        public string RefundAmount { get; set; }
        public string TotalAmount { get; set; }
        public string RefBy_Name { get; set; }
        public string RefBy_Address { get; set; }
        public bool EmergencyAdminssion { get; set; }
        public bool FileChargesApplicable { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public List<SelectListItem> lstStatus { get; set; }

        public List<PatientConsultantMasterModel> patientConsultantMastersList { get; set; }
        public string PaymentMode { get; set; }
        public bool Patient_Charges { get; set; }


    }
}

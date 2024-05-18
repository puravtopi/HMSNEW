using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class PatientGeneralDetailMasterModel :BaseResponseModel
    {
        public int Id { get; set; }
        public int Patient_Id { get; set; }
        public Decimal Temperature { get; set; }
        [Display(Name = "Adhar Card")]
        [Required(ErrorMessage = "This field is required.")]
        public string AdharCard { get; set; }
        public string Weight { get; set; }
        public string Bloodpressure { get; set; }
        public string Remark { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public List<SelectListItem> lstStatus { get; set; }

        public List<PatientGeneralDetailMasterModel> patientGeneralDetailMastersList { get; set; }
    }
}

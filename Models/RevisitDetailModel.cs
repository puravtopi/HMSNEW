using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class RevisitDetailModel : BaseResponseModel
    {
        public int Id { get; set; }
        public int Patient_Id { get; set; }
        [Display(Name = "UHID")]
        public int UHID { get; set; }

        [Display(Name = "Consultant Name")]
        public int ConsultantId { get; set; }
        [Display(Name = "Discount")]
        public float Discount { get; set; }
        [Display(Name = "Gen Remarks")]
        public string GenRemarks { get; set; }
        [Display(Name = "Clinical Diagnosis")]
        public string ClinicalDiagnosis { get; set; }

        [Display(Name = "Patient Name")]
        //[Required(ErrorMessage = "This field is required.")]
        public string PatientName { get; set; }
        [Display(Name = "Consultant Name")]
        public string ConsultantName { get; set; }
        [Display(Name = "Net Amount")]
        public float NetAmount { get; set; }
        [Display(Name = "General Advice")]
        public string GeneralAdvice { get; set; }
        [Display(Name = "Revisit Date")]
        public DateTime RevisitDate { get; set; }
        [Display(Name = "Revisit Charges")]
        public float RevisitCharges { get; set; }
        [Display(Name = "Receipt No")]
        public string ReceiptNo { get; set; }
        [Display(Name = "Net Amount")]
        public string RefReceiptNo { get; set; }
        [Display(Name = "Ref. Remarks")]
        public string RefRemarks { get; set; }
        [Display(Name = "Add To Template")]
        public bool AddToTemplate { get; set; }
        [Display(Name = "UNPAID")]
        public bool UNPAID { get; set; }
        [Display(Name = "Complimentary")]
        public bool Complimentary { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public int SpecifyRevisitDay { get; set; }
        public decimal OPDCharges { get; set; }
        public bool Patient_Emergency { get; set; }
        public List<SelectListItem> ConsultantList { get; set; }
        public List<SelectListItem> UHIDList { get; set; }
    }
}

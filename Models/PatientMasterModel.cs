using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace HMS.Models
{
    public class PatientMasterModel : BaseResponseModel
    {
        public int Id { get; set; }
        [Display(Name = "Fname")]
        [Required(ErrorMessage = "This field is required.")]
        public string Fname { get; set; }
        [Display(Name = "Lname")]
        [Required(ErrorMessage = "This field is required.")]
        public string Lname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "This field is required.")]
        public string Email { get; set; }
        [Display(Name = "ContactNo")]
        [Required(ErrorMessage = "This field is required.")]
        public string ContactNo { get; set; }
        [Display(Name = "Lname")]
        [Required(ErrorMessage = "This field is required.")]
        public DateTime DOB { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public int Clinic_Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public List<SelectListItem?>? lstStatus { get; set; }

        public List<PatientMasterModel> patientMastersList { get; set; }
        [Display(Name = "Father/Husband Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string Father_HusbandName { get; set; }
        [Display(Name = "Mother Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string MotherName { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string Occupation { get; set; }
        public string Houseno { get; set; }
        public string District { get; set; }
        public string Area { get; set; }
        public List<SelectListItem?>? MaritalStatusList { get; set; }
        public List<SelectListItem?>? GenderList { get; set; }
        public List<SelectListItem?>? BloodGroupList { get; set; }

        //PatientGeneralDetailMasterModel Fields
        public Decimal Temperature { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string AdharCard { get; set; }
        public string Weight { get; set; }
        public string Bloodpressure { get; set; }
        public string Remark { get; set; }

        //PatientConsultantMasterModel Fields      
        public string Departments { get; set; }

        public int? Department_id { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public int? User_id { get; set; }
        public string DepartmentName { get; set; }
        public List<SelectListItem> departmentList { get; set; }
        public string UserName { get; set; }
        public List<SelectListItem> User_DesignationList { get; set; }
        public int? Consultant_Id { get; set; }
        //public List<SelectListItem>? departmentList { get; set; }
        public List<SelectListItem>? consultantList { get; set; }
        public string Consultant_Charges { get; set; }
        public string Discount { get; set; }
        public string RefundAmount { get; set; }
        public string TotalAmount { get; set; }
        public string RefBy_Name { get; set; }
        public string RefBy_Address { get; set; }
        public bool EmergencyAdminssion { get; set; }
        public bool FileChargesApplicable { get; set; }
        //public string? Patient_Charges { get; set; }
        public string ReceiptNo { get; set; }
        public bool Patient_Charges { get; set; }

        public List<RevisitDetailModel?>? revisitDetailModel { get; set; }
        public bool? IsCheckRevisit { get; set; }
        public string PaymentMode { get; set; }
        public List<SelectListItem> PaymentModeList { get; set; }
        public string EntryDateTime { get; set; }
        public string UHID { get; set; }
        public List<PatientServiceMasterModel> patientServiceMastersModel { get; set; }
        public bool? IsCheckPatient { get; set; }
    }
}

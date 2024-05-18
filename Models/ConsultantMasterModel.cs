using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace HMS.Models
{
   
    public class ConsultantMasterModel: BaseResponseModel
    {
        public int Id { get; set; }
      
        public int Department_Id { get; set; }       
        public int Clinic_Id { get; set; }
        [Display(Name = "Consultant Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string ConsultantName { get; set; }       
        //New added field
        public string Consultant_Code { get; set; }
        public string Speciality_in { get; set; }
        public string ConsultantName_in { get; set; }
        public string Sub_Deptt { get; set; }
        //Address Details
        public string HouseNo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        //Charges Details
        public string Consultant_OPD_Charge{ get; set; }
        public string Consultant_Night_Charge { get; set; }
        public string Consultant_Emergency_Charge { get; set; }
        public string Consultant_Revisit_ChargesForOPD { get; set; }
        public string Consultant_Revisit_ChargesForIP { get; set; }    
        public string Hospital_OPD_Charge { get; set; }
        public string Hospital_Night_Charge { get; set; }
        public string Hospital_Emergency_Charge { get; set; }
        public string Hospital_Revisit_ChargesForOPD { get; set; }
        public string Hospital_Revisit_ChargesForIP { get; set; }
        public string SpecifyRevisit { get; set; }
        public bool DifferentReceiptForHospitalCharges { get; set; }
       
        //Room-Wise Visiting Charges
        public string Consultant_Deluxe_Charges { get; set; }
        public string Consultant_Others_Charges { get; set; }
        public string Consultant_Private_Charge { get; set; }
        public string Consultant_Semi_Private_Charge { get; set; }
        public string Hospital_Deluxe_Charges { get; set; }
        public string Hospital_Others_Charges { get; set; }
        public string Hospital_Private_Charge { get; set; }
        public string Hospital_Semi_Private_Charge { get; set; }
        //
        public string FormelInfo{ get; set; }
        public string Specifications    { get; set; }
        //========================================================================
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; } 
        public List<SelectListItem> lstStatus { get; set; }
        public List<ConsultantMasterModel> consultantMastersList { get; set; }
        public List<SelectListItem> departmentList { get; set; }                
        public string Departments { get; set; }        
        public string DepartmentName { get; set; }
    }
}

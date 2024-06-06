using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace HMS.Models
{
    public class PatientInitialAssessmentMasterModel:BaseResponseModel
    {
        public int Id { get; set;}
        public int Patient_Id { get; set;}
        public string ChiefComplaints { get; set;}
        public string PresentIllness  { get; set;}
        public string PastHistory { get; set;}
        public string FutureHistory   { get; set;}
        public string PersonalHistory { get; set;}
        public string TravelHistory   { get; set;}
        public string Remake  { get; set;}
        public DateTime? CreatedDate { get; set;}
        public int CreatedBy { get; set;}
        public DateTime? UpdatedDate { get; set;}
        public int UpdatedBy   { get; set;}
        public bool Active { get; set;}
        public bool IsDelete    { get; set;}
        public DateTime? DeletedDate { get; set;}
        public  int DeletedBy   { get; set;}
        public List<SelectListItem> lstStatus { get; set; }

        public List<PatientInitialAssessmentMasterModel> patientInitialAssessmentMasterModelsList { get; set; }

    }
}

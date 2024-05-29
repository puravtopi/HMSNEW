using HMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HMS.Common
{
    public static class SessionHelper
    {
        public static string SessionEmailId = "UserEmail";
        public static string SessionClinicID = "Clinicid";
        public static string SessionPatient_Id = "Patient_Id";
        public static string SessionClinicDepartment = "ClinicDept";
        public static string SessionClinicDesignation = "ClinicDesig";
        public static string SessionUserId = "UserId";
        public static string SessionUserName = "UserName";
        
    }
}
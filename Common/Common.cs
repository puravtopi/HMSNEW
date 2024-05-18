using HMS.Models;

namespace HMS.Common
{
    public class Common
    {
        //Common Message 
        public const string MsgSave = "Record has been saved successfully.";
        public const string MsgUpdate = "Record has been updated successfully.";
        public const string MsgDelete = "Record has been deleted successfully.";
        public const string MsgNotExist = "Record does not exist in our system.";

        public const string Temp_Success = "Success";
        public const string Temp_Error = "Error";
    }

    public static class CommonSp
    {
        // front
        // Clinic Master
        public const string getAllClinicMaster = "usp_ClinicMaster_GetAll";
        public const string saveClinicMaster = "usp_ClinicMaster_Insert";
        public const string updateClinicMaster = "usp_ClinicMaster_Update";
        public const string deleteByIdClinicMaster = "usp_ClinicMaster_DeleteById";
        public const string getByIdClinicMaster = "usp_ClinicMaster_GetById";
        public const string updateClinicMasterCode = "usp_ClinicMaster_UpdateCode";
        //Department Master
        public const string getAllDepartmentMaster = "usp_DepartmentMaster_GetAll";
        public const string saveDepartmentMaster = "usp_DepartmentMaster_Insert";
        public const string updateDepartmentMaster = "usp_DepartmentMaster_Update";
        public const string deleteByIdDepartmentMaster = "usp_DepartmentMaster_DeleteById";
        public const string getByIdDepartmentMaster = "usp_DepartmentMaster_GetById";
        //Designation Master
        public const string getAllDesignationMaster = "usp_DesignationMaster_GetAll";
        public const string saveDesignationMaster = "usp_DesignationMaster_Insert";
        public const string updateDesignationMaster = "usp_DesignationMaster_Update";
        public const string deleteByIdDesignationMaster = "usp_DesignationMaster_DeleteById";
        public const string getByIdDesignationMaster = "usp_DesignationMaster_GetById";
        //User Master
        public const string getAllUserMaster = "usp_UserMaster_GetAll";
        public const string saveUserMaster = "usp_UserMaster_Insert";
        public const string updateUserMaster = "usp_UserMaster_Update";
        public const string deleteByIdUserMaster = "usp_UserMaster_DeleteById";
        public const string getByIdUserMaster = "usp_UserMaster_GetById";
        public const string getByEmailUserMaster = "usp_UserMaster_GetByEmail";
        //// Clinic Department Master
        //public const string getAllClinicDepartmentMaster = "usp_ClinicDepartmentMaster_GetAll";
        //public const string saveClinicDepartmentMaster = "usp_ClinicDepartmentMaster_Insert";
        //public const string updateClinicDepartmentMaster = "usp_ClinicDepartmentMaster_Update";
        //public const string deleteByIdClinicDepartmentMaster = "usp_ClinicDepartmentMaster_DeleteById";
        //public const string getByIdClinicDepartmentMaster = "usp_ClinicDepartmentMaster_GetById";
        //// Clinic Designation Master
        //public const string getAllClinicDesignationMaster = "usp_ClinicDesignationMaster_GetAll";
        //public const string saveClinicDesignationMaster = "usp_ClinicDesignationMaster_Insert";
        //public const string updateClinicDesignationMaster = "usp_ClinicDesignationMaster_Update";
        //public const string deleteByIdClinicDesignationMaster = "usp_ClinicDesignationMaster_DeleteById";
        //public const string getByIdClinicDesignationMaster = "usp_ClinicDesignationMaster_GetById";
        //Change Password
        public const string updateChangePassword = "usp_UserMaster_Password_Update";
        //GROUPING 
        //public const string getClinicdeptToDept = "usp_ClinicDepartmentMaster_DepartmentId_Wise";
        //public const string getClinicDesigToDesig = "usp_ClinicDesignationMaster_DesignationId_Wise";
        //public const string getdepatAll = "usp_ClinicDepartmentMaster_GetAllDepartment";
        //public const string getdesigtAll = "usp_ClinicDesignationMaster_GetAllDesignation";

        //Listing Data with pageing  shorting
        public const string getByClinicIdWiseDEPT = "usp_DepartmentMaster_GetBy_ClinicIdWise";
        public const string getByClinicIdWiseDESIG = "usp_DesignationMaster_GetBy_ClinicIdWise";
        public const string getByClinicIdWiseUSER = "usp_UserMaster_GetByClinicId_Wise";
        public const string getByClinicIdWisePATIENT = "usp_PatientMaster_GetBy_ClinicIdWise";
        public const string getByClinicIdWiseCONSULTANT = "usp_ConsultantMaster_GetBy_ClinicIdWise";
        //Patient Master 
        public const string getAllPatientMaster = "usp_PatientMaster_GetAll";
        public const string savePatientMaster = "usp_PatientMaster_Insert";
        public const string updatePatientMaster = "usp_PatientMaster_Update";
        public const string deleteByIdPatientMaster = "usp_PatientMaster_DeleteById";
        public const string getByIdPatientMaster = "usp_PatientMaster_GetById";
        public const string getByIdPatientIdWise_Consultant= "usp_Consultant_GetBy_PatientId";
        // Consultant Master
        public const string getAllConsultantMaster = "usp_ConsultantMaster_GetAll";
        public const string saveConsultantMaster = "usp_ConsultantMaster_Insert";
        public const string updateConsultantMaster = "usp_ConsultantMaster_Update";
        public const string deleteByIdConsultantMaster = "usp_ConsultantMaster_DeleteById";
        public const string getByIdConsultantMaster = "usp_ConsultantMaster_GetById";
        public const string getAllConsultantMasterList = "usp_ConsultantMaster_GetAllConsultantMaster";
        public const string updateConsultantMasterCode = "usp_ConsultantMaster_UpdateCode"; 
        //Clinic wise Department And Designation Without paging Sorting
        public const string getByClinicIdWiseDEPTList = "usp_DepartmentMaster_GetBy_ClinicIdWise_List";
        public const string getByClinicIdWiseDESIGList = "usp_DesignationMaster_GetBy_ClinicIdWise_List";
        public const string getByClinicIdWiseUserList = "usp_UserMaster_GetByClinicId_WIse_List";
        //Patient General Details Master 
        public const string getAllPatientGeneralDetailMaster = "usp_PatientGeneralDetailMaster_GetAll";
        public const string savePatientGeneralDetailMaster = "usp_PatientGeneralDetailMaster_Insert";
        public const string updatePatientGeneralDetailMaster = "usp_PatientGeneralDetailMaster_Update";
        public const string deleteByIdPatientGeneralDetailMaster = "usp_PatientGeneralDetailMaster_DeleteById";
        public const string getByIdPatientGeneralDetailMaster = "usp_PatientGeneralDetailMaster_GetById";
        public const string getByIdPatientIdWise_GeneralDetailMaster = "usp_PatientGeneralDetailMaster_GetBy_PatientId";
        //Patient Consultant Details Master 
        public const string getAllPatientConsultantMaster = "usp_PatientConsultantMaster_GetAll";
        public const string savePatientConsultantMaster = "usp_PatientConsultantMaster_Insert";
        public const string updatePatientConsultantMaster = "usp_PatientConsultantMaster_Update";
        public const string deleteByIdPatientConsultantMaster = "usp_PatientConsultantMaster_DeleteById";
        public const string getByIdPatientConsultantMaster = "usp_PatientConsultantMaster_GetById";
        public const string getByDepartmentIdWise_Consultant = "usp_ConsultantMaster_GetBy_DepartmentIdWise";
        public const string getByPatientIdWise_Consultant = "usp_PatientConsultantMaster_GetBy_PatientId";
        //Patient Revisit Detail
        public const string saveRevisitDetail = "usp_RevisitDetail_Insert";
        public const string GetTopOneRevisitDetail = "usp_RevisitDetail_GetTopOneRevisitDetail";
        public const string getAllRevisitDetail = "usp_RevisitDetail_GetAllRevisitPatientIdWise";
        public const string getAllById = "usp_RevisitDetail_GetAllById";
        public const string updateRevisitDetail = "usp_RevisitDetail_Update";
        public const string deleteByIdRevisitDetail = "usp_RevisitDetail_DeleteById";


        public const string getByDepartmentIdWise_User = "usp_UserMaster_GetBy_DepartmentIdWise";
        public const string getByDesignationIdWise_User = "usp_UserMaster_GetBy_DesignationIdWise";
        
        // Service Head Master
        public const string getAllServiceHeadMaster = "usp_ServiceHeadMaster_GetAll";
        public const string saveServiceHeadMaster = "usp_ServiceHeadMaster_Insert";
        public const string updateServiceHeadMaster = "usp_ServiceHeadMaster_Update";
        public const string deleteByIdServiceHeadMaster = "usp_ServiceHeadMaster_DeleteById";
        public const string getByIdServiceHeadMaster = "usp_ServiceHeadMaster_GetById";
        // Service Master
        public const string getAllServiceMaster = "usp_ServiceMaster_GetAll";
        public const string saveServiceMaster = "usp_ServiceMaster_Insert";
        public const string updateServiceMaster = "usp_ServiceMaster_Update";
        public const string deleteByIdServiceMaster = "usp_ServiceMaster_DeleteById";
        public const string getByIdServiceMaster = "usp_ServiceMaster_GetById";

        public const string getByDepartmentWise_ServiceHead = "usp_ServiceHead_GetBy_DepartmentIdWise";
        public const string getByDepartmentWise_Service = "usp_Service_GetBy_DepartmentIdWise";
        
        public const string GetpatientData = "usp_PatientMaster_MaxValue";
        public const string GetUserWisePatient = "usp_PatientMaster_UserId_Wise";

        // Patient Service Master
        public const string getAllPatientServiceMaster = "usp_PatientServiceMaster_GetAll";
        public const string savePatientServiceMaster = "usp_PatientServiceMaster_Insert";
        public const string updatePatientServiceMaster = "usp_PatientServiceMaster_Update";
        public const string deleteByIdPatientServiceMaster = "usp_PatientServiceMaster_DeleteById";
        public const string getByIdPatientServiceMaster = "usp_PatientServiceMaster_GetById";
        public const string GetPatientWisePatientServiceData = "usp_PatientServiceMaster_GetPatientId_wise";

        public const string GetAllService_ServiceHeadWise = "usp_ServiceMaster_GetBy_ServiceHeadIdWise";
        
        //public const string GetPatientWisePatientServiceData = "usp_PatientServiceMaster_GetPatientId_wise";
        //public const string GetPatientWisePatientServiceData = "usp_PatientServiceMaster_GetPatientId_wise";


    }
    public class PageSizeDdl : BaseResponseModel
    {
        public int PageSizeId { get; set; }
        public string PageSizeValue { get; set; }
    }
    public class Temp_Message
    {
        public const string Success = "Success";
        public const string Error = "Error";
    }

    public class ConnStrings
    {
        public const string HMSConnectionstring = "HMSConnection";
       
    }

    public class Breadcrumb
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }
}

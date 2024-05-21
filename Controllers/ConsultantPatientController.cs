using HMS.Common;
using HMS.Interface;
using HMS.Models;
using HMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace HMS.Controllers
{
    public class ConsultantPatientController : Controller
    {
        private readonly ICommonService _commonService;
        private readonly IPatientMasterServices _patientMasterServices;
        private readonly IPatientGeneralDetailMasterServices _patientGeneralDetailMasterServices;
        PatientMasterModel patientMasterModel = new PatientMasterModel();
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        private readonly IUserMasterServices _userMasterServices;
        private readonly IDepartmentMasterServices _departmentMasterServices;


        public ConsultantPatientController(
          IPatientMasterServices patientMasterServices,
          IPatientGeneralDetailMasterServices patientGeneralDetailMasterServices,
          ICommonService commonService,IUserMasterServices userMasterServices
            ,IDepartmentMasterServices departmentMasterServices
          )
        {
            _patientGeneralDetailMasterServices = patientGeneralDetailMasterServices;
            _patientMasterServices = patientMasterServices;
            _commonService = commonService;
            _userMasterServices = userMasterServices;
            _departmentMasterServices= departmentMasterServices;

        }

        public IActionResult Index(int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortOrder = "Desc", string sortField = "CI.Id")
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Patient", Url = null },

                };

            // Set the Breadcrumbs collection in the ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;
            ViewBag.PageSizeId = PageSizeId;
            if (string.IsNullOrEmpty(sortField))
            {
                ViewBag.SortField = "Id";
                ViewBag.SortOrder = "Asc";
            }
            else
            {

                ViewBag.SortField = sortField;
                ViewBag.SortOrder = sortOrder;
            }

            ViewBag.searchString = searchString;
            int count;
            int TotalCount = 0;
            if (searchString != null)
            {
                searchString = searchString.Trim();
            }
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);



            //string cnd = " and id in (select Patient_Id  from  PatientConsultantMaster where User_Id=" + SessionUser.ToString() + ")";

            var res = _patientMasterServices.GetByClinicIdWisePatient(ref TotalCount,SessionUser,SclinicId, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder, "");
            patientMasterModel.lstPageSizeDdl = _commonService.GetPageSizeDDL();
            patientMasterModel.MaritalStatusList = _commonService.GetMaritalStatusList();
            patientMasterModel.GenderList = _commonService.GetGenderList();
            patientMasterModel.BloodGroupList = _commonService.GetBloodgroupList();
            patientMasterModel.departmentList = _commonService.GetDepartmentList(SclinicId);
            patientMasterModel.PaymentModeList = _commonService.GetPaymentModeList();
            for (int i = 0; i < res.Count; i++)
            {
                var data = _patientGeneralDetailMasterServices.GetByPatientIdWise(res[i].Id);
                if (data != null)
                {
                    res[i].Temperature = data.Temperature;
                    res[i].AdharCard = data.AdharCard;
                }

            }

            if (res.Count > 0)
            {
                if (res[0].DbCode == -1)
                {
                    ViewBag.ErroMsg = res[0].DbMsg;
                    count = 0;
                }
                else
                {
                    count = res.Count;
                    patientMasterModel.patientMastersList = res;

                }
            }
            patientMasterModel.Pager = new JW.Pager(TotalCount, currentPage, PageSizeId);
            if (TempData[Temp_Message.Success] != null)
                ModelState.AddModelError(Temp_Message.Success, TempData[Temp_Message.Success].ToString());
            patientMasterModel.SearchString = searchString;
            patientMasterModel.PageSizeId = PageSizeId;
            patientMasterModel.sortField = sortField;
            patientMasterModel.sortOrder = ViewBag.SortOrder;
            sortOrder = ViewBag.SortOrder;

            return View(patientMasterModel);
        }

        public IActionResult AddEdit(string patientId)
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Clinic", Url = null },
                    new Breadcrumb { Text = "Patient", Url = Url.Action("Index","PatientMaster") },
                     new Breadcrumb { Text = "Add/Edit", Url = null },

                };

            // Set the Breadcrumbs collection in the ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;
            //patientMasterModel.userlist = _commonService.GetUserDepartmentList(0);
            int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
            int dId = 0;
            if (patientId != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(patientId));
            if (dId != 0)
            {
                patientMasterModel = _patientMasterServices.GetById(dId);

                var dataGeneral = _patientGeneralDetailMasterServices.GetByPatientIdWise(patientMasterModel.Id);
                if (dataGeneral != null)
                {
                    patientMasterModel.Temperature = dataGeneral.Temperature;
                    patientMasterModel.AdharCard = dataGeneral.AdharCard;
                    patientMasterModel.Weight = dataGeneral.Weight;
                    patientMasterModel.Bloodpressure = dataGeneral.Bloodpressure;
                    patientMasterModel.Remark = dataGeneral.Remark;
                }

                var dataConsultant = _patientMasterServices.GetConsultantByPatientId(patientMasterModel.Id);
                if (dataConsultant != null)
                {
                    patientMasterModel.User_id = dataConsultant.User_Id;
                    patientMasterModel.Consultant_Charges = dataConsultant.Consultant_Charges;
                    patientMasterModel.Discount = dataConsultant.Discount;
                    //patientMasterModel.EmergencyAdminssion = dataConsultant.EmergencyAdminssion;
                    //patientMasterModel.FileChargesApplicable = dataConsultant.FileChargesApplicable;
                    patientMasterModel.RefBy_Address = dataConsultant.RefBy_Address;
                    patientMasterModel.RefBy_Name = dataConsultant.RefBy_Name;
                    patientMasterModel.RefundAmount = dataConsultant.RefundAmount;
                    patientMasterModel.TotalAmount = dataConsultant.TotalAmount;
                    patientMasterModel.PaymentMode = dataConsultant.PaymentMode;
                    patientMasterModel.Patient_Charges = dataConsultant.Patient_Charges;




                    if (patientMasterModel.User_id != null && patientMasterModel.User_id != 0)
                    {
                        var dataConsultantMst = _userMasterServices.GetById(patientMasterModel.User_id.Value);

                        patientMasterModel.Department_id = dataConsultantMst.Dept_id;
                        //ViewBag.Consultant_Code = dataConsultantMst.Consultant_Code;
                        patientMasterModel.User_DesignationList = _commonService.GetUserDepartmentList((int)dataConsultantMst.Dept_id);

                        var depatmentdata = _departmentMasterServices.GetById((int)dataConsultantMst.Dept_id);
                        patientMasterModel.DepartmentName = depatmentdata.DepartmentName;
                        var fristname = dataConsultantMst.FirstName != null ? dataConsultantMst.FirstName.ToString() : "";
                        var lastname = dataConsultantMst.LastName != null ? dataConsultantMst.LastName.ToString() : "";
                        patientMasterModel.UserName = fristname + " " + lastname;

                    }
                }

            }
            else
            {
                patientMasterModel.Active = true;
                patientMasterModel.User_DesignationList = _commonService.GetUserDepartmentList(0);
            }


            patientMasterModel.lstStatus = _commonService.GetStatusList();
            patientMasterModel.MaritalStatusList = _commonService.GetMaritalStatusList();
            patientMasterModel.GenderList = _commonService.GetGenderList();
            patientMasterModel.BloodGroupList = _commonService.GetBloodgroupList();
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            patientMasterModel.departmentList = _commonService.GetDepartmentList(SclinicId);
            patientMasterModel.PaymentModeList = _commonService.GetPaymentModeList();

            return View(patientMasterModel);
        }


    }
}

using HMS.Common;
using HMS.Interface;
using HMS.Models;
using HMS.Services;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace HMS.Controllers
{
    public class ReceptionistPatientController : Controller
    {
        private readonly ICommonService _commonService;
        private readonly IPatientMasterServices _patientMasterServices;
        private readonly IPatientGeneralDetailMasterServices _patientGeneralDetailMasterServices;
        PatientMasterModel patientMasterModel = new PatientMasterModel();
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        private readonly IPatientConsultantMasterServices _patientConsultantMasterServices;
        private readonly IUserMasterServices _userMasterServices;
        private readonly IDepartmentMasterServices _departmentMasterServices;
        private readonly IConsultantMasterServices _consultantMasterServices;
        private readonly IRevisitDetailMasterServices _revisitDetailMasterServices;
        private readonly IPatientServiceMasterServices _patientServiceMasterServices;
        private readonly IServiceHeadMasterService _serviceHeadMasterService;
        private readonly IServiceMasterService _serviceMasterService;


        public ReceptionistPatientController(
          IPatientMasterServices patientMasterServices,
          IPatientGeneralDetailMasterServices patientGeneralDetailMasterServices,
          ICommonService commonService,IUserMasterServices userMasterServices,
          IDepartmentMasterServices departmentMasterServices,
          IConsultantMasterServices consultantMasterServices, 
          IPatientConsultantMasterServices patientConsultantMasterServices,
          IRevisitDetailMasterServices revisitDetailMasterServices,
          IPatientServiceMasterServices patientServiceMasterServices,
          IServiceHeadMasterService serviceHeadMasterService,
          IServiceMasterService serviceMasterService
          )
        {
            _patientGeneralDetailMasterServices = patientGeneralDetailMasterServices;
            _patientMasterServices = patientMasterServices;
            _commonService = commonService;
            _userMasterServices = userMasterServices;
            _departmentMasterServices = departmentMasterServices;
            _consultantMasterServices = consultantMasterServices;
            _patientConsultantMasterServices = patientConsultantMasterServices;
            _revisitDetailMasterServices= revisitDetailMasterServices;
            _patientServiceMasterServices = patientServiceMasterServices;
            _serviceHeadMasterService= serviceHeadMasterService;
            _serviceMasterService= serviceMasterService;
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
                ViewBag.SortField = "CI.Id";
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
            var res = _patientMasterServices.GetByClinicIdWisePatient(ref TotalCount, SessionUser, SclinicId, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder);
            patientMasterModel.lstPageSizeDdl = _commonService.GetPageSizeDDL();
            patientMasterModel.MaritalStatusList = _commonService.GetMaritalStatusList();
            patientMasterModel.GenderList = _commonService.GetGenderList();
            patientMasterModel.BloodGroupList = _commonService.GetBloodgroupList();
            patientMasterModel.departmentList = _commonService.GetDepartmentList(SclinicId);
            patientMasterModel.PaymentModeList = _commonService.GetPaymentModeList();
            for (int i = 0; i < res.Count; i++)
            {
                var data = _patientGeneralDetailMasterServices.GetByPatientIdWise(res[i].Id);

                //patientMasterList
                if (data != null)
                {
                    res[i].Temperature = data.Temperature;
                    res[i].AdharCard = data.AdharCard;
                }
                var getAllRevisitDetail = _revisitDetailMasterServices.GetAll(res[i].Id);
                if (getAllRevisitDetail != null && getAllRevisitDetail.Count > 0)
                {
                    //res[i].revisitDetailModel = new List<RevisitDetailModel>();
                    res[i].revisitDetailModel = getAllRevisitDetail;
                    res[i].IsCheckRevisit = true;
                }
                else
                {
                    getAllRevisitDetail = new List<RevisitDetailModel>();
                    res[i].revisitDetailModel = getAllRevisitDetail;
                    res[i].IsCheckRevisit = false;
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
                //_patientMasterServices.PatientStatusUpdate(patientMasterModel.Id);

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
           //patientMasterModel.patientServiceMasters.departmentList = _commonService.GetDepartmentList(SclinicId);

            return View(patientMasterModel);
        }

        [HttpPost]
        public IActionResult AddEdit(PatientMasterModel model)
        {
            patientMasterModel.lstStatus = _commonService.GetStatusList();
            patientMasterModel.MaritalStatusList = _commonService.GetMaritalStatusList();
            model.MaritalStatusList = _commonService.GetMaritalStatusList();
            patientMasterModel.GenderList = _commonService.GetGenderList();
            patientMasterModel.BloodGroupList = _commonService.GetBloodgroupList();
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            patientMasterModel.departmentList = _commonService.GetDepartmentList(SclinicId);
            patientMasterModel.PaymentModeList = _commonService.GetPaymentModeList();


            model.departmentList = _commonService.GetDepartmentList(SclinicId);
            if (model.Consultant_Id != null)
            {
                var dataConsultantMst = _consultantMasterServices.GetById(model.Consultant_Id.Value);

                patientMasterModel.Department_id = dataConsultantMst.Department_Id;
                ViewBag.Consultant_Code = dataConsultantMst.Consultant_Code;
                //model.consultantList = _commonService.GetConsultantList(dataConsultantMst.Department_Id);
                var Desig = _commonService.GetDesignationListById(11);
                //ViewBag.Consultant_Code = dataConsultantMst.Consultant_Code;
                model.consultantList = Desig;
                //model.c = Desig[0].Text;
            }

            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {

                    List<PatientMasterModel> patient = new List<PatientMasterModel>();
                    patient = _patientMasterServices.GetPatientData();
                    int uhid = 0;

                    if (patient.Count > 0)
                        uhid = patient.Max(a => a.Id);
                    if (model.PaymentMode == "Due")
                    {
                        model.UHID = null;
                    }
                    else
                    {
                        model.UHID = (uhid + 1).ToString();
                    }


                    model.DOB = model.DOB.Date;
                    model.CreatedBy = SclinicId;
                    if (model.Active == true)
                    {
                        model.IsDelete = false;
                    }
                    else
                    {
                        model.IsDelete = true;
                    }
                    model.Clinic_Id = SclinicId;
                    //model.Active = true;
                    if (!string.IsNullOrEmpty(model.EntryDateTime))
                    {

                        var d = model.EntryDateTime.Replace('T', ' ');
                        model.EntryDateTime = d;
                    }

                    model.ReceiptNo = _commonService.GenerateReciptNo();

                    var res = _patientMasterServices.Insert(model);

                    if (res.DbCode == 1)
                    {

                        TempData[Temp_Message.Success] = res.DbMsg;

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData[Temp_Message.Error] = res.DbMsg;
                        return RedirectToAction("AddEdit");
                    }

                }
                else
                {
                    model.UpdatedBy = SclinicId;
                    model.Clinic_Id = SclinicId;
                    if (model.Active == true)
                    {
                        model.IsDelete = false;
                    }
                    else
                    {
                        model.IsDelete = true;
                    }
                    if (!string.IsNullOrEmpty(model.EntryDateTime))
                    {

                        var d = model.EntryDateTime.Replace('T', ' ');
                        model.EntryDateTime = d;
                    }
                    var res = _patientMasterServices.Update(model);

                    //Patient GeneralDetail Master data updated
                    var patientGeneralDetailMaster = _patientGeneralDetailMasterServices.GetByPatientIdWise(model.Id);

                    patientGeneralDetailMaster.Temperature = model.Temperature;
                    patientGeneralDetailMaster.AdharCard = model.AdharCard;
                    patientGeneralDetailMaster.Patient_Id = model.Id;
                    patientGeneralDetailMaster.Weight = model.Weight;
                    patientGeneralDetailMaster.Bloodpressure = model.Bloodpressure;
                    patientGeneralDetailMaster.Remark = model.Remark;
                    patientGeneralDetailMaster.UpdatedBy = SclinicId;
                    if (patientGeneralDetailMaster.Active == true)
                    {
                        patientGeneralDetailMaster.IsDelete = false;
                    }
                    else
                    {
                        patientGeneralDetailMaster.IsDelete = true;
                    }
                    var patientGeneralDetail = _patientGeneralDetailMasterServices.Update(patientGeneralDetailMaster);
                    //Patient Consultant Master data updated
                    var patientConsultant = _patientConsultantMasterServices.GetByPatientIdWise(model.Id);

                    patientConsultant.User_Id = (int)model.User_id;
                    patientConsultant.Consultant_Charges = model.Consultant_Charges;
                    patientConsultant.Discount = model.Discount;
                    patientConsultant.RefundAmount = model.RefundAmount;
                    patientConsultant.TotalAmount = model.TotalAmount;
                    patientConsultant.RefBy_Name = model.RefBy_Name;
                    patientConsultant.RefBy_Address = model.RefBy_Address;
                    patientConsultant.PaymentMode = model.PaymentMode;
                    patientConsultant.UpdatedBy = SclinicId;
                    patientConsultant.Patient_Charges = model.Patient_Charges;
                    if (patientConsultant.Active == true)
                    {
                        patientConsultant.IsDelete = false;
                    }
                    else
                    {
                        patientConsultant.IsDelete = true;
                    }
                    var patientConsultantDetails = _patientConsultantMasterServices.Update(patientConsultant);

                    if (res.DbCode == 1)
                    {
                        TempData[Temp_Message.Success] = res.DbMsg;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData[Temp_Message.Error] = res.DbMsg;
                        return RedirectToAction("AddEdit");
                    }
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();

            }

            return View(model);
        }

        public IActionResult ReceptionistPatientAddEdit(string patientId)
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
                //_patientMasterServices.PatientStatusUpdate(patientMasterModel.Id);

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
            //patientMasterModel.patientServiceMasters.departmentList = _commonService.GetDepartmentList(SclinicId);

            return View(patientMasterModel);

        }
        [HttpPost]
        public IActionResult ReceptionistPatientAddEdit(PatientMasterModel model)
        {
            patientMasterModel.lstStatus = _commonService.GetStatusList();
            patientMasterModel.MaritalStatusList = _commonService.GetMaritalStatusList();
            model.MaritalStatusList = _commonService.GetMaritalStatusList();
            patientMasterModel.GenderList = _commonService.GetGenderList();
            patientMasterModel.BloodGroupList = _commonService.GetBloodgroupList();
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            patientMasterModel.departmentList = _commonService.GetDepartmentList(SclinicId);
            patientMasterModel.PaymentModeList = _commonService.GetPaymentModeList();


            model.departmentList = _commonService.GetDepartmentList(SclinicId);
            if (model.Consultant_Id != null)
            {
                var dataConsultantMst = _consultantMasterServices.GetById(model.Consultant_Id.Value);

                patientMasterModel.Department_id = dataConsultantMst.Department_Id;
                ViewBag.Consultant_Code = dataConsultantMst.Consultant_Code;
                //model.consultantList = _commonService.GetConsultantList(dataConsultantMst.Department_Id);
                var Desig = _commonService.GetDesignationListById(11);
                //ViewBag.Consultant_Code = dataConsultantMst.Consultant_Code;
                model.consultantList = Desig;
                //model.c = Desig[0].Text;
            }

            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {

                    List<PatientMasterModel> patient = new List<PatientMasterModel>();
                    patient = _patientMasterServices.GetPatientData();
                    int uhid = 0;

                    if (patient.Count > 0)
                        uhid = patient.Max(a => a.Id);
                    if (model.PaymentMode == "Due")
                    {
                        model.UHID = null;
                    }
                    else
                    {
                        model.UHID = (uhid + 1).ToString();
                    }


                    model.DOB = model.DOB.Date;
                    model.CreatedBy = SclinicId;
                    if (model.Active == true)
                    {
                        model.IsDelete = false;
                    }
                    else
                    {
                        model.IsDelete = true;
                    }
                    model.Clinic_Id = SclinicId;
                    //model.Active = true;
                    if (!string.IsNullOrEmpty(model.EntryDateTime))
                    {

                        var d = model.EntryDateTime.Replace('T', ' ');
                        model.EntryDateTime = d;
                    }

                    model.ReceiptNo = _commonService.GenerateReciptNo();

                    var res = _patientMasterServices.Insert(model);

                    if (res.DbCode == 1)
                    {

                        TempData[Temp_Message.Success] = res.DbMsg;

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData[Temp_Message.Error] = res.DbMsg;
                        return RedirectToAction("ReceptionistPatientAddEdit");
                    }

                }
                else
                {
                    model.UpdatedBy = SclinicId;
                    model.Clinic_Id = SclinicId;
                    if (model.Active == true)
                    {
                        model.IsDelete = false;
                    }
                    else
                    {
                        model.IsDelete = true;
                    }
                    if (!string.IsNullOrEmpty(model.EntryDateTime))
                    {

                        var d = model.EntryDateTime.Replace('T', ' ');
                        model.EntryDateTime = d;
                    }
                    var res = _patientMasterServices.Update(model);

                    //Patient GeneralDetail Master data updated
                    var patientGeneralDetailMaster = _patientGeneralDetailMasterServices.GetByPatientIdWise(model.Id);

                    patientGeneralDetailMaster.Temperature = model.Temperature;
                    patientGeneralDetailMaster.AdharCard = model.AdharCard;
                    patientGeneralDetailMaster.Patient_Id = model.Id;
                    patientGeneralDetailMaster.Weight = model.Weight;
                    patientGeneralDetailMaster.Bloodpressure = model.Bloodpressure;
                    patientGeneralDetailMaster.Remark = model.Remark;
                    patientGeneralDetailMaster.UpdatedBy = SclinicId;
                    if (patientGeneralDetailMaster.Active == true)
                    {
                        patientGeneralDetailMaster.IsDelete = false;
                    }
                    else
                    {
                        patientGeneralDetailMaster.IsDelete = true;
                    }
                    var patientGeneralDetail = _patientGeneralDetailMasterServices.Update(patientGeneralDetailMaster);
                    //Patient Consultant Master data updated
                    var patientConsultant = _patientConsultantMasterServices.GetByPatientIdWise(model.Id);

                    patientConsultant.User_Id = (int)model.User_id;
                    patientConsultant.Consultant_Charges = model.Consultant_Charges;
                    patientConsultant.Discount = model.Discount;
                    patientConsultant.RefundAmount = model.RefundAmount;
                    patientConsultant.TotalAmount = model.TotalAmount;
                    patientConsultant.RefBy_Name = model.RefBy_Name;
                    patientConsultant.RefBy_Address = model.RefBy_Address;
                    patientConsultant.PaymentMode = model.PaymentMode;
                    patientConsultant.UpdatedBy = SclinicId;
                    patientConsultant.Patient_Charges = model.Patient_Charges;
                    if (patientConsultant.Active == true)
                    {
                        patientConsultant.IsDelete = false;
                    }
                    else
                    {
                        patientConsultant.IsDelete = true;
                    }
                    var patientConsultantDetails = _patientConsultantMasterServices.Update(patientConsultant);

                    if (res.DbCode == 1)
                    {
                        TempData[Temp_Message.Success] = res.DbMsg;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData[Temp_Message.Error] = res.DbMsg;
                        return RedirectToAction("ReceptionistPatientAddEdit");
                    }
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();

            }

            return View(model);
        }
        public IActionResult Delete(string deleteId)
        {
            int dId = 0;
            if (deleteId != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(deleteId));
            if (dId != 0)
            {
                int Deleted_By = 1;
                var res = _patientMasterServices.DeleteById(dId, Deleted_By);
                if (res.DbCode == 1)
                {
                    TempData[Temp_Message.Success] = res.DbMsg;
                }
                else
                {
                    TempData[Temp_Message.Error] = res.DbMsg;
                }
            }
            return RedirectToAction("Index");
        }

       
    }
}

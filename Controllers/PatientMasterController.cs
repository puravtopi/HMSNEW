﻿using HMS.Common;
using HMS.Interface;
using HMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using HMS.Services;
using System.Security.Cryptography;
using System.Runtime.Intrinsics.X86;
using System.Drawing;
using System.IO;

using System.Xml.Linq;

using System.Data;
using System.Net.Mime;
using Microsoft.AspNetCore.Hosting.Server;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using System.Text.RegularExpressions;
using iTextSharp.text.html.simpleparser;
using Org.BouncyCastle.Crypto.Tls;
using Microsoft.AspNetCore.Hosting;
using Org.BouncyCastle.Ocsp;

namespace HMS.Controllers
{
    public class PatientMasterController : Controller
    {
        private readonly IPatientMasterServices _patientMasterServices;
        private readonly IPatientGeneralDetailMasterServices _patientGeneralDetailMasterServices;
        private readonly IPatientConsultantMasterServices _patientConsultantMasterServices;
        private readonly IClinicMasterServices _clinicMasterServices;
        private readonly IUserMasterServices _userMasterServices;
        private readonly ICommonService _commonService;
        private readonly IConsultantMasterServices _consultantMasterServices;
        private readonly IRevisitDetailMasterServices _revisitDetailMasterServices;
        private readonly IPatientServiceMasterServices _patientServiceMasterServices;
        private readonly IDepartmentMasterServices _departmentMasterServices;
        private readonly IServiceHeadMasterService _serviceHeadMasterService;
        private readonly IServiceMasterService _serviceMasterService;
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        PatientMasterModel patientMasterModel = new PatientMasterModel();
        PatientGeneralDetailMasterModel PatientGeneralDetailMasterModel = new PatientGeneralDetailMasterModel();
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public PatientMasterController(ICommonService commonService,
            IPatientMasterServices patientMasterServices,
            IPatientGeneralDetailMasterServices patientGeneralDetailMasterServices,
            IConsultantMasterServices consultantMasterServices,
            IRevisitDetailMasterServices revisitDetailMasterServices,
            IPatientConsultantMasterServices patientConsultantMasterServices,
            IClinicMasterServices clinicMasterServices, IUserMasterServices userMasterServices,
            IWebHostEnvironment webHostEnvironment, IPatientServiceMasterServices patientServiceMasterServices,
            IDepartmentMasterServices departmentMasterServices,IServiceHeadMasterService serviceHeadMasterService
            ,IServiceMasterService serviceMasterService
            )
        {
            _patientMasterServices = patientMasterServices;
            _commonService = commonService;
            _patientGeneralDetailMasterServices = patientGeneralDetailMasterServices;
            _consultantMasterServices = consultantMasterServices;
            _revisitDetailMasterServices = revisitDetailMasterServices;
            _patientConsultantMasterServices = patientConsultantMasterServices;
            _clinicMasterServices = clinicMasterServices;
            _userMasterServices = userMasterServices;
            _webHostEnvironment = webHostEnvironment;
            _patientServiceMasterServices = patientServiceMasterServices;
            _departmentMasterServices= departmentMasterServices;
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
            var res = _patientMasterServices.GetByClinicIdWisePatient(ref TotalCount,SessionUser, SclinicId, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder);
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
                        
                    }

                }

            }
            else
            {
                patientMasterModel.Active = true;
                patientMasterModel.User_DesignationList = _commonService.GetUserDepartmentList(0);
            }
            patientMasterModel.StateList = _commonService.GetState();

            patientMasterModel.lstStatus = _commonService.GetStatusList();
            patientMasterModel.MaritalStatusList = _commonService.GetMaritalStatusList();
            patientMasterModel.GenderList = _commonService.GetGenderList();
            patientMasterModel.BloodGroupList = _commonService.GetBloodgroupList();
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            //patientMasterModel.departmentList = _commonService.GetDepartmentList(SclinicId);
            patientMasterModel.departmentList = _commonService.GetConsultantDept(SclinicId);
            patientMasterModel.PaymentModeList = _commonService.GetPaymentModeList();

            return View(patientMasterModel);
        }

        [HttpPost]
        public IActionResult AddEdit(PatientMasterModel model)
        {

            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);

            patientMasterModel.StateList = _commonService.GetState();
           
          //  patientMasterModel.CityList = _commonService.GetCitiesByStateId(stateId);
            patientMasterModel.lstStatus = _commonService.GetStatusList();
            patientMasterModel.MaritalStatusList = _commonService.GetMaritalStatusList();
            model.MaritalStatusList = _commonService.GetMaritalStatusList();
            patientMasterModel.GenderList = _commonService.GetGenderList();
            patientMasterModel.BloodGroupList = _commonService.GetBloodgroupList();
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
                    model.CreatedBy = SessionUser;
                    model.Active = true;
                    if (model.Active == true)
                    {
                        model.IsDelete = false;
                    }
                    else
                    {
                        model.IsDelete = true;
                    }
                    model.Clinic_Id = SclinicId;
                    
                    if (!string.IsNullOrEmpty(model.EntryDateTime))
                    {

                        var d = model.EntryDateTime.Replace('T', ' ');
                        model.EntryDateTime = d;
                    }

                    model.ReceiptNo = _commonService.GenerateReciptNo();
                    model.ActivityBy = SessionUser;
                    model.ActivityDate = DateTime.Now;
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
                    model.UpdatedBy = SessionUser;
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

        public IActionResult Delete(string deleteId)
        {
            int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
            int dId = 0;
            if (deleteId != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(deleteId));
            if (dId != 0)
            {
                int Deleted_By = SessionUser;
               // var ActivityTypeId = deleteId;
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

        public ActionResult RevisitDetail(int patientId, int Id)
        {
            try
            {
                RevisitDetailModel revisitDetailModel = new RevisitDetailModel();
                //Designation Wise List

                //var Desig = _consultantMasterServices.GetConsultantList();
                //if (Desig != null)
                //{
                //    revisitDetailModel.ConsultantList = Desig;
                //}

                patientMasterModel = _patientMasterServices.GetById(patientId);


                var patientConsultant = _patientMasterServices.GetConsultantByPatientId(patientId);
                var user = _userMasterServices.GetById(patientConsultant.User_Id);


                if (patientMasterModel != null)
                {
                    revisitDetailModel.PatientName = patientMasterModel.Fname + " " + patientMasterModel.Lname;
                }
                var consultantList = _commonService.GetDesignationListById((int)user.Desig_Id);
                //ViewBag.Consultant_Code = dataConsultantMst.Consultant_Code;
                revisitDetailModel.ConsultantList = consultantList;
                revisitDetailModel.ConsultantName = user.FirstName + " " + user.LastName;
                revisitDetailModel.ConsultantId = user.Id;
                revisitDetailModel.Patient_Id = patientId;
                revisitDetailModel.OPDCharges = user.OPD_Charge;
                revisitDetailModel.SpecifyRevisitDay = (int)user.SpecifyRevisit;  // Assuming user.SpecifyRevisit is float
                revisitDetailModel.SpecifyRevisitDay = (int)user.SpecifyRevisit;
                //revisitDetailModel.RevisitCharges = user.SpecifyRevisit;
                //revisitDetailModel.SpecifyRevisitDay = user.SpecifyRevisit;
                if (float.TryParse(user.Revisit_Charge, out float revisitCharge))
                {
                    revisitDetailModel.RevisitCharges = revisitCharge;
                }
                else
                {
                    // Handle parsing error, you can set a default value or throw an exception
                    revisitDetailModel.RevisitCharges = 0;  // Default value if parsing fails
                                                            // Optionally, you can log the error or notify the user
                }
                if (Id > 0)
                {
                    var getAllRevisitData = _revisitDetailMasterServices.GetAllById(Id);
                    revisitDetailModel = getAllRevisitData;
                    revisitDetailModel.ConsultantList = consultantList;
                }
                else
                {
                    int _min = 10000;
                    int _max = 99999;
                    Random r = new Random();
                    int num = r.Next(_min, _max);
                    revisitDetailModel.ReceiptNo = "RCP" + num.ToString();
                    num = r.Next(_min, _max);
                    revisitDetailModel.UHID = patientId;

                    revisitDetailModel.RevisitDate = DateTime.Now;
                    revisitDetailModel.RefReceiptNo = "REF" + num.ToString();
                }
                return PartialView("_RevisitDetail", revisitDetailModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult AddEditRevisitDetail(RevisitDetailModel model)
        {
            try
            {
                int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
                int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
                if (ModelState.IsValid)
                {
                    int SpecifyRevisitDay = model.SpecifyRevisitDay;
                    bool IsCheckNewConsultant = true;
                    patientMasterModel = _patientMasterServices.GetById(model.Patient_Id);

                    if (patientMasterModel != null)
                    {
                        DateTime dtPtientvisit = patientMasterModel.CreatedDate.Value;
                        DateTime dtRevisit = model.RevisitDate;
                        DateTime dtrevisitdate = Convert.ToDateTime(dtRevisit);
                        int noOfDay = model.SpecifyRevisitDay;
                        //double dSpecifyRevisitDay = (dtrevisitdate.Date - dtPtientvisit.Date).TotalDays;
                        //if (dSpecifyRevisitDay > noOfDay)
                        //{
                        //    SpecifyRevisitDay = false;
                        //}
                    }
                    var getTopOneRevisitDetail = _revisitDetailMasterServices.GetTopOneRevisitDetail();
                    if (getTopOneRevisitDetail != null)
                    {
                        int currentConsultantId = model.ConsultantId;
                        int oldcurrentConsultantId = getTopOneRevisitDetail.ConsultantId;
                        if (currentConsultantId != oldcurrentConsultantId)
                        {
                            if (model.OPDCharges <= 0)
                            {
                                IsCheckNewConsultant = false;
                            }
                        }
                    }
                    if (SpecifyRevisitDay !=0)
                    {
                        if (IsCheckNewConsultant)
                        {
                            if (model.Id == 0)
                            {
                                model.CreatedBy = SessionUser;
                                model.Active = true;
                                if (model.Active == true)
                                {
                                    model.IsDelete = false;
                                }
                                else
                                {
                                    model.IsDelete = true;
                                }
                                if (model.RevisitDate == null)
                                {
                                    model.RevisitDate = DateTime.Now;
                                }
                                model.CreatedBy = SessionUser;
                                var res = _revisitDetailMasterServices.Insert(model);
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
                                model.UpdatedDate = DateTime.Now;
                                model.UpdatedBy = SessionUser;
                                var res = _revisitDetailMasterServices.Update(model);
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
                            TempData[Temp_Message.Error] = "If you change new consultant please add OPDCharges";
                            return RedirectToAction("Index");
                        }

                    }
                    else
                    {
                        TempData[Temp_Message.Error] = "Your visit day is out of range please new appointment create ";
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                              .Where(y => y.Count > 0)
                              .ToList();

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult DeteleRevisitDetail(string deleteId)
        {
            int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
            int dId = 0;
            if (deleteId != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(deleteId));
            if (dId != 0)
            {
                int Deleted_By = SessionUser;
                var res = _revisitDetailMasterServices.DeteleRevisitDetail(dId, Deleted_By);
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

        //Download PDF Code
        public IActionResult CreatePDFDocument(int patientId)
        {
            //Create document  
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            //Create PDF Table  
            PdfPTable tableLayout = new PdfPTable(4);

            PatientMasterModel model = new PatientMasterModel();
            model = _patientMasterServices.GetById(patientId);
            int SessionUserId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
            PatientGeneralDetailMasterModel patientGeneral = _patientGeneralDetailMasterServices.GetByPatientIdWise(patientId);
            PatientConsultantMasterModel patientConsultant = _patientConsultantMasterServices.GetByPatientIdWise(patientId);
            //ConsultantMasterModel consultantMaster = _consultantMasterServices.GetById(patientConsultant.Consultant_Id);
            UserMasterModel userMaster = _userMasterServices.GetById(SessionUserId);
            ClinicMasterModel clinicMaster = _clinicMasterServices.GetById(model.Clinic_Id);
            //Create a PDF file in specific path  
            string webRootPath = _webHostEnvironment.WebRootPath;
            string DownloadPDFPath = Path.Combine(webRootPath, "DownloadPDF");
            if (!Directory.Exists(DownloadPDFPath))
            {
                Directory.CreateDirectory(DownloadPDFPath);
                ViewBag.Message = "Directory created successfully.";
            }

            string fileName = model.Id + "_PatientDetails.pdf";
            string filePath = Path.Combine(webRootPath, "DownloadPDF", fileName);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            //Open the PDF document  
            doc.Open();

            doc.SetMargins(40, 40, 45, 45);
            Font boldUpperCaseFont = new Font(Font.FontFamily.HELVETICA, 18, Font.NORMAL, BaseColor.BLACK);
            var clinicname_lb = clinicMaster.ClinicName != null ? clinicMaster.ClinicName : "";
            Paragraph Clinicname = new Paragraph(clinicname_lb.ToUpper(), boldUpperCaseFont);
            Clinicname.Alignment = Element.ALIGN_CENTER;
            doc.Add(Clinicname);

            var add = clinicMaster.Address != null ? clinicMaster.Address : "";
            Paragraph address = new Paragraph(/*" :" + */add);
            address.Alignment = Element.ALIGN_CENTER;
            doc.Add(address);

            var mob = clinicMaster.Mobile != null ? clinicMaster.Mobile : "";
            Paragraph phone = new Paragraph("Phone No." + mob);
            phone.Alignment = Element.ALIGN_CENTER;
            doc.Add(phone);

            // Create a font with bold style and size 13
            //Paragraph Heading = new Paragraph("Collection Of Receipt", new Font(Font.NORMAL, 13, 2, new iTextSharp.text.BaseColor(Color.Black)));
            Font boldUnderlineFont = new Font(Font.FontFamily.HELVETICA, 13, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK);
            // Create a paragraph with the bold and underlined text
            Paragraph Heading = new Paragraph("Collection Of Receipt", boldUnderlineFont);
            Heading.Alignment = Element.ALIGN_CENTER;
            doc.Add(Heading);
            // Add some space after the heading and add text original copy
            //doc.Add(new Paragraph("\n\n"));            
            Paragraph originalCopy = new Paragraph();
            originalCopy.Add(Chunk.TABBING);  // First tab
            originalCopy.Add(Chunk.TABBING);  // Second tab
            originalCopy.Add(new Chunk("Original Copy", new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.BLACK)));     
            originalCopy.Alignment = Element.ALIGN_RIGHT;             
            doc.Add(originalCopy);
            doc.Add(new Paragraph("\n"));

            ////doc.Add(new Phrase("Collection Of Receipt", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(Color.Black))));            
            //Paragraph Partline = new Paragraph("=================================================================");
            //Partline.Alignment = Element.ALIGN_CENTER;
            //doc.Add(Partline);

            //var firstname = userMaster.FirstName != null ? userMaster.FirstName : "";
            //var lastname = userMaster.LastName != null ? userMaster.LastName : "";
            //Paragraph ConsultantName = new Paragraph("Consultant Name :" + firstname + " " + lastname);
            //ConsultantName.Alignment = Element.ALIGN_CENTER;
            //doc.Add(ConsultantName);


            doc.Add(new Paragraph("\n\n"));
            doc.Add(new Paragraph("Receipt No.  :" + model.ReceiptNo));
            doc.Add(new Paragraph("UHID  :" + model.UHID));

            var dt = userMaster.CreatedDate.ToString() != null ? userMaster.CreatedDate.ToString() : "";
            Paragraph date = new Paragraph("Date:" + dt);
            date.Alignment = Element.ALIGN_RIGHT;
            doc.Add(date);

            doc.Add(new Paragraph("\n\n"));
            var consultantfname = userMaster.FirstName != null ? userMaster.FirstName : "";
            var consultantlname = userMaster.LastName != null ? userMaster.LastName : "";
            Paragraph Consultant = new Paragraph(consultantfname + " " + consultantlname);
            var fname = model.Fname != null ? model.Fname : "";
            var lname = model.Lname != null ? model.Lname : "";
            var age = model.Age != null ? model.Age : "";
            var gender = model.Gender != null ? model.Gender : "";
            var totalamount = patientConsultant.TotalAmount != null ? patientConsultant.TotalAmount : "";

            //Chunk underlinedText = new Chunk("           "+fname + " " + lname + " (Age:" + age + "-" + gender + ")" + "             ");
            //underlinedText.Font.SetStyle(Font.UNDERLINE);
            //Paragraph underlinedParagraph = new Paragraph(underlinedText);
            //doc.Add(underlinedParagraph);
            
            // Convert the total amount to words
            //string totalAmountInWords = NumberToWordsConverter.ConvertAmountToWords(totalamount);
            //doc.Add(new Paragraph("Received With thanks from  " + fname + " " + lname + " " + "(Age:" + age + "-" + gender + ") The Sum Of " + " "));
            //doc.Add(new Paragraph("Rupees " + totalAmountInWords + " (" + totalamount + " Rs.) " + " By cash on account Of Services Charges. " + " (" + Consultant + ")"));
            ////doc.Add(new Paragraph("Rupees " + totalamount + " " + " By cash on account Of Services Charges." + " "+"("+ Consultant+ ")"));
            //doc.Add(new Paragraph("\n\n\n"));
            //doc.Add(new Paragraph("RS." + totalamount));
            string totalAmountStr = patientConsultant.TotalAmount; // Assuming this is a string
            decimal totalAmount;

            if (decimal.TryParse(totalAmountStr, out totalAmount))
            {
                // Proceed with using totalAmount as a decimal
                // Convert the total amount to words
                string totalAmountInWords = NumberToWordsConverter.ConvertAmountToWords(totalAmount);
                doc.Add(new Paragraph("Received With thanks from  " + fname + " " + lname + " " + "(Age:" + age + "-" + gender + ") The Sum Of " + "Rupees "));
                doc.Add(new Paragraph(totalAmountInWords + " (" + totalAmount + " Rs.) " + " By cash on account Of Services Charges. (" + Consultant + ")"));
                doc.Add(new Paragraph("\n\n\n"));
                //doc.Add(new Paragraph("RS." + totalAmount));
                Paragraph totalAmountParagraph = new Paragraph("RS." + totalAmount);

               // Font textboxFont = new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK);

                PdfPCell cell = new PdfPCell(totalAmountParagraph)
                {
                    BorderColor = BaseColor.BLACK, 
                    Padding = 2f, 
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(240, 240, 240) 
                };
                PdfPTable table = new PdfPTable(1)
                {
                    WidthPercentage = 20 // Set the table to take the full width
                };
                table.AddCell(cell);
                doc.Add(table);
            }
            else
            {                
                doc.Add(new Paragraph("Error: Invalid total amount."));
            }

            //var updatedate = userMaster.UpdatedDate != null ? userMaster.UpdatedDate.ToString() : "";
            //Paragraph data = new Paragraph("Login:" + firstname.ToUpper() + "-" + updatedate);
            //data.Alignment = Element.ALIGN_CENTER;
            //doc.Add(data);

            Paragraph authorisedSignatory = new Paragraph("Authorised Signatory");
            authorisedSignatory.Alignment = Element.ALIGN_RIGHT;
            doc.Add(authorisedSignatory);

            Font smallFont = new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.WHITE);
            string noteText = "Please note that records is valid in that hospital premises and it should be deposited in the accounts section at the time of billing.";
            PdfPCell noteCell = new PdfPCell(new Phrase(noteText, smallFont))
            {
                BackgroundColor = new BaseColor(0, 0, 0),
                Border = PdfPCell.NO_BORDER,
                Padding = 5
            };
            PdfPTable noteTable = new PdfPTable(1)
            {
                WidthPercentage = 100 
            };
            noteTable.AddCell(noteCell);           
            noteTable.HorizontalAlignment = Element.ALIGN_CENTER;
            doc.Add(noteTable);

            Font smallColorFont = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK);
            Paragraph note1 = new Paragraph(DateTime.Now + "            *This is computer generated receipt and no need to signature.", smallColorFont);
            note1.Alignment = Element.ALIGN_CENTER;
            doc.Add(note1);

            //File(stream, contentType, fileName);
            doc.Close();

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", fileName);
        }

        public ActionResult AddEditPatientServiceDetails(string patientId)
        {
            try
            {
                int Patient_Id = 0;
                if (patientId != null)
                    Patient_Id = Convert.ToInt32(encryptDecrypt.DecryptString(patientId));
                List<PatientServiceMasterModel> patientServiceMasterList = new List<PatientServiceMasterModel>();
                PatientServiceMasterModel patientServiceMaster = new PatientServiceMasterModel();
                patientMasterModel = _patientMasterServices.GetById(Patient_Id);

                var patientConsultant = _patientMasterServices.GetConsultantByPatientId(Patient_Id);
                var user = _userMasterServices.GetById(patientConsultant.User_Id);

                if (patientMasterModel != null)
                {
                    patientServiceMaster.UHID = patientMasterModel.UHID;
                    patientServiceMaster.PatientName = patientMasterModel.Fname + " " + patientMasterModel.Lname;
                    patientServiceMaster.Department = patientServiceMaster.Department_Id.ToString();
                    patientServiceMaster.ServiceHeadName = patientServiceMaster.ServiceHead_Id.ToString();
                    patientServiceMaster.ServiceName = patientServiceMaster.Service_Id.ToString();
                    patientServiceMaster.ReceiptNo = patientMasterModel.ReceiptNo;

                }
                patientServiceMaster.ConsultantName = user.FirstName + " " + user.LastName;
                patientServiceMaster.Patient_Id = Patient_Id;
                patientServiceMaster.Consultant_Id = user.Id;
               
                HttpContext.Session.SetInt32(SessionHelper.SessionPatient_Id, Patient_Id);

                int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
                patientServiceMaster.departmentList = _commonService.GetDepartmentList(SclinicId);
                patientServiceMaster.serviceHeadList = _commonService.GetDepartmentwiseServiceheadList(patientServiceMaster.Department_Id);
                patientServiceMaster.serviceList = _commonService.GetServiceHeadwiseServiceList(patientServiceMaster.ServiceHead_Id);
                patientServiceMaster.serviceLists = _serviceMasterService.GetByServiceHeadIdWiseServiceList(patientServiceMaster.ServiceHead_Id);
                patientServiceMaster.patientServiceMastersList = _serviceMasterService.GetAllPatientServiceMasterDetails(Patient_Id);
                //_serviceMasterService.GetByServiceHeadIdWiseServiceList(ServiceHead_Id);
                //return PartialView("_PatientServiceDetail", patientServiceMaster);
                patientServiceMasterList.Add(patientServiceMaster);
                return View(patientServiceMaster);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult AddEditPatientServiceDetails(PatientServiceMasterModel model)
        {
            try
            {
                int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
                int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
                if (ModelState.IsValid)
                {
                    //bool SpecifyRevisitDay = true;
                    //bool IsCheckNewConsultant = true;
                    patientMasterModel = _patientMasterServices.GetById(model.Patient_Id);

                    if (patientMasterModel != null)
                    {
                        DateTime dtPtientvisit = patientMasterModel.CreatedDate.Value;
                        model.ReceiptNo = patientMasterModel.ReceiptNo;
                    }
                    var getTopOneRevisitDetail = _revisitDetailMasterServices.GetTopOneRevisitDetail();

                    model.Department_Id = int.Parse(model.DepartmentName);
                    model.ServiceHead_Id = int.Parse(model.ServiceHeadName);
                    model.Consultant_Id = model.Consultant_Id;
                    model.Revisit_Id = getTopOneRevisitDetail.Id;
                    if (model.Id == 0)
                    {
                        model.CreatedBy = SessionUser;
                        model.Active = true;
                        if (model.Active == true)
                        {
                            model.IsDelete = false;
                        }
                        else
                        {
                            model.IsDelete = true;
                        }
                        //if (model.RevisitDate == null)
                        //{
                        //    model.RevisitDate = DateTime.Now.ToString();
                        //}
                       
                        model.ServiceDate = DateTime.Now;
                        model.RefundDate = DateTime.Now;
                        model.CreatedBy = SessionUser;
                        var res = _patientServiceMasterServices.Insert(model);
                        if (res.DbCode >= 1)
                        {
                            ServiceMasterModel modelServiceMasterModel = new ServiceMasterModel();
                            modelServiceMasterModel.CreatedBy = SessionUser;
                            modelServiceMasterModel.Active = true;
                            modelServiceMasterModel.PatientServiceMasterId = res.DbCode;
                            modelServiceMasterModel.ServiceId = int.Parse(model.ServiceName);
                            modelServiceMasterModel.Discount = model.Discount;
                            modelServiceMasterModel.Charges = model.Charges;
                            modelServiceMasterModel.NetAmount = decimal.Parse(model.NetAmount);
                            if (modelServiceMasterModel.Active == true)
                            {
                                modelServiceMasterModel.IsDelete = false;
                            }
                            else
                            {
                                modelServiceMasterModel.IsDelete = true;
                            }
                            modelServiceMasterModel.CreatedDate = DateTime.Now;
                            modelServiceMasterModel.CreatedBy = SessionUser;
                            var ress = _serviceMasterService.InsertPatientServiceMasterDetails(modelServiceMasterModel);
                            //TempData[Temp_Message.Success] = res.DbMsg;
                            //return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData[Temp_Message.Error] = res.DbMsg;
                            return RedirectToAction("AddEdit");
                        }
                    }
                    else
                    {
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = SessionUser;
                        var res = _patientServiceMasterServices.Update(model);
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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult PatientServiceMasterDetails(int serviceHead_Id)
        {
            try
            {
                List<ServiceMasterModel> serviceMasterModel = new List<ServiceMasterModel>();
                serviceMasterModel = _serviceMasterService.GetByServiceHeadIdWiseServiceList(serviceHead_Id);
                return PartialView("_PatientServiceMasterDetail", serviceMasterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult addDetailforservice(List<ServiceMasterModel> OnFarms)
        {
            try
            {
                int PatientServiceMasterId = 0;
                int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
                int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
                int Patient_Id = (int)HttpContext.Session.GetInt32(SessionHelper.SessionPatient_Id);

                //if (ModelState.IsValid)
                //{
                PatientServiceMasterModel models = new PatientServiceMasterModel();

                patientMasterModel = _patientMasterServices.GetById(OnFarms[0].Patient_Id);

                    if (patientMasterModel != null)
                    {
                        DateTime dtPtientvisit = patientMasterModel.CreatedDate.Value;
                        models.ReceiptNo = patientMasterModel.ReceiptNo;
                    }
                    var getTopOneRevisitDetail = _revisitDetailMasterServices.GetTopOneRevisitDetail();


                    models.Consultant_Id = getTopOneRevisitDetail.ConsultantId;
                    models.Revisit_Id = getTopOneRevisitDetail.Id;
                    models.Patient_Id = Patient_Id;
                    models.CreatedBy = SessionUser;
                    models.Active = true;
                    if (models.Active == true)
                    {
                        models.IsDelete = false;
                    }
                    else
                    {
                        models.IsDelete = true;
                    }
                models.Department_Id = OnFarms[0].Department_Id;
                models.ServiceHead_Id = OnFarms[0].ServiceHead_Id;
                models.CreatedDate = DateTime.Now;
                    models.ServiceDate = DateTime.Now;
                    models.RefundDate = DateTime.Now;
                    var res = _patientServiceMasterServices.Insert(models);
                    if (res.DbCode > 1)
                    {
                        PatientServiceMasterId = res.DbCode;
                    }
                //}
                //}
                //else
                //{
                //    var errors = ModelState.Select(x => x.Value.Errors)
                //              .Where(y => y.Count > 0)
                //              .ToList();

                //}

                foreach (ServiceMasterModel model in OnFarms)
                {
                    if (model.IsChecked)
                    {
                        model.PatientServiceMasterId = PatientServiceMasterId;
                        model.CreatedBy = 1;
                        model.Active = true;
                        if (model.Active == true)
                        {
                            model.IsDelete = false;
                        }
                        else
                        {
                            model.IsDelete = true;
                        }
                        model.CreatedBy = SessionUser;
                        model.CreatedDate = DateTime.Now;
                        model.ServiceId = model.Id;
                        var ress = _serviceMasterService.InsertPatientServiceMasterDetails(model);
                        //if (ress.DbCode == 1)
                        //{
                        //    TempData[Temp_Message.Success] = ress.DbMsg;
                        //    return RedirectToAction("Index");
                        //}
                        //else
                        //{
                        //    TempData[Temp_Message.Error] = res.DbMsg;
                        //    return RedirectToAction("AddEdit");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index");
        }

        public IActionResult DetelePatientServiceDetails(string deleteId)
        {
            int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
            int dId = 0;
            if (deleteId != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(deleteId));
            if (dId != 0)
            {
                int Deleted_By = SessionUser;

                var res = _patientServiceMasterServices.DeleteById(dId, Deleted_By);
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
        public IActionResult GetCities(int stateId)
        {
            var cities = _departmentMasterServices.GetCityWiseState(stateId);

            var cityList = cities.Select(c => new
            {
                value = c.Id,
                text = c.Name
            }).ToList();

            return Json(cityList);
        }

        public IActionResult PrintPatientDetails()
        {
            return View();
        }

        public static class NumberToWordsConverter
        {
            private static readonly string[] unitsMap = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                                                  "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen",
                                                  "Eighteen", "Nineteen" };
            private static readonly string[] tensMap = { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty",
                                                 "Ninety" };

            public static string NumberToWords(int number)
            {
                if (number == 0)
                    return "Zero";

                if (number < 0)
                    return "Minus " + NumberToWords(Math.Abs(number));

                string words = "";

                if ((number / 1000000) > 0)
                {
                    words += NumberToWords(number / 1000000) + " Million ";
                    number %= 1000000;
                }

                if ((number / 1000) > 0)
                {
                    words += NumberToWords(number / 1000) + " Thousand ";
                    number %= 1000;
                }

                if ((number / 100) > 0)
                {
                    words += NumberToWords(number / 100) + " Hundred ";
                    number %= 100;
                }

                if (number > 0)
                {
                    if (words != "")
                        words += "and ";

                    if (number < 20)
                        words += unitsMap[number];
                    else
                    {
                        words += tensMap[number / 10];
                        if ((number % 10) > 0)
                            words += "-" + unitsMap[number % 10];
                    }
                }

                return words;
            }

            public static string ConvertAmountToWords(decimal amount)
            {
                int intPart = (int)amount;
                int decimalPart = (int)((amount - intPart) * 100);

                string words = NumberToWords(intPart);

                if (decimalPart > 0)
                {
                    words += " and " + NumberToWords(decimalPart) + " Cents";
                }

                return words + " Only";
            }
        }

    }
}

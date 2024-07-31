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
    public class ConsultantPatientController : Controller
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
        private readonly IActivityMasterDetailsServices _activityMasterDetailsServices;
        private readonly IPatientInitialAssessmentMasterServices _patientInitialAssessmentMasterServices;
        PatientInitialAssessmentMasterModel patientInitialAssessmentMasterModel = new PatientInitialAssessmentMasterModel();


        public ConsultantPatientController(
          IPatientMasterServices patientMasterServices,
          IPatientGeneralDetailMasterServices patientGeneralDetailMasterServices,
          ICommonService commonService,IUserMasterServices userMasterServices,
          IDepartmentMasterServices departmentMasterServices,
          IConsultantMasterServices consultantMasterServices, 
          IPatientConsultantMasterServices patientConsultantMasterServices,
          IRevisitDetailMasterServices revisitDetailMasterServices,
          IPatientServiceMasterServices patientServiceMasterServices,
          IServiceHeadMasterService serviceHeadMasterService,
          IServiceMasterService serviceMasterService,
          IActivityMasterDetailsServices activityMasterDetailsServices,
          IPatientInitialAssessmentMasterServices patientInitialAssessmentMasterServices
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
            _activityMasterDetailsServices= activityMasterDetailsServices;
            _patientInitialAssessmentMasterServices= patientInitialAssessmentMasterServices;
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



            string qcnd = " and CI.IsChecked = 0 OR CI.IsChecked IS NULL and CI.id in (select Patient_Id  from  PatientConsultantMaster where User_Id=" + SessionUser.ToString() + ")";

            var res = _patientMasterServices.GetByClinicIdWisePatient(ref TotalCount,SessionUser,SclinicId, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder, qcnd);
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

                var getAllPatientServiceDetail = _patientServiceMasterServices.GetAll(res[i].Id);
                if (getAllPatientServiceDetail != null && getAllPatientServiceDetail.Count > 0)
                {
                    res[i].patientServiceMastersModel = getAllPatientServiceDetail;
                    res[i].IsCheckPatientService = true;
                }
                else
                {
                    getAllPatientServiceDetail = new List<PatientServiceMasterModel>();
                    res[i].patientServiceMastersModel = getAllPatientServiceDetail;
                    res[i].IsCheckPatientService = false;
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
                    patientMasterModel.Discount1 = dataConsultant.Discount;
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
                    var remake = _patientInitialAssessmentMasterServices.GetByPatientId(patientMasterModel.Id);
                    if(remake!=null)
                    {
                        HttpContext.Session.SetInt32(SessionHelper.SessionpatientInitialAssessment_Id,remake.Id);
                        patientMasterModel.ChiefComplaints = remake.ChiefComplaints;
                        patientMasterModel.PresentIllness= remake.PresentIllness;
                        patientMasterModel.PastHistory = remake.PastHistory;
                        patientMasterModel.FutureHistory = remake.FutureHistory;
                        patientMasterModel.PersonalHistory = remake.PersonalHistory;
                        patientMasterModel.TravelHistory = remake.TravelHistory;
                        patientMasterModel.Remake=remake.Remake;
                    }

                    
                    
                }

            }
            else
            {
                patientMasterModel.Active = true;
                patientMasterModel.User_DesignationList = _commonService.GetUserDepartmentList(0);
            }
            HttpContext.Session.SetInt32(SessionHelper.SessionPatient_Id, patientMasterModel.Id);

            patientMasterModel.lstStatus = _commonService.GetStatusList();
            patientMasterModel.MaritalStatusList = _commonService.GetMaritalStatusList();
            patientMasterModel.GenderList = _commonService.GetGenderList();
            patientMasterModel.BloodGroupList = _commonService.GetBloodgroupList();
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            patientMasterModel.DepartmentList1 = _commonService.GetDepartmentList(SclinicId);
            patientMasterModel.ServiceHeadList=_commonService.GetDepartmentwiseServiceheadList(SclinicId);
            patientMasterModel.ServiceList=_commonService.GetServiceHeadwiseServiceList(SclinicId);
            patientMasterModel.PaymentModeList = _commonService.GetPaymentModeList();
            patientMasterModel.serviceMasterLists = _serviceMasterService.GetByServiceHeadIdWiseServiceList(patientMasterModel.ServiceHead_Id);
            //patientMasterModel.patientServiceMasters.departmentList = _commonService.GetDepartmentList(SclinicId);
            patientMasterModel.departmentList = _commonService.GetDepartmentList(SclinicId);

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
            int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
            patientMasterModel.DepartmentList1 = _commonService.GetDepartmentList(SclinicId);
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

                    //ActivityMasterDetails
                    model.ActivityBy = SessionUser;
                    model.Description = null;

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
                revisitDetailModel.RevisitCharges = user.SpecifyRevisit;
                //revisitDetailModel.SpecifyRevisitDay = user.SpecifyRevisit;
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

                    revisitDetailModel.RevisitDate = DateTime.Now.ToString("MM/dd/yyyy");
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
                if (ModelState.IsValid)
                {
                    bool SpecifyRevisitDay = true;
                    bool IsCheckNewConsultant = true;
                    patientMasterModel = _patientMasterServices.GetById(model.Patient_Id);

                    if (patientMasterModel != null)
                    {
                        DateTime dtPtientvisit = patientMasterModel.CreatedDate.Value;
                        string dtRevisit = model.RevisitDate;
                        DateTime dtrevisitdate = Convert.ToDateTime(dtRevisit);
                        int noOfDay = model.SpecifyRevisitDay;
                        double dSpecifyRevisitDay = (dtrevisitdate.Date - dtPtientvisit.Date).TotalDays;
                        if (dSpecifyRevisitDay > noOfDay)
                        {
                            SpecifyRevisitDay = false;
                        }
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
                    if (SpecifyRevisitDay)
                    {
                        if (IsCheckNewConsultant)
                        {
                            if (model.Id == 0)
                            {
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
                                if (model.RevisitDate == null)
                                {
                                    model.RevisitDate = DateTime.Now.ToString();
                                }
                                model.CreatedDate = DateTime.Now;
                                model.UpdatedDate = DateTime.Now;
                                model.DeletedDate = DateTime.Now;
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
                                model.CreatedBy = 1;
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
            int dId = 0;
            if (deleteId != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(deleteId));
            if (dId != 0)
            {
                int Deleted_By = 1;
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

        public ActionResult PatientServiceDetails(int Patient_Id, int Id)
        {
            try
            {
                PatientServiceMasterModel patientServiceMaster = new PatientServiceMasterModel();

                var patientMasterModel = _patientMasterServices.GetById(Patient_Id);

                var patientConsultant = _patientMasterServices.GetConsultantByPatientId(Patient_Id);
                var user = _userMasterServices.GetById(patientConsultant.User_Id);
                patientServiceMaster.Patient_Id = Patient_Id;
                if (patientMasterModel != null)
                {
                    patientServiceMaster.UHID = patientMasterModel.UHID;
                    patientServiceMaster.PatientName = patientMasterModel.Fname + " " + patientMasterModel.Lname;

                }
                if (patientConsultant != null)
                {
                    patientServiceMaster.ConsultantName = user.FirstName + " " + user.LastName;
                    patientServiceMaster.Consultant_Id = patientConsultant.Id;
                }


                patientServiceMaster.patientServiceMastersList = _patientServiceMasterServices.GetAll(Patient_Id);
                for (int i = 0; i < patientServiceMaster.patientServiceMastersList.Count; i++)
                {
                    patientServiceMaster.patientServiceMastersList[i].PatientName = patientMasterModel.Fname + " " + patientMasterModel.Lname;
                    patientServiceMaster.patientServiceMastersList[i].ConsultantName = user.FirstName + " " + user.LastName;
                    var dept = _departmentMasterServices.GetById(patientServiceMaster.patientServiceMastersList[i].Department_Id);
                    patientServiceMaster.patientServiceMastersList[i].DepartmentName = dept.DepartmentName;

                    var servicehead = _serviceHeadMasterService.GetById(patientServiceMaster.patientServiceMastersList[i].ServiceHead_Id);
                    patientServiceMaster.patientServiceMastersList[i].ServiceHeadName = servicehead.ServiceHeadName;

                    var service = _serviceMasterService.GetById(patientServiceMaster.patientServiceMastersList[i].Service_Id);
                    patientServiceMaster.patientServiceMastersList[i].ServiceName = service.ServiceName;

                }
                patientServiceMaster.Department = patientServiceMaster.Department_Id.ToString();
                patientServiceMaster.ServiceHead = patientServiceMaster.ServiceHead_Id.ToString();
                patientServiceMaster.Service = patientServiceMaster.Service_Id.ToString();

                int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
                patientServiceMaster.departmentList = _commonService.GetDepartmentList(SclinicId);
                patientServiceMaster.serviceHeadList = _commonService.GetDepartmentwiseServiceheadList(patientServiceMaster.Department_Id);
                patientServiceMaster.serviceList = _commonService.GetServiceHeadwiseServiceList(patientServiceMaster.ServiceHead_Id);
                patientServiceMaster.lstStatus = _commonService.GetStatusList();





                return PartialView("_PatientServiceDetail", patientServiceMaster);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IActionResult DetelePatientServiceDetails(string deleteId)
        {
            int dId = 0;
            if (deleteId != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(deleteId));
            if (dId != 0)
            {
                int Deleted_By = 1;
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


        public ActionResult PatientServiceMasterDetails(int serviceHead_Id)
        {
            try
            {
                List<ServiceMasterModel> serviceMasterModel = new List<ServiceMasterModel>();
                serviceMasterModel = _serviceMasterService.GetByServiceHeadIdWiseServiceList(serviceHead_Id);
                return PartialView("_ConsultantPatientServiceDetail", serviceMasterModel);
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


                    model.Department_Id = int.Parse(model.Department);
                    model.ServiceHead_Id = int.Parse(model.ServiceHead);
                    model.Consultant_Id = model.Consultant_Id;
                    model.Revisit_Id = getTopOneRevisitDetail.Id;
                    //model.Service_Id = int.Parse(model.ServiceName);
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
                        var res = _patientServiceMasterServices.Insert(model);
                        if (res.DbCode >= 1)
                        {
                            ServiceMasterModel modelServiceMasterModel = new ServiceMasterModel();
                            modelServiceMasterModel.CreatedBy = SessionUser;
                            modelServiceMasterModel.Active = true;
                            modelServiceMasterModel.PatientServiceMasterId = res.DbCode;
                            modelServiceMasterModel.ServiceId = int.Parse(model.Service);
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
                return RedirectToAction("AddEdit");
            }
            catch (Exception ex)
            {
                throw;
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

                patientMasterModel = _patientMasterServices.GetById(Patient_Id);

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

        [HttpPost]
        public IActionResult AddEditPatientInitialAssessment(PatientInitialAssessmentMasterModel model)
        {
            try
            {
                patientInitialAssessmentMasterModel.lstStatus = _commonService.GetStatusList();
                int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
                int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
                if(model.Id==0)
                {
                    if (HttpContext.Session.GetInt32(SessionHelper.SessionpatientInitialAssessment_Id) != null )
                    { 
                        int PatientInitialAssessment_ID = (int)HttpContext.Session.GetInt32(SessionHelper.SessionpatientInitialAssessment_Id);
                        if (PatientInitialAssessment_ID != null)
                        {
                            model.Id = PatientInitialAssessment_ID;
                        }
                    }
                    else
                    {
                        model.Id = 0;
                    }
                }

                    if (ModelState.IsValid)
                {
                    
                    if (model.Id == 0)
                    {
                        model.CreatedBy = SessionUser;
                        if (model.Active == true)
                        {
                            model.IsDelete = false;
                        }
                        else
                        {
                            model.IsDelete = true;
                        }

                        var res = _patientInitialAssessmentMasterServices.Insert(model);
                        if (res.DbCode == 1)
                        {
                            TempData[Temp_Message.Success] = res.DbMsg;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData[Temp_Message.Error] = res.DbMsg;
                            //return RedirectToAction("AddEdit");
                        }
                    }
                    else
                    {

                        model.UpdatedBy = SessionUser;
                        if (model.Active == true)
                        {
                            model.IsDelete = false;
                        }
                        else
                        {
                            model.IsDelete = true;
                        }

                        var res = _patientInitialAssessmentMasterServices.Update(model);

                        if (res.DbCode == 1)
                        {
                            TempData[Temp_Message.Success] = res.DbMsg;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData[Temp_Message.Error] = res.DbMsg;
                            //return RedirectToAction("AddEdit");

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

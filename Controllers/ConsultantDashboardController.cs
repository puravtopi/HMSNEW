using HMS.Common;
using HMS.Interface;
using HMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection;

namespace HMS.Controllers
{
    public class ConsultantDashboardController : Controller
    {
        private readonly ICommonService _commonService;
        private readonly IPatientMasterServices _patientMasterServices;
        private readonly IPatientGeneralDetailMasterServices _patientGeneralDetailMasterServices;
        PatientMasterModel patientMasterModel = new PatientMasterModel();
        private readonly IRevisitDetailMasterServices _revisitDetailMasterServices;
        private readonly IPatientConsultantMasterServices _patientConsultantMasterServices;

        public ConsultantDashboardController(
          IPatientMasterServices patientMasterServices,
          IPatientGeneralDetailMasterServices patientGeneralDetailMasterServices,
          ICommonService commonService,IRevisitDetailMasterServices revisitDetailMasterServices,
          IPatientConsultantMasterServices patientConsultantMasterServices
          )
        {
            _patientGeneralDetailMasterServices = patientGeneralDetailMasterServices;
            _patientMasterServices = patientMasterServices;
            _commonService = commonService;
            _revisitDetailMasterServices= revisitDetailMasterServices;
            _patientConsultantMasterServices=patientConsultantMasterServices;

        }
        public IActionResult Index(int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortOrder = "Desc", string sortField = "CI.Id")
        {
            //var breadcrumbs = new List<Breadcrumb>
            //    {
            //        new Breadcrumb { Text = "HMS", Url = null },
            //        new Breadcrumb { Text = "Master", Url = null },
            //        new Breadcrumb { Text = "Patient", Url = null },

            //    };

            //// Set the Breadcrumbs collection in the ViewBag
            //ViewBag.Breadcrumbs = breadcrumbs;
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
            
            if(res.Count > 0)
            {
                ViewBag.total = res.Count;
            }
            else { ViewBag.total = 0; }
            var r = 0;
            var Appointments = 0;
            for (int i = 0; i < res.Count; i++)
            {
                
                var dateTime = DateTime.Parse(res[i].EntryDateTime);
                if (dateTime.ToString("MM-dd-yyyy") == DateTime.Now.ToString("MM-dd-yyyy"))
                {
                    
                    r++;
                   ViewBag.todayPatient = r;
                    
                }
                else
                {
                    if(r==0)
                    {
                        ViewBag.todayPatient = 0;
                    }
                  
                }


                if (dateTime.ToString("MM-dd-yyyy") == DateTime.Now.ToString("MM-dd-yyyy"))
                {
                    if(dateTime.ToString("hh:mm") != DateTime.Now.ToString("hh:mm"))
                    {
                        var datapa = _patientConsultantMasterServices.GetByPatientIdWise(res[i].Id);
                        if(datapa != null)
                        {
                            if(ViewBag.netamount==null)
                            {
                                decimal netamount = decimal.Parse(datapa.TotalAmount);
                                if (netamount == null)
                                {
                                    ViewBag.netamount = netamount;
                                }
                                else
                                {

                                    ViewBag.netamount = netamount;
                                }
                            }
                            else
                            {
                                decimal netamount1 = decimal.Parse(datapa.TotalAmount);
                                ViewBag.netamount += netamount1;
                            }
                            
                        }
                        else
                        {
                            
                            ViewBag.netamount = 0;
                        }
                        
                    }
                    
                }
                if (res[i].PaymentMode=="Due")
                {
                    
                    Appointments++;
                    ViewBag.todayAppointments = Appointments;
                }
                else
                {
                    if(Appointments == 0)
                    {
                        ViewBag.todayAppointments = 0;
                    }
                    
                }
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
                    ViewBag.RevisitCount = getAllRevisitDetail.Count;
                }
                else
                {
                    getAllRevisitDetail = new List<RevisitDetailModel>();
                    res[i].revisitDetailModel = getAllRevisitDetail;
                    res[i].IsCheckRevisit = false;
                    ViewBag.RevisitCount = getAllRevisitDetail.Count;
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
    }
}

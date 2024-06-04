using HMS.Common;
using HMS.Interface;
using HMS.Models;
using HMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IUserMasterServices _userMasterServices;
        private readonly IConsultantServices _consultantServices;
        ConsultantDashboardModel consultantDashboard = new ConsultantDashboardModel();

        public ConsultantDashboardController(
          IPatientMasterServices patientMasterServices,
          IConsultantServices consultantServices,
          IPatientGeneralDetailMasterServices patientGeneralDetailMasterServices,
          ICommonService commonService, IRevisitDetailMasterServices revisitDetailMasterServices,
          IPatientConsultantMasterServices patientConsultantMasterServices, IUserMasterServices userMasterServices
          )
        {
            _patientGeneralDetailMasterServices = patientGeneralDetailMasterServices;
            _patientMasterServices = patientMasterServices;
            _commonService = commonService;
            _revisitDetailMasterServices = revisitDetailMasterServices;
            _patientConsultantMasterServices = patientConsultantMasterServices;
            _userMasterServices = userMasterServices;
            _consultantServices = consultantServices;
        }


        public IActionResult Index()
        {
            int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);

            if (SessionUser > 0 && SessionUser != null)
            {
                var data = _userMasterServices.GetById(SessionUser);
                if (data != null)
                {
                    var firstname = data.FirstName != null ? data.FirstName : "";
                    var lastname = data.LastName != null ? data.LastName : "";
                    ViewBag.Consultantname =  firstname + " " + lastname;
                }

            }
            var year =DateTime.Now.ToString("yyyy");

            var result = _consultantServices.GetDashboardCount(SessionUser);

            var ChartCount = _consultantServices.GetDashboardChartCount(SessionUser,Int32.Parse(year));
            string CurrentMonth = String.Format("{0:MMMM}", DateTime.Now);
            string LastMonth = String.Format("{0:MMMM}", DateTime.Now.AddMonths(-1));


            for (int i=0;i<ChartCount.Count;i++)
            {
                if (ChartCount[i].RevisitCount>0)
                {
                    ChartCount[i].SumOfTotalAmount = ChartCount[i].SumOfTotalAmount+ ChartCount[i].RevisitCount;
                    
                }
                
                if (ChartCount[i].Months == CurrentMonth)
                {
                    ViewBag.MonthlyCollection = ChartCount[i].SumOfTotalAmount;
                }
                if (ChartCount[i].Months == LastMonth)
                {
                    ViewBag.LastMonthlyCollection = ChartCount[i].SumOfTotalAmount;
                }

            }
           
            var AvrageCount = _consultantServices.GetDashboardAvrageCount(SessionUser);
            var patientMaster = _patientMasterServices.GetConsultantPatient(SessionUser, DateTime.Now.ToString("yyyy-MM-dd"));
            
            ViewBag.PatientList= patientMaster;
            ViewBag.todayPatient = result.TotalPatient;
            ViewBag.netamount = result.TotalIncome;
            ViewBag.todayAppointments = 0;
            ViewBag.revisitCount = 0;
            ViewBag.totalServices = 0;
            ViewBag.totalPatient = result.TotalPatient + 0;
            ViewBag.Chartdata= JsonConvert.SerializeObject(ChartCount);
            ViewBag.patientIsChecked = result.PatientIsCheckedCount;
            ViewBag.PatienIsCheckedpending = result.PatientIsCheckedPendingCount;
            ViewBag.Currentdate = DateAndTime.Now.ToString("dd-MM-yyyy");
            ViewBag.StartedTime = result.StartedTime;
            ViewBag.EndedTime = result.EndedTime;
            ViewBag.PercentageChange = AvrageCount.PercentageChange;
            ViewBag.ChangeDirection = AvrageCount.ChangeDirection;
            ViewBag.RevenueChangeDirection = AvrageCount.RevenueChangeDirection;
            ViewBag.RevenuePercentageChange = AvrageCount.RevenuePercentageChange;
            ViewBag.ServiceChangeDirection = AvrageCount.ServiceChangeDirection;
            ViewBag.TodayServiceCount = AvrageCount.TodayServiceCount;
            ViewBag.TodayTotalAmount = AvrageCount.TodayRevenue;
            ViewBag.YesterdayTotalAmount = AvrageCount.YesterdayRevenue;

            float ServiceCount=float.Parse(AvrageCount.ServicePercentageChange.ToString().TrimStart('-'));
            if (ServiceCount > 100)
            {
                AvrageCount.ServicePercentageChange = ServiceCount / 10;
            }
            ViewBag.ServicePercentageChange = AvrageCount.ServicePercentageChange;         
            
            return View();
        }
        public IActionResult GetDataForYear(int year)
        {
            int sessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);

            // Call your service method to fetch data for the specified year
            var chartData = _consultantServices.GetDashboardChartCount(sessionUser, year);
            for (int i = 0; i < chartData.Count; i++)
            {
                if (chartData[i].RevisitCount > 0)
                {
                    chartData[i].SumOfTotalAmount = chartData[i].SumOfTotalAmount + chartData[i].RevisitCount;
                }
            }
            // Convert the data to JSON and return it
            return Json(chartData);
        }

        

        //public IActionResult Index(int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortOrder = "Desc", string sortField = "CI.Id")
        //{


        //    int count;
        //    int TotalCount = 0;

        //    int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
        //    int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
        //    var res = _patientMasterServices.GetByClinicIdWisePatient(ref TotalCount, SessionUser, SclinicId, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder);
        //    patientMasterModel.lstPageSizeDdl = _commonService.GetPageSizeDDL();
        //    patientMasterModel.MaritalStatusList = _commonService.GetMaritalStatusList();
        //    patientMasterModel.GenderList = _commonService.GetGenderList();
        //    patientMasterModel.BloodGroupList = _commonService.GetBloodgroupList();
        //    patientMasterModel.departmentList = _commonService.GetDepartmentList(SclinicId);
        //    patientMasterModel.PaymentModeList = _commonService.GetPaymentModeList();
        //    if (SessionUser > 0 && SessionUser != null)
        //    {
        //        var data = _userMasterServices.GetById(SessionUser);
        //        if (data != null)
        //        {
        //            var firstname = data.FirstName != null ? data.FirstName : "";
        //            var lastname = data.LastName != null ? data.LastName : "";
        //            ViewBag.Consultantname = firstname + " " + lastname;
        //        }

        //    }
        //    if (res.Count > 0)
        //    {
        //        ViewBag.total = res.Count;
        //    }
        //    else { ViewBag.total = 0; }
        //    var r = 0;
        //    var Appointments = 0;
        //    for (int i = 0; i < res.Count; i++)
        //    {

        //        var dateTime = DateTime.Parse(res[i].EntryDateTime);
        //        if (dateTime.ToString("MM-dd-yyyy") == DateTime.Now.ToString("MM-dd-yyyy"))
        //        {

        //            r++;
        //            ViewBag.todayPatient = r;

        //        }
        //        else
        //        {
        //            if (r == 0)
        //            {
        //                ViewBag.todayPatient = 0;
        //            }

        //        }


        //        if (dateTime.ToString("MM-dd-yyyy") == DateTime.Now.ToString("MM-dd-yyyy"))
        //        {
        //            if (dateTime.ToString("hh:mm") != DateTime.Now.ToString("hh:mm"))
        //            {
        //                var datapa = _patientConsultantMasterServices.GetByPatientIdWise(res[i].Id);
        //                if (datapa != null)
        //                {
        //                    if (ViewBag.netamount == null)
        //                    {
        //                        decimal netamount = decimal.Parse(datapa.TotalAmount);
        //                        if (netamount == null)
        //                        {
        //                            ViewBag.netamount = netamount;
        //                        }
        //                        else
        //                        {

        //                            ViewBag.netamount = netamount;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        decimal netamount1 = decimal.Parse(datapa.TotalAmount);
        //                        ViewBag.netamount += netamount1;
        //                    }

        //                }
        //                else
        //                {

        //                    ViewBag.netamount = 0;
        //                }

        //            }

        //        }
        //        if (res[i].PaymentMode == "Due")
        //        {

        //            Appointments++;
        //            ViewBag.todayAppointments = Appointments;
        //        }
        //        else
        //        {
        //            if (Appointments == 0)
        //            {
        //                ViewBag.todayAppointments = 0;
        //            }

        //        }
        //        var data = _patientGeneralDetailMasterServices.GetByPatientIdWise(res[i].Id);

        //        //patientMasterList
        //        if (data != null)
        //        {
        //            res[i].Temperature = data.Temperature;
        //            res[i].AdharCard = data.AdharCard;
        //        }

        //        var getAllRevisitDetail = _revisitDetailMasterServices.GetAll(res[i].Id);
        //        if (getAllRevisitDetail != null && getAllRevisitDetail.Count > 0)
        //        {
        //            //res[i].revisitDetailModel = new List<RevisitDetailModel>();
        //            res[i].revisitDetailModel = getAllRevisitDetail;
        //            res[i].IsCheckRevisit = true;
        //            ViewBag.RevisitCount = getAllRevisitDetail.Count;
        //        }
        //        else
        //        {
        //            getAllRevisitDetail = new List<RevisitDetailModel>();
        //            res[i].revisitDetailModel = getAllRevisitDetail;
        //            res[i].IsCheckRevisit = false;
        //            ViewBag.RevisitCount = getAllRevisitDetail.Count;
        //        }


        //    }


        //    if (res.Count > 0)
        //    {
        //        if (res[0].DbCode == -1)
        //        {
        //            ViewBag.ErroMsg = res[0].DbMsg;
        //            count = 0;
        //        }
        //        else
        //        {
        //            count = res.Count;
        //            patientMasterModel.patientMastersList = res;

        //        }
        //    }

        //    patientMasterModel.Pager = new JW.Pager(TotalCount, currentPage, PageSizeId);
        //    if (TempData[Temp_Message.Success] != null)
        //        ModelState.AddModelError(Temp_Message.Success, TempData[Temp_Message.Success].ToString());
        //    patientMasterModel.SearchString = searchString;
        //    patientMasterModel.PageSizeId = PageSizeId;
        //    patientMasterModel.sortField = sortField;
        //    patientMasterModel.sortOrder = ViewBag.SortOrder;
        //    sortOrder = ViewBag.SortOrder;

        //    return View(patientMasterModel);
        //}
    }
}

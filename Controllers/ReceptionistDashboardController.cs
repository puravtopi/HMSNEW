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
    public class ReceptionistDashboardController : Controller
    {
        private readonly ICommonService _commonService;
        private readonly IPatientMasterServices _patientMasterServices;
        private readonly IPatientGeneralDetailMasterServices _patientGeneralDetailMasterServices;
        PatientMasterModel patientMasterModel = new PatientMasterModel();
        private readonly IRevisitDetailMasterServices _revisitDetailMasterServices;
        private readonly IPatientConsultantMasterServices _patientConsultantMasterServices;
        private readonly IUserMasterServices _userMasterServices;
        private readonly IReceptionistMasterServices _receptionistMasterServices;

        ReceptionistDashboardModel ReceptionistDashboard= new ReceptionistDashboardModel();
        
        public ReceptionistDashboardController(
          IPatientMasterServices patientMasterServices,
          
          IPatientGeneralDetailMasterServices patientGeneralDetailMasterServices,
          ICommonService commonService, IRevisitDetailMasterServices revisitDetailMasterServices,
          IPatientConsultantMasterServices patientConsultantMasterServices, IUserMasterServices userMasterServices,
          IReceptionistMasterServices receptionistMasterServices
          )
        {
            _patientGeneralDetailMasterServices = patientGeneralDetailMasterServices;
            _patientMasterServices = patientMasterServices;
            _commonService = commonService;
            _revisitDetailMasterServices = revisitDetailMasterServices;
            _patientConsultantMasterServices = patientConsultantMasterServices;
            _userMasterServices = userMasterServices;
            _receptionistMasterServices= receptionistMasterServices;
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

            var result = _receptionistMasterServices.GetDashboardCount(SessionUser);

            var ChartCount = _receptionistMasterServices.GetDashboardChartCount(SessionUser,Int32.Parse(year));
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
           
            var AvrageCount = _receptionistMasterServices.GetDashboardAvrageCount(SessionUser);
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
            var chartData = _receptionistMasterServices.GetDashboardChartCount(sessionUser, year);
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

    }
}

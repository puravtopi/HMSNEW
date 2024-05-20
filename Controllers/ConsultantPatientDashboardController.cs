using HMS.Common;
using HMS.Interface;
using HMS.Models;
using HMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HMS.Controllers
{
    public class ConsultantPatientDashboardController : Controller
    {
        private readonly ICommonService _commonService;
        private readonly IPatientMasterServices _patientMasterServices;
        private readonly IPatientGeneralDetailMasterServices _patientGeneralDetailMasterServices;
        PatientMasterModel patientMasterModel = new PatientMasterModel();


        public ConsultantPatientDashboardController(
          IPatientMasterServices patientMasterServices,
          IPatientGeneralDetailMasterServices patientGeneralDetailMasterServices,
          ICommonService commonService

          )
        {
            _patientGeneralDetailMasterServices = patientGeneralDetailMasterServices;
            _patientMasterServices = patientMasterServices;
            _commonService = commonService;

        }
        public IActionResult Index(int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortOrder = "Desc", string sortField = "Id")
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Patient", Url = null },

                };

            // Set the Breadcrumbs collection in the ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;
            int count;            
            int TotalCount = 0;
            if (searchString != null)
            {
                searchString = searchString.Trim();
            }
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
            string cnd = " and id in (select Patient_Id  from  PatientConsultantMaster where User_Id=" + SessionUser.ToString() + ")";

            var res = _patientMasterServices.GetByClinicIdWisePatient(ref TotalCount, SclinicId, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder, cnd);
                
            
            for (int i = 0; i < res.Count; i++)
            {
                var data = _patientGeneralDetailMasterServices.GetByPatientIdWise(res[i].Id);
                if (data != null)
                {
                    res[i].Temperature = data.Temperature;
                    res[i].AdharCard = data.AdharCard;
                }

                if (res[i].CreatedDate==DateTime.Now)
                {
                    for(int j = 0; j < res[i].Id;j++)
                    {
                        int total = res[i].Id;
                        ViewBag.TotalPatient = total;
                    }
                    
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

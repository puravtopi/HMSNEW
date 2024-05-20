using HMS.Common;
using HMS.Interface;
using HMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using HMS.Services;

namespace HMS.Controllers
{
    public class ConsultantMasterController : Controller
    {
        private readonly IConsultantMasterServices _consultantMasterServices;
        private readonly ICommonService _commonService;
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        ConsultantMasterModel consultantMaster=new ConsultantMasterModel();
        private readonly IDepartmentMasterServices _departmentMasterServices;
        


        public ConsultantMasterController(IConsultantMasterServices consultantMasterServices, ICommonService commonService,IDepartmentMasterServices departmentMasterServices)
        {
            _consultantMasterServices = consultantMasterServices;
            _commonService = commonService;
            _departmentMasterServices= departmentMasterServices;
        }

        public IActionResult Index(int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortOrder = "Desc", string sortField = "Id")
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Consultant", Url = null },

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
            var res = _consultantMasterServices.GetByClinicIdWiseConsultant(ref TotalCount, SclinicId, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder);
            consultantMaster.lstPageSizeDdl = _commonService.GetPageSizeDDL();
            consultantMaster.departmentList = _commonService.GetDepartmentList(SclinicId);
            for (int i = 0;i<res.Count;i++)
            {
                var data = _departmentMasterServices.GetById(res[i].Department_Id);
                res[i].DepartmentName = data.DepartmentName;
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
                    consultantMaster.consultantMastersList = res;
                }
            }
            consultantMaster.Pager = new JW.Pager(TotalCount, currentPage, PageSizeId);
            if (TempData[Temp_Message.Success] != null)
                ModelState.AddModelError(Temp_Message.Success, TempData[Temp_Message.Success].ToString());
            consultantMaster.SearchString = searchString;
            consultantMaster.PageSizeId = PageSizeId;
            consultantMaster.sortField = sortField;
            consultantMaster.sortOrder = ViewBag.SortOrder;
            sortOrder = ViewBag.SortOrder;

            return View(consultantMaster);
        }

        public IActionResult AddEdit(string consultantid)
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Consultant Master", Url = Url.Action("Index","ConsultantMaster") },
                     new Breadcrumb { Text = "Add/Edit", Url = null },

                }; 

            // Set the Breadcrumbs collection in the ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            int dId = 0;
            if (consultantid != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(consultantid));
            if (dId != 0)
            {
                consultantMaster = _consultantMasterServices.GetById(dId);
                consultantMaster.Departments = consultantMaster.Department_Id.ToString();
            }
            else
            {
                consultantMaster.Active = true;
            }
            consultantMaster.lstStatus = _commonService.GetStatusList();
            consultantMaster.departmentList = _commonService.GetDepartmentList(SclinicId);



            return View(consultantMaster);
        }

        [HttpPost]
        public IActionResult AddEdit(ConsultantMasterModel model)
        {
            consultantMaster.lstStatus = _commonService.GetStatusList();            
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            consultantMaster.departmentList = _commonService.GetDepartmentList(SclinicId);
            if (ModelState.IsValid)
            {

                if (model.Id == 0)
                {

                    model.CreatedBy = 1;
                    if (model.Active == true)
                    {
                        model.IsDelete = false;
                    }
                    else
                    {
                        model.IsDelete = true;
                    }
                    model.Department_Id = Int32.Parse(model.Departments);                    
                    model.Clinic_Id = SclinicId;
                    //model.Active = true;
                    var res = _consultantMasterServices.Insert(model);
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
                    
                    if (model.Active == true)
                    {
                        model.IsDelete = false;
                    }
                    else
                    {
                        model.IsDelete = true;
                    }
                    if (!string.IsNullOrEmpty(model.ConsultantName))
                    {
                        string consultantName = model.ConsultantName.Substring(0, 3);
                        string consultantID = _commonService.GetPrefix(model.Id);
                        model.Consultant_Code = consultantName + consultantID;
                        model.Id = model.Id;
                        var resCode = _consultantMasterServices.UpdateConsultant_Code(model);
                    }
                    model.Clinic_Id = SclinicId;
                    model.Department_Id = Int32.Parse(model.Departments);
                    var res = _consultantMasterServices.Update(model);
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
                var res = _consultantMasterServices.DeleteById(dId, Deleted_By);
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


        public JsonResult GetBYDepartmentIdWiseConsultantData(int Department_Id)
        { 
            var data = _commonService.GetConsultantList(Department_Id);       
            return Json(data);
        }

        public JsonResult GetConsultantById(int consultant_Id)
        {
            var data = _consultantMasterServices.GetById(consultant_Id);
            return Json(data);
        }
    }
}

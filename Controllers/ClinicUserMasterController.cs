using HMS.Common;
using HMS.Interface;
using HMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using HMS.Services;

namespace HMS.Controllers
{
    public class ClinicUserMasterController : Controller
    {
        private readonly IUserMasterServices _UserMasterServices;
        private readonly ICommonService _commonService;
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        UserMasterModel userMasterModel = new UserMasterModel();
        private readonly IDepartmentMasterServices _DepartmentMasterServices;
        private readonly IDesignationMasterServices _DesignationMasterServices;
        UserMasterModel UserMasterModel = new UserMasterModel();
        private readonly Random _random = new Random();
        public ClinicUserMasterController(IUserMasterServices UserMasterServices, ICommonService commonService,IDepartmentMasterServices departmentMasterServices, IDesignationMasterServices designationMasterServices)
        {
            _UserMasterServices = UserMasterServices;
            _commonService = commonService;
            _DepartmentMasterServices = departmentMasterServices;
            _DesignationMasterServices = designationMasterServices;
        }

        public IActionResult Index(int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortOrder = "Desc", string sortField = "Id")
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Clinic User", Url = null },

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
            var res = _UserMasterServices.GetByClinicIdWiseUser(ref TotalCount, SclinicId, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder);
            UserMasterModel.lstPageSizeDdl = _commonService.GetPageSizeDDL();
            userMasterModel.designationList = _commonService.GetDesignationList(SclinicId);
            userMasterModel.departmentList = _commonService.GetDepartmentList(SclinicId);

            if (res[0].DbCode == -1)
            {
                ViewBag.ErroMsg = res[0].DbMsg;
                count = 0;
            }
            else
            {
                count = res.Count;
                UserMasterModel.userMasterList = res;
            }
            UserMasterModel.Pager = new JW.Pager(TotalCount, currentPage, PageSizeId);
            if (TempData[Temp_Message.Success] != null)
                ModelState.AddModelError(Temp_Message.Success, TempData[Temp_Message.Success].ToString());
            UserMasterModel.SearchString = searchString;
            UserMasterModel.PageSizeId = PageSizeId;
            UserMasterModel.sortField = sortField;
            UserMasterModel.sortOrder = ViewBag.SortOrder;
            sortOrder = ViewBag.SortOrder;

            return View(UserMasterModel);
        }

        public IActionResult AddEdit(string Userid)
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Clinic User Master", Url = Url.Action("Index","ClinicUserMaster") },
                     new Breadcrumb { Text = "Add/Edit", Url = null },

                };

            // Set the Breadcrumbs collection in the ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;
            int dId = 0;
            if (Userid != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(Userid));
            //Get Session Data            
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            if (dId != 0)
            {
                userMasterModel = _UserMasterServices.GetById(dId);
                userMasterModel.Departments = userMasterModel.Dept_id.ToString();
                userMasterModel.Designations = userMasterModel.Desig_Id.ToString();

            }
            else
            {
                userMasterModel.Active = true;
            }
            userMasterModel.lstStatus = _commonService.GetStatusList();
            userMasterModel.designationList = _commonService.GetDesignationList(SclinicId);
            userMasterModel.departmentList = _commonService.GetDepartmentList(SclinicId);


            return View(userMasterModel);
        }

        [HttpPost]
        public IActionResult AddEdit(UserMasterModel model)
        {
            userMasterModel.lstStatus = _commonService.GetStatusList();
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            userMasterModel.designationList = _commonService.GetDesignationList(SclinicId);
            userMasterModel.departmentList = _commonService.GetDepartmentList(SclinicId);

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
                    model.Clinic_Id = SclinicId;
                    model.Dept_id = Int32.Parse(model.Departments);
                    model.Desig_Id = Int32.Parse(model.Designations);
                    int randomNumber = _random.Next(10000000, 99999999);
                    string password = _commonService.GetMd5HashNewMethod(randomNumber.ToString());
                    model.Password = password;
                    
                    //model.Active = true;
                    var res = _UserMasterServices.Insert(model);
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
                    model.Clinic_Id = SclinicId;                    
                    model.UpdatedBy = SclinicId;
                    if (model.Active == true)
                    {
                        model.IsDelete = false;
                    }
                    else
                    {
                        model.IsDelete = true;
                    }
                    var res = _UserMasterServices.Update(model);
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
                var res = _UserMasterServices.DeleteById(dId, Deleted_By);
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

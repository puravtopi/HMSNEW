using HMS.Common;
using HMS.Interface;
using HMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Controllers
{
    public class DepartmentMasterController : Controller
    {
        private readonly IDepartmentMasterServices _DepartmentMasterServices;
        private readonly ICommonService _commonService;
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        DepartmentMasterModel DepartmentMasterModel = new DepartmentMasterModel ();


        public DepartmentMasterController(IDepartmentMasterServices DepartmentMasterServices, ICommonService commonService)
        {
            _DepartmentMasterServices = DepartmentMasterServices;
            _commonService = commonService;
        }

        public IActionResult Index(int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortOrder = "Desc", string sortField = "Id")
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Department", Url = null },

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
            var res = _DepartmentMasterServices.GetAll(ref TotalCount, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder);
            DepartmentMasterModel.lstPageSizeDdl = _commonService.GetPageSizeDDL();

            if (res[0].DbCode == -1)
            {
                ViewBag.ErroMsg = res[0].DbMsg;
                count = 0;
            }
            else
            {
                count = res.Count;
                DepartmentMasterModel.departmentMasterList = res;
            }
            DepartmentMasterModel.Pager = new JW.Pager(TotalCount, currentPage, PageSizeId);
            if (TempData[Temp_Message.Success] != null)
                ModelState.AddModelError(Temp_Message.Success, TempData[Temp_Message.Success].ToString());
            DepartmentMasterModel.SearchString = searchString;
            DepartmentMasterModel.PageSizeId = PageSizeId;
            DepartmentMasterModel.sortField = sortField;
            DepartmentMasterModel.sortOrder = ViewBag.SortOrder;
            sortOrder = ViewBag.SortOrder;

            return View(DepartmentMasterModel);
        }

        public IActionResult AddEdit(string departmentid)
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Department Master", Url = Url.Action("Index","DepartmentMaster") },
                     new Breadcrumb { Text = "Add/Edit", Url = null },

                };

            // Set the Breadcrumbs collection in the ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;
            int dId = 0;
            if (departmentid != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(departmentid));
            if (dId != 0)
            {
                DepartmentMasterModel = _DepartmentMasterServices.GetById(dId);
            }
            else
            {
                DepartmentMasterModel.Active = true;
            }
            DepartmentMasterModel.lstStatus = _commonService.GetStatusList();

            return View(DepartmentMasterModel);
        }

        [HttpPost]
        public IActionResult AddEdit(DepartmentMasterModel model)
        {
            DepartmentMasterModel.lstStatus = _commonService.GetStatusList();
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            if (ModelState.IsValid)
            {

                if (model.Id == 0)
                {

                    model.CreatedBy = 1;
                    if (model.Active == true)
                    { 
                        model.IsDelete = false;   
                    }
                    else {
                        model.IsDelete = true;
                    }
                    model.Clinic_Id = SclinicId;
                    //model.Active = true;
                    var res = _DepartmentMasterServices.Insert(model);
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
                    model.Clinic_Id= SclinicId;
                    var res = _DepartmentMasterServices.Update(model);
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
                var res = _DepartmentMasterServices.DeleteById(dId, Deleted_By);
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

using HMS.Common;
using HMS.Interface;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using HMS.Services;
using Microsoft.AspNetCore.Http;

namespace HMS.Controllers
{
    public class ClinicDesignationMasterController : Controller
    {

        private readonly IDesignationMasterServices _DesignationMasterServices;
        private readonly ICommonService _commonService;
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        DesignationMasterModel DesignationMasterModel = new DesignationMasterModel();

        public ClinicDesignationMasterController(IDesignationMasterServices DesignationMasterServices, ICommonService commonService)
        {
            _DesignationMasterServices = DesignationMasterServices;
            _commonService = commonService;
        }
        public IActionResult Index(int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortOrder = "Desc", string sortField = "Id")
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Clinic Designation", Url = null },

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
            var res = _DesignationMasterServices.GetByClinicIdWiseDesig(ref TotalCount, SclinicId, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder);
            DesignationMasterModel.lstPageSizeDdl = _commonService.GetPageSizeDDL();
            if(res.Count>0)
            {
                if (res[0].DbCode == -1)
                {
                    ViewBag.ErroMsg = res[0].DbMsg;
                    count = 0;
                }
                else
                {
                    count = res.Count;
                    DesignationMasterModel.designationMasterList = res;
                }
            }
            DesignationMasterModel.Pager = new JW.Pager(TotalCount, currentPage, PageSizeId);
            if (TempData[Temp_Message.Success] != null)
                ModelState.AddModelError(Temp_Message.Success, TempData[Temp_Message.Success].ToString());
            DesignationMasterModel.SearchString = searchString;
            DesignationMasterModel.PageSizeId = PageSizeId;
            DesignationMasterModel.sortField = sortField;
            DesignationMasterModel.sortOrder = ViewBag.SortOrder;
            sortOrder = ViewBag.SortOrder;

            return View(DesignationMasterModel);
        }

        public IActionResult AddEdit(string Designationid,string designationName = null)
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Clinic", Url = null },
                    new Breadcrumb { Text = "Designation", Url = Url.Action("Index","ClinicDesignationMaster") },
                     new Breadcrumb { Text = "Add/Edit", Url = null },

                };

            // Set the Breadcrumbs collection in the ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;
            int dId = 0;
            if (Designationid != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(Designationid));
            if (dId != 0)
            {
                DesignationMasterModel = _DesignationMasterServices.GetById(dId);
            }
            else
            {
                DesignationMasterModel.Active = true;
               DesignationMasterModel = new DesignationMasterModel
               {
                    Active = true,
                    DesignationName = designationName // Set the Designatiopn name if passed
                };
            }
            DesignationMasterModel.lstStatus = _commonService.GetStatusList();

            return View(DesignationMasterModel);
        }

        [HttpPost]
        public IActionResult AddEdit(DesignationMasterModel model)
        {
            DesignationMasterModel.lstStatus = _commonService.GetStatusList();
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            if (ModelState.IsValid)
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
                    model.Clinic_Id = SclinicId;
                    
                    var res = _DesignationMasterServices.Insert(model);
                    if (res.DbCode == 1)
                    {
                        TempData[Temp_Message.Success] = res.DbMsg;
                        // return RedirectToAction("Index");
                        return RedirectToAction("Index", new { designationId = encryptDecrypt.EncryptString(model.Id.ToString()), designationName = model.DesignationName });
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
                    model.Clinic_Id = SclinicId;
                    var res = _DesignationMasterServices.Update(model);
                    if (res.DbCode == 1)
                    {
                        TempData[Temp_Message.Success] = res.DbMsg;
                        //return RedirectToAction("Index");
                        return RedirectToAction("Index", new { designationId = encryptDecrypt.EncryptString(model.Id.ToString()), designationName = model.DesignationName });
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
                var res = _DesignationMasterServices.DeleteById(dId, Deleted_By);
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

        [HttpPost]
        public IActionResult SaveDesignationAndAddUser(string designationName)
        {
            if (string.IsNullOrEmpty(designationName))
            {
                return Json(new { success = false });
            }

            var designation = new DesignationMasterModel
            {
                DesignationName = designationName,
                Active = true,
                CreatedBy = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID),
                Clinic_Id = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID)
            };

            var res = _DesignationMasterServices.Insert(designation);

            if (res.DbCode == 1)
            {
                return Json(new { success = true, designationId = res.Id });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}


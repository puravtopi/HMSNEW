﻿using HMS.Common;
using HMS.Interface;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;

namespace HMS.Controllers
{
    public class ActivityMasterController : Controller
    {
        private readonly IActivityMasterServices _activityMasterServices;
        private readonly ICommonService _commonService;
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        ActivityMasterModel activityMasterModel = new ActivityMasterModel();
        


        public ActivityMasterController(ICommonService commonService,       
            IActivityMasterServices activityMasterServices)
        {

            _commonService = commonService;            
            _activityMasterServices=activityMasterServices;
        }

        public IActionResult Index(int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortOrder = "Desc", string sortField = "Id")
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Activity ", Url = null },

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
            var res = _activityMasterServices.GetAll(ref TotalCount, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder);
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            
                       

            if (res[0].DbCode == -1)
            {
                ViewBag.ErroMsg = res[0].DbMsg;
                count = 0;
            }
            else
            {
                count = res.Count;
                activityMasterModel.activityMastersList = res;
            }
            activityMasterModel.Pager = new JW.Pager(TotalCount, currentPage, PageSizeId);
            if (TempData[Temp_Message.Success] != null)
                ModelState.AddModelError(Temp_Message.Success, TempData[Temp_Message.Success].ToString());
            activityMasterModel.SearchString = searchString;
            activityMasterModel.PageSizeId = PageSizeId;
            activityMasterModel.sortField = sortField;
            activityMasterModel.sortOrder = ViewBag.SortOrder;
            sortOrder = ViewBag.SortOrder;

            return View(activityMasterModel);
         }

        public IActionResult AddEdit(string activityid)
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Activity Master", Url = Url.Action("Index","ActivityMaster") },
                     new Breadcrumb { Text = "Add/Edit", Url = null },

                };

            // Set the Breadcrumbs collection in the ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;
            int dId = 0;
            if (activityid != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(activityid));
            if (dId != 0)
            {
                activityMasterModel = _activityMasterServices.GetById(dId);                
            }
            else
            {
                activityMasterModel.Active = true;
            }
           
            activityMasterModel.lstStatus = _commonService.GetStatusList();
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            
            return View(activityMasterModel);
        }

        [HttpPost]
        public IActionResult AddEdit(ActivityMasterModel model)
        {
            try
            {
                activityMasterModel.lstStatus = _commonService.GetStatusList();
                int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
                int SessionUser = (int)HttpContext.Session.GetInt32(SessionHelper.SessionUserId);
                
                if (ModelState.IsValid)
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
                       
                        var res = _activityMasterServices.Insert(model);
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
                        if (model.Active == true)
                        {
                            model.IsDelete = false;
                        }
                        else
                        {
                            model.IsDelete = true;
                        }
                       
                        var res = _activityMasterServices.Update(model);

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
            catch (Exception ex)
            {
                throw ex;
            }
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
                var res = _activityMasterServices.DeleteById(dId, Deleted_By);
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
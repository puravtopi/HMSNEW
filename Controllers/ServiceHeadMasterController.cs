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
    public class ServiceHeadMasterController : Controller
    {
        private readonly IServiceHeadMasterService _serviceHeadMaster;
        private readonly ICommonService _commonService;
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        ServiceHeadMasterModel serviceHeadmodel = new ServiceHeadMasterModel();
        private readonly IDepartmentMasterServices _DepartmentMasterServices;


        public ServiceHeadMasterController(ICommonService commonService, IDepartmentMasterServices departmentMasterServices,IServiceHeadMasterService serviceHeadMaster)
        {
           
            _commonService = commonService;
            _DepartmentMasterServices = departmentMasterServices;
           _serviceHeadMaster=serviceHeadMaster;
        }

        public IActionResult Index(int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortOrder = "Desc", string sortField = "Id")
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Service Head", Url = null },

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
            var res = _serviceHeadMaster.GetAll(ref TotalCount, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder);
            serviceHeadmodel.lstPageSizeDdl = _commonService.GetPageSizeDDL();

            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            serviceHeadmodel.departmentList = _commonService.GetDepartmentList(SclinicId);
            for (int i = 0; i < res.Count; i++)
            {
                //int deptid = Convert.ToInt32(serviceHeadmodel.departmentList[i].Value);
                var data = _DepartmentMasterServices.GetById(res[i].Department_Id);
                res[i].DepartmentName = data.DepartmentName;
            }

            if (res[0].DbCode == -1)
            {
                ViewBag.ErroMsg = res[0].DbMsg;
                count = 0;
            }
            else
            {
                count = res.Count;
                serviceHeadmodel.serviceHeadsList = res;
            }
            serviceHeadmodel.Pager = new JW.Pager(TotalCount, currentPage, PageSizeId);
            if (TempData[Temp_Message.Success] != null)
                ModelState.AddModelError(Temp_Message.Success, TempData[Temp_Message.Success].ToString());
            serviceHeadmodel.SearchString = searchString;
            serviceHeadmodel.PageSizeId = PageSizeId;
            serviceHeadmodel.sortField = sortField;
            serviceHeadmodel.sortOrder = ViewBag.SortOrder;
            sortOrder = ViewBag.SortOrder;

            return View(serviceHeadmodel);
        }

        public IActionResult AddEdit(string serviceheadid)
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "ServiceHead Master", Url = Url.Action("Index","ServiceHeadMaster") },
                     new Breadcrumb { Text = "Add/Edit", Url = null },

                };

            // Set the Breadcrumbs collection in the ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;
            int dId = 0;
            if (serviceheadid != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(serviceheadid));
            if (dId != 0)
            {
                serviceHeadmodel = _serviceHeadMaster.GetById(dId);
                serviceHeadmodel.Departments = serviceHeadmodel.Department_Id.ToString();
            }
            else
            {
                serviceHeadmodel.Active = true;
            }
            
            serviceHeadmodel.lstStatus = _commonService.GetStatusList();

            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            serviceHeadmodel.departmentList = _commonService.GetDepartmentList(SclinicId);

            return View(serviceHeadmodel);
        }

        [HttpPost]
        public IActionResult AddEdit(ServiceHeadMasterModel model)

        {
            try
            {
                serviceHeadmodel.lstStatus = _commonService.GetStatusList();
                int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
                serviceHeadmodel.departmentList = _commonService.GetDepartmentList(SclinicId);
                
                if (ModelState.IsValid)
                {

                    if (model.Id == 0)
                    {
                        model.CreatedBy = SclinicId ;
                        if (model.Active == true)
                        {
                            model.IsDelete = false;
                        }
                        else
                        {
                            model.IsDelete = true;
                        }
                        model.Department_Id = Int32.Parse(model.Departments);
                        var res = _serviceHeadMaster.Insert(model);
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
                        model.Department_Id = Int32.Parse(model.Departments);
                        var res = _serviceHeadMaster.Update(model);

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
            int dId = 0;
            if (deleteId != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(deleteId));
            if (dId != 0)
            {
                int Deleted_By = 1;
                var res = _serviceHeadMaster.DeleteById(dId, Deleted_By);
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

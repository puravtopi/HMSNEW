using HMS.Common;
using HMS.Interface;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;

namespace HMS.Controllers
{
    public class ServiceMasterController : Controller
    {
        private readonly IServiceMasterService _serviceMaster;
        private readonly ICommonService _commonService;
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        ServiceMasterModel servicemodel = new ServiceMasterModel();
        private readonly IDepartmentMasterServices _DepartmentMasterServices;


        public ServiceMasterController(ICommonService commonService, 
            IDepartmentMasterServices departmentMasterServices, IServiceMasterService serviceMaster)
        {

            _commonService = commonService;
            _DepartmentMasterServices = departmentMasterServices;
            _serviceMaster = serviceMaster;
        }

        public IActionResult Index(int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortOrder = "Desc", string sortField = "Id")
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Service ", Url = null },

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
            var res = _serviceMaster.GetAll(ref TotalCount, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder);
            servicemodel.lstPageSizeDdl = _commonService.GetPageSizeDDL();


            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            servicemodel.departmentList = _commonService.GetDepartmentList(SclinicId);
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
                servicemodel.serviceMasterList = res;
            }
            servicemodel.Pager = new JW.Pager(TotalCount, currentPage, PageSizeId);
            if (TempData[Temp_Message.Success] != null)
                ModelState.AddModelError(Temp_Message.Success, TempData[Temp_Message.Success].ToString());
            servicemodel.SearchString = searchString;
            servicemodel.PageSizeId = PageSizeId;
            servicemodel.sortField = sortField;
            servicemodel.sortOrder = ViewBag.SortOrder;
            sortOrder = ViewBag.SortOrder;

            return View(servicemodel);
        }

        public IActionResult AddEdit(string serviceid)
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Service Master", Url = Url.Action("Index","ServiceMaster") },
                     new Breadcrumb { Text = "Add/Edit", Url = null },

                };

            // Set the Breadcrumbs collection in the ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;
            int dId = 0;
            if (serviceid != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(serviceid));
            if (dId != 0)
            {

                servicemodel = _serviceMaster.GetById(dId);
                servicemodel.Departments = servicemodel.Department_Id.ToString();
                
            }
            else
            {
                servicemodel.Active = true;
            }
           
            servicemodel.lstStatus = _commonService.GetStatusList();
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            servicemodel.departmentList = _commonService.GetDepartmentList(SclinicId);
            servicemodel.ServiceHeadList = _commonService.GetDepartmentwiseServiceheadList(servicemodel.Department_Id);

            return View(servicemodel);
        }

        [HttpPost]
        public IActionResult AddEdit(ServiceMasterModel model)
        {
            try
            {
                servicemodel.lstStatus = _commonService.GetStatusList();
                int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
                servicemodel.departmentList = _commonService.GetDepartmentList(SclinicId);
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
                        model.ServiceHead_Id = Int32.Parse(model.ServiceHead);
                        model.Department_Id = Int32.Parse(model.Departments);
                        var res = _serviceMaster.Insert(model);
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

                        model.UpdatedBy = 1;
                        if (model.Active == true)
                        {
                            model.IsDelete = false;
                        }
                        else
                        {
                            model.IsDelete = true;
                        }
                        model.ServiceHead_Id = 3;
                        model.Department_Id = Int32.Parse(model.Departments);
                        var res = _serviceMaster.Update(model);

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
                var res = _serviceMaster.DeleteById(dId, Deleted_By);
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

        public JsonResult GetBYDepartmentIdWiseServicedData(int Department_Id)
        {
            var data = _commonService.GetDepartmentwiseServiceheadList(Department_Id);
            return Json(data);
        }
        public JsonResult GetByServiceHeadIdWiseServicedData(int ServiceHead_Id)
        {
            var data = _commonService.GetServiceHeadwiseServiceList(ServiceHead_Id);
            return Json(data);
        }
        public JsonResult GetServiceCharges(int Service_Id)
        {
            var data = _serviceMaster.GetById(Service_Id);
            return Json(data);
        }
    }
}

using HMS.Common;
using HMS.Interface;
using HMS.Models;
using HMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HMS.Controllers
{
    public class ClinicMasterController : Controller
    {

        private readonly IClinicMasterServices _ClinicMasterServices;
        private readonly ICommonService _commonService;
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        ClinicMasterModel clinicMasterModel = new ClinicMasterModel();
        private readonly IDepartmentMasterServices _DepartmentMasterServices;
        private readonly IDesignationMasterServices _DesignationMasterServices;

        public ClinicMasterController(IClinicMasterServices ClinicMasterServices, ICommonService commonService, IDepartmentMasterServices departmentMasterServices, IDesignationMasterServices designationMasterServices)
        {
            _ClinicMasterServices = ClinicMasterServices;
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
                    new Breadcrumb { Text = "Clinic", Url = null },

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
            var res = _ClinicMasterServices.GetAll(ref TotalCount, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder);
            clinicMasterModel.lstPageSizeDdl = _commonService.GetPageSizeDDL();

            if (res[0].DbCode == -1)
            {
                ViewBag.ErroMsg = res[0].DbMsg;
                count = 0;
            }
            else
            {
                count = res.Count;
                clinicMasterModel.clinicMasterList = res;
            }
            clinicMasterModel.Pager = new JW.Pager(TotalCount, currentPage, PageSizeId);
            if (TempData[Temp_Message.Success] != null)
                ModelState.AddModelError(Temp_Message.Success, TempData[Temp_Message.Success].ToString());
            clinicMasterModel.SearchString = searchString;
            clinicMasterModel.PageSizeId = PageSizeId;
            clinicMasterModel.sortField = sortField;
            clinicMasterModel.sortOrder = ViewBag.SortOrder;
            sortOrder = ViewBag.SortOrder;

            return View(clinicMasterModel);
        }

        public IActionResult AddEdit(string clinicid)
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "Clinic Master", Url = Url.Action("Index","ClinicMaster") },
                     new Breadcrumb { Text = "Add/Edit", Url = null },

                };

            // Set the Breadcrumbs collection in the ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;
            int dId = 0;
            if (clinicid != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(clinicid));
            if (dId != 0)
            {

                clinicMasterModel = _ClinicMasterServices.GetById(dId);
            }
            else
            {
                clinicMasterModel.Active = true;
            }
            //state list
            clinicMasterModel.StateList = _commonService.GetState();

            if (clinicMasterModel.StateId > 0)
            {
                clinicMasterModel.CityList = _DepartmentMasterServices.GetCityWiseState(clinicMasterModel.StateId).Select(city => new SelectListItem
                {
                    Text = city.Name,
                    Value = city.Id.ToString()
                }).ToList();
            }

            var Dept = _DepartmentMasterServices.GetByClinicIdWiseDeptList(dId);
            if (Dept != null)
            {
                clinicMasterModel.departmentList = Dept;
            }
            //Designation Wise List
            var Desig = _DesignationMasterServices.GetByClinicIdWiseDesigList(dId);
            if (Desig != null)
            {
                clinicMasterModel.designationList = Desig;
            }
            clinicMasterModel.lstStatus = _commonService.GetStatusList();

            return View(clinicMasterModel);
        }

        [HttpPost]
        public IActionResult AddEdit(ClinicMasterModel model, IFormFile Logo)
        
        {
            try
            {
                clinicMasterModel.lstStatus = _commonService.GetStatusList();
                //   int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
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
                        if (Logo != null)
                            model.Logo = Logo.FileName;
                        else
                            model.Logo = "";
                        //This Code For Generate Password For temporary Use
                        string password = _commonService.GetMd5HashNewMethod("123456");
                        model.Password = password;
                        var res = _ClinicMasterServices.Insert(model);
                        if (res.DbCode == 1)
                        {
                            if (res.Id > 0)
                            {
                                if (!string.IsNullOrEmpty(model.ClinicName))
                                {
                                    string clinicName = model.ClinicName.Substring(0, 3);
                                    string clinicNum = _commonService.GetPrefix(res.Id);
                                    model.Code = clinicName + clinicNum;
                                    model.Id = res.Id;
                                    var resCode = _ClinicMasterServices.UpdateCode(model);
                                }
                            }

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
                        string password = _commonService.GetMd5HashNewMethod("123456");
                        model.Password = password;
                        model.UpdatedBy = 1;
                        if (model.Active == true)
                        {
                            model.IsDelete = false;
                        }
                        else
                        {
                            model.IsDelete = true;
                        }
                        var res = _ClinicMasterServices.Update(model);

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
                var res = _ClinicMasterServices.DeleteById(dId, Deleted_By);
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

        public IActionResult GetCities(int stateId)
        {
            var cities = _DepartmentMasterServices.GetCityWiseState(stateId);

            // Convert cities to a list of SelectListItem
            var cityList = cities.Select(c => new
            {
                value = c.Id,
                text = c.Name
            }).ToList();

            return Json(cityList);
        }


    }
}

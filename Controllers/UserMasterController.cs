using HMS.Common;
using HMS.Interface;
using HMS.Models;
using HMS.Security;
using HMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Controllers
{
    public class UserMasterController : Controller
    {
        private readonly IUserMasterServices _UserMasterServices;
        private readonly ICommonService _commonService;
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        UserMasterModel UserMasterModel = new UserMasterModel();


        public UserMasterController(IUserMasterServices UserMasterServices, ICommonService commonService)
        {
            _UserMasterServices = UserMasterServices;
            _commonService = commonService;
        }

        public IActionResult Index(int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortOrder = "Desc", string sortField = "Id")
        {
            var breadcrumbs = new List<Breadcrumb>
                {
                    new Breadcrumb { Text = "HMS", Url = null },
                    new Breadcrumb { Text = "Master", Url = null },
                    new Breadcrumb { Text = "User", Url = null },

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
            searchString = " ";

            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);

            string qcnd = " and Clinic_Id=" + SclinicId.ToString();

            if (searchString != null)
            {
                searchString = searchString.Trim();
            }
            var res = _UserMasterServices.GetAll(ref TotalCount, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder, qcnd);
            UserMasterModel.lstPageSizeDdl = _commonService.GetPageSizeDDL();

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
                    new Breadcrumb { Text = "User Master", Url = Url.Action("Index","UserMaster") },
                     new Breadcrumb { Text = "Add/Edit", Url = null },

                };

            // Set the Breadcrumbs collection in the ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;
            int dId = 0;
            if (Userid != null)
                dId = Convert.ToInt32(encryptDecrypt.DecryptString(Userid));
            if (dId != 0)
            {
                UserMasterModel = _UserMasterServices.GetById(dId);
            }
            else
            {
                UserMasterModel.Active = true;
            }
            UserMasterModel.lstStatus = _commonService.GetStatusList();

            return View(UserMasterModel);
        }

        [HttpPost]
        public IActionResult AddEdit(UserMasterModel model)
        {
            UserMasterModel.lstStatus = _commonService.GetStatusList();
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
                    else
                    {
                        model.IsDelete = true;
                    }

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
                    model.UpdatedBy = SclinicId;
                    model.Clinic_Id = SclinicId;
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
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            int SclinicId = (int)HttpContext.Session.GetInt32(SessionHelper.SessionClinicID);
            var ClinicIDWiseList = _UserMasterServices.GetByClinicIdWiseUserList(SclinicId);

            for (int i = 0; i < ClinicIDWiseList.Count; i++)
            {
                var data = _UserMasterServices.GetById(ClinicIDWiseList[i].Id);
                if (data != null)
                {
                    if (model.Password == data.Password)
                    {
                        if (model.NewPassowrd == model.ConfimPassword)
                        {
                            string password = _commonService.GetMd5HashNewMethod(model.NewPassowrd);

                            UserMasterModel user = new UserMasterModel();
                            user.Id = data.Id;
                            user.Password = password;
                            user.UpdatedBy = SclinicId;
                            var res = _UserMasterServices.UpdatePassword(user);
                            if (res.DbCode == 1)
                            {
                                TempData[Temp_Message.Success] = res.DbMsg;
                                return RedirectToAction("Index", "ClinicDashboard");
                            }
                            else
                            {
                                TempData[Temp_Message.Error] = res.DbMsg;
                            }
                        }
                        else
                        {
                            TempData[Temp_Message.Error] = "NewPassword & ConfimPassword Not Match";
                        }
                    }
                    else
                    {
                        TempData[Temp_Message.Error] = "Password Not Match";
                    }
                }
            }
            return View(ClinicIDWiseList);
        }

        public JsonResult GetChargesByUserId(int User_Id)
        {
            var data = _UserMasterServices.GetById(User_Id);

            return Json(data);
        }
        public JsonResult GetBYDepartmentIdWiseUserData(int Department_Id)
        {
            var data = _commonService.GetUserDepartmentList(Department_Id);
            return Json(data);
        }
    }

}


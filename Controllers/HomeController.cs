using HMS.Common;
using HMS.Interface;
using HMS.Models;
using HMS.Security;
using HMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public SessionManager sessionManager = null;
        private readonly IConfiguration config;
        private readonly ISession _session = null;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserMasterServices _userMasterServices;
        private readonly ICommonService _commonService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor, IUserMasterServices userMasterServices, ICommonService commonService)
        {
            _logger = logger;
            sessionManager = new SessionManager(httpContextAccessor);
            config = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userMasterServices = userMasterServices;
            _session = _httpContextAccessor.HttpContext.Session;
            _commonService = commonService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {

            UserMasterModel model = new UserMasterModel();
            if (Request.Cookies[".AspNetCore.Session"] != null)
            {
                Response.Cookies.Delete(".AspNetCore.Session");
            }

            if (Request.Cookies["AuthenticationToken"] != null)
            {
                Response.Cookies.Delete("AuthenticationToken");

            }
            HttpContext.Session.Clear();
            sessionManager.ClearUserSession();
            try
            {
                if (HttpContext.Request.Cookies.Keys.Contains(".AspNetCore.Antiforgery.Fac3F_fAMYA"))
                {
                    return RedirectToAction("Login", "Account");
                }
                TempData.Clear();
            }
            catch { }
            ViewBag.PageURL = config.GetSection("PageURL").Value;

            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {

            if (ModelState.IsValid)
            {

                var Userdata = _userMasterServices.GetByEmail(model.Email);

                if (Userdata != null)
                {
                    //Set Session
                    HttpContext.Session.SetString(SessionHelper.SessionEmailId, Userdata.Email);
                    HttpContext.Session.SetInt32(SessionHelper.SessionClinicID, (int)Userdata.Clinic_Id);
                    HttpContext.Session.SetInt32(SessionHelper.SessionUserId, Userdata.Id);
                    HttpContext.Session.SetString(SessionHelper.SessionUserName, Userdata.Email);

                    var consulttypeId = _commonService.GetConsultantId(Userdata.Clinic_Id.Value);
                    var ReceptionistId = _commonService.GetReceptionistId(Userdata.Clinic_Id.Value);

                    string password = _commonService.GetMd5HashNewMethod(model.Password);

                    if (Userdata.Password == password)
                    {
                        if (Userdata.FirstName == "SuperAdmin" || Userdata.FirstName == "Admin")
                        {

                            return RedirectToAction("Index", "Dashboard");
                        }
                        else if (consulttypeId == Userdata.Desig_Id)
                        {
                            return RedirectToAction("Index", "ConsultantDashboard");
                        }
                        else if (ReceptionistId == Userdata.Desig_Id)
                        {
                            return RedirectToAction("Index", "ReceptionistDashboard");
                        }
                        else
                        {
                            return RedirectToAction("Index", "ClinicDashboard");
                        }

                    }
                    else
                    {
                        TempData[Temp_Message.Error] = "Invalid Password.";
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    TempData[Temp_Message.Error] = "Please Enter Correct Email";
                }

            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();

            }
            UserMasterModel userLoginModel = new UserMasterModel()
            {
                Email = model.Email,
                Password = model.Password
            };
            return View(userLoginModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult LogOut()
        {
            return RedirectToAction("Login");
        }
    }
}

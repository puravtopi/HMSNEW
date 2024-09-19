using HMS.Interface;
using System.Collections.Generic;
using System;
using HMS.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Security.Cryptography;
using HMS.Models;
using System.Data;
using System.Linq;
using Dapper;


namespace HMS.Services
{
    public class CommonService : ICommonService
    {
        private readonly IDapperService _dapper;
        private readonly IDesignationMasterServices _designationMasterServices;
        private readonly IDepartmentMasterServices _departmentMasterServices;
        private readonly IConsultantMasterServices _consultantMasterServices;
        private readonly IUserMasterServices _userMasterServices;
        private readonly IClinicMasterServices _clinicMasterServices;
        private readonly IServiceHeadMasterService _serviceHeadServices;
        private readonly IServiceMasterService _serviceMasterService;
        public CommonService(IDapperService dapper, IDesignationMasterServices designationMasterServices,
            IDepartmentMasterServices departmentMasterServices, IConsultantMasterServices consultantMasterServices,
            IClinicMasterServices clinicMasterServices,
            IUserMasterServices userMasterServices, IServiceHeadMasterService serviceHeadServices, IServiceMasterService serviceMasterService)
        {
            _dapper = dapper;
            _designationMasterServices = designationMasterServices;
            _departmentMasterServices = departmentMasterServices;
            _consultantMasterServices = consultantMasterServices;
            _userMasterServices = userMasterServices;
            _clinicMasterServices = clinicMasterServices;
            _serviceHeadServices = serviceHeadServices;
            _serviceMasterService = serviceMasterService;
        }
        public List<PageSizeDdl> GetPageSizeDDL()
        {
            try
            {
                List<PageSizeDdl> listItems = new List<PageSizeDdl>();
                listItems.Add(new PageSizeDdl { PageSizeId = 10, PageSizeValue = "10" });
                listItems.Add(new PageSizeDdl { PageSizeId = 20, PageSizeValue = "20" });
                listItems.Add(new PageSizeDdl { PageSizeId = 30, PageSizeValue = "30" });
                listItems.Add(new PageSizeDdl { PageSizeId = 40, PageSizeValue = "40" });
                listItems.Add(new PageSizeDdl { PageSizeId = 50, PageSizeValue = "50" });
                return listItems;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GenerateReciptNo()
        {
            int _min = 10000;
            int _max = 99999;
            Random r = new Random();
            int num = r.Next(_min, _max);
            return "RCP" + num.ToString();
        }

        public List<SelectListItem> GetStatusList()
        {
            try
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem { Value = "True", Text = "Yes" });
                listItems.Add(new SelectListItem { Value = "False", Text = "No" });

                //listItems.Insert(0, new SelectListItem() { Value = null, Text = "Select" });
                return listItems;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetDesignationList(int ClinicId)
        {

            var res = _designationMasterServices.GetByClinicIdWiseDesigList(ClinicId);
            var designationlist = new List<SelectListItem>();
            //designationlist.Add(new SelectListItem
            //{
            //    Text = "Select Designation"
            //});
            foreach (var item in res)
            {
                designationlist.Add(new SelectListItem
                {
                    Text = item.DesignationName,
                    Value = item.Id.ToString()
                });
            }
            return designationlist;
        }

        public List<SelectListItem> GetDepartmentList(int ClinicId)
        {
            var res = _departmentMasterServices.GetByClinicIdWiseDeptList(ClinicId);
            var departmentList = new List<SelectListItem>();
            //departmentList.Add(new SelectListItem
            //{
            //    Text = "Select",
            //    Value = "0"
            //});
            foreach (var item in res)
            {

                departmentList.Add(new SelectListItem
                {
                    Text = item.DepartmentName,
                    Value = item.Id.ToString()
                });
            }
            return departmentList;
        }

        public List<SelectListItem> GetConsultantDept(int ClinicId) 
        {
            var res = _departmentMasterServices.GetConsultantWiseDept(ClinicId);
            var departmentList = new List<SelectListItem>();
            departmentList.Add(new SelectListItem
            {
                Text = "Select",
                Value = "0"
            });
            foreach (var item in res)
            {

                departmentList.Add(new SelectListItem
                {
                    Text = item.DepartmentName,
                    Value = item.Id.ToString()
                });
            }
            return departmentList;
        }

        public List<SelectListItem> GetConsultantList(int Department_Id)
        {
            var res = _consultantMasterServices.GetBYDepartmentIdWiseConsultant(Department_Id);
            var lst = new List<SelectListItem>();

            foreach (var item in res)
            {

                lst.Add(new SelectListItem
                {
                    Text = item.ConsultantName,
                    Value = item.Id.ToString()
                });
            }
            return lst;
        }

        public List<SelectListItem> GetUserDepartmentList(int Department_Id)
        {
            var res = _userMasterServices.GetByDepartmentIdWiseUserList(Department_Id);
            var lst = new List<SelectListItem>();

            foreach (var item in res)
            {
                var firstname = item.FirstName != null ? item.FirstName.ToString() : " ";
                var lastname = item.LastName != null ? item.LastName.ToString() : "";
                lst.Add(new SelectListItem
                {
                    Text = firstname + " " + lastname,
                    Value = item.Id.ToString()
                });
            }
            return lst;
        }

        public List<SelectListItem> GetMaritalStatusList()
        {
            try
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem { Text = "Select" });
                listItems.Add(new SelectListItem { Value = "Married", Text = "Married" });
                listItems.Add(new SelectListItem { Value = "UnMarried", Text = "UnMarried" });

                //listItems.Insert(0, new SelectListItem() { Value = null, Text = "Select" });
                return listItems;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetGenderList()
        {
            try
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem { Text = "Select" });
                listItems.Add(new SelectListItem { Value = "Male", Text = "Male" });
                listItems.Add(new SelectListItem { Value = "Female", Text = "Female" });

                //listItems.Insert(0, new SelectListItem() { Value = null, Text = "Select" });
                return listItems;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetBloodgroupList()
        {
            try
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem { Text = "Select" });
                listItems.Add(new SelectListItem { Value = "A+", Text = "A+" });
                listItems.Add(new SelectListItem { Value = "B+", Text = "B+" });
                listItems.Add(new SelectListItem { Value = "O+", Text = "O+" });
                listItems.Add(new SelectListItem { Value = "AB+", Text = "AB+" });
                listItems.Add(new SelectListItem { Value = "A-", Text = "A-" });
                listItems.Add(new SelectListItem { Value = "B-", Text = "B-" });
                listItems.Add(new SelectListItem { Value = "O-", Text = "O-" });
                listItems.Add(new SelectListItem { Value = "AB-", Text = "AB-" });

                //listItems.Insert(0, new SelectListItem() { Value = null, Text = "Select" });
                return listItems;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetMd5HashNewMethod(string password)
        {
            try
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] encrypt;
                UTF8Encoding encode = new UTF8Encoding();
                //encrypt the given password string into Encrypted data  
                encrypt = md5.ComputeHash(encode.GetBytes(password));
                StringBuilder encryptdata = new StringBuilder();
                //Create a new string by using the encrypted data  
                for (int i = 0; i < encrypt.Length; i++)
                {
                    encryptdata.Append(encrypt[i].ToString());
                }
                return encryptdata.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetPrefix(int IdString)
        {
            string returnString = string.Empty;
            try
            {
                if (IdString >= 0 && IdString < 9)
                {
                    returnString = "0000" + IdString;
                }
                else if (IdString >= 10 && IdString < 99)
                {
                    returnString = "000" + IdString;
                }
                else if (IdString >= 100 && IdString < 999)
                {
                    returnString = "00" + IdString;
                }
                else if (IdString >= 1000 && IdString < 9999)
                {
                    returnString = "0" + IdString;
                }
                return returnString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> GetPaymentModeList()
        {
            try
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem { Text = "Select" });
                listItems.Add(new SelectListItem { Value = "Cash", Text = "Cash" });
                listItems.Add(new SelectListItem { Value = "Debit Card", Text = "Debit Card" });
                listItems.Add(new SelectListItem { Value = "Credit Card", Text = "Credit Card" });
                listItems.Add(new SelectListItem { Value = "UPI", Text = "UPI" });
                listItems.Add(new SelectListItem { Value = "Due", Text = "Due" });

                //listItems.Insert(0, new SelectListItem() { Value = null, Text = "Select" });
                return listItems;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetConsultantId(int ClinicId)
        {
            var res = _clinicMasterServices.GetConsultantIdById(ClinicId);
            return res;
        }
        public int GetReceptionistId(int ClinicId)
        {
            var res = _clinicMasterServices.GetReceptionistIdById(ClinicId);
            return res;
        }

        public List<SelectListItem> GetDesignationListById(int Desig_Id)
        {

            var res = _userMasterServices.GetByDesignationIdWiseUserList(Desig_Id);
            var designationlist = new List<SelectListItem>();
            //designationlist.Add(new SelectListItem
            //{
            //    Text = "Select Designation"
            //});
            foreach (var item in res)
            {
                designationlist.Add(new SelectListItem
                {
                    Text = item.FirstName+"  "+ item.LastName,
                    Value = item.Id.ToString()
                });
            }
            return designationlist;
        }

        public List<SelectListItem> GetDepartmentwiseServiceheadList(int Department_Id)
        {
            var res = _serviceHeadServices.GetByDepartmentIdWiseServiceList(Department_Id);
            var serviceHeadList = new List<SelectListItem>();            
            foreach (var item in res)
            {
                serviceHeadList.Add(new SelectListItem
                {
                    Text = item.ServiceHeadName,
                    Value = item.Id.ToString()
                });
            }
            return serviceHeadList;
        }

        public List<SelectListItem> GetServiceHeadwiseServiceList(int ServiceHead_Id)
        {
            var res = _serviceMasterService.GetByServiceHeadIdWiseServiceList(ServiceHead_Id);
            var serviceHeadList = new List<SelectListItem>();
            foreach (var item in res)
            {
                serviceHeadList.Add(new SelectListItem
                {
                    Text = item.ServiceName,
                    Value = item.Id.ToString()
                });
            }
            return serviceHeadList;
        }
        public List<SelectListItem> GetState()
        {
            var states = _departmentMasterServices.GetState();
            return states.Select(state => new SelectListItem
            {
                Text = state.Name,
                Value = state.Id.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetCitiesByStateId(int stateId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StateId", stateId);

            var city = _departmentMasterServices.GetCityWiseState(stateId);
            return city.Select(city => new SelectListItem
            {
                Text = city.Name,
                Value = city.Id.ToString()
            }).ToList();
        }
    }
}

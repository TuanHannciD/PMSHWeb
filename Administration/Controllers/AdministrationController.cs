using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Administration.Services.Implements;
using Administration.Services.Interfaces;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace Administration.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdministrationController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IAdministrationService _iAdministrationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdministrationController(ILogger<AdministrationController> logger,
                IMemoryCache cache, IConfiguration configuration, IAdministrationService iAdministrationService, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _iAdministrationService = iAdministrationService;
            _httpContextAccessor = httpContextAccessor;

        }
        public IActionResult Index()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpGet]

        public IActionResult GetMemberList(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.MemberList(code, name, inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  InactiveText = !string.IsNullOrEmpty(d["InactiveText"].ToString()) ? d["InactiveText"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdatedDate = !string.IsNullOrEmpty(d["UpdatedDate"].ToString()) ? d["UpdatedDate"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            //  report.DataSource = dataTable;

            // Không cần gán parameter
            // report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }
        public IActionResult MemberList()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpPost]
        public ActionResult InsertMember()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                MemberTypeModel member = new MemberTypeModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";

                // Thông tin người dùng
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = MemberTypeBO.Instance.Insert(member);

                pt.CommitTransaction();

                return Json(new { success = true, id = memberId });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult UpdateMember()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                MemberTypeModel member = new MemberTypeModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";

                // Thông tin người dùng
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.CreatedDate = DateTime.Now;
                member.UpdatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedDate = DateTime.Now;

                
               MemberTypeBO.Instance.Update(member);

                pt.CommitTransaction();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpGet]
        public IActionResult GetMemberDescription(int id)
        {
            try
            {
                string desc = MemberTypeBO.Instance.GetDescriptionById(id);

                return Json(new
                {
                    success = true,
                    description = desc ?? ""
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult DeleteMember()
        {
            try
            {

                MemberTypeModel memberModel = (MemberTypeModel)MemberTypeBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                MemberTypeBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetMemberCategory(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.MemberCategory(code, name, inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  InactiveText = !string.IsNullOrEmpty(d["InactiveText"].ToString()) ? d["InactiveText"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdatedDate = !string.IsNullOrEmpty(d["UpdatedDate"].ToString()) ? d["UpdatedDate"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            //  report.DataSource = dataTable;

            // Không cần gán parameter
            // report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }
        public IActionResult MemberCategory()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpPost]
        public ActionResult InsertMemberCategory()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                MemberCategoryModel member = new MemberCategoryModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                //member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";

                // Thông tin người dùng
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = MemberCategoryBO.Instance.Insert(member);

                pt.CommitTransaction();

                return Json(new { success = true, id = memberId });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult UpdateMemberCategory()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                MemberCategoryModel member = new MemberCategoryModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                //member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";

                // Thông tin người dùng
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.CreatedDate = DateTime.Now;
                member.UpdatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedDate = DateTime.Now;


                MemberCategoryBO.Instance.Update(member);

                pt.CommitTransaction();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult DeleteMemberCategory()
        {
            try
            {

                MemberCategoryModel memberModel = (MemberCategoryModel)MemberCategoryBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                MemberCategoryBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetCity(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.City(code, name, inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  Country = !string.IsNullOrEmpty(d["Country"].ToString()) ? d["Country"] : "",
                                  InactiveText = !string.IsNullOrEmpty(d["InactiveText"].ToString()) ? d["InactiveText"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdatedDate = !string.IsNullOrEmpty(d["UpdatedDate"].ToString()) ? d["UpdatedDate"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            //  report.DataSource = dataTable;

            // Không cần gán parameter
            // report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }
        public IActionResult City()
        {
            List<CountryModel> listctry = PropertyUtils.ConvertToList<CountryModel>(CountryBO.Instance.FindAll());
            ViewBag.CountryList = listctry;
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpPost]
        public ActionResult InsertCity()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                CityModel member = new CityModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                string countryValue = Request.Form["countryId"];
                member.CountryID = int.TryParse(countryValue, out int cId) ? cId : 0;

                // Thông tin người dùng
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = CityBO.Instance.Insert(member);

                pt.CommitTransaction();

                return Json(new { success = true, id = memberId });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult UpdateCity()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                CityModel member = new CityModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                string countryValue = Request.Form["countryId"];
                member.CountryID = int.TryParse(countryValue, out int cId) ? cId : 0;
                // Thông tin người dùng
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.CreatedDate = DateTime.Now;
                member.UpdatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedDate = DateTime.Now;


                CityBO.Instance.Update(member);

                pt.CommitTransaction();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult DeleteCity()
        {
            try
            {

                CityModel memberModel = (CityModel)CityBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                CityBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetCountry(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Country(code, name, inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  InactiveText = !string.IsNullOrEmpty(d["InactiveText"].ToString()) ? d["InactiveText"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdatedDate = !string.IsNullOrEmpty(d["UpdatedDate"].ToString()) ? d["UpdatedDate"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            //  report.DataSource = dataTable;

            // Không cần gán parameter
            // report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }
        public IActionResult Country()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpPost]
        public ActionResult InsertCountry()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                CountryModel member = new CountryModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = CountryBO.Instance.Insert(member);

                pt.CommitTransaction();

                return Json(new { success = true, id = memberId });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult UpdateCountry()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                CountryModel member = new CountryModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.CreatedDate = DateTime.Now;
                member.UpdatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedDate = DateTime.Now;


                CountryBO.Instance.Update(member);

                pt.CommitTransaction();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult DeleteCountry()
        {
            try
            {

                CountryModel memberModel = (CountryModel)CountryBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Country" });

                }
                CountryBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Country was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetLanguage(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Language(code, name, inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  InactiveText = !string.IsNullOrEmpty(d["InactiveText"].ToString()) ? d["InactiveText"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdatedDate = !string.IsNullOrEmpty(d["UpdatedDate"].ToString()) ? d["UpdatedDate"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            //  report.DataSource = dataTable;

            // Không cần gán parameter
            // report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }
        public IActionResult Language()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpPost]
        public ActionResult InsertLanguage()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                LanguageModel member = new LanguageModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = LanguageBO.Instance.Insert(member);

                pt.CommitTransaction();

                return Json(new { success = true, id = memberId });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult UpdateLanguage()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                LanguageModel member = new LanguageModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.CreatedDate = DateTime.Now;
                member.UpdatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedDate = DateTime.Now;


                LanguageBO.Instance.Update(member);

                pt.CommitTransaction();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult DeleteLanguage()
        {
            try
            {

                LanguageModel memberModel = (LanguageModel)LanguageBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Country" });

                }
                LanguageBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Country was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetNationality(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Nationality(code, name, inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  InactiveText = !string.IsNullOrEmpty(d["InactiveText"].ToString()) ? d["InactiveText"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdatedDate = !string.IsNullOrEmpty(d["UpdatedDate"].ToString()) ? d["UpdatedDate"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            //  report.DataSource = dataTable;

            // Không cần gán parameter
            // report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }
        public IActionResult Nationality()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpPost]
        public ActionResult InsertNationality()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                NationalityModel member = new NationalityModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = NationalityBO.Instance.Insert(member);

                pt.CommitTransaction();

                return Json(new { success = true, id = memberId });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult UpdateNationality()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                NationalityModel member = new NationalityModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.CreatedDate = DateTime.Now;
                member.UpdatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedDate = DateTime.Now;


                NationalityBO.Instance.Update(member);

                pt.CommitTransaction();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult DeleteNationality()
        {
            try
            {

                NationalityModel memberModel = (NationalityModel)NationalityBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Country" });

                }
                NationalityBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Country was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetTitle(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Title(code, name, inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  InactiveText = !string.IsNullOrEmpty(d["InactiveText"].ToString()) ? d["InactiveText"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdatedDate = !string.IsNullOrEmpty(d["UpdatedDate"].ToString()) ? d["UpdatedDate"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            //  report.DataSource = dataTable;

            // Không cần gán parameter
            // report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }
        public IActionResult Title()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpPost]
        public ActionResult InsertTitle()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                TitleModel member = new TitleModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = TitleBO.Instance.Insert(member);

                pt.CommitTransaction();

                return Json(new { success = true, id = memberId });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult UpdateTitle()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                TitleModel member = new TitleModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.CreatedDate = DateTime.Now;
                member.UpdatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedDate = DateTime.Now;


                TitleBO.Instance.Update(member);

                pt.CommitTransaction();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult DeleteTitle()
        {
            try
            {

                TitleModel memberModel = (TitleModel)TitleBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Country" });

                }
                TitleBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Country was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetTerritory(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Territory(code, name, inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  InactiveText = !string.IsNullOrEmpty(d["InactiveText"].ToString()) ? d["InactiveText"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdatedDate = !string.IsNullOrEmpty(d["UpdatedDate"].ToString()) ? d["UpdatedDate"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            //  report.DataSource = dataTable;

            // Không cần gán parameter
            // report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }
        public IActionResult Territory()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpPost]
        public ActionResult InsertTerritory()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                TerritoryModel member = new TerritoryModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = TerritoryBO.Instance.Insert(member);

                pt.CommitTransaction();

                return Json(new { success = true, id = memberId });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult UpdateTerritory()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                TerritoryModel member = new TerritoryModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.CreatedDate = DateTime.Now;
                member.UpdatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedDate = DateTime.Now;


                TerritoryBO.Instance.Update(member);

                pt.CommitTransaction();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult DeleteTerritory()
        {
            try
            {

                TerritoryModel memberModel = (TerritoryModel)TerritoryBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Country" });

                }
                TerritoryBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Country was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetState(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.State(code, name, inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["ZipCode"].ToString()) ? d["ZipCode"] : "",
                                  Name = !string.IsNullOrEmpty(d["StateName"].ToString()) ? d["StateName"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  InactiveText = !string.IsNullOrEmpty(d["InactiveText"].ToString()) ? d["InactiveText"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdatedDate = !string.IsNullOrEmpty(d["UpdatedDate"].ToString()) ? d["UpdatedDate"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            //  report.DataSource = dataTable;

            // Không cần gán parameter
            // report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }
        public IActionResult State()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpPost]
        public ActionResult InsertState()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                StateModel member = new StateModel();

                // Lấy dữ liệu từ form
                member.ZipCode = Request.Form["txtcode"].ToString();
                member.StateName = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = StateBO.Instance.Insert(member);

                pt.CommitTransaction();

                return Json(new { success = true, id = memberId });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult UpdateState()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                StateModel member = new StateModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.ZipCode = Request.Form["txtcode"].ToString();
                member.StateName = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.CreatedDate = DateTime.Now;
                member.UpdatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedDate = DateTime.Now;


                StateBO.Instance.Update(member);

                pt.CommitTransaction();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult DeleteState()
        {
            try
            {

                StateModel memberModel = (StateModel)StateBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Country" });

                }
                StateBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Country was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetVIP(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.VIP(code, name, inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  InactiveText = !string.IsNullOrEmpty(d["InactiveText"].ToString()) ? d["InactiveText"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdatedDate = !string.IsNullOrEmpty(d["UpdatedDate"].ToString()) ? d["UpdatedDate"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            //  report.DataSource = dataTable;

            // Không cần gán parameter
            // report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }
        public IActionResult VIP()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpPost]
        public ActionResult InsertVIP()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                VIPModel member = new VIPModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = VIPBO.Instance.Insert(member);

                pt.CommitTransaction();

                return Json(new { success = true, id = memberId });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult UpdateVIP()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                VIPModel member = new VIPModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.CreatedDate = DateTime.Now;
                member.UpdatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedDate = DateTime.Now;


                VIPBO.Instance.Update(member);

                pt.CommitTransaction();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult DeleteVIP()
        {
            try
            {

                VIPModel memberModel = (VIPModel)VIPBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Country" });

                }
                VIPBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Country was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetMarket(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Market(code, name, inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",                               
                                  InactiveText = !string.IsNullOrEmpty(d["InactiveText"].ToString()) ? d["InactiveText"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdatedDate = !string.IsNullOrEmpty(d["UpdatedDate"].ToString()) ? d["UpdatedDate"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            //  report.DataSource = dataTable;

            // Không cần gán parameter
            // report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var market = MarketBO.Instance.GetById(id);
            if (market == null)
                return NotFound();

            return Json(market); // trả JSON ra cho Ajax
        }
        public IActionResult Market()
        {
            List<MarketTypeModel> listmktype = PropertyUtils.ConvertToList<MarketTypeModel>(MarketTypeBO.Instance.FindAll());
            ViewBag.MarketTypeList = listmktype;
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertMarket()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                MarketModel member = new MarketModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";

                string marketTypeValue = Request.Form["marketTypeID"];
                member.MarketTypeID = int.TryParse(marketTypeValue, out int mId) ? mId : 0;

                string groupTypeValue = Request.Form["groupType"];
                member.GroupType = int.TryParse(groupTypeValue, out int sId) ? sId : 0;

                member.Regional = Request.Form["txtregional"].ToString();
                // Thông tin người dùng
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = MarketBO.Instance.Insert(member);

                pt.CommitTransaction();

                return Json(new { success = true, id = memberId });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult UpdateMarket()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                MarketModel member = new MarketModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                string marketTypeValue = Request.Form["marketTypeID"];
                member.MarketTypeID = int.TryParse(marketTypeValue, out int mId) ? mId : 0;

                string groupTypeValue = Request.Form["groupType"];
                member.GroupType = int.TryParse(groupTypeValue, out int sId) ? sId : 0;
                member.Regional = Request.Form["txtregional"].ToString();
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.CreatedDate = DateTime.Now;
                member.UpdatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedDate = DateTime.Now;


                MarketBO.Instance.Update(member);

                pt.CommitTransaction();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult DeleteMarket()
        {
            try
            {

                MarketModel memberModel = (MarketModel)MarketBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                MarketBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        
    }
}

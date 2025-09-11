using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Administration.Services.Implements;
using Administration.Services.Interfaces;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using DevExpress.CodeParser;
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

                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    MemberTypeBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = MemberTypeBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    MemberTypeBO.Instance.Update(member);
                }

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

                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    MemberCategoryBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = MemberCategoryBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    MemberCategoryBO.Instance.Update(member);
                }

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    CityBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = CityBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    CityBO.Instance.Update(member);
                }

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    CountryBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = CountryBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    CountryBO.Instance.Update(member);
                }

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    LanguageBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = LanguageBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    LanguageBO.Instance.Update(member);
                }

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    NationalityBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = NationalityBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    NationalityBO.Instance.Update(member);
                }

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    TitleBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = TitleBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    TitleBO.Instance.Update(member);
                }

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    TerritoryBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = TerritoryBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    TerritoryBO.Instance.Update(member);
                }

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    StateBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = StateBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    StateBO.Instance.Update(member);
                }

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    VIPBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = VIPBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    VIPBO.Instance.Update(member);
                }

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    MarketBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = MarketBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    MarketBO.Instance.Update(member);
                }

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
        [HttpGet]
        public IActionResult GetMarketType(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.MarketType(code, name, inactive);
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
      
        public IActionResult MarketType()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertMarketType()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                MarketTypeModel member = new MarketTypeModel();

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
                long memberId = MarketTypeBO.Instance.Insert(member);

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
        public ActionResult UpdateMarketType()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                MarketTypeModel member = new MarketTypeModel();

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    MarketTypeBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = MarketTypeBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    MarketTypeBO.Instance.Update(member);
                }

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
        public ActionResult DeleteMarketType()
        {
            try
            {

                MarketTypeModel memberModel = (MarketTypeModel)MarketTypeBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                MarketTypeBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetPickupDropPlace(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.PickupDropPlace(code, name, inactive);
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

        public IActionResult PickupDropPlace()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertPickupDropPlace()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                PickupDropPlaceModel member = new PickupDropPlaceModel();

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
                long memberId = PickupDropPlaceBO.Instance.Insert(member);

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
        public ActionResult UpdatePickupDropPlace()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                PickupDropPlaceModel member = new PickupDropPlaceModel();

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    PickupDropPlaceBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = PickupDropPlaceBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    PickupDropPlaceBO.Instance.Update(member);
                }

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
        public ActionResult DeletePickupDropPlace()
        {
            try
            {

                PickupDropPlaceModel memberModel = (PickupDropPlaceModel)PickupDropPlaceBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                PickupDropPlaceBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetTransportType(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.TransportType(code, name, inactive);
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

        public IActionResult TransportType()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertTransportType()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                TransportTypeModel member = new TransportTypeModel();

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
                long memberId = TransportTypeBO.Instance.Insert(member);

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
        public ActionResult UpdateTransportType()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                TransportTypeModel member = new TransportTypeModel();

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    TransportTypeBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = TransportTypeBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    TransportTypeBO.Instance.Update(member);
                }

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
        public ActionResult DeleteTransportType()
        {
            try
            {

                TransportTypeModel memberModel = (TransportTypeModel)TransportTypeBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                TransportTypeBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetReason(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Reason(code, name, inactive);
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

        public IActionResult Reason()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertReason()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                ReasonModel member = new ReasonModel();

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
                long memberId = ReasonBO.Instance.Insert(member);

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
        public ActionResult UpdateReason()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                ReasonModel member = new ReasonModel();

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    ReasonBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = ReasonBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    ReasonBO.Instance.Update(member);
                }

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
        public ActionResult DeleteReason()
        {
            try
            {

                ReasonModel memberModel = (ReasonModel)ReasonBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                ReasonBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetOrigin(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Origin(code, name, inactive);
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

        public IActionResult Origin()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertOrigin()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                OriginModel member = new OriginModel();

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
                long memberId = OriginBO.Instance.Insert(member);

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
        public ActionResult UpdateOrigin()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                OriginModel member = new OriginModel();

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    OriginBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = OriginBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    OriginBO.Instance.Update(member);
                }

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
        public ActionResult DeleteOrigin()
        {
            try
            {

                OriginModel memberModel = (OriginModel)OriginBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                OriginBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetSource(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Source(code, name, inactive);
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

        public IActionResult Source()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertSource()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                SourceModel member = new SourceModel();

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
                long memberId = SourceBO.Instance.Insert(member);

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
        public ActionResult UpdateSource()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                SourceModel member = new SourceModel();

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
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    SourceBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = SourceBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    SourceBO.Instance.Update(member);
                }

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
        public ActionResult DeleteSource()
        {
            try
            {

                SourceModel memberModel = (SourceModel)SourceBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                SourceBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }


        }
        [HttpGet]
        public IActionResult GetAlertsSetup(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.AlertsSetup(code, name, inactive);
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

        public IActionResult AlertsSetup()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertAlertsSetup()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                AlertsSetupModel member = new AlertsSetupModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                // Thông tin người dùng
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = AlertsSetupBO.Instance.Insert(member);

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
        public ActionResult UpdateAlertsSetup()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                AlertsSetupModel member = new AlertsSetupModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    AlertsSetupBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = AlertsSetupBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    AlertsSetupBO.Instance.Update(member);
                }

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
        public ActionResult DeleteAlertsSetup()
        {
            try
            {

                AlertsSetupModel memberModel = (AlertsSetupModel)AlertsSetupBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                AlertsSetupBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }


        }
        [HttpGet]
        public IActionResult GetComment(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Comment(code, name, inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  CommentType = !string.IsNullOrEmpty(d["CommentType"].ToString()) ? d["CommentType"] : "",
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
        public IActionResult Comment()
        {
            List<CommentTypeModel> listctry = PropertyUtils.ConvertToList<CommentTypeModel>(CommentTypeBO.Instance.FindAll());
            ViewBag.CommentTypeList = listctry;
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpPost]
        public ActionResult InsertComment()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                CommentModel member = new CommentModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                string commentValue = Request.Form["commentTypeID"];
                member.CommentTypeID = int.TryParse(commentValue, out int cId) ? cId : 0;

                // Thông tin người dùng
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;

                // Gọi BO để lưu
                long memberId = CommentBO.Instance.Insert(member);

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
        public ActionResult UpdateComment()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                CommentModel member = new CommentModel();

                // Lấy ID từ form (có khi edit)
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                  && Request.Form["inactive"].ToString() == "on";
                string commentValue = Request.Form["commentTypeID"];
                member.CommentTypeID = int.TryParse(commentValue, out int cId) ? cId : 0;
                // Thông tin người dùng
                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    CommentBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = CommentBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    CommentBO.Instance.Update(member);
                }

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
        public ActionResult DeleteComment()
        {
            try
            {

                CommentModel memberModel = (CommentModel)CommentBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                CommentBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetCommentType(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.CommentType(code, name, inactive);
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

        public IActionResult CommentType()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertCommentType()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                CommentTypeModel member = new CommentTypeModel();

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
                long memberId = CommentTypeBO.Instance.Insert(member);

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
        public ActionResult UpdateCommentType()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                CommentTypeModel member = new CommentTypeModel();

                // Lấy ID từ form
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                   && Request.Form["inactive"].ToString() == "on";

                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    CommentTypeBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = CommentTypeBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    CommentTypeBO.Instance.Update(member);
                }

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
        public ActionResult DeleteCommentType()
        {
            try
            {

                CommentTypeModel memberModel = (CommentTypeModel)CommentTypeBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                CommentTypeBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetSeason(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Season(code, name, inactive);
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

        public IActionResult Season()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertSeason()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                SeasonModel member = new SeasonModel();

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
                long memberId = SeasonBO.Instance.Insert(member);

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
        public ActionResult UpdateSeason()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                SeasonModel member = new SeasonModel();

                // Lấy ID từ form
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                   && Request.Form["inactive"].ToString() == "on";

                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    SeasonBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = SeasonBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    SeasonBO.Instance.Update(member);
                }

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
        public ActionResult DeleteSeason()
        {
            try
            {

                SeasonModel memberModel = (SeasonModel)SeasonBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                SeasonBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetZone(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Zone(code, name, inactive);
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

        public IActionResult Zone()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertZone()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                ZoneModel member = new ZoneModel();

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
                long memberId = ZoneBO.Instance.Insert(member);

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
        public ActionResult UpdateZone()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                ZoneModel member = new ZoneModel();

                // Lấy ID từ form
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                   && Request.Form["inactive"].ToString() == "on";

                string loginName = HttpContext.Session.GetString("LoginName") ?? "";

                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    ZoneBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = ZoneBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    ZoneBO.Instance.Update(member);
                }

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
        public ActionResult DeleteZone()
        {
            try
            {

                ZoneModel memberModel = (ZoneModel)ZoneBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                ZoneBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetDepartment(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Department(code, name, inactive);
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

        public IActionResult Department()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertDepartment()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                DepartmentModel member = new DepartmentModel();

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
                if (string.IsNullOrWhiteSpace(member.Code))
                    return Json(new { success = false, message = "Code không được để trống." });

                if (string.IsNullOrWhiteSpace(member.Name))
                    return Json(new { success = false, message = "Name không được để trống." });
                // Gọi BO để lưu
                long memberId = DepartmentBO.Instance.Insert(member);

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
        public ActionResult UpdateDepartment()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                DepartmentModel member = new DepartmentModel();

                // Lấy ID từ form
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                   && Request.Form["inactive"].ToString() == "on";

                string loginName = HttpContext.Session.GetString("LoginName") ?? "";
                if (string.IsNullOrWhiteSpace(member.Code))
                    return Json(new { success = false, message = "Code không được để trống." });

                if (string.IsNullOrWhiteSpace(member.Name))
                    return Json(new { success = false, message = "Name không được để trống." });
                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    DepartmentBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = DepartmentBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    DepartmentBO.Instance.Update(member);
                }

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
        public ActionResult DeleteDepartment()
        {
            try
            {

                DepartmentModel memberModel = (DepartmentModel)DepartmentBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                DepartmentBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetOwner(string code, string name, int inactive)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.Owner(code, name, inactive);
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

        public IActionResult Owner()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertOwner()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                OwnerModel member = new OwnerModel();

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
                if (string.IsNullOrWhiteSpace(member.Code))
                    return Json(new { success = false, message = "Code không được để trống." });

                if (string.IsNullOrWhiteSpace(member.Name))
                    return Json(new { success = false, message = "Name không được để trống." });
                // Gọi BO để lưu
                long memberId = OwnerBO.Instance.Insert(member);

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
        public ActionResult UpdateOwner()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                OwnerModel member = new OwnerModel();

                // Lấy ID từ form
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["txtcode"].ToString();
                member.Name = Request.Form["txtname"].ToString();
                member.Description = Request.Form["txtdescription"].ToString();
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                   && Request.Form["inactive"].ToString() == "on";

                string loginName = HttpContext.Session.GetString("LoginName") ?? "";
                if (string.IsNullOrWhiteSpace(member.Code))
                    return Json(new { success = false, message = "Code không được để trống." });

                if (string.IsNullOrWhiteSpace(member.Name))
                    return Json(new { success = false, message = "Name không được để trống." });
                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    OwnerBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = OwnerBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    OwnerBO.Instance.Update(member);
                }

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
        public ActionResult DeleteOwner()
        {
            try
            {

                OwnerModel memberModel = (OwnerModel)OwnerBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                OwnerBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
         [HttpGet]
        public IActionResult GetPropertyType(string code, string description, int sequence)
        {
            try
            {


                DataTable dataTable = _iAdministrationService.PropertyType(code, description, sequence);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Sequence = !string.IsNullOrEmpty(d["Sequence"].ToString()) ? d["Sequence"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdatedDate = !string.IsNullOrEmpty(d["UpdatedDate"].ToString()) ? d["UpdatedDate"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
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

        public IActionResult PropertyType()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertPropertyType()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                PropertyTypeModel member = new PropertyTypeModel();

                // Lấy dữ liệu từ form
                member.Code = Request.Form["code"].ToString();
                member.Description = Request.Form["description"].ToString();
                int seqValue;
                if (int.TryParse(Request.Form["seq"], out seqValue))
                {
                    member.Sequence = seqValue;
                }
                else
                {
                    member.Sequence = 0; // hoặc giá trị mặc định
                }
                member.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                member.UpdatedBy = member.CreatedBy;
                member.CreatedDate = DateTime.Now;
                member.UpdatedDate = DateTime.Now;
                if (string.IsNullOrWhiteSpace(member.Code))
                    return Json(new { success = false, message = "Code không được để trống." });

                long memberId = PropertyTypeBO.Instance.Insert(member);

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
        public ActionResult UpdatePropertyType()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                PropertyTypeModel member = new PropertyTypeModel();

                // Lấy ID từ form
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;

                // Lấy dữ liệu từ form
                member.Code = Request.Form["code"].ToString();
                member.Description = Request.Form["description"].ToString();
                int seqValue;
                if (int.TryParse(Request.Form["seq"], out seqValue))
                {
                    member.Sequence = seqValue;
                }
                else
                {
                    member.Sequence = 0; // hoặc giá trị mặc định
                }

                string loginName = HttpContext.Session.GetString("LoginName") ?? "";
                if (string.IsNullOrWhiteSpace(member.Code))
                    return Json(new { success = false, message = "Code không được để trống." });
              
                if (member.ID == 0) // Insert mới
                {
                    member.CreatedBy = loginName;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    PropertyTypeBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = PropertyTypeBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.CreatedBy = oldData.CreatedBy;
                        member.CreatedDate = oldData.CreatedDate;
                    }

                    member.UpdatedBy = loginName;
                    member.UpdatedDate = DateTime.Now;

                    PropertyTypeBO.Instance.Update(member);
                }

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
        public ActionResult DeletePropertyType()
        {
            try
            {

                PropertyTypeModel memberModel = (PropertyTypeModel)PropertyTypeBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                PropertyTypeBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetReservationType()
        {
            try
            {
                DataTable dataTable = _iAdministrationService.ReservationType();
                var colNames = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                Console.WriteLine("Columns in ReservationType: " + colNames);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  Sequence = !string.IsNullOrEmpty(d["Sequence"].ToString()) ? d["Sequence"] : "",
                                  ArrivalTimeRequired = !string.IsNullOrEmpty(d["ArrivalTimeRequired"].ToString()) ? d["ArrivalTimeRequired"] : "",
                                  CreditCardRequired = !string.IsNullOrEmpty(d["CreditCardRequired"].ToString()) ? d["CreditCardRequired"] : "",
                                  Deduct = !string.IsNullOrEmpty(d["Deduct"].ToString()) ? d["Deduct"] : "",
                                  DepositRequired = !string.IsNullOrEmpty(d["DepositRequired"].ToString()) ? d["DepositRequired"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
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
        public IActionResult ReservationType()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertReservationType()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                ReservationTypeModel member = new ReservationTypeModel();
                
                // Lấy dữ liệu từ form
                member.Code = Request.Form["code"].ToString();
                member.Name = Request.Form["name"].ToString();
                member.Deduct = !string.IsNullOrEmpty(Request.Form["deduct"])
                                 && Request.Form["deduct"].ToString() == "on";
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                 && Request.Form["inactive"].ToString() == "on";
                member.ArrivalTimeRequired = !string.IsNullOrEmpty(Request.Form["arrivalTimeRequired"])
                                 && Request.Form["arrivalTimeRequired"].ToString() == "on";
                member.CreditCardRequired = !string.IsNullOrEmpty(Request.Form["creditCardRequired"])
                                 && Request.Form["creditCardRequired"].ToString() == "on";
                member.DepositRequired = !string.IsNullOrEmpty(Request.Form["depositRequired"])
                                 && Request.Form["depositRequired"].ToString() == "on";
                int seqValue;
                if (int.TryParse(Request.Form["seq"], out seqValue))
                {
                    member.Sequence = seqValue;
                }
                else
                {
                    member.Sequence = 0; // hoặc giá trị mặc định
                }
                member.UserInsertID = HttpContext.Session.GetInt32("UserID") ?? 0;
                member.UserUpdateID = member.UserInsertID;
                member.CreateDate = DateTime.Now;
                member.UpdateDate = DateTime.Now;
                if (string.IsNullOrWhiteSpace(member.Code))
                    return Json(new { success = false, message = "Code không được để trống." });

                long memberId = ReservationTypeBO.Instance.Insert(member);

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
        public ActionResult UpdateReservationType()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                ReservationTypeModel member = new ReservationTypeModel();

                // Lấy ID từ form
                member.ID = !string.IsNullOrEmpty(Request.Form["id"])
                             ? int.Parse(Request.Form["id"])
                             : 0;
                member.Deduct = !string.IsNullOrEmpty(Request.Form["deduct"])
                                 && Request.Form["deduct"].ToString() == "on";
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                 && Request.Form["inactive"].ToString() == "on";
                member.ArrivalTimeRequired = !string.IsNullOrEmpty(Request.Form["arrivalTimeRequired"])
                                 && Request.Form["arrivalTimeRequired"].ToString() == "on";
                member.CreditCardRequired = !string.IsNullOrEmpty(Request.Form["creditCardRequired"])
                                 && Request.Form["creditCardRequired"].ToString() == "on";
                member.DepositRequired = !string.IsNullOrEmpty(Request.Form["depositRequired"])
                                 && Request.Form["depositRequired"].ToString() == "on";
                // Lấy dữ liệu từ form
                member.Code = Request.Form["code"].ToString();
                member.Name = Request.Form["name"].ToString();
                int seqValue;
                if (int.TryParse(Request.Form["seq"], out seqValue))
                {
                    member.Sequence = seqValue;
                }
                else
                {
                    member.Sequence = 0; // hoặc giá trị mặc định
                }

                int loginName = HttpContext.Session.GetInt32("UserID") ?? 0;
                if (string.IsNullOrWhiteSpace(member.Code))
                    return Json(new { success = false, message = "Code không được để trống." });

                if (member.ID == 0) // Insert mới
                {
                    member.UserInsertID = loginName;
                    member.CreateDate = DateTime.Now;
                    member.UserUpdateID = loginName;
                    member.UpdateDate = DateTime.Now;

                    ReservationTypeBO.Instance.Insert(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = ReservationTypeBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {
                        member.UserInsertID = oldData.UserInsertID;
                        member.CreateDate = oldData.CreateDate;
                    }

                    member.UserUpdateID = loginName;
                    member.UpdateDate = DateTime.Now;

                    ReservationTypeBO.Instance.Update(member);
                }

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
        public ActionResult DeleteReservationType()
        {
            try
            {

                ReservationTypeModel memberModel = (ReservationTypeModel)ReservationTypeBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (memberModel == null || memberModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                ReservationTypeBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetCurrency()
        {
            try
            {
                DataTable dataTable = _iAdministrationService.Currency();
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  IsMaster = !string.IsNullOrEmpty(d["IsMaster"].ToString()) ? d["IsMaster"] : "",
                                  Trans = !string.IsNullOrEmpty(d["Trans"].ToString()) ? d["Trans"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  Inactive = !string.IsNullOrEmpty(d["Inactive"].ToString()) ? d["Inactive"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
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
        public IActionResult Currency()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public ActionResult InsertCurrency()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                CurrencyModel member = new CurrencyModel();

                member.IsShow = !string.IsNullOrEmpty(Request.Form["isShow"])
                                 && Request.Form["isShow"].ToString() == "on";
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                 && Request.Form["inactive"].ToString() == "on";
                member.MasterStatus = !string.IsNullOrEmpty(Request.Form["masterStatus"])
                                 && Request.Form["masterStatus"].ToString() == "on";
                // Lấy dữ liệu từ form
                member.ID = Request.Form["code"].ToString();
                member.Description = Request.Form["description"].ToString();
                int seqValue;
                if (int.TryParse(Request.Form["seq"], out seqValue))
                {
                    member.Decimals = seqValue;
                }
                else
                {
                    member.Decimals = 0; // hoặc giá trị mặc định
                }
                member.UserInsertID = HttpContext.Session.GetInt32("UserID") ?? 0;
                member.UserUpdateID = member.UserInsertID;
                member.CreateDate = DateTime.Now;
                member.UpdateDate = DateTime.Now;
                if (string.IsNullOrWhiteSpace(member.ID))
                    return Json(new { success = false, message = "Code không được để trống." });

                string memberId = CurrencyBO.Instance.InsertStringId(member);

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
        public ActionResult UpdateCurrency()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                CurrencyModel member = new CurrencyModel();

                // Lấy ID từ form
                member.IsShow = !string.IsNullOrEmpty(Request.Form["isShow"])
                                 && Request.Form["isShow"].ToString() == "on";
                member.Inactive = !string.IsNullOrEmpty(Request.Form["inactive"])
                                 && Request.Form["inactive"].ToString() == "on";
                member.MasterStatus = !string.IsNullOrEmpty(Request.Form["masterStatus"])
                                 && Request.Form["masterStatus"].ToString() == "on";
                // Lấy dữ liệu từ form
                member.ID = Request.Form["code"].ToString();
                member.Description = Request.Form["description"].ToString();
                int seqValue;
                if (int.TryParse(Request.Form["seq"], out seqValue))
                {
                    member.Decimals = seqValue;
                }
                else
                {
                    member.Decimals = 0; // hoặc giá trị mặc định
                }

                int loginName = HttpContext.Session.GetInt32("UserID") ?? 0;
                if (string.IsNullOrWhiteSpace(member.ID))
                    return Json(new { success = false, message = "Code không được để trống." });

                if (member.ID == "") // Insert mới
                {
                    member.UserInsertID = loginName;
                    member.CreateDate = DateTime.Now;
                    member.UserUpdateID = loginName;
                    member.UpdateDate = DateTime.Now;

                    CurrencyBO.Instance.InsertStringId(member);
                }
                else // Update
                {
                    // Trước khi update, lấy lại bản ghi cũ từ DB để giữ CreatedBy, CreatedDate
                    var oldData = CurrencyBO.Instance.GetById(member.ID, pt.Connection, pt.Transaction);

                    if (oldData != null)
                    {

                        member.UserInsertID = oldData.UserInsertID;
                        member.CreateDate = oldData.CreateDate;
                    }

                    member.UserUpdateID = loginName;
                    member.UpdateDate = DateTime.Now;

                    CurrencyBO.Instance.UpdateStringId(member);
                }

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
        public ActionResult DeleteCurrency()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                // Lấy ID string từ form
                string id = Request.Form["id"].ToString();

                if (string.IsNullOrEmpty(id))
                    return Json(new { code = 1, msg = "ID is null or empty" });

                // Lấy model theo ID
                CurrencyModel memberModel = CurrencyBO.Instance.GetById(id, pt.Connection, pt.Transaction);


                if (memberModel == null || string.IsNullOrEmpty(memberModel.ID))
                    return Json(new { code = 1, msg = "Cannot find Currency" });

                // Xóa model trực tiếp bằng string ID
                CurrencyBO.Instance.DeleteStringId(memberModel);

                return Json(new { code = 0, msg = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }
        }


        [HttpGet]
        public IActionResult GetCurrencyById(string id)
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                CurrencyModel member = CurrencyBO.Instance.GetById(id, pt.Connection, pt.Transaction);

                pt.CommitTransaction();

                if (member == null)
                    return Json(new { success = false, message = "Not found" });

                return Json(new
                {
                    success = true,
                    id = member.ID,
                    description = member.Description ?? "",
                    decimals = member.Decimals,
                    inactive = member.Inactive,
                    isMaster = member.MasterStatus,
                    isShow = member.IsShow
                });
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





    }
}

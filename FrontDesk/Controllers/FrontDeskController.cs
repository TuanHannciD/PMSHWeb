using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using DevExpress.XtraRichEdit.Import.Html;
using FrontDesk.Services.Implements;
using FrontDesk.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BaseBusiness.util; 
using Microsoft.Data.SqlClient;
namespace FrontDesk.Controllers
{
    public class FrontDeskController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FrontDeskController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IFrontDeskService _iFrontDeskService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FrontDeskController(ILogger<FrontDeskController> logger,
                IMemoryCache cache, IConfiguration configuration, IFrontDeskService iFrontDeskService, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _iFrontDeskService = iFrontDeskService;
            _httpContextAccessor = httpContextAccessor;

        }

        public IActionResult TelephoneBook()
        {
            List<TelephoneBookCategoryModel> tlplist = PropertyUtils.ConvertToList<TelephoneBookCategoryModel>(TelephoneBookCategoryBO.Instance.FindAll());
            var sortedList = tlplist.OrderBy(x => x.Name).ToList();
            ViewBag.TelephoneBookCategoryList = sortedList;
            return View();
        }
        [HttpGet]
        public JsonResult GetTelephoneBook(int? categoryId = null)
        {
            try
            {
                var list = TelephoneBookBO.Instance.FindAll();
                var result = list.Cast<TelephoneBookModel>().ToList();

                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    result = result.Where(x => x.TelephoneBookCategoryID == categoryId.Value).ToList();
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetTelephoneById(int id)
        {
            try
            {
                var obj = (TelephoneBookModel)TelephoneBookBO.Instance.FindByPrimaryKey(id);
                return Json(obj);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult InsertTelephoneBook(string name, string telephone, string address, string remark, string webAddress, int categoryId, int color)
        {
            try
            {
                var vnPhoneRegex = new System.Text.RegularExpressions.Regex(@"^((0|\+84)(3|5|7|8|9)[0-9]{8}|0\d{2,3}\d{7,8})$");

                if (string.IsNullOrWhiteSpace(telephone) || !vnPhoneRegex.IsMatch(telephone))
                {
                    return Json(new { success = false, message = "Số điện thoại Việt Nam không hợp lệ (di động hoặc máy bàn)!" });
                }
                int userId = HttpContext.Session.GetInt32("UserID") ?? 0;

                var model = new TelephoneBookModel
                {
                    Name = name,
                    Telephone = telephone,
                    Address = address,
                    Remark = remark,
                    WebAddress = webAddress,
                    TelephoneBookCategoryID = categoryId,
                    Color = color,
                    CreateDate = DateTime.Now,
                    UserInsertID = userId
                };

                TelephoneBookBO.Instance.Insert(model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult UpdateTelephoneBook(int id, string name, string telephone, string address,
    string remark, string webAddress, int categoryId, int color)
        {
            try
            {
                int userId = HttpContext.Session.GetInt32("UserID") ?? 0;

                var model = new TelephoneBookModel
                {
                    ID = id,
                    Name = name,
                    Telephone = telephone,
                    Address = address,
                    Remark = remark,
                    WebAddress = webAddress,
                    TelephoneBookCategoryID = categoryId,
                    Color = color,
                    UserUpdateID = userId,
                    UpdateDate = DateTime.Now
                };

                TelephoneBookBO.Instance.Update(model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult DeleteTelephoneBook(int id)
        {
            try
            {
                TelephoneBookBO.Instance.Delete(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult TelephoneSwitchSearch(string roomNo, int foStatus)
        {
            try
            {
                DataTable dataTable = _iFrontDeskService.TelephoneSwitch(roomNo, foStatus);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  id = d["ID"]?.ToString(),
                                  roomNo = d["RoomNo"]?.ToString(),
                                  foStatus = d["FOStatus"]?.ToString(),
                                  code = d["Code"]?.ToString(),
                                  guestName = d["GuestName"]?.ToString(),
                                  checkInDate = d["CheckInDate"]?.ToString(),
                                  checkOutDate = d["CheckOutDate"]?.ToString()
                              }).ToList();

                // Dùng Ok thay vì Json để System.Text.Json serialize theo đúng tên bạn đặt
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        public IActionResult TelephoneSwitch()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }

        [HttpPost]
        public IActionResult UpdateTelephoneSwitch([FromBody] TelephoneSwitchUpdateModel model)
        {
            try
            {
                string sqlGetObjectId = "exec spTelephoneSwitchSearch @RoomNo, @FOStatus";
                var spParams = new SqlParameter[]
                {
                    new SqlParameter("@RoomNo", model.RoomNo),
                    new SqlParameter("@FOStatus", 2)
                };

                DataTable objDt = DataTableHelper.ExecuteQuery(sqlGetObjectId, spParams);

                int objectId = 0;
                if (objDt.Rows.Count > 0 && objDt.Rows[0]["ID"] != DBNull.Value)
                {
                    objectId = Convert.ToInt32(objDt.Rows[0]["ID"]);
                }
                int userId = HttpContext.Session.GetInt32("UserID") ?? 0;
                string userName = HttpContext.Session.GetString("LoginName") ?? "system";
              
                var computerName = Environment.MachineName;

                string sqlSelect = @"SELECT TOP 1 NewValue 
                             FROM RoomStatusHistory 
                             WHERE RoomNo = @RoomNo 
                             ORDER BY ChangeDate DESC";

                var selectParams = new SqlParameter[]
                {
                    new SqlParameter("@RoomNo", model.RoomNo)
                };

                DataTable dt = DataTableHelper.ExecuteQuery(sqlSelect, selectParams);

                string oldValue = "Off"; // mặc định
                if (dt.Rows.Count > 0 && dt.Rows[0]["NewValue"] != DBNull.Value)
                {
                    oldValue = dt.Rows[0]["NewValue"].ToString();
                }

                string newValue = model.NewValue;

                string sql1 = @"INSERT INTO RoomStatusHistory 
                        (ObjectID, TableName, UserName, RoomNo, Action, ComputerName, OldValue, NewValue, ChangeDate) 
                        VALUES (@ObjectID, @TableName, @UserName, @RoomNo, @Action, @ComputerName, @OldValue, @NewValue, GETDATE());
                        SELECT SCOPE_IDENTITY();";

                var parameters1 = new SqlParameter[]
                {
                    new SqlParameter("@ObjectID", objectId),
                    new SqlParameter("@TableName", "Room"),
                    new SqlParameter("@UserName", userName),  // lấy trực tiếp từ session
                    new SqlParameter("@RoomNo", model.RoomNo),
                    new SqlParameter("@Action", "Telephone switch"),
                    new SqlParameter("@ComputerName", computerName),
                    new SqlParameter("@OldValue", oldValue),
                    new SqlParameter("@NewValue", newValue)
                };

                int historyId = DataTableHelper.ExecuteInsertAndReturnId(sql1, parameters1);

                string sql2 = @"INSERT INTO TelephoneSwitch (RoomNo, GuestName, Status, CreateDate) 
                        VALUES (@RoomNo, @GuestName, @Status, GETDATE());
                        SELECT SCOPE_IDENTITY();";

                int status = model.NewValue == "On" ? 1 : 0;

                var parameters2 = new SqlParameter[]
                {
                    new SqlParameter("@RoomNo", model.RoomNo),
                    new SqlParameter("@GuestName", model.GuestName ?? (object)DBNull.Value),
                    new SqlParameter("@Status", status)
                };

                int switchId = DataTableHelper.ExecuteInsertAndReturnId(sql2, parameters2);

                return Json(new { success = true, historyId, switchId, userName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult DialingInformation()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpGet]
        public IActionResult GetDialingInformation(DateTime fromDate, DateTime toDate, string phoneNo, int view)
        {
            try
            {
                DataTable dataTable = _iFrontDeskService.DialingInformation(fromDate, toDate, phoneNo, view);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  FromPhoneNo = !string.IsNullOrEmpty(d["From Phone No"].ToString()) ? d["From Phone No"] : "",
                                  ToPhoneNo = !string.IsNullOrEmpty(d["To Phone No"].ToString()) ? d["To Phone No"] : "",
                                  TimeStart = !string.IsNullOrEmpty(d["Time Start"].ToString()) ? d["Time Start"] : "",
                                  TimeEnd = !string.IsNullOrEmpty(d["Time End"].ToString()) ? d["Time End"] : "",
                                  Duration = !string.IsNullOrEmpty(d["Duration"].ToString()) ? d["Duration"] : "",
                                  Area = !string.IsNullOrEmpty(d["Area"].ToString()) ? d["Area"] : "",
                                  Amount = !string.IsNullOrEmpty(d["Amount"].ToString()) ? d["Amount"] : "",
                                  Currency = !string.IsNullOrEmpty(d["Currency"].ToString()) ? d["Currency"] : "",
                              }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
   

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using Dapper;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RoomManagement.Services.Implements;
using RoomManagement.Services.Interfaces;

namespace RoomManagement.Controllers
{
    public class RoomManagementController : Controller
    {
        private readonly ILogger<RoomManagementController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly IRoomManagementService _iRoomManagementService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoomManagementController(ILogger<RoomManagementController> logger, IMemoryCache cache, IConfiguration configuration, IRoomManagementService iRoomManagementService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _cache = cache;
            _configuration = configuration;
            _iRoomManagementService = iRoomManagementService;
            _httpContextAccessor = httpContextAccessor;
        }
        // Define your actions here
        public IActionResult Discrepancy()
        {
            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;
            List<RoomClassModel> listrl = PropertyUtils.ConvertToList<RoomClassModel>(RoomClassBO.Instance.FindAll());
            ViewBag.RoomClassList = listrl;
            List<RoomModel> listroom = PropertyUtils.ConvertToList<RoomModel>(RoomBO.Instance.FindAll());
            ViewBag.RoomList = listroom;
            List<FloorModel> listfloor = PropertyUtils.ConvertToList<FloorModel>(FloorBO.Instance.FindAll());
            ViewBag.FloorList = listfloor;
            return View();
        }
        [HttpGet]
        public IActionResult GetDiscrepancy(int sleep, int skip, int person, string floor, string room, string zone, string roomClass)
        {
            try
            {


                DataTable dataTable = _iRoomManagementService.Discrepancy(sleep, skip, person, floor, room, zone, roomClass);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  RoomNo = !string.IsNullOrEmpty(d["Room No"].ToString()) ? d["Room No"] : "",
                                  RoomStatus = !string.IsNullOrEmpty(d["Room Status"].ToString()) ? d["Room Status"] : "",
                                  FOStatus = !string.IsNullOrEmpty(d["FO Status"].ToString()) ? d["FO Status"] : "",
                                  HKStatus = !string.IsNullOrEmpty(d["HK Status"].ToString()) ? d["HK Status"] : "",
                                  HKPersons = !string.IsNullOrEmpty(d["HK Persons"].ToString()) ? d["HK Persons"] : "",
                                  FOPersons = !string.IsNullOrEmpty(d["FO Persons"].ToString()) ? d["FO Persons"] : "",
                                  Discrepancy = !string.IsNullOrEmpty(d["Discrepancy"].ToString()) ? d["Discrepancy"] : "",
                           
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

        [HttpPost("UpdateHKFOStatus")]
        public IActionResult UpdateHKFOStatus([FromBody] RoomUpdateDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.RoomNo))
                return BadRequest(new { success = false, message = "Không có phòng nào được chọn" });

            try
            {
                string connString = new AppConfiguration()?.ConnectionString;
                if (string.IsNullOrEmpty(connString))
                    return BadRequest(new { success = false, message = "Chuỗi kết nối không hợp lệ" });

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string sql = @"
                UPDATE Room
                SET HKFOStatus = @NewHKFOStatus
                WHERE RoomNo = @RoomNo
                  ";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add("@NewHKFOStatus", SqlDbType.Int).Value = dto.NewHKFOStatus;
                        cmd.Parameters.Add("@RoomNo", SqlDbType.VarChar, 10).Value = dto.RoomNo;

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            return BadRequest(new { success = false, message = "Phòng không thỏa điều kiện hoặc không tồn tại" });
                    }
                }

                return Ok(new { success = true, message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.ToString() });
            }
        }

        [HttpGet]
        public IActionResult GetItemDailyInventory(
                int groupId,
                DateTime firstDate,
                DateTime secondDate,
                DateTime thirthDate,
                DateTime fourthDate,
                DateTime fifthDate,
                DateTime sixthDate,
                DateTime seventhDate,
                DateTime eighthDate,
                DateTime ninthDate,
                DateTime tenthDate,
                DateTime eleventhDate,
                DateTime twelvethDate,
                DateTime thirtheenDate,
                DateTime fourtheenDate,
                DateTime fiftheenDate)
        {
            try
            {


                DataTable dataTable = _iRoomManagementService.ItemDailyInventory(
                groupId,
                firstDate,
                secondDate,
                thirthDate,
                fourthDate,
                fifthDate,
                sixthDate,
                seventhDate,
                eighthDate,
                ninthDate,
                tenthDate,
                eleventhDate,
                twelvethDate,
                thirtheenDate,
                fourtheenDate,
                fiftheenDate);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  ItemID = !string.IsNullOrEmpty(d["ItemID"].ToString()) ? d["ItemID"] : "",
                                  ItemName = !string.IsNullOrEmpty(d["Item Name"].ToString()) ? d["Item Name"] : "",
                                  FirstDate = !string.IsNullOrEmpty(d["FirstDate"].ToString()) ? d["FirstDate"] : "",
                                  SecondDate = !string.IsNullOrEmpty(d["SecondDate"].ToString()) ? d["SecondDate"] : "",
                                  ThirthDate = !string.IsNullOrEmpty(d["ThirthDate"].ToString()) ? d["ThirthDate"] : "",
                                  FourthDate = !string.IsNullOrEmpty(d["FourthDate"].ToString()) ? d["FourthDate"] : "",
                                  FifthDate = !string.IsNullOrEmpty(d["FifthDate"].ToString()) ? d["FifthDate"] : "",
                                  SixthDate = !string.IsNullOrEmpty(d["SixthDate"].ToString()) ? d["SixthDate"] : "",
                                  SeventhDate = !string.IsNullOrEmpty(d["SeventhDate"].ToString()) ? d["SeventhDate"] : "",
                                  EighthDate = !string.IsNullOrEmpty(d["EighthDate"].ToString()) ? d["EighthDate"] : "",
                                  NinthDate = !string.IsNullOrEmpty(d["NinthDate"].ToString()) ? d["NinthDate"] : "",
                                  TenthDate = !string.IsNullOrEmpty(d["TenthDate"].ToString()) ? d["TenthDate"] : "",
                                  EleventhDate = !string.IsNullOrEmpty(d["EleventhDate"].ToString()) ? d["EleventhDate"] : "",
                                  TwelvethDate = !string.IsNullOrEmpty(d["TwelvethDate"].ToString()) ? d["TwelvethDate"] : "",
                                  ThirtheenDate = !string.IsNullOrEmpty(d["ThirtheenDate"].ToString()) ? d["ThirtheenDate"] : "",
                                  FourtheenDate = !string.IsNullOrEmpty(d["FourtheenDate"].ToString()) ? d["FourtheenDate"] : "",
                                  FiftheenDate = !string.IsNullOrEmpty(d["FiftheenDate"].ToString()) ? d["FiftheenDate"] : "",
                              
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
        public IActionResult ItemDailyInventory()
        {       
            return View();
        }

        [HttpPost("UpdateItemInventory")]
        public IActionResult UpdateItemInventory([FromBody] InventoryUpdateRequest model)
        {
            if (model == null)
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });

            try
            {
                var userId = HttpContext.Session.GetInt32("UserID") ?? 0;
                if (userId == 0)
                {
                    return Unauthorized(new { success = false, message = "User chưa đăng nhập" });
                }
                model.UserID = userId;

                _logger.LogInformation($"UpdateItemInventory called with ItemID={model.ItemID}, Date={model.Date}, Quantity={model.Quantity}, UserID={model.UserID}");

                bool success = _iRoomManagementService.UpdateItemInventory(model);

                if (success)
                    return Ok(new { success = true, message = "Cập nhật thành công" });
                else
                    return NotFound(new { success = false, message = "Không tìm thấy dữ liệu phù hợp" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật ItemInventory");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetItemInventoryAvailable(
                int groupId,
                DateTime firstDate,
                DateTime secondDate,
                DateTime thirthDate,
                DateTime fourthDate,
                DateTime fifthDate,
                DateTime sixthDate,
                DateTime seventhDate,
                DateTime eighthDate,
                DateTime ninthDate,
                DateTime tenthDate,
                DateTime elevenDate,
                DateTime twelveDate,
                DateTime thirtheenDate,
                DateTime fourtheenDate,
                DateTime fiftheenDate)
        {
            try
            {


                DataTable dataTable = _iRoomManagementService.ItemInventoryAvailable(
                groupId,
                firstDate,
                secondDate,
                thirthDate,
                fourthDate,
                fifthDate,
                sixthDate,
                seventhDate,
                eighthDate,
                ninthDate,
                tenthDate,
                elevenDate,
                twelveDate,
                thirtheenDate,
                fourtheenDate,
                fiftheenDate);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  ItemID = !string.IsNullOrEmpty(d["ItemID"].ToString()) ? d["ItemID"] : "",
                                  ItemName = !string.IsNullOrEmpty(d["Item Name"].ToString()) ? d["Item Name"] : "",
                                  FirstDate1 = !string.IsNullOrEmpty(d["FirstDate1"].ToString()) ? d["FirstDate1"] : "",
                                  SecondDate1 = !string.IsNullOrEmpty(d["SecondDate1"].ToString()) ? d["SecondDate1"] : "",
                                  ThirthDate1 = !string.IsNullOrEmpty(d["ThirthDate1"].ToString()) ? d["ThirthDate1"] : "",
                                  FourthDate1 = !string.IsNullOrEmpty(d["FourthDate1"].ToString()) ? d["FourthDate1"] : "",
                                  FifthDate1 = !string.IsNullOrEmpty(d["FifthDate1"].ToString()) ? d["FifthDate1"] : "",
                                  SixthDate1 = !string.IsNullOrEmpty(d["SixthDate1"].ToString()) ? d["SixthDate1"] : "",
                                  SeventhDate1 = !string.IsNullOrEmpty(d["SeventhDate1"].ToString()) ? d["SeventhDate1"] : "",
                                  EighthDate1 = !string.IsNullOrEmpty(d["EighthDate1"].ToString()) ? d["EighthDate1"] : "",
                                  NinthDate1 = !string.IsNullOrEmpty(d["NinthDate1"].ToString()) ? d["NinthDate1"] : "",
                                  TenthDate1 = !string.IsNullOrEmpty(d["TenthDate1"].ToString()) ? d["TenthDate1"] : "",
                                  EleventhDate1 = !string.IsNullOrEmpty(d["EleventhDate1"].ToString()) ? d["EleventhDate1"] : "",
                                  TwelvethDate1 = !string.IsNullOrEmpty(d["TwelvethDate1"].ToString()) ? d["TwelvethDate1"] : "",
                                  ThirtheenDate1 = !string.IsNullOrEmpty(d["ThirtheenDate1"].ToString()) ? d["ThirtheenDate1"] : "",
                                  FourtheenDate1 = !string.IsNullOrEmpty(d["FourtheenDate1"].ToString()) ? d["FourtheenDate1"] : "",
                                  FiftheenDate1 = !string.IsNullOrEmpty(d["FiftheenDate1"].ToString()) ? d["FiftheenDate1"] : "",

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
        public IActionResult ItemInventoryAvailable()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetItemResvDetail(int itemID, DateTime day)
        {
            try
            {
                DataTable dataTable = _iRoomManagementService.ItemResvDetail(itemID, day);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ConfirmationNo = !string.IsNullOrEmpty(d["Confirmation No"].ToString()) ? d["Confirmation No"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  RoomNo = !string.IsNullOrEmpty(d["Room No"].ToString()) ? d["Room No"] : "",
                                  ReservedFrom = !string.IsNullOrEmpty(d["Reserved From"].ToString()) ? d["Reserved From"] : "",
                                  ReservedTo = !string.IsNullOrEmpty(d["Reserved To"].ToString()) ? d["Reserved To"] : "",
                                  Qty = !string.IsNullOrEmpty(d["Qty"].ToString()) ? d["Qty"] : "",
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
        public IActionResult ItemResvDetail()
        {
            return View();
        }



    }
}

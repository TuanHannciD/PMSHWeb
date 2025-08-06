using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using HouseKeeping.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HouseKeeping.Commons.Helpers;
using System.Security.Policy;
using DevExpress.XtraCharts.Native;
using DevExpress.Charts.Native;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace HouseKeeping.Controllers
{
    public class HouseKeepingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HouseKeepingController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IHouseKeepingService _iHouseKeepingService;
        public HouseKeepingController(ILogger<HouseKeepingController> logger,
             IMemoryCache cache, IConfiguration configuration, IHouseKeepingService iHouseKeepingService)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _iHouseKeepingService = iHouseKeepingService;
        }
        public IActionResult RoomControlPanel()
        {
            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;
            return View();
        }
        public IActionResult RoomFacilityForecast()
        {
            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;
            return View();
        }

        public IActionResult RoomStatus()
        {
            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;
            List<RoomTypeModel> listrt = PropertyUtils.ConvertToList<RoomTypeModel>(RoomTypeBO.Instance.FindAll());
            ViewBag.RoomTypeList = listrt;
            List<RoomModel> listroom = PropertyUtils.ConvertToList<RoomModel>(RoomBO.Instance.FindAll());
            ViewBag.RoomList = listroom;
            return View();
        }
        public IActionResult HouseStatus()
        {
            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;
            List<RoomTypeModel> listrt = PropertyUtils.ConvertToList<RoomTypeModel>(RoomTypeBO.Instance.FindAll());
            ViewBag.RoomTypeList = listrt;
            List<BusinessDateModel> businessDateModel = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
            ViewBag.BusinessDate = businessDateModel[0].BusinessDate;
            return View();
        }
        public IActionResult RoomPlan()
        {
            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;
            List<RoomTypeModel> listrt = PropertyUtils.ConvertToList<RoomTypeModel>(RoomTypeBO.Instance.FindAll());
            ViewBag.RoomTypeList = listrt;
            List<RoomModel> listroom = PropertyUtils.ConvertToList<RoomModel>(RoomBO.Instance.FindAll());
            ViewBag.RoomList = listroom;
            List<FloorModel> listfloor = PropertyUtils.ConvertToList<FloorModel>(FloorBO.Instance.FindAll());
            ViewBag.FloorList = listfloor;
            ViewBag.cboNationality = ListItemHelper.GetNationalityProvider();
            ViewBag.cboTitle = ListItemHelper.GetTitleProvider();
            ViewBag.cboCity = ListItemHelper.GetCityProvider();
            ViewBag.cboVIP = ListItemHelper.GetVIPProvider();
            ViewBag.cboMemberType = ListItemHelper.GetMemberTypeProvider();
            ViewBag.cboProfileAgent = ListItemHelper.GetProfileAgentProvider();
            ViewBag.cboProfileCompany = ListItemHelper.GetProfileCompanyProvider();
            ViewBag.cboProfileContact = ListItemHelper.GetProfileContactProvider();
            ViewBag.cboRoomType = ListItemHelper.GetRoomTyeProvider();
            ViewBag.cboCurrency = ListItemHelper.GetCurrencyProvider();
            ViewBag.cboPackage = ListItemHelper.GetPackagesProvider();
            ViewBag.cboReason = ListItemHelper.GetReasonProvider();
            ViewBag.cboReservationType = ListItemHelper.GetReservationTypeProvider();
            ViewBag.cboSource = ListItemHelper.GetSourceProvider();
            ViewBag.cboMarket = ListItemHelper.GetMarketProvider();
            ViewBag.cboProfile = ListItemHelper.GetProfileProvider();
            ViewBag.cboAllotmentType = ListItemHelper.GetAllotmentTypeProvider();
            ViewBag.cboPersonInCharge = ListItemHelper.GetPersonInChargeProvider();
            ViewBag.cboPaymentMethod = ListItemHelper.GetPaymentMethodProvider();
            ViewBag.cboPromotion = ListItemHelper.GetPromotionProvider();
            ViewBag.cboGroupPreferenceProvider = ListItemHelper.GetGroupPreferenceProvider();
            ViewBag.cboTransportType = ListItemHelper.GetTransportTypeProvider();
            ViewBag.cboItem = ListItemHelper.GetItemInventoryProvider();
            return View();
        }
        public IActionResult FloorPlan()
        {
         
            List<RoomTypeModel> listrt = PropertyUtils.ConvertToList<RoomTypeModel>(RoomTypeBO.Instance.FindAll());
            ViewBag.RoomTypeList = listrt;
            
            List<FloorModel> listfloor = PropertyUtils.ConvertToList<FloorModel>(FloorBO.Instance.FindAll());
            ViewBag.FloorList = listfloor;

            List<BlockModel> listbl = PropertyUtils.ConvertToList<BlockModel>(BlockBO.Instance.FindAll());
            ViewBag.BlockList = listbl;
            return View();
        }

        public IActionResult AttendantPoint()
        {

            List<hkpAttendantModel> listatt = PropertyUtils.ConvertToList<hkpAttendantModel>(hkpAttendantBO.Instance.FindAll());
            ViewBag.hkpAttendantList = listatt;
            List<BusinessDateModel> businessDateModel = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
            ViewBag.BusinessDate = businessDateModel[0].BusinessDate;
            return View();
        }
        public IActionResult TaskSheetStatus()
        {

            List<hkpAttendantModel> listatt = PropertyUtils.ConvertToList<hkpAttendantModel>(hkpAttendantBO.Instance.FindAll());
            ViewBag.hkpAttendantList = listatt;
            List<BusinessDateModel> businessDateModel = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
            ViewBag.BusinessDate = businessDateModel[0].BusinessDate;
            return View();
        }
        public IActionResult TaskAssignment()
        {

            List<hkpFacilityTaskModel> listhkpft = PropertyUtils.ConvertToList<hkpFacilityTaskModel>(hkpFacilityTaskBO.Instance.FindAll());
            ViewBag.hkpFacilityTaskList = listhkpft;
            List<BusinessDateModel> businessDateModel = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
            ViewBag.BusinessDate = businessDateModel[0].BusinessDate;
            List<hkpTaskSheetModel> listhkpts = PropertyUtils
           .ConvertToList<hkpTaskSheetModel>(hkpTaskSheetBO.Instance.FindAll())
           .Where(x => x.TaskSheetDate.Date == businessDateModel[0].BusinessDate.Date)
           .ToList();
            ViewBag.hkpTaskSheetList = listhkpts;

            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;

            List<hkpSectionModel> listsshkp = PropertyUtils.ConvertToList<hkpSectionModel>(hkpSectionBO.Instance.FindAll());
            ViewBag.hkpSectionList = listsshkp;
            List<hkpAttendantModel> listatt= PropertyUtils.ConvertToList<hkpAttendantModel>(hkpAttendantBO.Instance.FindAll());
            ViewBag.hkpAttendantList = listatt;
            return View();
        }
        public IActionResult GuestServiceStatus()
        {
            List<RoomModel> listroom = PropertyUtils.ConvertToList<RoomModel>(RoomBO.Instance.FindAll());

            // Lọc FOStatus = 1 và sắp xếp theo RoomNo
            var filteredRoomList = listroom
                .Where(r => r.FOStatus == 1)
                .OrderBy(r => r.RoomNo)
                .ToList();
            List<FloorModel> listfloor = PropertyUtils.ConvertToList<FloorModel>(FloorBO.Instance.FindAll());
            ViewBag.RoomList = filteredRoomList;
            ViewBag.FloorList = listfloor;
            ViewBag.cboNationality = ListItemHelper.GetNationalityProvider();
            ViewBag.cboTitle = ListItemHelper.GetTitleProvider();
            ViewBag.cboCity = ListItemHelper.GetCityProvider();
            ViewBag.cboVIP = ListItemHelper.GetVIPProvider();
            ViewBag.cboMemberType = ListItemHelper.GetMemberTypeProvider();
            ViewBag.cboProfileAgent = ListItemHelper.GetProfileAgentProvider();
            ViewBag.cboProfileCompany = ListItemHelper.GetProfileCompanyProvider();
            ViewBag.cboProfileContact = ListItemHelper.GetProfileContactProvider();
            ViewBag.cboRoomType = ListItemHelper.GetRoomTyeProvider();
            ViewBag.cboCurrency = ListItemHelper.GetCurrencyProvider();
            ViewBag.cboPackage = ListItemHelper.GetPackagesProvider();
            ViewBag.cboReason = ListItemHelper.GetReasonProvider();
            ViewBag.cboReservationType = ListItemHelper.GetReservationTypeProvider();
            ViewBag.cboSource = ListItemHelper.GetSourceProvider();
            ViewBag.cboMarket = ListItemHelper.GetMarketProvider();
            ViewBag.cboProfile = ListItemHelper.GetProfileProvider();
            ViewBag.cboAllotmentType = ListItemHelper.GetAllotmentTypeProvider();
            ViewBag.cboPersonInCharge = ListItemHelper.GetPersonInChargeProvider();
            ViewBag.cboPaymentMethod = ListItemHelper.GetPaymentMethodProvider();
            ViewBag.cboPromotion = ListItemHelper.GetPromotionProvider();
            ViewBag.cboGroupPreferenceProvider = ListItemHelper.GetGroupPreferenceProvider();
            ViewBag.cboTransportType = ListItemHelper.GetTransportTypeProvider();
            ViewBag.cboItem = ListItemHelper.GetItemInventoryProvider();
            return View();
        }

        [HttpGet]
        public IActionResult RoomControlPanelData(DateTime fromDate, DateTime toDate, string zone)
        {
            try
            {
                DataTable dataTable = _iHouseKeepingService.RoomControlPanelData(fromDate, toDate, zone);

                // Tạo danh sách các ngày trong khoảng fromDate -> toDate
                var dateRange = Enumerable.Range(0, (toDate - fromDate).Days + 1)
                                          .Select(offset => fromDate.AddDays(offset))
                                          .ToList();

                var result = dataTable.AsEnumerable().Select(d =>
                {
                    var rowData = new Dictionary<string, object>
                    {
                        ["Statisticname"] = d["Statisticname"]?.ToString() ?? "",
                        ["Index"] = d["Index"]?.ToString() ?? ""
                    };

                    foreach (var date in dateRange)
                    {
                        string columnName = date.ToString("yyyy-MM-dd"); // đồng bộ với JS
                        string dtColName = date.ToString("yyyy/MM/dd");   // tên cột trong DataTable

                        if (dataTable.Columns.Contains(dtColName))
                        {
                            var cellValue = d[dtColName];
                            rowData[columnName] = cellValue == DBNull.Value ? "" : cellValue;
                        }
                        else
                        {
                            rowData[columnName] = ""; // nếu không có cột thì cũng để ""
                        }
                    }

                    return rowData;
                }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult TaskSheetReport(DateTime fromDatere, DateTime toDatere, string taskcodeexpan, string attendantrepop, string tasksheetpopre, string reportstyle,string dueoutonly)
        {
            taskcodeexpan = taskcodeexpan ?? "";
            attendantrepop = attendantrepop ?? "";
            tasksheetpopre = tasksheetpopre ?? "";
            reportstyle = reportstyle ?? "";
            dueoutonly = dueoutonly ?? "";
            try
            {
                DataTable dataTable = new DataTable();

                if (reportstyle == "1")
                {
                    dataTable = _iHouseKeepingService.RoomAttendantDailyWorkSheet(fromDatere, toDatere, taskcodeexpan, attendantrepop, tasksheetpopre, dueoutonly);
                    var result = (from d in dataTable.AsEnumerable()
                                  select new
                                  {
                                      RoomNo = d["Room No."]?.ToString() ?? "",
                                      RoomType = d["Room Type"]?.ToString() ?? "",
                                      VC = d["VC"]?.ToString() ?? "",
                                      OD = d["OD"]?.ToString() ?? "",
                                      DO = d["DO"]?.ToString() ?? "",
                                      VD = d["VD"]?.ToString() ?? "",
                                      OOO = d["OOO"]?.ToString() ?? "",
                                      OOS = d["OOS"]?.ToString() ?? "",
                                      IN = d["IN"]?.ToString() ?? "",
                                      OUT = d["OUT"]?.ToString() ?? "",
                                      Status = d["Status"]?.ToString() ?? "",
                                      MW = d["MW"]?.ToString() ?? "",
                                      BR = d["BR"]?.ToString() ?? "",
                                      YK = d["YK"]?.ToString() ?? "",
                                      SL = d["SL"]?.ToString() ?? "",
                                      BT = d["BT"]?.ToString() ?? "",
                                      FT = d["FT"]?.ToString() ?? "",
                                      TB = d["TB"]?.ToString() ?? "",
                                      TR = d["TR"]?.ToString() ?? "",
                                      HT = d["HT"]?.ToString() ?? "",
                                      BM = d["BM"]?.ToString() ?? "",
                                      AM = d["AM"]?.ToString() ?? "",
                                      Chocolate = d["Chocolate"]?.ToString() ?? "",
                                      Note = d["Note"]?.ToString() ?? "",
                                      TaskSheetDate = d["TaskSheetDate"]?.ToString() ?? "",
                                      AttendantID = d["AttendantID"]?.ToString() ?? "",
                                      TaskSheetNo = d["TaskSheetNo"]?.ToString() ?? "",
                                      EmployeeName = d["EmployeeName"]?.ToString() ?? "",
                                      MainGuestNationality = d["MainGuestNationality"]?.ToString() ?? "",
                                      Special = d["Special"]?.ToString() ?? "",
                                      ItemInventory = d["ItemInventory"]?.ToString() ?? ""
                                  }).ToList();
                    return Json(result);
                }
                else if (reportstyle == "2")
                {
                    dataTable = _iHouseKeepingService.RAWorkSheet(fromDatere, toDatere, taskcodeexpan, attendantrepop, tasksheetpopre, dueoutonly);
                    var result = (from d in dataTable.AsEnumerable()
                                  select new
                                  {
                                      RoomNo = d["Room No."]?.ToString() ?? "",
                                      RoomStatus = d["Room Status"]?.ToString() ?? "",
                                      IN = d["IN"]?.ToString() ?? "",
                                      OUT = d["OUT"]?.ToString() ?? "",
                                      StatusChange = d["Status Change"]?.ToString() ?? "",
                                      Choco = d["Choco."]?.ToString() ?? "",
                                      EB = d["EB"]?.ToString() ?? "",
                                      VIP = d["VIP"]?.ToString() ?? "",
                                      Bath = d["Bath"]?.ToString() ?? "",
                                      Hand = d["Hand"]?.ToString() ?? "",
                                      Face = d["Face"]?.ToString() ?? "",
                                      Mat = d["Mat"]?.ToString() ?? "",
                                      Remarks = d["Remarks"]?.ToString() ?? "",
                                      TaskSheetDate = d["TaskSheetDate"]?.ToString() ?? "",
                                      AttendantID = d["AttendantID"]?.ToString() ?? "",
                                      TaskSheetNo = d["TaskSheetNo"]?.ToString() ?? "",
                                      EmployeeName = d["EmployeeName"]?.ToString() ?? ""
                                  }).ToList();
                    return Json(result);
                }
                else if (reportstyle == "3")
                {
                    dataTable = _iHouseKeepingService.SupChecklistFloor(fromDatere, toDatere, taskcodeexpan, attendantrepop, tasksheetpopre, dueoutonly);
                    var result = (from d in dataTable.AsEnumerable()
                                  select new
                                  {
                                      RoomNo = d["Room No."]?.ToString() ?? "",
                                      RoomStatus = d["Room Status"]?.ToString() ?? "",
                                      TimeIn = d["TimeIn"]?.ToString() ?? "",
                                      TimeOut = d["TimeOut"]?.ToString() ?? "",
                                      StatusChange = d["Status Change"]?.ToString() ?? "",
                                      Note = d["Note"]?.ToString() ?? "",
                                      Floor = d["Floor"]?.ToString() ?? "",
                                      Country = d["Country"]?.ToString() ?? "",
                                      Specials = d["Specials"]?.ToString() ?? "",
                                      RoomType = d["RoomType"]?.ToString() ?? "",
                                      ItemInventory = d["ItemInventory"]?.ToString() ?? ""
                                  }).ToList();
                    return Json(result);
                }

               

                return Json("");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult TaskAssignmentData(DateTime fromDate, string tasksheet, string zone)
        {
            tasksheet = tasksheet ?? "";
            zone = zone ?? "";
            if (string.IsNullOrWhiteSpace(zone))
            {
                List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());

                // Ghép các zone theo định dạng: HT'',''DM'',''A1'',''A2
                zone = string.Join("','", listzo.Select(z => z.Code));
            }
            try
            {
                DataTable dataTable = _iHouseKeepingService.TaskAssignmentData(fromDate, tasksheet, zone);

                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  TaskDate = d["TaskDate"].ToString() ?? "",
                                  TaskSheetNo = d["TaskSheetNo"].ToString() ?? "",
                                  Section = d["Section"].ToString() ?? "",
                                  Credits = d["Credits"].ToString() ?? "",
                                  Rooms = d["Rooms"].ToString() ?? "",
                                  CompletedOn = d["CompletedOn"].ToString() ?? "",
                                  Status = d["Status"].ToString() ?? "",
                                  TaskInstructions = d["TaskInstructions"].ToString() ?? "",
                                  ID = d["ID"].ToString() ?? "",
                                  FacilityTaskID = d["FacilityTaskID"].ToString() ?? "",
                                  Attendant = d["Attendant"].ToString() ?? "",
                                  Employee = d["Employee"].ToString() ?? "",
                                  TaskCode = d["TaskCode"].ToString() ?? ""
                              }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult SaveTaskSheetDetail(string taskcodenew, string attendantnew, DateTime businessDate,string userName)
        {
            taskcodenew = taskcodenew ?? "";
            attendantnew = attendantnew ?? "";
            List<hkpAttendantModel> listatt = PropertyUtils.ConvertToList<hkpAttendantModel>(hkpAttendantBO.Instance.FindAll());
            int attendantId = Convert.ToInt32(attendantnew);
            var filteredList = listatt.Where(x => x.ID == attendantId).ToList();
            hkpSectionModel hkpss = (hkpSectionModel)hkpSectionBO.Instance.FindByPrimaryKey(filteredList[0].SectionID);

            List<hkpFacilityTaskModel> listTasks = new List<hkpFacilityTaskModel>();

            var taskCodeList = taskcodenew.Split(',')
                                          .Where(x => !string.IsNullOrWhiteSpace(x))
                                          .Select(x => Convert.ToInt32(x.Trim()))
                                          .ToList();

            foreach (var code in taskCodeList)
            {
                var jkpFt = (hkpFacilityTaskModel)hkpFacilityTaskBO.Instance.FindByPrimaryKey(code);
                if (jkpFt != null)
                {
                    listTasks.Add(jkpFt);
                }
            }

            // Tạo chuỗi ngăn cách bởi dấu phẩy, ví dụ: "C,F,T"
            string result = string.Join(",", listTasks.Select(t => t.Code));

            List<hkpTaskSheetModel> listats = PropertyUtils.ConvertToList<hkpTaskSheetModel>(hkpTaskSheetBO.Instance.FindAll());

            var filteredListts = listats
            .Where(x => x.TaskSheetDate.Date == businessDate)
            .ToList();

            // Lấy số lớn nhất (nếu có)
            int maxTaskSheetNo = filteredListts.Any()
                ? filteredListts.Max(x => x.TaskSheetNo)
                : 0;


            // (Tuỳ chọn) Tạo mã mới tiếp theo
            int newTaskSheetNo = maxTaskSheetNo + 1;
            try
            {
                hkpTaskSheetModel modelts = new hkpTaskSheetModel
                {
                    TaskSheetDate = businessDate,
                    TaskSheetNote = "",
                    Status = false,
                    CreatedBy = userName,
                    CreatedDate = DateTime.Now,
                    SessionID = hkpss.ID.ToString(),
                    SessionName = hkpss.Code,
                    FacilityTaskID = taskcodenew,
                    FacilityTask = result,
                    TaskSheetNo = newTaskSheetNo,
                    AttendantID = attendantId,
                    UpdateBy = userName,
                    UpdateDate = DateTime.Now,
                };
                int tasksheetno = (int)hkpTaskSheetBO.Instance.Insert(modelts);

                var jkpFt = (hkpTaskSheetModel)hkpTaskSheetBO.Instance.FindByPrimaryKey(tasksheetno);

                return Json(jkpFt.TaskSheetNo);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        //[HttpGet]
        //public IActionResult ExpandedTaskSheet(DateTime fromDate)
        //{

        //    try
        //    {
        //        DataTable dataTable = _iHouseKeepingService.HKPGetTaskSheets(fromDate,page);

        //        var result = (from d in dataTable.AsEnumerable()
        //                      select new
        //                      {
        //                          TaskDate = d["TaskDate"].ToString() ?? "",
        //                          TaskSheetNo = d["TaskSheetNo"].ToString() ?? "",
        //                          Section = d["Section"].ToString() ?? "",
        //                          Credits = d["Credits"].ToString() ?? "",
        //                          Rooms = d["Rooms"].ToString() ?? "",
        //                          CompletedOn = d["CompletedOn"].ToString() ?? "",
        //                          Status = d["Status"].ToString() ?? "",
        //                          TaskInstructions = d["TaskInstructions"].ToString() ?? "",
        //                          ID = d["ID"].ToString() ?? "",
        //                          FacilityTaskID = d["FacilityTaskID"].ToString() ?? "",
        //                          Attendant = d["Attendant"].ToString() ?? "",
        //                          Employee = d["Employee"].ToString() ?? "",
        //                          TaskCode = d["TaskCode"].ToString() ?? ""
        //                      }).ToList();

        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message);
        //    }
        //}

        [HttpGet]
        public IActionResult GuestServiceStatusData(string servicestatsu, string room, string roomStatus)
        {
            try
            {
                DataTable dataTable = _iHouseKeepingService.GuestServiceStatusData(servicestatsu, room, roomStatus);

                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"].ToString() : "",
                                  Check = d["Check"]?.ToString() ?? "",
                                  RoomNo = d["RoomNo"]?.ToString() ?? "",
                                  RoomTypeCode = d["RoomTypeCode"]?.ToString() ?? "",
                                  hkstatus = d["HKStatus"]?.ToString() ?? "",
                                  fostatus = d["FOStatus"]?.ToString() ?? "",
                                  GuestServiceStatus = d["GuestServiceStatus"]?.ToString() ?? "",

                                  Floor = d["Floor"]?.ToString() ?? "",
                                  ReservationStatus = d["ReservationStatus"]?.ToString() ?? "",
                                  ResvID = d["ResvID"]?.ToString() ?? "",
                                  CreatedBy = d["CreatedBy"]?.ToString() ?? "",
                                  CreatedDate = d["CreatedDate"]?.ToString() ?? "",
                                  UpdatedBy = d["UpdatedBy"]?.ToString() ?? "",
                                  UpdatedDate = d["UpdatedDate"]?.ToString() ?? ""
                              }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult TaskSheetStatusData(DateTime fromDate, DateTime toDate,string attendant, string room)
        {
            attendant = attendant ?? "";
            room = room ?? "";
            try
            {
                DataTable dataTable = _iHouseKeepingService.TaskSheetStatusData(fromDate, toDate, attendant, room);

                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  colIndex = d["colIndex"]?.ToString() ?? "",
                                  RoomNo = d["RoomNo"]?.ToString() ?? "",
                                  RoomType = d["RoomType"]?.ToString() ?? "",
                                  hkstatusID = d["HKStatusID"]?.ToString() ?? "",
                                  fostatus = d["FOStatus"]?.ToString() ?? "",
                                  Attendant = d["Attendant"]?.ToString() ?? "",
                                  TaskDate = d["TaskDate"]?.ToString() ?? "",
                                  TimeIn = d["TimeIn"]?.ToString() ?? "",
                                  TimeOut = d["TimeOut"]?.ToString() ?? "",
                                  TasksheetID = d["TasksheetID"]?.ToString() ?? "",
                                  AttendantID = d["AttendantID"]?.ToString() ?? "",
                                  Complete = d["Complete"]?.ToString() ?? "",
                                  Rooms = d["Rooms"]?.ToString() ?? "",
                                  Credits = d["Credits"]?.ToString() ?? ""

                              }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult AttendantPointData(DateTime fromDate, DateTime toDate, string attendant)
        {
            try
            {
                var attendantPointData = hkpAttendantPointBO.GethkpAttendantPoint(fromDate, toDate, attendant);
                DataTable dtattendantPoint = PropertyUtils.ConvertToDataTable(attendantPointData);

                var result = (from d in dtattendantPoint.AsEnumerable()
                              select new
                              {
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"].ToString() : "",
                                  AttendantDate = d["AttendantDate"]?.ToString() ?? "",
                                  AttendantID = d["AttendantID"]?.ToString() ?? "",
                                  Points = d["Points"]?.ToString() ?? "",
                                  CreatedBy = d["CreatedBy"]?.ToString() ?? "",
                                  CreatedDate = d["CreatedDate"]?.ToString() ?? "",
                                  UpdatedBy = d["UpdatedBy"]?.ToString() ?? "",
                                  UpdatedDate = d["UpdatedDate"]?.ToString() ?? ""
                              }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult FloorPlanData(string block, string floor, string roomtype, string owner)
        {
            try
            {
                int maxRoomCount = 0;
                var tableData = new List<Dictionary<string, object>>();
                var statusCounts = new Dictionary<string, int>();

                // Lấy danh sách tầng
                List<FloorModel> listfloor = PropertyUtils.ConvertToList<FloorModel>(FloorBO.Instance.FindAll());

                // Lấy danh sách phòng và convert sang DataTable
                var roomcount = RoomBO.GetRoomCountPlan();
                DataTable dtTotalRoom = PropertyUtils.ConvertToDataTable(roomcount);

                // Tính toán các trạng thái phòng
                statusCounts["OCN"] = dtTotalRoom.Select("HKStatusID=0 AND FOStatus=1").Length;
                statusCounts["VCN"] = dtTotalRoom.Select("HKStatusID=1 AND FOStatus=1").Length;
                statusCounts["OOO"] = dtTotalRoom.Select("HKStatusID=5 AND HKFOStatus=FOStatus AND FOPerson=HKPersons").Length;
                statusCounts["OOS"] = dtTotalRoom.Select("HKStatusID=6 AND HKFOStatus=FOStatus AND FOPerson=HKPersons").Length;
                statusCounts["VD"] = dtTotalRoom.Select("HKStatusID=2 AND FOStatus=0").Length;
                statusCounts["VC"] = dtTotalRoom.Select("HKStatusID=4 AND FOStatus=0").Length;
                statusCounts["OD"] = dtTotalRoom.Select("HKStatusID=2 AND FOStatus=1").Length;
                statusCounts["OC"] = dtTotalRoom.Select("HKStatusID=4 AND FOStatus=1").Length;
                statusCounts["DISC"] = dtTotalRoom.Select("HKFOStatus <> FOStatus OR FOPerson <> HKPersons").Length;
                statusCounts["DO"] = dtTotalRoom.Select("HKStatusID=7").Length;
                statusCounts["TOTAL"] = statusCounts.Values.Sum();

                // Duyệt từng tầng
                foreach (var f in listfloor)
                {
                    var suffixes = (f.Name == "1") ? new[] { "-A" } : new[] { "-A", "-B" };

                    foreach (var suffix in suffixes)
                    {
                        var floorCode = f.Name + suffix;

                        // Lấy danh sách phòng theo tầng
                        var rooms = RoomBO.GetFloorPlan(block, suffix, floorCode);
                        if (rooms == null) continue;

                        if (rooms.Count > maxRoomCount)
                            maxRoomCount = rooms.Count;

                        var row = new Dictionary<string, object>
                        {
                            ["Floor"] = floorCode
                        };

                        // Lọc room theo floor và roomtype nếu có
                        var filteredRooms = rooms
                            .Where(r =>
                                (string.IsNullOrEmpty(floor) || r.Floor == floor) &&
                                (string.IsNullOrEmpty(roomtype) || r.RoomTypeID.ToString() == roomtype))
                            .ToList();

                        for (int i = 0; i < maxRoomCount; i++)
                        {
                            if (i < filteredRooms.Count)
                            {
                                var r = filteredRooms[i];

                                row[(i + 1).ToString()] = new
                                {
                                    RoomNo = r.RoomNo,
                                    HKStatusID = r.HKStatusID,
                                    FOStatus = r.FOStatus,
                                    HKFOStatus = r.HKFOStatus,
                                    FOPerson = r.FOPerson,
                                    HKPersons = r.HKPersons,
                                    RoomType = r.RoomName
                                };
                            }
                            else
                            {
                                row[(i + 1).ToString()] = null;
                            }
                        }

                        tableData.Add(row);
                    }
                }

                return Json(new
                {
                    MaxRoomCount = maxRoomCount,
                    Data = tableData,
                    StatusCounts = statusCounts
                });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }





        [HttpGet]
        public IActionResult RoomFacilityForecastData(DateTime fromDate, DateTime toDate, string zone)
        {
            try
            {
                DataTable dataTable = _iHouseKeepingService.RoomFacilityForecastData(fromDate, toDate, zone);

                // Tạo danh sách các ngày trong khoảng fromDate -> toDate
                var dateRange = Enumerable.Range(0, (toDate - fromDate).Days + 1)
                                          .Select(offset => fromDate.AddDays(offset))
                                          .ToList();

                var result = dataTable.AsEnumerable().Select(d =>
                {
                    var rowData = new Dictionary<string, object>
                    {
                        ["Statisticname"] = d["Statisticname"]?.ToString() ?? "",
                        ["Index"] = d["Index"]?.ToString() ?? ""
                    };

                    foreach (var date in dateRange)
                    {
                        string columnName = date.ToString("yyyy-MM-dd"); // đồng bộ với JS
                        string dtColName = date.ToString("yyyy/MM/dd");   // tên cột trong DataTable

                        if (dataTable.Columns.Contains(dtColName))
                        {
                            var cellValue = d[dtColName];
                            rowData[columnName] = cellValue == DBNull.Value ? "" : cellValue;
                        }
                        else
                        {
                            rowData[columnName] = ""; // nếu không có cột thì cũng để ""
                        }
                    }

                    return rowData;
                }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult RoomStatusData(int cleannon_room, int clean, int dirty, int pickup, int oocheck, int oscheck, int vacant, int occupied, int arrivals, int arrived, int stayover, int dayuse, int dueout, int departed, int notReserved, int departuredarr, string roomType, string zone, string roomFrom, string roomTo)
        {
            roomType = roomType ?? "";
            zone = zone ?? "";
            roomFrom = roomFrom ?? "";
            roomTo = roomTo ?? "";
            try
            {
                DataTable dataTable = _iHouseKeepingService.RoomStatusData(cleannon_room, clean, dirty, pickup, oocheck, oscheck, vacant, occupied, arrivals, arrived, stayover, dayuse, dueout, departed, notReserved, departuredarr, roomType, zone, roomFrom, roomTo);

                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"].ToString() : "",
                                  RoomNumber = !string.IsNullOrEmpty(d["Room Number"].ToString()) ? d["Room Number"].ToString() : "",
                                  RoomType = !string.IsNullOrEmpty(d["Room Type"].ToString()) ? d["Room Type"].ToString() : "",
                                  RoomStatus = !string.IsNullOrEmpty(d["Room Status"].ToString()) ? d["Room Status"].ToString() : "",
                                  FOStatus = !string.IsNullOrEmpty(d["FO Status"].ToString()) ? d["FO Status"].ToString() : "",
                                  Floor = !string.IsNullOrEmpty(d["Floor"].ToString()) ? d["Floor"].ToString() : "",
                                  RoomClass = !string.IsNullOrEmpty(d["Room Class"].ToString()) ? d["Room Class"].ToString() : "",
                                  HKStatusID = !string.IsNullOrEmpty(d["HKStatusID"].ToString()) ? d["HKStatusID"].ToString() : "",
                                  ReservationStatus = !string.IsNullOrEmpty(d["Reservation Status"].ToString()) ? d["Reservation Status"].ToString() : "",
                                  Arrival = !string.IsNullOrEmpty(d["Arrival"].ToString()) ? d["Arrival"].ToString() : "",
                                  RoomName = !string.IsNullOrEmpty(d["RoomName"].ToString()) ? d["RoomName"].ToString() : ""
                              }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult CheckLogStatus(List<int> id, DateTime fromDate, DateTime toDate, string username)
        {
            username = string.IsNullOrEmpty(username) ? "" : username;
            int idroom = id[0];
            RoomModel modelRoom = (RoomModel)RoomBO.Instance.FindByPrimaryKey(idroom);
            try
            {
                DataTable dataTable = _iHouseKeepingService.CheckLogStatus(modelRoom.RoomNo, fromDate, toDate, username);

                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RoomNo = !string.IsNullOrEmpty(d["RoomNo"].ToString()) ? d["RoomNo"].ToString() : "",
                                  OldValue = !string.IsNullOrEmpty(d["OldValue"].ToString()) ? d["OldValue"].ToString() : "",
                                  NewValue = !string.IsNullOrEmpty(d["NewValue"].ToString()) ? d["NewValue"].ToString() : "",
                                  UserName = !string.IsNullOrEmpty(d["UserName"].ToString()) ? d["UserName"].ToString() : "",
                                  Action = !string.IsNullOrEmpty(d["Action"].ToString()) ? d["Action"].ToString() : "",
                                  ComputerName = !string.IsNullOrEmpty(d["ComputerName"].ToString()) ? d["ComputerName"].ToString() : "",
                                  ChangeDate = !string.IsNullOrEmpty(d["ChangeDate"].ToString()) ? d["ChangeDate"].ToString() : ""
                              }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult DataHSdetail(DateTime datebunisess, string roomtype, string zone, string inputId)
        {
            roomtype = string.IsNullOrEmpty(roomtype) ? "0" : roomtype;
            zone = string.IsNullOrEmpty(zone) ? "0" : zone;

            try
            {
                List<object> result = new List<object>();

                if (inputId == "outOfService")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusSummaryOutOfServiceDetail(datebunisess, roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RoomName = d["Room Name"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",
                                  Direction = d["Direction"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                else if (inputId == "outOfOrder")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusSummaryOutOfOrderDetail(datebunisess, roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RoomName = d["Room Name"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",
                                 
                                  FromDate = d["From Date"]?.ToString() ?? "",
                                  ToDate = d["To Date"]?.ToString() ?? ""
                              }).ToList<object>();
                }
                else if (inputId == "stayoverRoom")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusActivityStayOverDetail(datebunisess, roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Guest = d["Guest"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",

                                  ReservationNumber = d["Reservation Number"]?.ToString() ?? "",
                                  RoomNo = d["Room No"]?.ToString() ?? "",
                                  ArrivalDate = d["Arrival Date"]?.ToString() ?? "",
                                  DepartureDate = d["Departure Date"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                else if (inputId == "depExpectedRoom")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusActivityDepartureExpectedDetail(datebunisess, roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Guest = d["Guest"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",

                                  ReservationNumber = d["Reservation Number"]?.ToString() ?? "",
                                  RoomNo = d["Room No"]?.ToString() ?? "",
                                  ArrivalDate = d["Arrival Date"]?.ToString() ?? "",
                                  DepartureDate = d["Departure Date"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                else if (inputId == "actualRoom")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusActivityDepartureActualDetail(datebunisess, roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Guest = d["Guest"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",

                                  ReservationNumber = d["Reservation Number"]?.ToString() ?? "",
                                  RoomNo = d["Room No"]?.ToString() ?? "",
                                  ArrivalDate = d["Arrival Date"]?.ToString() ?? "",
                                  DepartureDate = d["Departure Date"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                else if (inputId == "arrivalExpectedRoom")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusActivityArrivalExpectedDetail(datebunisess, roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Guest = d["Guest"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",

                                  ReservationNumber = d["Reservation Number"]?.ToString() ?? "",
                                  RoomNo = d["Room No"]?.ToString() ?? "",
                                  ArrivalDate = d["Arrival Date"]?.ToString() ?? "",
                                  DepartureDate = d["Departure Date"]?.ToString() ?? "",
                                  Adult = d["Adult"]?.ToString() ?? "",
                                  Child = d["Child"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                else if (inputId == "arrivalActualRoom")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusActivityArrivalActualDetail(datebunisess, roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Guest = d["Guest"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",

                                  ReservationNumber = d["Reservation Number"]?.ToString() ?? "",
                                  RoomNo = d["Room No"]?.ToString() ?? "",
                                  ArrivalDate = d["Arr Date"]?.ToString() ?? "",
                                  DepartureDate = d["Dep Date"]?.ToString() ?? "",
                                  Adult = d["Adult"]?.ToString() ?? "",
                                  Child = d["Child"]?.ToString() ?? "",
                                  EmployeeCI = d["Employee CI"]?.ToString() ?? "",
                              
                              }).ToList<object>();
                }
                else if (inputId == "extendedStaysRoom")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusActivityExtendedStayDetail(datebunisess, roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Guest = d["Guest"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",

                                  ReservationNumber = d["Reservation Number"]?.ToString() ?? "",
                                  RoomNo = d["Room No"]?.ToString() ?? "",
                                  ArrivalDate = d["Arr Date"]?.ToString() ?? "",
                                  DepartureDate = d["Dep Date"]?.ToString() ?? "",
                                  Adult = d["Adult"]?.ToString() ?? "",
                                  Child = d["Child"]?.ToString() ?? "",
                        

                              }).ToList<object>();
                }
                else if (inputId == "earlyDeparturesRoom")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusActivityEarlyDepartureDetail(datebunisess, roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Guest = d["Guest"]?.ToString() ?? "",
                                

                                  ReservationNumber = d["Reservation Number"]?.ToString() ?? "",
                                  RoomNo = d["Room No"]?.ToString() ?? "",
                                  ArrivalDate = d["Arr Date"]?.ToString() ?? "",
                                  DepartureDate = d["Dep Date"]?.ToString() ?? "",
                               
                                 

                              }).ToList<object>();
                }
                else if (inputId == "dayUseRoom")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusActivityDayUseRoomDetail(datebunisess, roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Guest = d["Guest"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",

                                  ReservationNumber = d["Reservation Number"]?.ToString() ?? "",
                                  RoomNo = d["Room No"]?.ToString() ?? "",
                                  ArrivalDate = d["Arr Date"]?.ToString() ?? "",
                                  DepartureDate = d["Dep Date"]?.ToString() ?? "",



                              }).ToList<object>();
                }
                else if (inputId == "walkinRoom")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusActivityWalkInRoomDetail(datebunisess, roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Guest = d["Guest"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",

                                  ReservationNumber = d["Reservation Number"]?.ToString() ?? "",
                                  RoomNo = d["Room No"]?.ToString() ?? "",
                                  ArrivalDate = d["Arr Date"]?.ToString() ?? "",
                                  DepartureDate = d["Dep Date"]?.ToString() ?? "",



                              }).ToList<object>();
                }
                else if (inputId == "cleanVacant")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusHKVacantCleanDetail(roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                
                                  RoomName = d["Room Name"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",
                                  Zone = d["Zone"]?.ToString() ?? "",



                              }).ToList<object>();
                }
                else if (inputId == "cleanOccupied")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusHKInspectedOCCDetail(roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RoomName = d["Room Name"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",
                                  Zone = d["Zone"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                else if (inputId == "cleannonVacant")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusHKVCNDetail(roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RoomName = d["Room Name"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",
                                  Zone = d["Zone"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                else if (inputId == "cleannonOccupied")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusHKCleanOCCDetail(roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RoomName = d["Room Name"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",
                                  Zone = d["Zone"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                else if (inputId == "doeoutOccupied")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusActivityDueOutDetail(roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Guest = d["Guest"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",

                                  ReservationNumber = d["Reservation Number"]?.ToString() ?? "",
                                  RoomNo = d["Room No"]?.ToString() ?? "",
                                  ArrivalDate = d["Arr Date"]?.ToString() ?? "",
                                  DepartureDate = d["Dep Date"]?.ToString() ?? "",



                              }).ToList<object>();
                }
                else if (inputId == "dirtyVacant")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusHKVacantDirtyDetail(roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RoomName = d["Room Name"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",
                                  Zone = d["Zone"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                else if (inputId == "dirtyOccupied")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusHKDirtyOCCDetail(roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RoomName = d["Room Name"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",
                                  Zone = d["Zone"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                else if (inputId == "outoforderVacant")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusHKOutOfOrderVacantDetail(roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RoomName = d["Room Name"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",
                                  Zone = d["Zone"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                else if (inputId == "outofserviveVacant")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusHKOutOfServiceVacantDetail(roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RoomName = d["Room Name"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",
                                  Zone = d["Zone"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                else if (inputId == "outofserviveOccupied")
                {
                    DataTable dataTable = _iHouseKeepingService.StatusHKOutOfServiceOCCDetail(roomtype, zone);
                    result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RoomName = d["Room Name"]?.ToString() ?? "",
                                  RoomType = d["Room Type"]?.ToString() ?? "",
                                  Zone = d["Zone"]?.ToString() ?? "",
                              }).ToList<object>();
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }



        [HttpGet]
        public IActionResult HouseStatusData( DateTime datebunisess,  string roomtype,string zone)
        {
            roomtype = string.IsNullOrEmpty(roomtype) ? "0" : roomtype;

            zone = string.IsNullOrEmpty(zone) ? "0" : zone;
            try
            {
                DataTable dataTable = _iHouseKeepingService.SummaryTotalPhysicalRoom(roomtype, zone);

                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  TotalPhysicalRoom = !string.IsNullOrEmpty(d["TotalPhysicalRoom"].ToString()) ? d["TotalPhysicalRoom"].ToString() : "",                                
                              }).ToList();

                DataTable dataTable1 = _iHouseKeepingService.StatusSummaryOutOfOrder(datebunisess,roomtype, zone);

                var result1 = (from d in dataTable1.AsEnumerable()
                              select new
                              {
                                  OutOfOrder = !string.IsNullOrEmpty(d["OutOfOrder"].ToString()) ? d["OutOfOrder"].ToString() : "",
                              }).ToList();

                DataTable dataTable2 = _iHouseKeepingService.SummaryOutOfService(datebunisess, roomtype, zone);

                var result2 = (from d in dataTable2.AsEnumerable()
                               select new
                               {
                                   OutOfService = !string.IsNullOrEmpty(d["OutOfService"].ToString()) ? d["OutOfService"].ToString() : "",
                               }).ToList();

                DataTable dataTable3 = _iHouseKeepingService.ActivityStayOver(datebunisess, roomtype, zone);

                var result3 = (from d in dataTable3.AsEnumerable()
                               select new
                               {
                                   TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                   TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",
                               }).ToList();

                DataTable dataTable4 = _iHouseKeepingService.ActivityDepartureExpected(datebunisess, roomtype, zone);

                var result4 = (from d in dataTable4.AsEnumerable()
                               select new
                               {
                                   TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                   TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",
                               }).ToList();

                DataTable dataTable5 = _iHouseKeepingService.ActivityDepartureActual(datebunisess, roomtype, zone);

                var result5 = (from d in dataTable5.AsEnumerable()
                               select new
                               {
                                   TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                   TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",
                               }).ToList();

                DataTable dataTable6 = _iHouseKeepingService.ActivityArrivalExpected(datebunisess, roomtype, zone);

                var result6 = (from d in dataTable6.AsEnumerable()
                               select new
                               {
                                   TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                   TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",
                               }).ToList();

                DataTable dataTable7 = _iHouseKeepingService.ActivityArrivalActual(datebunisess, roomtype, zone);

                var result7 = (from d in dataTable7.AsEnumerable()
                               select new
                               {
                                   TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                   TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",
                               }).ToList();

                DataTable dataTable8 = _iHouseKeepingService.ActivityExtendedStay(datebunisess, roomtype, zone);

                var result8 = (from d in dataTable8.AsEnumerable()
                               select new
                               {
                                   TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                   TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",
                               }).ToList();

                DataTable dataTable9 = _iHouseKeepingService.ActivityEarlyDeparture(datebunisess, roomtype, zone);

                var result9 = (from d in dataTable9.AsEnumerable()
                               select new
                               {
                                   TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                   TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",
                               }).ToList();

                DataTable dataTable10 = _iHouseKeepingService.ActivityDayUseRoom(datebunisess, roomtype, zone);

                var result10 = (from d in dataTable10.AsEnumerable()
                               select new
                               {
                                   TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                   TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",
                               }).ToList();

                DataTable dataTable11 = _iHouseKeepingService.StatusHKInspected( roomtype, zone);

                var result11 = (from d in dataTable11.AsEnumerable()
                                select new
                                {
                                    Vacant = !string.IsNullOrEmpty(d["Vacant"].ToString()) ? d["Vacant"].ToString() : "",
                                    occ = !string.IsNullOrEmpty(d["OCC"].ToString()) ? d["OCC"].ToString() : "",
                                }).ToList();


                DataTable dataTable12 = _iHouseKeepingService.StatusHKClean(roomtype, zone);

                var result12 = (from d in dataTable12.AsEnumerable()
                                select new
                                {
                                    Vacant = !string.IsNullOrEmpty(d["Vacant"].ToString()) ? d["Vacant"].ToString() : "",
                                    occ = !string.IsNullOrEmpty(d["OCC"].ToString()) ? d["OCC"].ToString() : "",
                                }).ToList();

                DataTable dataTable13 = _iHouseKeepingService.StatusHKDirty(roomtype, zone);

                var result13 = (from d in dataTable13.AsEnumerable()
                                select new
                                {
                                    Vacant = !string.IsNullOrEmpty(d["Vacant"].ToString()) ? d["Vacant"].ToString() : "",
                                    occ = !string.IsNullOrEmpty(d["OCC"].ToString()) ? d["OCC"].ToString() : "",
                                }).ToList();

                DataTable dataTable14 = _iHouseKeepingService.StatusHKOutOfOrder(roomtype, zone);

                var result14 = (from d in dataTable14.AsEnumerable()
                                select new
                                {
                                    Vacant = !string.IsNullOrEmpty(d["Vacant"].ToString()) ? d["Vacant"].ToString() : "",
                                    occ = !string.IsNullOrEmpty(d["OCC"].ToString()) ? d["OCC"].ToString() : "",
                                }).ToList();

                DataTable dataTable15 = _iHouseKeepingService.StatusHKOutOfService(roomtype, zone);

                var result15 = (from d in dataTable15.AsEnumerable()
                                select new
                                {
                                    Vacant = !string.IsNullOrEmpty(d["Vacant"].ToString()) ? d["Vacant"].ToString() : "",
                                    occ = !string.IsNullOrEmpty(d["OCC"].ToString()) ? d["OCC"].ToString() : "",
                                }).ToList();

                DataTable dataTable16 = _iHouseKeepingService.StatusEndOfDayGroupAndBlock( datebunisess, roomtype, zone);

                var result16 = (from d in dataTable16.AsEnumerable()
                                select new
                                {
                                    TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                    TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",
                                }).ToList();

                DataTable dataTable17 = _iHouseKeepingService.StatusEndOfDayIndividual(datebunisess, roomtype, zone);

                var result17 = (from d in dataTable17.AsEnumerable()
                                select new
                                {
                                    TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                    TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",
                                }).ToList();

                DataTable dataTable18 = _iHouseKeepingService.StatusEndOfDayCHU(datebunisess, roomtype, zone);

                var result18 = (from d in dataTable18.AsEnumerable()
                                select new
                                {
                                    TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                    TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",
                                }).ToList();

                DataTable dataTable19 = _iHouseKeepingService.StatusEndOfDayMaxOccTonight(datebunisess, roomtype, zone);

                var result19 = (from d in dataTable19.AsEnumerable()
                                select new
                                {
                                    TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                    TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",
                                }).ToList();

                DataTable dataTable20 = _iHouseKeepingService.StatusEndOfDayRoomRevenue(datebunisess, roomtype, zone);

                var result20 = (from d in dataTable20.AsEnumerable()
                                select new
                                {
                                    Amount = !string.IsNullOrEmpty(d["Amount"].ToString()) ? d["Amount"].ToString() : "",
                                   
                                }).ToList();


                DataTable dataTable21 = _iHouseKeepingService.StatusEndOfDayMaxOccTonightVIP(datebunisess, roomtype, zone);

                var result21 = (from d in dataTable21.AsEnumerable()
                                   select new
                                   {
                                       vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                   }).ToList();


                DataTable dataTable22 = _iHouseKeepingService.StatusEndOfDayMaxOccTonightVIP(datebunisess, roomtype, zone);

                var result22 = (from d in dataTable22.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();

                DataTable dataTable23 = _iHouseKeepingService.StatusEndOfDayIndividualVIP(datebunisess, roomtype, zone);

                var result23 = (from d in dataTable23.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();

                DataTable dataTable24 = _iHouseKeepingService.StatusEndOfDayGroupAndBlockVIP(datebunisess, roomtype, zone);

                var result24 = (from d in dataTable24.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();


                DataTable dataTable25 = _iHouseKeepingService.StatusEndOfDayCHUVIP(datebunisess, roomtype, zone);

                var result25 = (from d in dataTable25.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();


                DataTable dataTable26 = _iHouseKeepingService.StatusActivityDepartureExpectedVIP(datebunisess, roomtype, zone);

                var result26 = (from d in dataTable26.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();

                DataTable dataTable27 = _iHouseKeepingService.StatusActivityStayOverVIP(datebunisess, roomtype, zone);

                var result27 = (from d in dataTable27.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();


                DataTable dataTable28 = _iHouseKeepingService.StatusActivityDepartureActualVIP(datebunisess, roomtype, zone);

                var result28 = (from d in dataTable28.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();


                DataTable dataTable29 = _iHouseKeepingService.StatusActivityArrivalExpectedVIP(datebunisess, roomtype, zone);

                var result29 = (from d in dataTable29.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();
                DataTable dataTable30 = _iHouseKeepingService.StatusActivityArrivalActualVIP(datebunisess, roomtype, zone);

                var result30 = (from d in dataTable30.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();

                DataTable dataTable31 = _iHouseKeepingService.StatusActivityExtendedStayVIP(datebunisess, roomtype, zone);

                var result31 = (from d in dataTable31.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();

                DataTable dataTable32 = _iHouseKeepingService.StatusActivityEarlyDepartureVIP(datebunisess, roomtype, zone);

                var result32 = (from d in dataTable32.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();


                DataTable dataTable33 = _iHouseKeepingService.StatusActivityDayUseRoomVIP(datebunisess, roomtype, zone);

                var result33 = (from d in dataTable33.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();
                DataTable dataTable34 = _iHouseKeepingService.StatusActivityWakeInRoomVIP(datebunisess, roomtype, zone);

                var result34 = (from d in dataTable34.AsEnumerable()
                                select new
                                {
                                    vip = !string.IsNullOrEmpty(d["vip"].ToString()) ? d["vip"].ToString() : "",

                                }).ToList();
                DataTable dataTable35 = _iHouseKeepingService.StatusActivityWalkInRoom(datebunisess, roomtype, zone);

                var result35 = (from d in dataTable35.AsEnumerable()
                                select new
                                {
                                    TotalRooms = !string.IsNullOrEmpty(d["TotalRooms"].ToString()) ? d["TotalRooms"].ToString() : "",
                                    TotalPersons = !string.IsNullOrEmpty(d["TotalPersons"].ToString()) ? d["TotalPersons"].ToString() : "",

                                }).ToList();
                List<RoomModel> roomzone = RoomBO.GetRoomZone(roomtype, zone)
                                         .Where(r => r.ID != 0)
                                         .ToList();

                return Json(new
                {
                    TotalPhysicalRoom = result,
                    OutOfOrder = result1,
                    OutOfService = result2,
                    StayOver = result3,
                    DepartureExpected = result4,
                    DepartureActual = result5,
                    ArrivalExpected = result6,
                    ArrivalActual = result7,
                    ExtendedStay = result8,
                    EarlyDeparture = result9,
                    DayUseRoom = result10,

                    HKInspected = result11,
                    HKClean = result12,
                    HKDirty = result13,
                    HKOutOfOrder = result14,
                    HKOutOfService = result15,

                    EndOfDayGroupAndBlock = result16,
                    EndOfDayIndividual = result17,
                    EndOfDayCHU = result18,
                    EndOfDayMaxOccTonight = result19,
                    EndOfDayRoomRevenue = result20,

                    EndOfDayMaxOccTonightVIP = result21,
                    EndOfDayMaxOccTonightVIPCount = result22,
                    EndOfDayIndividualVIP = result23,
                    EndOfDayGroupAndBlockVIP = result24,
                    EndOfDayCHUVIP = result25,

                    DepartureExpectedVIP = result26,
                    StayOverVIP = result27,
                    DepartureActualVIP = result28,
                    ArrivalExpectedVIP = result29,
                    ArrivalActualVIP = result30,
                    ExtendedStayVIP = result31,
                    EarlyDepartureVIP = result32,
                    DayUseRoomVIP = result33,
                    WakeInRoomVIP = result34,

                    WalkInRoom = result35,
                    Roomzone = roomzone
                });
                


            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult UpdateAttendantPoint(string  ID,DateTime Date, int  AttendantID, string UserName, int Point)
        {
            try
            {

                hkpAttendantPointModel modelH = new hkpAttendantPointModel
                {
                    AttendantDate = Date,
                    AttendantID = AttendantID,
                    Points = Point,
                    CreatedBy = UserName,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = UserName,
                    UpdatedDate = DateTime.Now
                };

                if (string.IsNullOrEmpty(ID)) 
                {
                    hkpAttendantPointBO.Instance.Insert(modelH);
                }
                else
                {
                    hkpAttendantPointModel modelhkpAtten = (hkpAttendantPointModel)hkpAttendantPointBO.Instance.FindByPrimaryKey(Convert.ToInt32(ID));


                        modelhkpAtten.AttendantDate = Date;
                    modelhkpAtten.UpdatedBy = UserName;
                    modelhkpAtten.Points = Point;
                    modelhkpAtten.AttendantID = AttendantID;
                    hkpAttendantPointBO.Instance.Update(modelhkpAtten);
                }



                return Json(new { success = true, message = "AttendantPoint insert successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteAttendantPoint(string ID)
        {
            try
            {

                hkpAttendantPointBO.Instance.Delete(Convert.ToInt32(ID));



                return Json(new { success = true, message = "AttendantPoint delete successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult EditGuestService(List<RoomModel> ids,int servicestatus)
        {
            try
            {
         

                foreach (var id in ids)
                {
                    RoomModel modelRoom = (RoomModel)RoomBO.Instance.FindByPrimaryKey(id.ID);
                    modelRoom.GuestServiceStatus = servicestatus;
                    modelRoom.TurndownStatus = servicestatus;


                    RoomBO.Instance.Update(modelRoom);
                }

             
                return Json(new { success = true, message = "Room status updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        [HttpPost]
        public IActionResult UpdateRoomStatus(List<int> ids, int status, string loginName)
        {
            try
            {
                string hostName = Dns.GetHostName();
                List<UsersModel> list = PropertyUtils.ConvertToList<UsersModel>(UsersBO.Instance.FindByAttribute("LoginName", loginName));
                int userId = list.Count > 0 ? list[0].ID : 0;

                List<string> failedRooms = new List<string>();

                foreach (var id in ids)
                {
                    RoomModel modelRoom = (RoomModel)RoomBO.Instance.FindByPrimaryKey(id);
                    if (modelRoom == null)
                    {
                        failedRooms.Add($"Room ID {id} not found.");
                        continue;
                    }

                    // Nếu phòng đang OOS hoặc OOO (giả định là status 5 hoặc 6)
                    if (modelRoom.HKStatusID == 5 || modelRoom.HKStatusID == 6)
                    {
                        failedRooms.Add($"Cannot change status for Out Of Order/Service room: {modelRoom.RoomNo}");
                        continue;
                    }

                    // Ghi lịch sử thay đổi
                    InsertHistory(modelRoom.RoomNo, modelRoom.HKStatusID.ToString(), status.ToString(), DateTime.Now, hostName, "Manual", modelRoom.ID, "Room", loginName);

                    modelRoom.HKStatusID = status;
                    modelRoom.UpdateDate = DateTime.Now;
                    modelRoom.UserUpdateID = userId;

                    RoomBO.Instance.Update(modelRoom);
                }

                if (failedRooms.Count > 0)
                {
                    return Json(new { success = false, message = string.Join("\n", failedRooms) });
                }

                return Json(new { success = true, message = "Room status updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public IActionResult UpdateRoomStatusPopup(int status, List<int> roomIds, int isFromTo, string loginName)
        {
            try
            {
                string hostName = Dns.GetHostName();
                List<UsersModel> list = PropertyUtils.ConvertToList<UsersModel>(
                    UsersBO.Instance.FindByAttribute("LoginName", loginName)
                );
                int userId = list.Count > 0 ? list[0].ID : 0;

                List<string> failedRooms = new List<string>();

                if (isFromTo == 0)
                {
                    foreach (var id in roomIds)
                    {
                        RoomModel modelRoom = (RoomModel)RoomBO.Instance.FindByPrimaryKey(id);
                        if (modelRoom == null)
                        {
                            failedRooms.Add($"Room ID {id} not found.");
                            continue;
                        }

                        InsertHistory(modelRoom.RoomNo, modelRoom.HKStatusID.ToString(), status.ToString(), DateTime.Now, hostName, "Manual", modelRoom.ID, "Room", loginName);

                        modelRoom.HKStatusID = status;
                        modelRoom.UpdateDate = DateTime.Now;
                        modelRoom.UserUpdateID = userId;

                        RoomBO.Instance.Update(modelRoom);
                    }
                }
                else
                {
                    if (roomIds.Count < 2)
                        return Json(new { success = false, message = "Missing From and To Room IDs." });

                    int fromId = roomIds[0];
                    int toId = roomIds[1];
                    int minId = Math.Min(fromId, toId);
                    int maxId = Math.Max(fromId, toId);

                    // Lấy tất cả phòng
                    List<RoomModel> allRooms = PropertyUtils.ConvertToList<RoomModel>(RoomBO.Instance.FindAll());

                    var selectedRooms = allRooms
                    .Where(r => int.Parse(r.RoomNo) >= minId && int.Parse(r.RoomNo) <= maxId)
                    .OrderBy(r => r.ID)
                    .ToList();


                    foreach (var modelRoom in selectedRooms)
                    {
                       

                        InsertHistory(modelRoom.RoomNo, modelRoom.HKStatusID.ToString(), status.ToString(), DateTime.Now, hostName, "Manual", modelRoom.ID, "Room", loginName);

                        modelRoom.HKStatusID = status;
                        modelRoom.UpdateDate = DateTime.Now;
                        modelRoom.UserUpdateID = userId;

                        RoomBO.Instance.Update(modelRoom);
                    }
                }

                if (failedRooms.Count > 0)
                {
                    return Json(new { success = false, message = string.Join("\n", failedRooms) });
                }

                return Json(new { success = true, message = "Room status updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public static void InsertHistory(string roomNo, string oldValue, string newValue, DateTime systemDate, string computerName, string action, int objectID, string tableName,string loginName)
        {
            RoomStatusHistoryModel modelH = new RoomStatusHistoryModel();
            modelH.ChangeDate = systemDate;
            modelH.OldValue = oldValue;
            modelH.NewValue = newValue;
            modelH.RoomNo = roomNo;
            modelH.ComputerName = computerName;
            modelH.UserName = loginName;
            modelH.Action = action;
            modelH.ObjectID = objectID;
            modelH.TableName = tableName;
            RoomStatusHistoryBO.Instance.Insert(modelH);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoomID(int ID)
        {
            try
            {
                RoomModel room = (RoomModel)RoomBO.Instance.FindByPrimaryKey(ID);
                return Json(room);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult RoomPlanData(DateTime fromDate, DateTime toDate, int orderbyroom, string owner)
        {
            owner = string.IsNullOrEmpty(owner) ? "" : owner;

            try
            {
                DataTable dataTable = _iHouseKeepingService.RoomPlanData(fromDate, toDate, orderbyroom, owner);

                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RoomNo = d["RoomNo"].ToString() ?? "",
                                  RoomID = d["RoomID"].ToString() ?? "",
                                  RoomTypeCode = d["RoomTypeCode"].ToString() ?? "",
                                  Status = d["Status"].ToString() ?? "",
                                  FOStatus = d["FOStatus"].ToString() ?? "",
                                  HKStatus = d["HKStatus"].ToString() ?? "",
                                  ZoneCode = d["ZoneCode"].ToString() ?? "",
                                  Smoking = d["Smoking"].ToString() ?? "",
                                  Floor = d["Floor"].ToString() ?? "",
                                  RoomNumber = d["RoomNumber"].ToString() ?? "",
                                  Comment = d["Comment"].ToString() ?? "",
                                  LastName = d["LastName"].ToString() ?? "",
                                  ArrivalDate = d["ArrivalDate"].ToString() ?? "",
                                  DepartureDate = d["DepartureDate"].ToString() ?? "",
                                  ConfirmationNo = d["ConfirmationNo"].ToString() ?? "",
                                  CreateDate = d["CreateDate"].ToString() ?? "",
                                  PaymentMethod = d["PaymentMethod"].ToString() ?? "",
                                  Agent = d["Agent"].ToString() ?? "",
                                  Company = d["Company"].ToString() ?? "",
                                  ShareRoomName = d["ShareRoomName"].ToString() ?? "",
                                  ReservationID = d["ReservationID"].ToString() ?? "",
                                  ReservationStatus = d["ReservationStatus"].ToString() ?? "",
                                  OOOID = d["OOOID"].ToString() ?? "",
                                  Code = d["Code"].ToString() ?? "",
                                  Description = d["Description"].ToString() ?? "",
                                  OOOStatus = d["OOOStatus"].ToString() ?? "",
                                  DisplaySequence = d["DisplaySequence"].ToString() ?? "",
                                  Type = d["Type"].ToString() ?? ""
                              }).ToList();


                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

    }
}

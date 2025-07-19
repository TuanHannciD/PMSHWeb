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
            ViewBag.businesDate = businessDateModel[0].BusinessDate;
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

        [HttpPost]
        public IActionResult HouseStatusData( DateTime datebunisess,  string roomtype,string zone)
        {
            roomtype = string.IsNullOrEmpty(roomtype) ? "0" : roomtype;
      
  
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

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
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

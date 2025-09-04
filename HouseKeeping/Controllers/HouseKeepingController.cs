using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using DevExpress.Charts.Native;
using DevExpress.Data.ODataLinq;
using DevExpress.DataAccess.DataFederation;
using DevExpress.XtraCharts.Native;
using HouseKeeping.Commons.Helpers;
using HouseKeeping.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static DevExpress.CodeParser.CodeStyle.Formatting.Rules;
using DevExpress.DataAccess.DataFederation;
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
            List<BusinessDateModel> businessDateModel = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
            ViewBag.BusinessDate = businessDateModel[0].BusinessDate;
            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;
            return View();
        }
        public IActionResult RoomFacilityForecast()
        {
            List<BusinessDateModel> businessDateModel = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
            ViewBag.BusinessDate = businessDateModel[0].BusinessDate;
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
            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;
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
            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;
            return View();
        }

        public IActionResult MakeupServiceRoom()
        {
            List<BusinessDateModel> businessDateModel = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
            ViewBag.BusinessDate = businessDateModel[0].BusinessDate;
            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;

            List<hkpSectionModel> listsshkp = PropertyUtils.ConvertToList<hkpSectionModel>(hkpSectionBO.Instance.FindAll());
            ViewBag.hkpSectionList = listsshkp;

            List<RoomTypeModel> listrt = PropertyUtils.ConvertToList<RoomTypeModel>(RoomTypeBO.Instance.FindAll());
            ViewBag.RoomTypeList = listrt;

            List<hkpAttendantModel> listatt = PropertyUtils.ConvertToList<hkpAttendantModel>(hkpAttendantBO.Instance.FindAll());
            ViewBag.hkpAttendantList = listatt;

            List<hkpFacilityTaskModel> listhkpft = PropertyUtils.ConvertToList<hkpFacilityTaskModel>(hkpFacilityTaskBO.Instance.FindAll());
            ViewBag.hkpFacilityTaskList = listhkpft;

            return View();
        }

        public IActionResult RoomAttendentDailyWorksheet(bool isPopup = false, string tasksheetnew = null)
        {
            List<hkpAttendantModel> listatt = PropertyUtils.ConvertToList<hkpAttendantModel>(hkpAttendantBO.Instance.FindAll());
            ViewBag.hkpAttendantList = listatt;

            List<BusinessDateModel> businessDateModel = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
            ViewBag.BusinessDate = businessDateModel[0].BusinessDate;

            List<hkpTaskSheetModel> listhkpts = PropertyUtils
                .ConvertToList<hkpTaskSheetModel>(hkpTaskSheetBO.Instance.FindAll())
                .Where(x => x.TaskSheetDate.Date == businessDateModel[0].BusinessDate.Date)
                .ToList();
            ViewBag.hkpTaskSheetList = listhkpts;

            if (isPopup)
            {
                ViewBag.TaskSheetNew = tasksheetnew;
                return PartialView("RoomAttendentDailyWorksheet");
            }

            return View();
        }
        public IActionResult TurndownTasksheet()
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
            List<hkpAttendantModel> listatt = PropertyUtils.ConvertToList<hkpAttendantModel>(hkpAttendantBO.Instance.FindAll());
            ViewBag.hkpAttendantList = listatt;
            List<RoomTypeModel> listrt = PropertyUtils.ConvertToList<RoomTypeModel>(RoomTypeBO.Instance.FindAll());
            ViewBag.RoomTypeList = listrt;

            List<FloorModel> listfloor = PropertyUtils.ConvertToList<FloorModel>(FloorBO.Instance.FindAll());
            ViewBag.FloorList = listfloor;
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
            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;
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

        [HttpPost]
        public IActionResult AutoMakeupServiceRoom(DateTime taskdateauto, string taskcodeauto, string attendantauto, string maxcreditauto, string userName, string roomIDs)
        {
            taskcodeauto = taskcodeauto ?? "";
            attendantauto = attendantauto ?? "";
            maxcreditauto = maxcreditauto ?? "";
            userName = userName ?? "";
            roomIDs = roomIDs ?? "";
            string[] _pSectionID = attendantauto
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();

            // Lấy toàn bộ danh sách Attendant từ DB
            List<hkpAttendantModel> listatt = PropertyUtils.ConvertToList<hkpAttendantModel>(
                hkpAttendantBO.Instance.FindAll()
            );

            // ✅ Lấy SectionID theo AttendantID trong _pSectionID
            var sectionIDs = listatt
                .Where(a => _pSectionID.Contains(a.ID.ToString()))
                .Select(a => a.SectionID)
                .Distinct()
                .ToList();
            var sectionIDsArray = sectionIDs.Select(x => x.ToString()).ToArray();

            SplitSection(sectionIDsArray);
            roomIDs = GetSplitString(roomIDs);
            string _ListSection = "";
            _ListSection = GetSplitString(_ListSection);
            try
            {
                DataTable Source = _iHouseKeepingService.AutoMakeupServiceRoom(roomIDs, _ListSection);
                if (Source.Rows.Count == 0)
                {
                    return Json(new { success = false, message = "Please choose attendant other" });
                }
                int[] _AttendantID = null;

                if (!string.IsNullOrEmpty(attendantauto))
                {
                    _AttendantID = attendantauto
                        .Split(',')                // tách thành ["1","2","3"]
                        .Select(x => int.Parse(x)) // convert sang int
                        .ToArray();                // ra int[]
                }
                if (_AttendantID == null)
                {
                    _SectionID = Section(Source, ref _ListSection);
                    _AttendantID = GetAttendant(ref attendantauto);
                }

                #region 2.Process Attendant by Section
                for (int i = 0; i < _SectionID.Length; i++)
                {
                    ProcessSourceAutoMakeupServiceRoom(Source, int.Parse(_SectionID[i].ToString()), attendantauto, taskdateauto, userName, taskcodeauto, maxcreditauto);
                }
                #endregion
                return Json(new { success = true, message = "Successfully" });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        private IActionResult ProcessSourceAutoMakeupServiceRoom(DataTable Source, int _secID, string attendantauto, DateTime taskdateauto, string userName, string taskcodeauto, string maxcreditauto)
        {
            var getdetailts = hkpAttendantBO.GethkpAttendantProcessSource(_secID, GetSplitString(attendantauto));
            DataTable dt = PropertyUtils.ConvertToDataTable(getdetailts);
            //  DataTable dt = TextUtils.Select("SELECT DISTINCT ID FROM dbo.hkpAttendant WITH (NOLOCK) WHERE SectionID = " + _secID + " AND ID IN ('" + ClassReservation.GetSplitString(_ListAttendant) + "') ");
            if (dt.Rows.Count > 0)
            {
                int _NoOfRoom = 0;
                decimal _pRoom = 0;
                DataTable dtTemp = null;
                GetDataTable_By(Source, _secID.ToString(), ref dtTemp);
                _pRoom = Convert.ToDecimal(dtTemp.Rows.Count.ToString()) / Convert.ToDecimal(dt.Rows.Count.ToString());
                _NoOfRoom = Convert.ToInt32(_pRoom);
                //_NoOfRoom = dtTemp.Rows.Count / dt.Rows.Count;
                int[] _TaskCode = null;
                if (!string.IsNullOrWhiteSpace(taskcodeauto))
                {
                    _TaskCode = taskcodeauto
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => int.TryParse(id, out int val) ? val : 0)
                        .Where(val => val > 0)
                        .ToArray();
                }

                // Lấy danh sách tất cả FacilityTask
                List<hkpFacilityTaskModel> listhkpft = PropertyUtils.ConvertToList<hkpFacilityTaskModel>(
                    hkpFacilityTaskBO.Instance.FindAll()
                );

                // Lọc lấy Instructions và nối thành chuỗi
                string instructionsList = "";
                if (_TaskCode != null && _TaskCode.Length > 0)
                {
                    instructionsList = string.Join(", ",
                        listhkpft
                            .Where(t => _TaskCode.Contains(t.ID)) // hoặc TaskCode nếu model có
                            .Select(t => t.Instructions)
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                    );
                }
                string codeList = "";
                if (_TaskCode != null && _TaskCode.Length > 0)
                {
                    codeList = string.Join(", ",
                        listhkpft
                            .Where(t => _TaskCode.Contains(t.ID)) // lọc theo ID
                            .Select(t => t.Code) // lấy Code
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                    );
                }
                #region 4.Model
                hkpTaskSheetModel mTS = new hkpTaskSheetModel();
                mTS.TaskSheetDate = taskdateauto;
                //mTS.TaskSheetNote = txtDescription.Text.Trim();
                mTS.Status = false;
                mTS.CreatedBy = mTS.UpdateBy = userName;
                mTS.CreatedDate = mTS.UpdateDate = DateTime.Now;
                mTS.SessionID = _secID.ToString();
                if (_secID > 0)
                {
                    hkpSectionModel mSec = (hkpSectionModel)hkpSectionBO.Instance.FindByPrimaryKey(_secID);
                    if (mSec != null)
                        mTS.SessionName = mSec.Code;
                }
                mTS.FacilityTaskID = taskcodeauto;
                mTS.FacilityTask = codeList;
                mTS.FacilityInstructions = instructionsList;

                hkpTaskSheetDetailModel mTSD = new hkpTaskSheetDetailModel();
                mTSD.CreatedBy = mTSD.UpdatedBy = userName;
                mTSD.CreatedDate = mTSD.UpdatedDate = DateTime.Now;
                mTSD.IsCompleted = false;
                mTSD.Status = 0;
                //mTSD.TaskNote = txtDescription.Text.Trim();
                mTSD.TimeIn = "";
                mTSD.TimeOut = "";
                mTSD.FacilityTaskID = taskcodeauto;
                mTSD.FacilityTask = codeList;
                mTSD.Credit = Convert.ToDecimal(maxcreditauto);
                mTSD.CompletedStatus = "";
                #endregion

                //Mở conn
                ProcessTransactions pt = new ProcessTransactions();
                pt.OpenConnection();
                pt.BeginTransaction();
                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == dtTemp.Rows.Count)
                            break;
                        //Insert to hkpTasksheet
                        mTS.TaskSheetNo = GetMaxTasksheetNo(taskdateauto);
                        mTS.AttendantID = TextUtils.ToInt(dt.Rows[i][0].ToString());
                        int _tsID = TextUtils.ToInt(pt.Insert(mTS).ToString());

                        int count = 0;
                        //Insert to hkpTasksheetDetail
                        for (int j = 0; j < dtTemp.Rows.Count; j++)
                        {
                            if (dtTemp.Rows[j]["flag"].ToString() == "0")
                            {
                                mTSD.TaskSheetID = _tsID;
                                mTSD.RoomNo = dtTemp.Rows[j]["RoomNo"].ToString();
                                mTSD.RoomType = dtTemp.Rows[j]["RoomTypeCode"].ToString();
                                mTSD.HKStatusID = TextUtils.ToInt(dtTemp.Rows[j]["HKStatusID"].ToString());
                                mTSD.FOStatus = TextUtils.ToInt(dtTemp.Rows[j]["FOStatus"].ToString());
                                pt.Insert(mTSD);
                                //Update Datatable
                                dtTemp.Rows[j]["flag"] = "1";
                                count = count + 1;
                                if (count == _NoOfRoom && i < dt.Rows.Count - 1)
                                    break;
                            }
                        }
                    }
                    //Nếu không bị lỗi - ghi dữ liệu vào bảng
                    pt.CommitTransaction();
                }
                catch (Exception ex)
                {
                    //Đóng connection
                    pt.CloseConnection();
                    return Json(new { success = false, message = "False" });
                }
                //Nếu bị lỗi Rollback lại dữ liệu đã ghi
                finally
                {
                    pt.CloseConnection();
                }

            }
            return Json(new { success = true, message = "Rooms retrieved successfully" });
        }
        private void SplitSection(string[] _SecID)
        {
            string pSection = "";
            for (int j = 0; j < _SecID.Length; j++)
            {
                if (j == 0)
                {
                    pSection = _SecID[0].ToString();
                    _ListSection = _SecID[0].ToString();
                }
                else
                {
                    if (_SecID[j].ToString() != pSection)
                    {
                        string[] _arr = _ListSection.Split(',');
                        if (Array.IndexOf(_arr, _SecID[j].ToString()) < 0)
                            _ListSection = _ListSection + "," + _SecID[j].ToString();
                        pSection = _SecID[j].ToString();
                    }
                }
            }
            _SectionID = _ListSection.Split(',');
        }

        [HttpGet]
        public IActionResult TaskSheetReport(DateTime fromDatere, DateTime toDatere, string taskcodeexpan, string attendantrepop, string tasksheetpopre, string reportstyle, string dueoutonly)
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
        public IActionResult SaveTaskSheetDetail(string taskcodenew, string attendantnew, DateTime businessDate, string userName)
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



        [HttpPost]
        public IActionResult SaveRoomTaskSheetDetail(string roomturndown, DateTime taskdate, string tasksheetroom, string taskcodenewroom, string maxcredit, string descriptiontc, string userName)
        {
            try
            {
                // Bước 1: Tách danh sách task code thành danh sách object
                var taskCodeList = taskcodenewroom.Split(',')
                                        .Where(x => !string.IsNullOrWhiteSpace(x))
                                        .Select(x => Convert.ToInt32(x.Trim()))
                                        .ToList();

                List<hkpFacilityTaskModel> listTasks = new List<hkpFacilityTaskModel>();
                foreach (var code in taskCodeList)
                {
                    var jkpFt = (hkpFacilityTaskModel)hkpFacilityTaskBO.Instance.FindByPrimaryKey(code);
                    if (jkpFt != null)
                    {
                        listTasks.Add(jkpFt);
                    }
                }

                // Ghép các mã task lại thành chuỗi, ví dụ: "C,F,T"
                string result = string.Join(",", listTasks.Select(t => t.Code));

                // Bước 2: Tách danh sách room ID
                var roomIdList = roomturndown.Split(',')
                                        .Where(x => !string.IsNullOrWhiteSpace(x))
                                        .Select(x => Convert.ToInt32(x.Trim()))
                                        .ToList();
                List<hkpTaskSheetModel> listhkpts = PropertyUtils
             .ConvertToList<hkpTaskSheetModel>(hkpTaskSheetBO.Instance.FindAll())
             .Where(x => x.TaskSheetNo == Convert.ToInt32(tasksheetroom)
                      && x.TaskSheetDate.Date == taskdate)
             .ToList();
                // Bước 3: Lặp qua từng room ID và insert bản ghi
                foreach (var roomId in roomIdList)
                {
                    var room = (RoomModel)RoomBO.Instance.FindByPrimaryKey(roomId);
                    hkpTaskSheetDetailModel modelts = new hkpTaskSheetDetailModel
                    {
                        TaskSheetID = listhkpts[0].ID,
                        CreatedBy = userName,
                        UpdatedBy = userName,
                        UpdatedDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        IsCompleted = false,
                        Status = 0,
                        TimeIn = "",
                        FacilityTaskID = taskcodenewroom,
                        FacilityTask = result,
                        TaskNote = descriptiontc,
                        Credit = Convert.ToInt32(maxcredit),
                        RoomNo = room.RoomNo,
                        RoomType = room.RoomTypeCode,
                        HKStatusID = room.HKStatusID,
                        FOStatus = room.FOStatus,

                    };

                    hkpTaskSheetDetailBO.Instance.Insert(modelts);
                }
                //DataTable dataTable = _iHouseKeepingService.TasksheetDetailsSearch(listhkpts[0].ID);
                //var result2 = (from d in dataTable.AsEnumerable()
                //              select new
                //              {
                //                  Room = d["Room"]?.ToString() ?? "",
                //                  RoomType = d["RoomType"]?.ToString() ?? "",
                //                  Credits = d["Credits"]?.ToString() ?? "",
                //                  Section = d["Section"]?.ToString() ?? "",
                //                  Floor = d["Floor"]?.ToString() ?? "",
                //                  hkstatusID = d["_HKStatusID"]?.ToString() ?? "",
                //                  TimeIn = d["TimeIn"]?.ToString() ?? "",
                //                  TimeOut = d["TimeOut"]?.ToString() ?? "",
                //                  TaskNote = d["TaskNote"]?.ToString() ?? "",
                //                  TaskCode = d["TaskCode"]?.ToString() ?? "",
                //                  Status = d["Status"]?.ToString() ?? "",
                //                  CompletedStatus = d["CompletedStatus"]?.ToString() ?? "",
                //                  ID = d["ID"]?.ToString() ?? "",
                //                  TasksheetID = d["TasksheetID"]?.ToString() ?? "",
                //                  AttendantID = d["AttendantID"]?.ToString() ?? "",
                //                  flag = d["flag"]?.ToString() ?? "",
                //                  HKStatusID = d["HKStatusID"]?.ToString() ?? "",
                //                  FOStatus = d["FOStatus"]?.ToString() ?? ""
                //              }).ToList()

                return Json(new { success = true, message = "TasksheetDetails insert successfully.", taskSheetID = listhkpts[0].ID });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult ViewTaskDetailGrid(int id)
        {
            try
            {
                DataTable dataTable = _iHouseKeepingService.TasksheetDetailsSearch(id);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Room = d["Room"]?.ToString() ?? "",
                                  RoomType = d["RoomType"]?.ToString() ?? "",
                                  Credits = d["Credits"]?.ToString() ?? "",
                                  Section = d["Section"]?.ToString() ?? "",
                                  Floor = d["Floor"]?.ToString() ?? "",
                                  HKStatusID_Internal = d["_HKStatusID"]?.ToString() ?? "",
                                  TimeIn = d["TimeIn"]?.ToString() ?? "",
                                  TimeOut = d["TimeOut"]?.ToString() ?? "",
                                  TaskNote = d["TaskNote"]?.ToString() ?? "",
                                  TaskCode = d["TaskCode"]?.ToString() ?? "",
                                  Status = d["Status"]?.ToString() ?? "",
                                  CompletedStatus = d["CompletedStatus"]?.ToString() ?? "",
                                  ID = d["ID"]?.ToString() ?? "",
                                  TasksheetID = d["TasksheetID"]?.ToString() ?? "",
                                  AttendantID = d["AttendantID"]?.ToString() ?? "",
                                  flag = d["flag"]?.ToString() ?? "",
                                  HKStatusID = d["HKStatusID"]?.ToString() ?? "",
                                  FOStatus = d["FOStatus"]?.ToString() ?? ""
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GuestServiceStatusData(string servicestatsu, string room, string roomStatus, string zone)
        {
            try
            {
                DataTable dataTable = _iHouseKeepingService.GuestServiceStatusData(servicestatsu, room, roomStatus, zone);

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
        public IActionResult TurndownTasksheetData(DateTime fromDate, string attendant, string IsDueOut, string roomStatus)
        {
            attendant = attendant ?? "";
            roomStatus = roomStatus ?? "";
            IsDueOut = IsDueOut ?? "";
            attendant = FormatIdList(attendant);
            roomStatus = FormatIdList(roomStatus);

            try
            {
                DataTable dataTable = _iHouseKeepingService.TurndownTasksheetData(fromDate, attendant, IsDueOut, roomStatus);

                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  TasksheetNo = d["TasksheetNo"]?.ToString() ?? "",
                                  RoomNo = d["RoomNo"]?.ToString() ?? "",
                                  RoomType = d["RoomType"]?.ToString() ?? "",
                                  Credits = d["Credits"]?.ToString() ?? "",
                                  FacilityTask = d["FacilityTask"]?.ToString() ?? "",
                                  hkstatusID_color = d["HKStatusID"]?.ToString() ?? "",
                                  FOStatus = d["FOStatus"]?.ToString() ?? "",
                                  Attendant = d["Attendant"]?.ToString() ?? "",
                                  AttendantID = d["AttendantID"]?.ToString() ?? "",
                                  TasksheetID = d["TasksheetID"]?.ToString() ?? "",
                                  ReservationID = d["ReservationID"]?.ToString() ?? "",
                                  Color = d["Color"]?.ToString() ?? "",
                                  TimeIn = d["TimeIn"]?.ToString() ?? "",
                                  TimeOut = d["TimeOut"]?.ToString() ?? "",
                                  RoomID = d["RoomID"]?.ToString() ?? "",
                                  Description = d["Description"]?.ToString() ?? "",
                                  TasksheetDetailID = d["TasksheetDetailID"]?.ToString() ?? ""
                              }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult RoomAttendentDailyWorksheetData(DateTime fromDate, string attendant, string tasksheet, string roomStatus)
        {

            attendant = attendant ?? "";
            tasksheet = tasksheet ?? "";
            roomStatus = roomStatus ?? "";

            try
            {

                attendant = FormatIdList(attendant);
                tasksheet = FormatIdList(tasksheet);
                roomStatus = FormatIdList(roomStatus);
                List<hkpFacilityCodeModel> listhkpcode = PropertyUtils.ConvertToList<hkpFacilityCodeModel>(hkpFacilityCodeBO.Instance.FindAll());
                string facilityCode = string.Join(",", listhkpcode.Where(x => !string.IsNullOrEmpty(x.Code)).OrderBy(x => x.Code).Select(x => x.Code));

                DataTable dataTable = _iHouseKeepingService.RoomAttendentDailyWorksheetData(fromDate, attendant, tasksheet, roomStatus, facilityCode);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  colIndex = d["colIndex"]?.ToString() ?? "",
                                  TasksheetNo = d["TasksheetNo"]?.ToString() ?? "",
                                  RoomNo = d["RoomNo"]?.ToString() ?? "",
                                  RoomType = d["RoomType"]?.ToString() ?? "",
                                  Credits = d["Credits"]?.ToString() ?? "",
                                  FacilityTask = d["FacilityTask"]?.ToString() ?? "",
                                  hkstatusID_color = d["HKStatusID"]?.ToString() ?? "",
                                  TimeIn = d["TimeIn"]?.ToString() ?? "",
                                  TimeOut = d["TimeOut"]?.ToString() ?? "",
                                  FOStatus = d["FOStatus"]?.ToString() ?? "",
                                  Description = d["Description"]?.ToString() ?? "",
                                  Attendant = d["Attendant"]?.ToString() ?? "",
                                  AttendantID = d["AttendantID"]?.ToString() ?? "",
                                  TasksheetID = d["TasksheetID"]?.ToString() ?? "",
                                  TasksheetDetailID = d["TasksheetDetailID"]?.ToString() ?? "",
                                  ReservationID = d["ReservationID"]?.ToString() ?? "",
                                  Color = d["Color"]?.ToString() ?? "",
                                  RoomID = d["RoomID"]?.ToString() ?? "",
                                  Complete = d["Complete"]?.ToString() ?? "",
                                  ID = d["ID"]?.ToString() ?? "",
                                  AM = d["AM"]?.ToString() ?? "",
                                  BM = d["BM"]?.ToString() ?? "",
                                  BR = d["BR"]?.ToString() ?? "",
                                  BT = d["BT"]?.ToString() ?? "",
                                  Chocolate = d["Chocolate"]?.ToString() ?? "",
                                  FT = d["FT"]?.ToString() ?? "",
                                  HT = d["HT"]?.ToString() ?? "",
                                  MW = d["MW"]?.ToString() ?? "",
                                  SL = d["SL"]?.ToString() ?? "",
                                  TB = d["TB"]?.ToString() ?? "",
                                  TR = d["TR"]?.ToString() ?? "",
                                  YK = d["YK"]?.ToString() ?? ""
                              }).ToList();

                DataTable dataTable2 = _iHouseKeepingService.TurndownTasksheet(fromDate, attendant, roomStatus);
                // Lấy toàn bộ TaskSheet của ngày fromDate (bỏ lọc theo tham số attendant)
                var taskSheetList = PropertyUtils.ConvertToList<hkpTaskSheetModel>(
        hkpTaskSheetBO.Instance.FindByAttribute("TasksheetDate", fromDate.ToString("yyyy-MM-dd"))
    ).Where(x => x.TaskSheetDate.Date == fromDate.Date)
     .ToList();

                // Map dữ liệu result2 kèm TaskSheetNo theo AttendantID của từng dòng
                var result2 = (from d in dataTable2.AsEnumerable()
                               let attendantIdStr = Convert.ToString(d["AttendantID"]) ?? ""
                               let attendantIdInt = int.TryParse(attendantIdStr, out var tempId) ? tempId : 0
                               let taskSheetNo = taskSheetList.FirstOrDefault(ts => ts.AttendantID == attendantIdInt).TaskSheetNo
                               select new
                               {
                                   TaskDate = d["TaskDate"]?.ToString() ?? "",
                                   RoomNo = d["RoomNo"]?.ToString() ?? "",
                                   RoomType = d["RoomType"]?.ToString() ?? "",
                                   HKStatusID = d["HKStatusID"]?.ToString() ?? "",
                                   FOStatus = d["FOStatus"]?.ToString() ?? "",
                                   Credits = d["Credits"]?.ToString() ?? "",
                                   Section = d["Section"]?.ToString() ?? "",
                                   TurndownStatus = d["TurndownStatus"]?.ToString() ?? "",
                                   FacilityTask = d["FacilityTask"]?.ToString() ?? "",
                                   Attendant = d["Attendant"]?.ToString() ?? "",
                                   AttendantID = attendantIdStr,
                                   TaskSheetNo = taskSheetNo,
                                   RoomStatusID = d["RoomStatusID"]?.ToString() ?? "",
                                   TimeIn = d["TimeIn"]?.ToString() ?? "",
                                   TimeOut = d["TimeOut"]?.ToString() ?? "",
                                   LastName = d["LastName"]?.ToString() ?? "",
                                   ArrivalDate = d["ArrivalDate"]?.ToString() ?? "",
                                   DepartureDate = d["DepartureDate"]?.ToString() ?? "",
                                   Status = d["Status"]?.ToString() ?? "",
                                   VIP = d["VIP"]?.ToString() ?? "",
                                   Adult = d["Adult"]?.ToString() ?? "",
                                   Child = d["Child"]?.ToString() ?? "",
                                   ArrivalTime = d["ArrivalTime"]?.ToString() ?? "",
                                   TasksheetID = d["TasksheetID"]?.ToString() ?? "",
                                   Specials = d["Specials"]?.ToString() ?? ""
                               }).ToList();

                return Json(new
                {
                    result1 = result,
                    result2 = result2
                });

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ViewTurnDown(string roomTypead, string sectionAd, string zoneAd, string fromRoom, string toRoom, string HKStatusID, string ReservationStatus, string arrived, string turndownStatus)
        {
            roomTypead = roomTypead ?? "";
            sectionAd = sectionAd ?? "";
            zoneAd = zoneAd ?? "";
            fromRoom = fromRoom ?? "";
            toRoom = toRoom ?? "";
            arrived = arrived ?? "";
            try
            {
                HKStatusID = FormatIdList(HKStatusID);
                ReservationStatus = FormatIdList(ReservationStatus);
                turndownStatus = FormatIdList(turndownStatus);
                roomTypead = FormatIdList(roomTypead);
                sectionAd = FormatIdList(sectionAd);
                zoneAd = FormatIdList(zoneAd);
                DataTable dataTable = _iHouseKeepingService.ViewTurnDown(roomTypead, sectionAd, zoneAd, fromRoom, toRoom, HKStatusID, ReservationStatus, arrived, turndownStatus);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Check = d["Check"]?.ToString() ?? "",
                                  RoomNo = d["RoomNo"]?.ToString() ?? "",
                                  RoomType = d["RoomType"]?.ToString() ?? "",
                                  hkstatusID = d["HKStatusID"]?.ToString() ?? "",
                                  VIP = d["VIP"]?.ToString() ?? "",
                                  Nationanity = d["Nationanity"]?.ToString() ?? "",
                                  GuestName = d["GuestName"]?.ToString() ?? "",
                                  Status = d["Status"]?.ToString() ?? "",
                                  TurndownStatus = d["TurndownStatus"]?.ToString() ?? "",
                                  ReservationID = d["ReservationID"]?.ToString() ?? "",
                                  RoomID = d["RoomID"]?.ToString() ?? "",
                                  Color = d["Color"]?.ToString() ?? ""
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        private string FormatIdList(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return "";

            var parts = ids.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(x => x.Trim());

            return string.Join("','", parts);
        }

        [HttpGet]
        public IActionResult TaskSheetStatusData(DateTime fromDate, DateTime toDate, string attendant, string room, string zone)
        {
            attendant = attendant ?? "";
            room = room ?? "";
            try
            {
                DataTable dataTable = _iHouseKeepingService.TaskSheetStatusData(fromDate, toDate, attendant, room, zone);

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
        public IActionResult ViewTaskAddUserGrid()
        {
            try
            {
                var getEmployee = hkpEmployeeBO.GetEmployee();
                DataTable dtgetEmployee = PropertyUtils.ConvertToDataTable(getEmployee);

                var result = (from d in dtgetEmployee.AsEnumerable()
                              select new
                              {
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"].ToString() : "",
                                  Name = d["Name"]?.ToString() ?? "",
                                  IsActive = d["IsActive"]?.ToString() ?? "",
                                  CreatedBy = d["CreatedBy"]?.ToString() ?? "",
                                  CreatedDate = d["CreatedDate"]?.ToString() ?? "",
                                  UpdatedBy = d["UpdatedBy"]?.ToString() ?? "",
                                  UpdatedDate = d["UpdatedDate"]?.ToString() ?? "",
                                  Inactive = d["Inactive"]?.ToString() ?? "",
                                  Description = d["Description"]?.ToString() ?? ""

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
        public IActionResult FloorPlanData(string block, string floor, string roomtype, string owner, string zone)
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
                        var rooms = RoomBO.GetFloorPlan(block, suffix, floorCode, zone);
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
        public IActionResult HouseStatusData(DateTime datebunisess, string roomtype, string zone)
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

                DataTable dataTable1 = _iHouseKeepingService.StatusSummaryOutOfOrder(datebunisess, roomtype, zone);

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

                DataTable dataTable11 = _iHouseKeepingService.StatusHKInspected(roomtype, zone);

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

                DataTable dataTable16 = _iHouseKeepingService.StatusEndOfDayGroupAndBlock(datebunisess, roomtype, zone);

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
        public IActionResult UpdateAttendantPoint(string ID, DateTime Date, int AttendantID, string UserName, int Point)
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
        public IActionResult SaveEmployeeTaskAss(List<int> idts, List<int> idau)
        {
            try
            {

                List<string> tsIds = new List<string>();
                List<string> tsNames = new List<string>();

                foreach (var id in idau)
                {
                    var hkpEmp = (hkpEmployeeModel)hkpEmployeeBO.Instance.FindByPrimaryKey(id);
                    if (hkpEmp != null)
                    {
                        tsIds.Add(id.ToString());
                        tsNames.Add(hkpEmp.Name);
                    }
                }
                string tsIdsString = string.Join(",", tsIds);
                string tsNamesString = string.Join(", ", tsNames);

                foreach (var taskSheetId in idts)
                {
                    var mTS = (hkpTaskSheetModel)hkpTaskSheetBO.Instance.FindByPrimaryKey(taskSheetId);
                    if (mTS != null)
                    {
                        mTS.EmployeeID = tsIdsString;
                        mTS.EmployeeName = tsNamesString;

                        hkpTaskSheetBO.Instance.Update(mTS);
                    }
                }




                return Json(new { success = true, message = "Add user successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult DeleteTasksheet(List<int> idts)
        {
            try
            {
                if (idts == null || idts.Count == 0)
                {
                    return Json(new { success = false, message = "No tasksheet IDs provided." });
                }

                foreach (var id in idts)
                {
                    // Xóa TaskSheet chính
                    hkpTaskSheetBO.Instance.Delete(id);

                    // Lấy danh sách detail liên quan
                    var details = PropertyUtils.ConvertToList<hkpTaskSheetDetailModel>(
                        hkpTaskSheetDetailBO.Instance.FindByAttribute("TaskSheetID", id)
                    );

                    // Xóa từng detail
                    foreach (var detail in details)
                    {
                        hkpTaskSheetDetailBO.Instance.Delete(detail.ID); // Giả sử detail có thuộc tính ID
                    }
                }

                return Json(new { success = true, message = "Tasksheet(s) and details deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult CompletedDetailtasksheet(int idts, string taskSheetNo, string hkStatusID_Internal)
        {
            try
            {
                var detailsList = PropertyUtils.ConvertToList<hkpTaskSheetDetailModel>(hkpTaskSheetDetailBO.Instance.FindByAttribute("ID", idts));

                if (detailsList != null && detailsList.Count > 0)
                {
                    var detail = detailsList.First(); // lấy bản ghi đầu tiên
                    detail.Status = 1;
                    detail.CompletedStatus = hkStatusID_Internal;

                    hkpTaskSheetDetailBO.Instance.Update(detail);
                    var taskSheetdetailModel = PropertyUtils.ConvertToList<hkpTaskSheetDetailModel>(hkpTaskSheetDetailBO.Instance.FindByAttribute("TasksheetID", detail.TaskSheetID));
                    bool isComplete = !taskSheetdetailModel.Any(d => d.Status == 0);
                    if (isComplete)
                    {
                        var taskSheetModel = (hkpTaskSheetModel)hkpTaskSheetBO.Instance.FindByPrimaryKey(detail.TaskSheetID);
                        if (taskSheetModel != null)
                        {
                            taskSheetModel.Status = true;
                            hkpTaskSheetBO.Instance.Update(taskSheetModel);
                        }
                    }

                }
                else
                {
                    return Json(new { success = false, message = "No detail found for the given TaskSheetID." });
                }


                return Json(new { success = true, message = "Tasksheet(s) marked as completed successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult Completedtasksheet(List<int> idts)
        {
            try
            {
                if (idts == null || idts.Count == 0)
                {
                    return Json(new { success = false, message = "No tasksheet IDs provided." });
                }

                foreach (var taskSheetId in idts)
                {
                    CompleteTasksheet(taskSheetId);
                }

                return Json(new { success = true, message = "Tasksheet(s) marked as completed successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private void CompleteTasksheet(int taskSheetId)
        {
            // 1. Update TaskSheet
            var taskSheetModel = (hkpTaskSheetModel)hkpTaskSheetBO.Instance.FindByPrimaryKey(taskSheetId);
            if (taskSheetModel != null)
            {
                taskSheetModel.Status = true;
                hkpTaskSheetBO.Instance.Update(taskSheetModel);
            }

            // 2. Update all TaskSheetDetail
            var details = PropertyUtils.ConvertToList<hkpTaskSheetDetailModel>(
                hkpTaskSheetDetailBO.Instance.FindByAttribute("TaskSheetID", taskSheetId)
            );
            foreach (var detail in details)
            {
                detail.Status = 1;
                hkpTaskSheetDetailBO.Instance.Update(detail);

                // 3. Update Room
                var roomList = RoomBO.Instance.FindByAttribute("RoomNo", detail.RoomNo);
                if (roomList != null && roomList.Count > 0)
                {
                    var roomModel = roomList[0] as RoomModel;
                    if (roomModel != null)
                    {
                        roomModel.TurndownStatus = 3;
                        // roomModel.GuestServiceStatus = 3; // Nếu cần set thêm
                        RoomBO.Instance.Update(roomModel);
                    }
                }
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
        public IActionResult EditGuestService(List<RoomModel> ids, int servicestatus)
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
        string _ListRoomNotAss = "";
        [HttpPost]
        public IActionResult AutoTasksheet(DateTime taskdateauto, string includeroomAS, string vcAuto, string vcnAuto, string vdAuto, string doAuto, string ocAuto, string ocnAuto, string odAuto, string oooAuto, string oosAuto, string arrivalOnly, string floorauto, string roomTypeauto, string zonecodeauto, string subzonecodeauto, string taskcodeauto, string attendantauto, string maxcreditauto, string descriptionauto, string userName)
        {
            includeroomAS = includeroomAS ?? "";
            vcAuto = vcAuto ?? "";
            vcnAuto = vcnAuto ?? "";
            vdAuto = vdAuto ?? "";
            doAuto = doAuto ?? "";
            ocAuto = ocAuto ?? "";
            ocnAuto = ocnAuto ?? "";
            odAuto = odAuto ?? "";
            oooAuto = oooAuto ?? "";
            oosAuto = oosAuto ?? "";
            arrivalOnly = arrivalOnly ?? "";
            floorauto = floorauto ?? "";
            roomTypeauto = roomTypeauto ?? "";
            zonecodeauto = zonecodeauto ?? "";
            subzonecodeauto = subzonecodeauto ?? "";
            taskcodeauto = taskcodeauto ?? "";
            attendantauto = attendantauto ?? "";
            maxcreditauto = maxcreditauto ?? "";
            descriptionauto = descriptionauto ?? "";
            userName = userName ?? "";
            try
            {
                if (!string.IsNullOrEmpty(attendantauto))
                    return Process_by_Attendant(taskdateauto, includeroomAS, vcAuto, vcnAuto, vdAuto, doAuto, ocAuto, ocnAuto, odAuto, oooAuto, oosAuto, arrivalOnly, floorauto, roomTypeauto, zonecodeauto, subzonecodeauto, taskcodeauto, attendantauto, maxcreditauto, descriptionauto, userName);
                else
                    return Process(taskdateauto, includeroomAS, vcAuto, vcnAuto, vdAuto, doAuto, ocAuto, ocnAuto, odAuto, oooAuto, oosAuto, arrivalOnly, floorauto, roomTypeauto, zonecodeauto, subzonecodeauto, taskcodeauto, attendantauto, maxcreditauto, descriptionauto, userName);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private IActionResult Process_by_Attendant(DateTime taskdateauto, string includeroomAS, string vcAuto, string vcnAuto, string vdAuto, string doAuto, string ocAuto, string ocnAuto, string odAuto, string oooAuto, string oosAuto, string arrivalOnly, string floorauto, string roomTypeauto, string zonecodeauto, string subzonecodeauto, string taskcodeauto, string attendantauto, string maxcreditauto, string descriptionauto, string userName)
        {
            #region Room & FO Status
            string IsDueOut = "";
            string HKStatusID = "";
            string FOStatus = "";

            string HK_FO = "";
            if (vcAuto == "1")
                HK_FO = " AND( (a.HKStatusID = 4 AND a.FOStatus = 0)";
            if (vcnAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 1 AND a.FOStatus = 0)";
                else
                    HK_FO = " AND( (a.HKStatusID = 1 AND a.FOStatus = 0)";
            if (vdAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 2 AND a.FOStatus = 0)";
                else
                    HK_FO = " AND( (a.HKStatusID = 2 AND a.FOStatus = 0)";
            if (doAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 7 AND a.FOStatus = 1)";
                else
                    HK_FO = " AND( (a.HKStatusID = 7 AND a.FOStatus = 1)";
            if (ocAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 4 AND a.FOStatus = 1)";
                else
                    HK_FO = " AND( (a.HKStatusID = 4 AND a.FOStatus = 1)";
            if (ocnAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 1 AND a.FOStatus = 1)";
                else
                    HK_FO = " AND( (a.HKStatusID = 1 AND a.FOStatus = 1)";
            if (odAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 2 AND a.FOStatus = 1)";
                else
                    HK_FO = " AND( (a.HKStatusID = 2 AND a.FOStatus = 1)";
            if (oosAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 6)";
                else
                    HK_FO = " AND( (a.HKStatusID = 6)";
            if (oooAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 5)";
                else
                    HK_FO = " AND( (a.HKStatusID = 5)";

            if (HK_FO != "")
                HK_FO = HK_FO + ")";
            #endregion

            #region Get Room Not Ass
            if (includeroomAS == "1")
            {
                ProcessIncludeAssigned(taskdateauto, taskcodeauto);
            }
            string floorautop = GetSplitString(floorauto);
            string roomTypeautop = GetSplitString(roomTypeauto);
            string zonecodeautop = GetSplitString(zonecodeauto);
            string subzonecodeautop = GetSplitString(subzonecodeauto);
            _ListRoomNotAss = GetSplitString(_ListRoomNotAss);
            #endregion
            DataTable Source = _iHouseKeepingService.TasksheetAutomatically(HK_FO, taskdateauto, floorautop, roomTypeautop, zonecodeautop, subzonecodeautop, includeroomAS, arrivalOnly, _ListRoomNotAss);
            if (Source.Rows.Count == 0)
            {
                return Json(new { success = false, message = "Room not available for your requests" });
            }
            int[] _AttendantID = null;
            if (!string.IsNullOrWhiteSpace(attendantauto))
            {
                _AttendantID = attendantauto
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => int.TryParse(id, out int val) ? val : 0)
                    .Where(val => val > 0)
                    .ToArray();
            }
            List<hkpAttendantModel> listatt = PropertyUtils.ConvertToList<hkpAttendantModel>(hkpAttendantBO.Instance.FindAll());

            // Lọc SectionID theo AttendantID
            int[] _aSectionID = null;
            if (_AttendantID != null && _AttendantID.Length > 0)
            {
                _aSectionID = listatt
                    .Where(a => _AttendantID.Contains(a.ID))
                    .Select(a => a.SectionID)
                    .Distinct() // tránh trùng
                    .ToArray();
            }

            #region 3.Update datatable 
            int _NoOfRoom = 0;
            decimal _pRoom = Convert.ToDecimal(Source.Rows.Count.ToString()) / Convert.ToDecimal(_AttendantID.Length);
            _NoOfRoom = Convert.ToInt32(_pRoom);
            for (int i = 0; i < _AttendantID.Length; i++)
            {
                int Count = 0;
                for (int k = 0; k < Source.Rows.Count; k++)
                {
                    if (_aSectionID[i] == TextUtils.ToInt(Source.Rows[k]["SectionID"].ToString()))
                    {
                        Count = Count + 1;
                        Source.Rows[k]["AttendantID"] = _AttendantID[i].ToString();
                    }
                    if (Count == _NoOfRoom)
                        break;
                }
            }


            #endregion

            #region 5.Process

            #region Model
            int[] _TaskCode = null;
            if (!string.IsNullOrWhiteSpace(taskcodeauto))
            {
                _TaskCode = taskcodeauto
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => int.TryParse(id, out int val) ? val : 0)
                    .Where(val => val > 0)
                    .ToArray();
            }

            // Lấy danh sách tất cả FacilityTask
            List<hkpFacilityTaskModel> listhkpft = PropertyUtils.ConvertToList<hkpFacilityTaskModel>(
                hkpFacilityTaskBO.Instance.FindAll()
            );

            // Lọc lấy Instructions và nối thành chuỗi
            string instructionsList = "";
            if (_TaskCode != null && _TaskCode.Length > 0)
            {
                instructionsList = string.Join(", ",
                    listhkpft
                        .Where(t => _TaskCode.Contains(t.ID)) // hoặc TaskCode nếu model có
                        .Select(t => t.Instructions)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                );
            }
            string codeList = "";
            if (_TaskCode != null && _TaskCode.Length > 0)
            {
                codeList = string.Join(", ",
                    listhkpft
                        .Where(t => _TaskCode.Contains(t.ID)) // lọc theo ID
                        .Select(t => t.Code) // lấy Code
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                );
            }
            hkpTaskSheetModel mTS = new hkpTaskSheetModel();
            mTS.TaskSheetDate = taskdateauto;
            mTS.TaskSheetNote = descriptionauto;
            mTS.Status = false;
            mTS.CreatedBy = mTS.UpdateBy = userName;
            mTS.CreatedDate = mTS.UpdateDate = DateTime.Now;

            //mTS.SessionID = _secID.ToString();
            //if (Source.Rows.Count > 0)
            //    mTS.SessionName = Source.Rows[0]["Section"].ToString();

            mTS.FacilityTaskID = taskcodeauto;
            mTS.FacilityTask = codeList;



            mTS.FacilityInstructions = instructionsList;

            hkpTaskSheetDetailModel mTSD = new hkpTaskSheetDetailModel();
            mTSD.CreatedBy = mTSD.UpdatedBy = userName;
            mTSD.CreatedDate = mTSD.UpdatedDate = DateTime.Now;
            mTSD.IsCompleted = false;
            mTSD.Status = 0;
            mTSD.TaskNote = descriptionauto.Trim();
            mTSD.TimeIn = "";
            mTSD.TimeOut = "";
            mTSD.FacilityTaskID = taskcodeauto;
            mTSD.FacilityTask = codeList;
            mTSD.Credit = decimal.Parse(maxcreditauto);
            mTSD.CompletedStatus = "";
            #endregion

            //Mở conn
            ProcessTransactions pt = new ProcessTransactions();
            pt.OpenConnection();
            pt.BeginTransaction();
            try
            {
                for (int j = 0; j < _AttendantID.Length; j++)
                {
                    string _tempSecID = "";
                    string _tempSecName = "";
                    int Count = 0;
                    //Insert to hkpTasksheet
                    mTS.TaskSheetNo = GetMaxTasksheetNo(taskdateauto);
                    mTS.AttendantID = _AttendantID[j];
                    int _tsID = TextUtils.ToInt(pt.Insert(mTS).ToString());

                    #region Insert to hkpTasksheetDetail
                    for (int k = 0; k < Source.Rows.Count; k++)
                    {
                        if ((_AttendantID[j] == TextUtils.ToInt(Source.Rows[k]["AttendantID"].ToString()) && TextUtils.ToInt(Source.Rows[k]["flag"].ToString()) == 0)
                            || (j == _AttendantID.Length - 1 && TextUtils.ToInt(Source.Rows[k]["flag"].ToString()) == 0))
                        {
                            if (_tempSecID != "")
                            {
                                _tempSecID = _tempSecID + "," + Source.Rows[k]["SectionID"].ToString();
                                _tempSecName = _tempSecName + "," + Source.Rows[k]["Section"].ToString();
                            }
                            else
                            {
                                _tempSecID = Source.Rows[k]["SectionID"].ToString();
                                _tempSecName = Source.Rows[k]["Section"].ToString();
                            }

                            mTSD.TaskSheetID = _tsID;
                            mTSD.RoomNo = Source.Rows[k]["RoomNo"].ToString();
                            mTSD.RoomType = Source.Rows[k]["RoomTypeCode"].ToString();
                            mTSD.HKStatusID = TextUtils.ToInt(Source.Rows[k]["HKStatusID"].ToString());
                            mTSD.FOStatus = TextUtils.ToInt(Source.Rows[k]["FOStatus"].ToString());
                            pt.Insert(mTSD);
                            //Update Datatable
                            Source.Rows[k]["flag"] = "1";
                            Count = Count + 1;
                            if (Count == _NoOfRoom && j < _AttendantID.Length - 1)
                                break;
                        }
                    }
                    #endregion

                    #region If is less than --> Continue
                    if (Count < _NoOfRoom)
                    {
                        int _Co = 0;
                        for (int l = 0; l < Source.Rows.Count; l++)
                        {
                            if (TextUtils.ToInt(Source.Rows[l]["AttendantID"].ToString()) == 0 && TextUtils.ToInt(Source.Rows[l]["flag"].ToString()) == 0)
                            {
                                if (_tempSecID != "")
                                {
                                    _tempSecID = _tempSecID + "," + Source.Rows[l]["SectionID"].ToString();
                                    _tempSecName = _tempSecName + "," + Source.Rows[l]["Section"].ToString();
                                }
                                else
                                {
                                    _tempSecID = Source.Rows[l]["SectionID"].ToString();
                                    _tempSecName = Source.Rows[l]["Section"].ToString();
                                }

                                mTSD.TaskSheetID = _tsID;
                                mTSD.RoomNo = Source.Rows[l]["RoomNo"].ToString();
                                mTSD.RoomType = Source.Rows[l]["RoomTypeCode"].ToString();
                                mTSD.HKStatusID = TextUtils.ToInt(Source.Rows[l]["HKStatusID"].ToString());
                                mTSD.FOStatus = TextUtils.ToInt(Source.Rows[l]["FOStatus"].ToString());
                                pt.Insert(mTSD);
                                //Update Datatable
                                Source.Rows[l]["flag"] = "1";
                                Source.Rows[l]["AttendantID"] = _AttendantID[j].ToString();
                                _Co = _Co + 1;
                                if (_Co == _NoOfRoom - Count && j < _AttendantID.Length - 1)
                                    break;
                            }
                        }
                    }
                    #endregion

                    #region Update section to hkpTasksheet
                    if (_tempSecID != "" && _tempSecName != "")
                    {
                        mTS.ID = _tsID;
                        mTS.SessionID = GetSection(_tempSecID.Split(','));
                        mTS.SessionName = GetSection(_tempSecName.Split(','));
                        pt.Update(mTS);
                    }
                    #endregion

                }
                //Nếu không bị lỗi - ghi dữ liệu vào bảng
                pt.CommitTransaction();
            }
            catch (Exception ex)
            {
                //Đóng connection
                pt.CloseConnection();

                return Json(new { success = false, message = "False" });
            }
            //Nếu bị lỗi Rollback lại dữ liệu đã ghi
            finally
            {
                pt.CloseConnection();
            }
            #endregion
            return Json(new { success = true, message = "Rooms retrieved successfully" });
        }
        private int GetMaxTasksheetNo(DateTime taskdateauto)
        {

            var getmax = hkpTaskSheetBO.GetMaxTasksheetNo(taskdateauto);
            DataTable dt = PropertyUtils.ConvertToDataTable(getmax);

            return TextUtils.ToInt(dt.Rows[0]["TaskSheetNo"].ToString()) + 1;
        }
        private static string GetSection(string[] _Sec)
        {
            string pSection = "";
            string pListSec = "";
            for (int j = 0; j < _Sec.Length; j++)
            {
                if (j == 0)
                {
                    pSection = _Sec[0].ToString();
                    pListSec = _Sec[0].ToString();
                }
                else
                {
                    if (_Sec[j].ToString() != pSection)
                    {
                        string[] _arr = pListSec.Split(',');
                        if (Array.IndexOf(_arr, _Sec[j].ToString()) < 0)
                            pListSec = pListSec + "," + _Sec[j].ToString();

                        pSection = _Sec[j].ToString();
                    }
                }
            }
            return pListSec;
        }
        int[] _AttendantID = null;
        string[] _SectionID = null;
        string _ListSection = "";
        string _ListAttendant = "";
        private IActionResult Process(DateTime taskdateauto, string includeroomAS, string vcAuto, string vcnAuto, string vdAuto, string doAuto, string ocAuto, string ocnAuto, string odAuto, string oooAuto, string oosAuto, string arrivalOnly, string floorauto, string roomTypeauto, string zonecodeauto, string subzonecodeauto, string taskcodeauto, string attendantauto, string maxcreditauto, string descriptionauto, string userName)
        {
            #region Room & FO Status
            string IsDueOut = "";
            string HKStatusID = "";
            string FOStatus = "";

            string HK_FO = "";
            if (vcAuto == "1")
                HK_FO = " AND( (a.HKStatusID = 4 AND a.FOStatus = 0)";
            if (vcnAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 1 AND a.FOStatus = 0)";
                else
                    HK_FO = " AND( (a.HKStatusID = 1 AND a.FOStatus = 0)";
            if (vdAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 2 AND a.FOStatus = 0)";
                else
                    HK_FO = " AND( (a.HKStatusID = 2 AND a.FOStatus = 0)";
            if (doAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 7 AND a.FOStatus = 1)";
                else
                    HK_FO = " AND( (a.HKStatusID = 7 AND a.FOStatus = 1)";
            if (ocAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 4 AND a.FOStatus = 1)";
                else
                    HK_FO = " AND( (a.HKStatusID = 4 AND a.FOStatus = 1)";
            if (ocnAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 1 AND a.FOStatus = 1)";
                else
                    HK_FO = " AND( (a.HKStatusID = 1 AND a.FOStatus = 1)";
            if (odAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 2 AND a.FOStatus = 1)";
                else
                    HK_FO = " AND( (a.HKStatusID = 2 AND a.FOStatus = 1)";
            if (oosAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 6)";
                else
                    HK_FO = " AND( (a.HKStatusID = 6)";
            if (oooAuto == "1")
                if (HK_FO != "")
                    HK_FO = HK_FO + " OR (a.HKStatusID = 5)";
                else
                    HK_FO = " AND( (a.HKStatusID = 5)";

            if (HK_FO != "")
                HK_FO = HK_FO + ")";
            #endregion

            #region Get Room Not Ass
            if (includeroomAS == "1")
            {
                ProcessIncludeAssigned(taskdateauto, taskcodeauto);
            }
            string floorautop = GetSplitString(floorauto);
            string roomTypeautop = GetSplitString(roomTypeauto);
            string zonecodeautop = GetSplitString(zonecodeauto);
            string subzonecodeautop = GetSplitString(subzonecodeauto);
            _ListRoomNotAss = GetSplitString(_ListRoomNotAss);
            #endregion
            DataTable Source = _iHouseKeepingService.TasksheetAutomatically(HK_FO, taskdateauto, floorautop, roomTypeautop, zonecodeautop, subzonecodeautop, includeroomAS, arrivalOnly, _ListRoomNotAss);
            if (Source.Rows.Count == 0)
            {
                return Json(new { success = false, message = "Room not available for your requests" });
            }
            if (_AttendantID == null)
            {
                _SectionID = Section(Source, ref _ListSection);
                _AttendantID = GetAttendant(ref _ListAttendant);
            }
            #region 2.Process Attendant by Section
            for (int i = 0; i < _SectionID.Length; i++)
            {
                ProcessSource(Source, int.Parse(_SectionID[i].ToString()), taskdateauto, floorauto, roomTypeauto, zonecodeauto, subzonecodeauto, taskcodeauto, attendantauto, maxcreditauto, descriptionauto, userName);
            }
            #endregion
            return Json(new { success = true, message = "Rooms retrieved successfully" });
        }
        private IActionResult ProcessSource(DataTable Source, int _secID, DateTime taskdateauto, string floorauto, string roomTypeauto, string zonecodeauto, string subzonecodeauto, string taskcodeauto, string attendantauto, string maxcreditauto, string descriptionauto, string userName)
        {
            int[] _TaskCode = null;
            if (!string.IsNullOrWhiteSpace(taskcodeauto))
            {
                _TaskCode = taskcodeauto
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => int.TryParse(id, out int val) ? val : 0)
                    .Where(val => val > 0)
                    .ToArray();
            }

            // Lấy danh sách tất cả FacilityTask
            List<hkpFacilityTaskModel> listhkpft = PropertyUtils.ConvertToList<hkpFacilityTaskModel>(
                hkpFacilityTaskBO.Instance.FindAll()
            );

            // Lọc lấy Instructions và nối thành chuỗi
            string instructionsList = "";
            if (_TaskCode != null && _TaskCode.Length > 0)
            {
                instructionsList = string.Join(", ",
                    listhkpft
                        .Where(t => _TaskCode.Contains(t.ID)) // hoặc TaskCode nếu model có
                        .Select(t => t.Instructions)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                );
            }
            string codeList = "";
            if (_TaskCode != null && _TaskCode.Length > 0)
            {
                codeList = string.Join(", ",
                    listhkpft
                        .Where(t => _TaskCode.Contains(t.ID)) // lọc theo ID
                        .Select(t => t.Code) // lấy Code
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                );
            }
            //DataTable dt = TextUtils.Select("SELECT DISTINCT ID FROM dbo.hkpAttendant WITH (NOLOCK) WHERE SectionID = " + _secID + " AND ID IN ('" + ClassReservation.GetSplitString(_ListAttendant) + "') ");
            var getdetailts = hkpAttendantBO.GethkpAttendantProcessSource(_secID, GetSplitString(_ListAttendant));
            DataTable dt = PropertyUtils.ConvertToDataTable(getdetailts);
            if (dt.Rows.Count > 0)
            {
                int _NoOfRoom = 0;
                decimal _pRoom = 0;
                decimal a = 0;
                DataTable dtTemp = null;
                GetDataTable_By(Source, _secID.ToString(), ref dtTemp);
                _pRoom = Convert.ToDecimal(dtTemp.Rows.Count.ToString()) / Convert.ToDecimal(dt.Rows.Count.ToString());
                _NoOfRoom = Convert.ToInt32(_pRoom);
                //_NoOfRoom = dtTemp.Rows.Count / dt.Rows.Count;  

                #region 4.Model
                hkpTaskSheetModel mTS = new hkpTaskSheetModel();
                mTS.TaskSheetDate = taskdateauto;
                mTS.TaskSheetNote = descriptionauto.Trim();
                mTS.Status = false;
                mTS.CreatedBy = mTS.UpdateBy = userName;
                mTS.CreatedDate = mTS.UpdateDate = DateTime.Now;
                mTS.SessionID = _secID.ToString();
                if (dtTemp.Rows.Count > 0)
                    mTS.SessionName = dtTemp.Rows[0]["Section"].ToString();

                mTS.FacilityTaskID = taskcodeauto;
                mTS.FacilityTask = codeList;
                mTS.FacilityInstructions = instructionsList;

                hkpTaskSheetDetailModel mTSD = new hkpTaskSheetDetailModel();
                mTSD.CreatedBy = mTSD.UpdatedBy = userName;
                mTSD.CreatedDate = mTSD.UpdatedDate = DateTime.Now;
                mTSD.IsCompleted = false;
                mTSD.Status = 0;
                mTSD.TaskNote = descriptionauto.Trim();
                mTSD.TimeIn = "";
                mTSD.TimeOut = "";
                mTSD.FacilityTaskID = taskcodeauto;
                mTSD.FacilityTask = codeList;
                mTSD.Credit = decimal.Parse(maxcreditauto);
                mTSD.CompletedStatus = "";
                #endregion

                //Mở conn
                ProcessTransactions pt = new ProcessTransactions();
                pt.OpenConnection();
                pt.BeginTransaction();
                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == dtTemp.Rows.Count)
                            break;
                        //Insert to hkpTasksheet
                        mTS.TaskSheetNo = GetMaxTasksheetNo(taskdateauto);
                        mTS.AttendantID = TextUtils.ToInt(dt.Rows[i][0].ToString());
                        int _tsID = TextUtils.ToInt(pt.Insert(mTS).ToString());

                        int count = 0;
                        //Insert to hkpTasksheetDetail
                        for (int j = 0; j < dtTemp.Rows.Count; j++)
                        {
                            if (dtTemp.Rows[j]["flag"].ToString() == "0")
                            {
                                mTSD.TaskSheetID = _tsID;
                                mTSD.RoomNo = dtTemp.Rows[j]["RoomNo"].ToString();
                                mTSD.RoomType = dtTemp.Rows[j]["RoomTypeCode"].ToString();
                                mTSD.HKStatusID = TextUtils.ToInt(dtTemp.Rows[j]["HKStatusID"].ToString());
                                mTSD.FOStatus = TextUtils.ToInt(dtTemp.Rows[j]["FOStatus"].ToString());
                                pt.Insert(mTSD);
                                //Update Datatable
                                dtTemp.Rows[j]["flag"] = "1";
                                count = count + 1;
                                if (count == _NoOfRoom && i < dt.Rows.Count - 1)
                                    break;
                            }
                        }
                    }
                    //Nếu không bị lỗi - ghi dữ liệu vào bảng
                    pt.CommitTransaction();

                }
                catch (Exception ex)
                {
                    //Đóng connection
                    pt.CloseConnection();
                    return Json(new { success = false, message = "False" });
                }
                //Nếu bị lỗi Rollback lại dữ liệu đã ghi
                finally
                {
                    pt.CloseConnection();
                }

            }
            return Json(new { success = true, message = "Rooms retrieved successfully" });
        }
        private void GetDataTable_By(DataTable _Origin, string _para, ref DataTable dt)
        {
            dt = _Origin.Clone();
            foreach (DataRow drow in _Origin.Select("SectionID='" + _para + "'"))
            {
                dt.ImportRow(drow);
            }
        }
        private int[] GetAttendant(ref string _ListAtt)
        {
            //DataTable dt = TextUtils.Select("SELECT ID FROM dbo.hkpAttendant WITH (NOLOCK) WHERE SectionID IN ('" + ClassReservation.GetSplitString(_ListSection) + "') ");
            var getdetailts = hkpAttendantBO.GethkpAttendantbySect(GetSplitString(_ListSection));
            DataTable dt = PropertyUtils.ConvertToDataTable(getdetailts);
            if (dt.Rows.Count > 0)
            {
                _AttendantID = new int[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _AttendantID[i] = int.Parse(dt.Rows[i][0].ToString());
                    if (i == 0)
                        _ListAtt = dt.Rows[i][0].ToString();
                    else
                        _ListAtt = _ListAtt + "," + dt.Rows[i][0].ToString();
                }
            }
            return _AttendantID;
        }
        private static string[] Section(DataTable dt, ref string _ListSec)
        {
            string pSection = "";
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (j == 0)
                {
                    pSection = dt.Rows[0]["SectionID"].ToString();
                    _ListSec = pSection;
                }
                else
                {
                    if (dt.Rows[j]["SectionID"].ToString() != pSection)
                    {
                        string[] _arr = _ListSec.Split(',');
                        if (Array.IndexOf(_arr, dt.Rows[j]["SectionID"].ToString()) < 0)
                            _ListSec = _ListSec + "," + dt.Rows[j]["SectionID"].ToString();
                        pSection = dt.Rows[j]["SectionID"].ToString();
                    }
                }
            }
            return _ListSec.Split(',');
        }
        private void ProcessIncludeAssigned(DateTime taskdateauto, string taskcodeauto)
        {
            var getdetailts = hkpTaskSheetDetailBO.GethkpTaskSheetDetail(taskdateauto);
            DataTable dt = PropertyUtils.ConvertToDataTable(getdetailts);


            _ListRoomNotAss = "";
            if (dt.Rows.Count > 0)
            {
                if (taskcodeauto != "")
                {
                    string[] _arrTaskCode = taskcodeauto.Split(',');
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string[] _arrRo = dt.Rows[i]["FacilityTask"].ToString().Split(',');
                        for (int j = 0; j < _arrTaskCode.Length; j++)
                        {
                            if (Array.IndexOf(_arrRo, _arrTaskCode[j].ToString()) >= 0)
                            {
                                if (_ListRoomNotAss == "")
                                    _ListRoomNotAss = dt.Rows[i]["RoomNo"].ToString();
                                else
                                    _ListRoomNotAss = _ListRoomNotAss + "," + dt.Rows[i]["RoomNo"].ToString();
                            }
                        }
                    }
                }
            }
        }
        public static string GetSplitString(string Name)
        {
            string paraName;
            string[] arrConfirmationNo = null;
            paraName = "";
            if (Name != "")
            {
                arrConfirmationNo = Name.Split(',');
                if (arrConfirmationNo.Length > 0)
                {
                    for (int i = 0; i < arrConfirmationNo.Length; i++)
                    {
                        if (i != 0)
                            paraName = paraName + "'" + "," + "'" + arrConfirmationNo[i].ToString().Trim();
                        else
                            paraName = arrConfirmationNo[i].ToString().Trim();
                    }
                }
            }
            return paraName;
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


        public static void InsertHistory(string roomNo, string oldValue, string newValue, DateTime systemDate, string computerName, string action, int objectID, string tableName, string loginName)
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
        [HttpGet]
        public IActionResult GetLostAndFound(DateTime fromDate, DateTime toDate)
        {
            try
            {


                DataTable dataTable = _iHouseKeepingService.LostAndFound(fromDate, toDate);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {

                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"] : "",
                                  TransactionDate = !string.IsNullOrEmpty(d["TransactionDate"].ToString()) ? d["TransactionDate"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Location = !string.IsNullOrEmpty(d["Location"].ToString()) ? d["Location"] : "",
                                  Finder = !string.IsNullOrEmpty(d["Finder"].ToString()) ? d["Finder"] : "",
                                  SurrenderBy = !string.IsNullOrEmpty(d["SurrenderBy"].ToString()) ? d["SurrenderBy"] : "",
                                  SignatureName = !string.IsNullOrEmpty(d["SignatureName"].ToString()) ? d["SignatureName"] : "",
                                  PlaceStore = !string.IsNullOrEmpty(d["PlaceStore"].ToString()) ? d["PlaceStore"] : "",
                                  SendDate = !string.IsNullOrEmpty(d["SendDate"].ToString()) ? d["SendDate"] : "",
                                  SendName = !string.IsNullOrEmpty(d["SendName"].ToString()) ? d["SendName"] : "",
                                  CodeQualityType = !string.IsNullOrEmpty(d["CodeQualityType"].ToString()) ? d["CodeQualityType"] : "",
                                  Notes = !string.IsNullOrEmpty(d["Notes"].ToString()) ? d["Notes"] : "",
                                  CreatedBy = !string.IsNullOrEmpty(d["CreatedBy"].ToString()) ? d["CreatedBy"] : "",
                                  CreatedDate = !string.IsNullOrEmpty(d["CreatedDate"].ToString()) ? d["CreatedDate"] : "",
                                  UpdatedBy = !string.IsNullOrEmpty(d["UpdatedBy"].ToString()) ? d["UpdatedBy"] : "",
                                  UpdateDate = !string.IsNullOrEmpty(d["UpdateDate"].ToString()) ? d["UpdateDate"] : "",
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
        public IActionResult LostAndFound()
        {
            List<lafZoneModel> listzone = PropertyUtils.ConvertToList<lafZoneModel>(lafZoneBO.Instance.FindAll());
            ViewBag.lafZoneList = listzone;
            return View();
        }

        //[HttpPost]
        //public IActionResult Insertlaf(int id, LostAndFoundModel model)
        //{
        //    var username = HttpContext.Session.GetString("LoginName") ?? "";

        //    model.CreatedBy = username;
        //    model.UpdatedBy = username;


        //    using (var conn = new SqlConnection(DBUtils.GetDBConnectionString()))
        //    {
        //        conn.Open();
        //        using (var cmd = new SqlCommand(@"
        //        insert into lafLostAndFound 
        //        (StatusID, TransactionDate, Description, ZoneID, Location, Finder, SurrenderBy, SignatureName, PlaceStore, SendDate, SendName, QualityTypeID, Notes, CreatedBy, CreatedDate, UpdatedBy, UpdateDate) 
        //        values 
        //        (@StatusID, @TransactionDate, @Description, @ZoneID, @Location, @Finder, @SurrenderBy, @SignatureName, @PlaceStore, @SendDate, @SendName, @QualityTypeID, @Notes, @CreatedBy, @CreatedDate, @UpdatedBy, @UpdateDate);
        //        SELECT SCOPE_IDENTITY();", conn))
        //        {
        //            cmd.Parameters.AddWithValue("@StatusID", model.StatusID);
        //            cmd.Parameters.AddWithValue("@TransactionDate", model.TransactionDate);
        //            cmd.Parameters.AddWithValue("@Description", model.Description ?? "");
        //            cmd.Parameters.AddWithValue("@ZoneID", model.ZoneID);
        //            cmd.Parameters.AddWithValue("@Location", model.Location ?? "");
        //            cmd.Parameters.AddWithValue("@Finder", model.Finder ?? "");
        //            cmd.Parameters.AddWithValue("@SurrenderBy", model.SurrenderBy ?? "");
        //            cmd.Parameters.AddWithValue("@SignatureName", model.SignatureName ?? "");
        //            cmd.Parameters.AddWithValue("@PlaceStore", model.PlaceStore ?? "");
        //            cmd.Parameters.AddWithValue("@SendDate", model.SendDate == DateTime.MinValue ? new DateTime(1900, 1, 1) : model.SendDate);
        //            cmd.Parameters.AddWithValue("@SendName", model.SendName ?? "");
        //            cmd.Parameters.AddWithValue("@QualityTypeID", model.QualityTypeID);
        //            cmd.Parameters.AddWithValue("@Notes", model.Notes ?? "");
        //            cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
        //            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
        //            cmd.Parameters.AddWithValue("@UpdatedBy", model.UpdatedBy);
        //            cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);

        //            var newId = Convert.ToInt32(cmd.ExecuteScalar());
        //            return Json(new { success = true, id = newId });
        //        }
        //    }
        //}

        //            using (SqlCommand cmd = new SqlCommand(sql, conn))
        //            {
        //                // Gán tham số
        //                cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = model.StatusID;
        //                cmd.Parameters.Add("@TransactionDate", SqlDbType.DateTime).Value = model.TransactionDate != DateTime.MinValue ? model.TransactionDate : (object)DBNull.Value;
        //                cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 4000).Value = model.Description ?? (object)DBNull.Value;
        //                cmd.Parameters.Add("@ZoneID", SqlDbType.Int).Value = model.ZoneID;
        //                cmd.Parameters.Add("@Location", SqlDbType.NVarChar, 4000).Value = model.Location ?? (object)DBNull.Value;
        //                cmd.Parameters.Add("@Finder", SqlDbType.NVarChar, 4000).Value = model.Finder ?? (object)DBNull.Value;
        //                cmd.Parameters.Add("@SurrenderBy", SqlDbType.NVarChar, 4000).Value = model.SurrenderBy ?? (object)DBNull.Value;
        //                cmd.Parameters.Add("@SignatureName", SqlDbType.NVarChar, 4000).Value = model.SignatureName ?? (object)DBNull.Value;
        //                cmd.Parameters.Add("@PlaceStore", SqlDbType.NVarChar, 4000).Value = model.PlaceStore ?? (object)DBNull.Value;
        //                cmd.Parameters.Add("@SendDate", SqlDbType.DateTime).Value = model.SendDate != DateTime.MinValue ? model.SendDate : (object)DBNull.Value;
        //                cmd.Parameters.Add("@SendName", SqlDbType.NVarChar, 4000).Value = model.SendName ?? (object)DBNull.Value;
        //                cmd.Parameters.Add("@QualityTypeID", SqlDbType.Int).Value = model.QualityTypeID;
        //                cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 4000).Value = model.Notes ?? (object)DBNull.Value;
        //                cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 20).Value = model.UpdatedBy ?? (object)DBNull.Value;
        //                cmd.Parameters.Add("@UpdateDate", SqlDbType.DateTime).Value = model.UpdateDate != DateTime.MinValue ? model.UpdateDate : (object)DBNull.Value;
        //                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

        //                int rowsAffected = cmd.ExecuteNonQuery();

        //                if (rowsAffected > 0)
        //                    return Json(new { success = true, message = "Cập nhật thành công" });
        //                else
        //                    return Json(new { success = false, message = "Không tìm thấy bản ghi để cập nhật" });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log lỗi ở đây nếu cần
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        //[HttpPost]
        //public IActionResult Deletelaf(int id, LostAndFoundModel model)
        //{
        //    var username = HttpContext.Session.GetString("LoginName") ?? "";

        //    using (var conn = new SqlConnection(DBUtils.GetDBConnectionString()))
        //    {
        //        conn.Open();

        //        // Check xem Card có tồn tại chưa
        //        string checkSql = "SELECT COUNT(*) FROM lafLostAndFound WHERE ID = @ID";
        //        using (var checkCmd = new SqlCommand(checkSql, conn))
        //        {
        //            checkCmd.Parameters.AddWithValue("@ID", model.ID);
        //            int count = (int)checkCmd.ExecuteScalar();
        //            if (count == 0)
        //            {
        //                return NotFound(new { success = false, message = "LAF ID không tồn tại trong DB" });
        //            }
        //        }

        //        // Xóa bản ghi
        //        string sql = "DELETE FROM lafLostAndFound WHERE ID = @ID";
        //        using (var cmd = new SqlCommand(sql, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@ID", model.ID);

        //            int rows = cmd.ExecuteNonQuery();
        //            return Ok(new { success = rows > 0, id = model.ID });
        //        }
        //    }
        //}
        [HttpPost]
        public ActionResult InsertLostAndFound()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                lafLostAndFoundModel lafModel = new lafLostAndFoundModel();

                // Lấy dữ liệu từ form (dùng .ToString())
                lafModel.StatusID = !string.IsNullOrEmpty(Request.Form["statusID"].ToString())
                                    ? int.Parse(Request.Form["statusID"].ToString())
                                    : 0;

                lafModel.TransactionDate = !string.IsNullOrEmpty(Request.Form["transactionDate"].ToString())
                                           ? DateTime.Parse(Request.Form["transactionDate"].ToString())
                                           : DateTime.Now;

                lafModel.Description = Request.Form["description"].ToString();
                lafModel.ZoneID = !string.IsNullOrEmpty(Request.Form["zoneID"].ToString())
                                  ? int.Parse(Request.Form["zoneID"].ToString())
                                  : 0;

                lafModel.Location = Request.Form["location"].ToString();
                lafModel.Finder = Request.Form["finder"].ToString();
                lafModel.SurrenderBy = Request.Form["surrenderBy"].ToString();
                lafModel.SignatureName = Request.Form["signatureName"].ToString();
                lafModel.PlaceStore = Request.Form["placeStore"].ToString();

                lafModel.SendDate = !string.IsNullOrEmpty(Request.Form["sendDate"].ToString())
                                    ? DateTime.Parse(Request.Form["sendDate"].ToString())
                                    : DateTime.MinValue;

                lafModel.SendName = Request.Form["sendName"].ToString();
                lafModel.QualityTypeID = !string.IsNullOrEmpty(Request.Form["qualityTypeID"].ToString())
                                         ? int.Parse(Request.Form["qualityTypeID"].ToString())
                                         : 0;

                lafModel.Notes = Request.Form["notes"].ToString();

                // Thông tin người dùng
                lafModel.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                lafModel.UpdatedBy = lafModel.CreatedBy;
                lafModel.CreatedDate = DateTime.Now;
                lafModel.UpdateDate = DateTime.Now;

                // Gọi BO để lưu
                long lafId = lafLostAndFoundBO.Instance.Insert(lafModel);

                pt.CommitTransaction();

                return Json(new { success = true, id = lafId });
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
        public ActionResult UpdateLostAndFound()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                lafLostAndFoundModel lafModel = new lafLostAndFoundModel();

                lafModel.ID = !string.IsNullOrEmpty(Request.Form["id"])
                  ? int.Parse(Request.Form["id"])
                  : 0;

                // Lấy dữ liệu từ form (dùng .ToString())
                lafModel.StatusID = !string.IsNullOrEmpty(Request.Form["statusID"].ToString())
                                    ? int.Parse(Request.Form["statusID"].ToString())
                                    : 0;

                lafModel.TransactionDate = !string.IsNullOrEmpty(Request.Form["transactionDate"].ToString())
                                           ? DateTime.Parse(Request.Form["transactionDate"].ToString())
                                           : DateTime.Now;

                lafModel.Description = Request.Form["description"].ToString();
                lafModel.ZoneID = !string.IsNullOrEmpty(Request.Form["zoneID"].ToString())
                                  ? int.Parse(Request.Form["zoneID"].ToString())
                                  : 0;

                lafModel.Location = Request.Form["location"].ToString();
                lafModel.Finder = Request.Form["finder"].ToString();
                lafModel.SurrenderBy = Request.Form["surrenderBy"].ToString();
                lafModel.SignatureName = Request.Form["signatureName"].ToString();
                lafModel.PlaceStore = Request.Form["placeStore"].ToString();

                lafModel.SendDate = !string.IsNullOrEmpty(Request.Form["sendDate"].ToString())
                                    ? DateTime.Parse(Request.Form["sendDate"].ToString())
                                    : DateTime.MinValue;

                lafModel.SendName = Request.Form["sendName"].ToString();
                lafModel.QualityTypeID = !string.IsNullOrEmpty(Request.Form["qualityTypeID"].ToString())
                                         ? int.Parse(Request.Form["qualityTypeID"].ToString())
                                         : 0;

                lafModel.Notes = Request.Form["notes"].ToString();

                // Thông tin người dùng
                lafModel.CreatedBy = HttpContext.Session.GetString("LoginName") ?? "";
                lafModel.UpdatedBy = lafModel.CreatedBy;
                lafModel.CreatedDate = DateTime.Now;
                lafModel.UpdateDate = DateTime.Now;

                // Gọi BO để lưu
                lafLostAndFoundBO.Instance.Update(lafModel);

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
        public ActionResult DeleteLostAndFound()
        {
            try
            {

                lafLostAndFoundModel lafModel = (lafLostAndFoundModel)lafLostAndFoundBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                if (lafModel == null || lafModel.ID == 0)
                {
                    return Json(new { code = 1, msg = "Can not find Lost And Found" });

                }
                lafLostAndFoundBO.Instance.Delete(int.Parse(Request.Form["id"].ToString()));
                return Json(new { code = 0, msg = "Delete Lost And Found was successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = ex.Message });
            }

        }

    }
}


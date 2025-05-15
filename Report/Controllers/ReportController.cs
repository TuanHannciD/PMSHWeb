using BaseBusiness.BO;
using BaseBusiness.util;
using BaseBusiness.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Report.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraReports.UI;

namespace Report.Controllers
{
    public class ReportController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReportController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IReportService _iReportService;

        public ReportController(ILogger<ReportController> logger,
             IMemoryCache cache, IConfiguration configuration, IReportService iReportService)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _iReportService = iReportService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BookingSource()
        {
            return View();

        }
        public IActionResult ReservationSummaryReport()
        {
            return View();

        }
        public IActionResult GroupReservationReport()
        {
            return View();

        }
        public IActionResult GuestStayOverReport()
        {
            return View();

        }
        public IActionResult GuestStayReport()
        {
            return View();

        }
        public IActionResult TraceReport()
        {
            List<RoomClassModel> list = PropertyUtils.ConvertToList<RoomClassModel>(RoomClassBO.Instance.FindAll());
            List<DepartmentModel> list2 = PropertyUtils.ConvertToList<DepartmentModel>(DepartmentBO.Instance.FindAll());
            ViewBag.RoomClassList = list;
            ViewBag.DepartmentList = list2;
            return View();

        }
        public IActionResult ReportNationalityStatistics()
        {

            return View();

        }
        public IActionResult RatecodebyDateReport()
        {
            List<RateCodeModel> list = PropertyUtils.ConvertToList<RateCodeModel>(RateCodeBO.Instance.FindAll());
            ViewBag.RateCodeList = list;
            return View();

        }
        public IActionResult GuestMarketReport()
        {
            List<ZoneModel> list = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = list;
            return View();
        }
        [HttpGet]
        public IActionResult GuestMarket(DateTime fromDate, DateTime toDate, string currency, int zoneId)
        {
            // Lấy danh sách RoomType có ZoneID tương ứng
            List<RoomTypeModel> roomTypesInZone = PropertyUtils
                .ConvertToList<RoomTypeModel>(RoomTypeBO.Instance.FindAll())
                .Where(rt => rt.ZoneID == zoneId)
                .ToList();

            // Lấy danh sách các ID, nối thành chuỗi nếu GuestMarketReport yêu cầu string zonecode
            string zonecode = string.Join(",", roomTypesInZone.Select(rt => rt.ID.ToString()));

            // Tạo report
            XtraReport report = new OneSPMSh.Report.ReportNationalityStatistics();

            // Lấy dữ liệu cho báo cáo
            DataTable dataTable = _iReportService.GuestMarketReport(fromDate, toDate, currency, zonecode);
            report.DataSource = dataTable;

            report.RequestParameters = false;

            return PartialView("_ReportViewerPartial", report);
        }

        [HttpGet]
        public IActionResult RatecodebyDate(DateTime fromDate, DateTime toDate, string ratecode)
        {
            try
            {
                //XtraReport report = new OneSPMSh.Report.RatecodebyDateReport();

                DataTable dataTable = _iReportService.RatecodebyDate(fromDate, toDate, ratecode);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  RateCode = !string.IsNullOrEmpty(d["RateCode"].ToString()) ? d["RateCode"] : "",
                                  Descripton = !string.IsNullOrEmpty(d["Descripton"].ToString()) ? d["Descripton"] : "",
                                  FromDate = !string.IsNullOrEmpty(d["FromDate"].ToString()) ? d["FromDate"] : "",
                                  ToDate = !string.IsNullOrEmpty(d["ToDate"].ToString()) ? d["ToDate"] : "",
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Rate = !string.IsNullOrEmpty(d["Rate"].ToString()) ? d["Rate"] : "",
                                  RoomTypeCode = !string.IsNullOrEmpty(d["RoomTypeCode"].ToString()) ? d["RoomTypeCode"] : "",
                                  CurrencyID = !string.IsNullOrEmpty(d["CurrencyID"].ToString()) ? d["CurrencyID"] : "",
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
        public IActionResult NationalityStatistics(DateTime fromDate, DateTime toDate, string status, string sortOder)
        {
            // XtraReport report = new OneSPMSh.Report.ReportNationalityStatistics();
            try
            {
                DataTable dataTable = _iReportService.ReportNationalityStatistics(fromDate, toDate, status, sortOder);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Nationality = !string.IsNullOrEmpty(d["Nationality"].ToString()) ? d["Nationality"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  ArrAdult = !string.IsNullOrEmpty(d["ArrAdult"].ToString()) ? d["ArrAdult"] : "",
                                  ArrChild = !string.IsNullOrEmpty(d["ArrChild"].ToString()) ? d["ArrChild"] : "",
                                  ArrChild1 = !string.IsNullOrEmpty(d["ArrChild1"].ToString()) ? d["ArrChild1"] : "",
                                  ArrChild2 = !string.IsNullOrEmpty(d["ArrChild2"].ToString()) ? d["ArrChild2"] : "",
                                  ArrTotal = !string.IsNullOrEmpty(d["ArrTotal"].ToString()) ? d["ArrTotal"] : "",
                                  ArrPercen = !string.IsNullOrEmpty(d["ArrPercen"].ToString()) ? d["ArrPercen"] : "",
                                  NightAdult = !string.IsNullOrEmpty(d["NightAdult"].ToString()) ? d["NightAdult"] : "",
                                  NightChild = !string.IsNullOrEmpty(d["NightChild"].ToString()) ? d["NightChild"] : "",
                                  NightChild1 = !string.IsNullOrEmpty(d["NightChild1"].ToString()) ? d["NightChild1"] : "",
                                  NightChild2 = !string.IsNullOrEmpty(d["NightChild2"].ToString()) ? d["NightChild2"] : "",
                                  BedNightPercen = !string.IsNullOrEmpty(d["BedNightPercen"].ToString()) ? d["BedNightPercen"] : "",
                                  NightTotal = !string.IsNullOrEmpty(d["NightTotal"].ToString()) ? d["NightTotal"] : "",
                                  RoomNightPercen = !string.IsNullOrEmpty(d["RoomNightPercen"].ToString()) ? d["RoomNightPercen"] : "",
                                  StayDur = !string.IsNullOrEmpty(d["StayDur"].ToString()) ? d["StayDur"] : "",
                                  PYStayDur = !string.IsNullOrEmpty(d["PYStayDur"].ToString()) ? d["PYStayDur"] : "",
                                  ArrPreYear = !string.IsNullOrEmpty(d["ArrPreYear"].ToString()) ? d["ArrPreYear"] : "",
                                  NightPreYear = !string.IsNullOrEmpty(d["NightPreYear"].ToString()) ? d["NightPreYear"] : "",
                                  RoomNightPreYear = !string.IsNullOrEmpty(d["RoomNightPreYear"].ToString()) ? d["RoomNightPreYear"] : "",
                                  RoomNightPreYPercen = !string.IsNullOrEmpty(d["RoomNightPreYPercen"].ToString()) ? d["RoomNightPreYPercen"] : "",
                                  ProfitLoss = !string.IsNullOrEmpty(d["ProfitLoss"].ToString()) ? d["ProfitLoss"] : "",
                                  ProfitLossPercen = !string.IsNullOrEmpty(d["ProfitLossPercen"].ToString()) ? d["ProfitLossPercen"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            // report.DataSource = dataTable;

            // Không cần gán parameter
            //report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }
        [HttpGet]
        public IActionResult BookingSourceData(DateTime fromDate, DateTime toDate)
        {
            //XtraReport report = new OneSPMSh.Report.Report1();
            try
            {
                DataTable dataTable = _iReportService.GetBookingSourceData(fromDate, toDate);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Source = !string.IsNullOrEmpty(d["Source"].ToString()) ? d["Source"] : "",
                                  RNActual = !string.IsNullOrEmpty(d["RNActual"].ToString()) ? d["RNActual"] : "",
                                  BUSMIXActual = !string.IsNullOrEmpty(d["BUSMIXActual"].ToString()) ? d["BUSMIXActual"] : "",
                                  ADRActual = !string.IsNullOrEmpty(d["ADRActual"].ToString()) ? d["ADRActual"] : "",
                                  REVForcast = !string.IsNullOrEmpty(d["REVForcast"].ToString()) ? d["REVForcast"] : "",
                                  REVMIXForcast = !string.IsNullOrEmpty(d["REVMIXForcast"].ToString()) ? d["REVMIXForcast"] : "",
                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            //report.DataSource = dataTable;

            //// Không cần gán parameter
            //report.RequestParameters = false;

            //return PartialView("_ReportViewerPartial", report);
        }
        [HttpGet]
        public IActionResult ReservationSummary(DateTime fromDate, DateTime toDate)
        {
            //XtraReport report = new OneSPMSh.Report.ReservationSummaryReport();

            try
            {
                DataTable dataTable = _iReportService.ReservationSummaryReport(fromDate, toDate);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ByBookingCode = !string.IsNullOrEmpty(d["ByBookingCode"].ToString()) ? d["ByBookingCode"] : "",
                                  ByRoom = !string.IsNullOrEmpty(d["ByRoom"].ToString()) ? d["ByRoom"] : "",
                                  ByRoomNight = !string.IsNullOrEmpty(d["ByRoomNight"].ToString()) ? d["ByRoomNight"] : "",

                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            // report.DataSource = dataTable;

            // Không cần gán parameter
            //  report.RequestParameters = false;

            //  return PartialView("_ReportViewerPartial", report);
        }
        [HttpGet]
        public IActionResult GroupReservation(DateTime fromDate, DateTime toDate, string noofRoom)
        {
            //XtraReport report = new OneSPMSh.Report.GroupReservationReport();
            try
            {
                DataTable dataTable = _iReportService.GroupReservation(fromDate, toDate, noofRoom);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ConfirmationNo = !string.IsNullOrEmpty(d["ConfirmationNo"].ToString()) ? d["ConfirmationNo"] : "",
                                  CRSNo = !string.IsNullOrEmpty(d["CRSNo"].ToString()) ? d["CRSNo"] : "",
                                  GroupCode = !string.IsNullOrEmpty(d["GroupCode"].ToString()) ? d["GroupCode"] : "",
                                  ReservationTypeCode = !string.IsNullOrEmpty(d["ReservationTypeCode"].ToString()) ? d["ReservationTypeCode"] : "",
                                  ArrivalDate = !string.IsNullOrEmpty(d["ArrivalDate"].ToString()) ? d["ArrivalDate"] : "",
                                  DepartureDate = !string.IsNullOrEmpty(d["DepartureDate"].ToString()) ? d["DepartureDate"] : "",
                                  TotalRoom = !string.IsNullOrEmpty(d["TotalRoom"].ToString()) ? d["TotalRoom"] : "",
                                  Nights = !string.IsNullOrEmpty(d["Nights"].ToString()) ? d["Nights"] : "",
                                  ReservationHolder = !string.IsNullOrEmpty(d["ReservationHolder"].ToString()) ? d["ReservationHolder"] : "",
                                  SaleInCharge = !string.IsNullOrEmpty(d["SaleInCharge"].ToString()) ? d["SaleInCharge"] : "",
                                  RoomNight = !string.IsNullOrEmpty(d["RoomNight"].ToString()) ? d["RoomNight"] : "",
                                  Persons = !string.IsNullOrEmpty(d["Persons"].ToString()) ? d["Persons"] : "",
                                  RoomOccupancy = !string.IsNullOrEmpty(d["RoomOccupancy"].ToString()) ? d["RoomOccupancy"] : "",
                                  PersonRoom = !string.IsNullOrEmpty(d["PersonRoom"].ToString()) ? d["PersonRoom"] : "",
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
        public IActionResult GuestStayOver(DateTime fromDate, DateTime toDate)
        {
            //XtraReport report = new OneSPMSh.Report.GuestStayOverReport();
            try
            {
                DataTable dataTable = _iReportService.GuestStayOver(fromDate, toDate);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ConfirmationNo = !string.IsNullOrEmpty(d["ConfirmationNo"].ToString()) ? d["ConfirmationNo"] : "",
                                  Lastname = !string.IsNullOrEmpty(d["Lastname"].ToString()) ? d["Lastname"] : "",
                                  Title = !string.IsNullOrEmpty(d["Title"].ToString()) ? d["Title"] : "",
                                  Country = !string.IsNullOrEmpty(d["Country"].ToString()) ? d["Country"] : "",
                                  ArrivalDate = !string.IsNullOrEmpty(d["ArrivalDate"].ToString()) ? d["ArrivalDate"] : "",
                                  DepartureDate = !string.IsNullOrEmpty(d["DepartureDate"].ToString()) ? d["DepartureDate"] : "",
                                  NoOfAdult = !string.IsNullOrEmpty(d["NoOfAdult"].ToString()) ? d["NoOfAdult"] : "",
                                  NoOfChild = !string.IsNullOrEmpty(d["NoOfChild"].ToString()) ? d["NoOfChild"] : "",
                                  NoOfChild1 = !string.IsNullOrEmpty(d["NoOfChild1"].ToString()) ? d["NoOfChild1"] : "",
                                  NoOfChild2 = !string.IsNullOrEmpty(d["NoOfChild2"].ToString()) ? d["NoOfChild2"] : "",
                                  NoOfRoom = !string.IsNullOrEmpty(d["NoOfRoom"].ToString()) ? d["NoOfRoom"] : "",
                                  RoomType = !string.IsNullOrEmpty(d["RoomType"].ToString()) ? d["RoomType"] : "",
                                  RoomNo = !string.IsNullOrEmpty(d["RoomNo"].ToString()) ? d["RoomNo"] : "",
                                  RoomNight = !string.IsNullOrEmpty(d["RoomNight"].ToString()) ? d["RoomNight"] : "",
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  ReservationHolder = !string.IsNullOrEmpty(d["ReservationHolder"].ToString()) ? d["ReservationHolder"] : "",

                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            // report.DataSource = dataTable;

            // Không cần gán parameter
            // report.RequestParameters = false;

            // return PartialView("_ReportViewerPartial", report);
        }

        [HttpGet]
        public IActionResult GuestStay(string noofName, string stayno, string stayand)
        {
            //XtraReport report = new OneSPMSh.Report.GuestStayReport();
            try
            {
                DataTable dataTable = _iReportService.GuestStay(noofName, stayno, stayand);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Code = !string.IsNullOrEmpty(d["Code"].ToString()) ? d["Code"] : "",
                                  Vip = !string.IsNullOrEmpty(d["Vip"].ToString()) ? d["Vip"] : "",
                                  GuestName = !string.IsNullOrEmpty(d["GuestName"].ToString()) ? d["GuestName"] : "",
                                  StayNo = !string.IsNullOrEmpty(d["StayNo"].ToString()) ? d["StayNo"] : "",
                                  DateOfBirth = !string.IsNullOrEmpty(d["DateOfBirth"].ToString()) ? d["DateOfBirth"] : "",
                                  Passport = !string.IsNullOrEmpty(d["Passport"].ToString()) ? d["Passport"] : "",
                                  IdentityCard = !string.IsNullOrEmpty(d["IdentityCard"].ToString()) ? d["IdentityCard"] : "",
                                  Address = !string.IsNullOrEmpty(d["Address"].ToString()) ? d["Address"] : "",
                                  City = !string.IsNullOrEmpty(d["City"].ToString()) ? d["City"] : "",
                                  Nat = !string.IsNullOrEmpty(d["Nat"].ToString()) ? d["Nat"] : "",
                                  HandPhone = !string.IsNullOrEmpty(d["HandPhone"].ToString()) ? d["HandPhone"] : "",
                                  Telephone = !string.IsNullOrEmpty(d["Telephone"].ToString()) ? d["Telephone"] : "",


                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            // report.DataSource = dataTable;

            // Không cần gán parameter
            //report.RequestParameters = false;

            //     return PartialView("_ReportViewerPartial", report);
        }
        [HttpGet]
        public IActionResult TraceReportView(DateTime fromDate, DateTime toDate, int roomClass, int department, int status, int byAlphabetical, int byRoom, int byVip, int pseudoRoom, int reserved, int checkedIn, int dueout, int individual, int blockcode, int vipOnly)
        {
            XtraReport report = new OneSPMSh.Report.TraceReport();

            DataTable dataTable = _iReportService.TraceReportView(fromDate, toDate, roomClass, department, status, byAlphabetical, byRoom, byVip, pseudoRoom, reserved, checkedIn, dueout, individual, blockcode, vipOnly);

            // 👉 Thêm cột TraceDateOnly
            if (!dataTable.Columns.Contains("TraceDateOnly"))
                dataTable.Columns.Add("TraceDateOnly", typeof(DateTime));

            foreach (DataRow row in dataTable.Rows)
            {
                if (row["TraceDate"] != DBNull.Value)
                {
                    DateTime traceDate = Convert.ToDateTime(row["TraceDate"]);
                    row["TraceDateOnly"] = traceDate.Date; // 🛠 chỉ lấy phần Ngày
                }
            }
            report.Parameters["DateIn"].Value = DateTime.Now;
            report.DataSource = dataTable;
            report.RequestParameters = false;

            return PartialView("_ReportViewerPartial", report);
        }


























        //[HttpGet]
        //public IActionResult TestChartReport(DateTime fromDate, DateTime toDate)
        //{
        //    // Khởi tạo report
        //    XtraReport report = new OneSPMSh.Report.Report2();

        //    // Lấy dữ liệu gốc
        //    DataTable originalTable = BookingSourceReportBO.GetBookingSourceData(fromDate, toDate);
        //    DataTable singleRowTable = originalTable.Clone();

        //    if (originalTable.Rows.Count > 0)
        //    {
        //        singleRowTable.ImportRow(originalTable.Rows[0]); // Chỉ lấy dòng đầu tiên
        //    }

        //    // Gán nguồn dữ liệu
        //    report.DataSource = singleRowTable;

        //    // Lấy biểu đồ từ report
        //    var chart = report.FindControl("xrChart2", true) as DevExpress.XtraReports.UI.XRChart;
        //    if (chart != null)
        //    {
        //        chart.Series.Clear();

        //        string[] valueColumns = { "RNActual", "BUSMIXActual", "ADRActual", "REVForcast", "REVMIXForcast" };

        //        foreach (var column in valueColumns)
        //        {
        //            var series = new DevExpress.XtraCharts.Series(column, DevExpress.XtraCharts.ViewType.Bar);
        //            series.ArgumentDataMember = "Source         "; // Trục X
        //            series.ValueDataMembers.AddRange(column);  // Trục Y
        //            chart.Series.Add(series);
        //        }

        //        var diagram = chart.Diagram as DevExpress.XtraCharts.XYDiagram;
        //        if (diagram != null)
        //        {
        //            diagram.AxisY.WholeRange.SetMinMaxValues(0, 1000);
        //            diagram.AxisY.Title.Text = "Giá trị";
        //            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;

        //            diagram.AxisX.Title.Text = "Nguồn đặt phòng";
        //            diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
        //        }
        //    }

        //    return PartialView("_ReportViewerPartial", report);
        //}


    }

}

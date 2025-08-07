using System.Data;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using Cashiering.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace Cashiering.Controllers
{
    public class CashieringController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CashieringController> _logger;
        private readonly IMemoryCache _cache;
        private readonly ICashieringService _iCashieringService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CashieringController(ILogger<CashieringController> logger,
                IMemoryCache cache, IConfiguration configuration, ICashieringService iCashieringService, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _iCashieringService = iCashieringService;
            _httpContextAccessor = httpContextAccessor;

        }

        public IActionResult Index()
        {
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        public IActionResult PostingJournal()
        {
            List<TransactionGroupModel> tlplist = PropertyUtils.ConvertToList<TransactionGroupModel>(TransactionGroupBO.Instance.FindAll());
            ViewBag.TransactionGroupList = tlplist; // Truyền danh sách TransactionGroupModel vào ViewBag để sử dụng trong View
            List<TransactionSubGroupModel> transg = PropertyUtils.ConvertToList<TransactionSubGroupModel>(TransactionSubGroupBO.Instance.FindAll());
            ViewBag.TransactionSubGroupList = transg; // Truyền danh sách TransactionSubGroupModel vào ViewBag để sử dụng trong View
            List<UsersModel> user = PropertyUtils.ConvertToList<UsersModel>(UsersBO.Instance.FindAll());
            ViewBag.UsersList = user;
            List<TransactionsModel> trans = PropertyUtils.ConvertToList<TransactionsModel>(TransactionsBO.Instance.FindAll());
            ViewBag.TransactionsList = trans;
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpGet]
        public IActionResult GetPostingJournal(
            string cashierNo,
            string transactionCodeList,
            string roomNoList,
            DateTime fromDate,
            DateTime toDate,
            string fromProfitCode,
            string toProfitCode,
            string groupID,
            string subgroupID)
        {
            try
            {
                DataTable dataTable = _iCashieringService.PostingJournal(cashierNo, transactionCodeList, roomNoList, fromDate, toDate, fromProfitCode, toProfitCode, groupID, subgroupID);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ConfirmationNo = !string.IsNullOrEmpty(d["ConfirmationNo"].ToString()) ? d["ConfirmationNo"] : "",
                                  CRSNo = !string.IsNullOrEmpty(d["CRSNo"].ToString()) ? d["CRSNo"] : "",
                                  FolioNo = !string.IsNullOrEmpty(d["FolioNo"].ToString()) ? d["FolioNo"] : "",
                                  AccountName = !string.IsNullOrEmpty(d["AccountName"].ToString()) ? d["AccountName"] : "",
                                  Room = !string.IsNullOrEmpty(d["Room"].ToString()) ? d["Room"] : "",
                                  SubGroupCode = !string.IsNullOrEmpty(d["SubGroupCode"].ToString()) ? d["SubGroupCode"] : "",
                                  GroupCode = !string.IsNullOrEmpty(d["GroupCode"].ToString()) ? d["GroupCode"] : "",
                                  TransactionCode = !string.IsNullOrEmpty(d["TransactionCode"].ToString()) ? d["TransactionCode"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  Supplement = !string.IsNullOrEmpty(d["Supplement"].ToString()) ? d["Supplement"] : "",
                                  Reference = !string.IsNullOrEmpty(d["Reference"].ToString()) ? d["Reference"] : "",
                                  Time = !string.IsNullOrEmpty(d["Time"].ToString()) ? d["Time"] : "",
                                  TransactionDate = !string.IsNullOrEmpty(d["TransactionDate"].ToString()) ? d["TransactionDate"] : "",
                                  ReservationID = !string.IsNullOrEmpty(d["ReservationID"].ToString()) ? d["ReservationID"] : "",
                                  WinNo = !string.IsNullOrEmpty(d["WinNo"].ToString()) ? d["WinNo"] : "",
                                  CashierNo = !string.IsNullOrEmpty(d["CashierNo"].ToString()) ? d["CashierNo"] : "",
                                  ShiftID = !string.IsNullOrEmpty(d["ShiftID"].ToString()) ? d["ShiftID"] : "",
                                  InvoiceNo = !string.IsNullOrEmpty(d["InvoiceNo"].ToString()) ? d["InvoiceNo"] : "",
                                  Market = !string.IsNullOrEmpty(d["Market"].ToString()) ? d["Market"] : "",
                                  Source = !string.IsNullOrEmpty(d["Source"].ToString()) ? d["Source"] : "",
                                  Package = !string.IsNullOrEmpty(d["Package"].ToString()) ? d["Package"] : "",
                                  Origin = !string.IsNullOrEmpty(d["Origin"].ToString()) ? d["Origin"] : "",
                                  Company = !string.IsNullOrEmpty(d["Company"].ToString()) ? d["Company"] : "",
                                  Group = !string.IsNullOrEmpty(d["Group"].ToString()) ? d["Group"] : "",
                                  Agent = !string.IsNullOrEmpty(d["Agent"].ToString()) ? d["Agent"] : "",
                                  ReservationHolder = !string.IsNullOrEmpty(d["ReservationHolder"].ToString()) ? d["ReservationHolder"] : "",
                                  Country = !string.IsNullOrEmpty(d["Country"].ToString()) ? d["Country"] : "",
                                  Market_C = !string.IsNullOrEmpty(d["Market_C"].ToString()) ? d["Market_C"] : "",
                                  Source_C = !string.IsNullOrEmpty(d["Source_C"].ToString()) ? d["Source_C"] : "",
                                  Origin_C = !string.IsNullOrEmpty(d["Origin_C"].ToString()) ? d["Origin_C"] : "",
                                  Company_C = !string.IsNullOrEmpty(d["Company_C"].ToString()) ? d["Company_C"] : "",
                                  Group_C = !string.IsNullOrEmpty(d["Group_C"].ToString()) ? d["Group_C"] : "",
                                  Agent_C = !string.IsNullOrEmpty(d["Agent_C"].ToString()) ? d["Agent_C"] : "",
                                  ReservationHolder_C = !string.IsNullOrEmpty(d["ReservationHolder_C"].ToString()) ? d["ReservationHolder_C"] : "",
                                  HolderCode = !string.IsNullOrEmpty(d["HolderCode"].ToString()) ? d["HolderCode"] : "",
                                  Country_C = !string.IsNullOrEmpty(d["Country_C"].ToString()) ? d["Country_C"] : "",
                                  ProfitCenterCode = !string.IsNullOrEmpty(d["ProfitCenterCode"].ToString()) ? d["ProfitCenterCode"] : "",
                                  CreditVND = !string.IsNullOrEmpty(d["CreditVND"].ToString()) ? d["CreditVND"] : "",
                                  CreditUSD = !string.IsNullOrEmpty(d["CreditUSD"].ToString()) ? d["CreditUSD"] : "",
                                  DebitVND = !string.IsNullOrEmpty(d["DebitVND"].ToString()) ? d["DebitVND"] : "",
                                  DebitUSD = !string.IsNullOrEmpty(d["DebitUSD"].ToString()) ? d["DebitUSD"] : "",
                                  ArrivalDate = !string.IsNullOrEmpty(d["ArrivalDate"].ToString()) ? d["ArrivalDate"] : "",
                                  DepartureDate = !string.IsNullOrEmpty(d["DepartureDate"].ToString()) ? d["DepartureDate"] : "",
                                  Nationality = !string.IsNullOrEmpty(d["Nationality"].ToString()) ? d["Nationality"] : "",
                                  ProfileCode = !string.IsNullOrEmpty(d["ProfileCode"].ToString()) ? d["ProfileCode"] : "",


                              }).ToList();

                return Ok(result); 
            }
            catch (Exception ex)
            {

                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetSearchTransactionJournalByNotVatInfor(
            string cashierNo, string transactionCodeList, string roomNoList,
            DateTime fromDate, DateTime toDate, string fromProfitCode, string toProfitCode, string groupID, string subgroupID)
        {
            try
            {
                DataTable dataTable = _iCashieringService.SearchTransactionJournalByNotVatInfor(
                    cashierNo, transactionCodeList, roomNoList, fromDate, toDate, fromProfitCode, toProfitCode, groupID, subgroupID);

                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ConfirmationNo = !string.IsNullOrEmpty(d["ConfirmationNo"].ToString()) ? d["ConfirmationNo"] : "",
                                  CRSNo = !string.IsNullOrEmpty(d["CRSNo"].ToString()) ? d["CRSNo"] : "",
                                  FolioNo = !string.IsNullOrEmpty(d["FolioNo"].ToString()) ? d["FolioNo"] : "",
                                  AccountName = !string.IsNullOrEmpty(d["AccountName"].ToString()) ? d["AccountName"] : "",
                                  Room = !string.IsNullOrEmpty(d["Room"].ToString()) ? d["Room"] : "",
                                  SubGroupCode = !string.IsNullOrEmpty(d["SubGroupCode"].ToString()) ? d["SubGroupCode"] : "",
                                  GroupCode = !string.IsNullOrEmpty(d["GroupCode"].ToString()) ? d["GroupCode"] : "",
                                  TransactionCode = !string.IsNullOrEmpty(d["TransactionCode"].ToString()) ? d["TransactionCode"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  Supplement = !string.IsNullOrEmpty(d["Supplement"].ToString()) ? d["Supplement"] : "",
                                  Reference = !string.IsNullOrEmpty(d["Reference"].ToString()) ? d["Reference"] : "",
                                  Time = !string.IsNullOrEmpty(d["Time"].ToString()) ? d["Time"] : "",
                                  TransactionDate = !string.IsNullOrEmpty(d["TransactionDate"].ToString()) ? d["TransactionDate"] : "",
                                  ReservationID = !string.IsNullOrEmpty(d["ReservationID"].ToString()) ? d["ReservationID"] : "",
                                  WinNo = !string.IsNullOrEmpty(d["WinNo"].ToString()) ? d["WinNo"] : "",
                                  CashierNo = !string.IsNullOrEmpty(d["CashierNo"].ToString()) ? d["CashierNo"] : "",
                                  ShiftID = !string.IsNullOrEmpty(d["ShiftID"].ToString()) ? d["ShiftID"] : "",
                                  InvoiceNo = !string.IsNullOrEmpty(d["InvoiceNo"].ToString()) ? d["InvoiceNo"] : "",
                                  Market = !string.IsNullOrEmpty(d["Market"].ToString()) ? d["Market"] : "",
                                  Source = !string.IsNullOrEmpty(d["Source"].ToString()) ? d["Source"] : "",
                                  Package = !string.IsNullOrEmpty(d["Package"].ToString()) ? d["Package"] : "",
                                  Origin = !string.IsNullOrEmpty(d["Origin"].ToString()) ? d["Origin"] : "",
                                  Company = !string.IsNullOrEmpty(d["Company"].ToString()) ? d["Company"] : "",
                                  Group = !string.IsNullOrEmpty(d["Group"].ToString()) ? d["Group"] : "",
                                  Agent = !string.IsNullOrEmpty(d["Agent"].ToString()) ? d["Agent"] : "",
                                  ReservationHolder = !string.IsNullOrEmpty(d["ReservationHolder"].ToString()) ? d["ReservationHolder"] : "",
                                  Country = !string.IsNullOrEmpty(d["Country"].ToString()) ? d["Country"] : "",
                                  Market_C = !string.IsNullOrEmpty(d["Market_C"].ToString()) ? d["Market_C"] : "",
                                  Source_C = !string.IsNullOrEmpty(d["Source_C"].ToString()) ? d["Source_C"] : "",
                                  Origin_C = !string.IsNullOrEmpty(d["Origin_C"].ToString()) ? d["Origin_C"] : "",
                                  Company_C = !string.IsNullOrEmpty(d["Company_C"].ToString()) ? d["Company_C"] : "",
                                  Group_C = !string.IsNullOrEmpty(d["Group_C"].ToString()) ? d["Group_C"] : "",
                                  Agent_C = !string.IsNullOrEmpty(d["Agent_C"].ToString()) ? d["Agent_C"] : "",
                                  ReservationHolder_C = !string.IsNullOrEmpty(d["ReservationHolder_C"].ToString()) ? d["ReservationHolder_C"] : "",
                                  HolderCode = !string.IsNullOrEmpty(d["HolderCode"].ToString()) ? d["HolderCode"] : "",
                                  Country_C = !string.IsNullOrEmpty(d["Country_C"].ToString()) ? d["Country_C"] : "",
                                  ProfitCenterCode = !string.IsNullOrEmpty(d["ProfitCenterCode"].ToString()) ? d["ProfitCenterCode"] : "",
                                  CreditVND = !string.IsNullOrEmpty(d["CreditVND"].ToString()) ? d["CreditVND"] : "",
                                  CreditUSD = !string.IsNullOrEmpty(d["CreditUSD"].ToString()) ? d["CreditUSD"] : "",
                                  DebitVND = !string.IsNullOrEmpty(d["DebitVND"].ToString()) ? d["DebitVND"] : "",
                                  DebitUSD = !string.IsNullOrEmpty(d["DebitUSD"].ToString()) ? d["DebitUSD"] : "",
                                  ArrivalDate = !string.IsNullOrEmpty(d["ArrivalDate"].ToString()) ? d["ArrivalDate"] : "",
                                  DepartureDate = !string.IsNullOrEmpty(d["DepartureDate"].ToString()) ? d["DepartureDate"] : "",
                                  Nationality = !string.IsNullOrEmpty(d["Nationality"].ToString()) ? d["Nationality"] : "",
                                  ProfileCode = !string.IsNullOrEmpty(d["ProfileCode"].ToString()) ? d["ProfileCode"] : "",


                              }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetSearchTransactionJournalByVatInfor(
            string cashierNo, string transactionCodeList, string roomNoList,
            DateTime fromDate, DateTime toDate, string fromProfitCode, string toProfitCode, string groupID, string subgroupID)
        {
            try
            {
                DataTable dataTable = _iCashieringService.SearchTransactionJournalByVatInfor(
                    cashierNo, transactionCodeList, roomNoList, fromDate, toDate, fromProfitCode, toProfitCode, groupID, subgroupID);

                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ConfirmationNo = !string.IsNullOrEmpty(d["ConfirmationNo"].ToString()) ? d["ConfirmationNo"] : "",
                                  CRSNo = !string.IsNullOrEmpty(d["CRSNo"].ToString()) ? d["CRSNo"] : "",
                                  FolioNo = !string.IsNullOrEmpty(d["FolioNo"].ToString()) ? d["FolioNo"] : "",
                                  AccountName = !string.IsNullOrEmpty(d["AccountName"].ToString()) ? d["AccountName"] : "",
                                  Room = !string.IsNullOrEmpty(d["Room"].ToString()) ? d["Room"] : "",
                                  SubGroupCode = !string.IsNullOrEmpty(d["SubGroupCode"].ToString()) ? d["SubGroupCode"] : "",
                                  GroupCode = !string.IsNullOrEmpty(d["GroupCode"].ToString()) ? d["GroupCode"] : "",
                                  TransactionCode = !string.IsNullOrEmpty(d["TransactionCode"].ToString()) ? d["TransactionCode"] : "",
                                  Description = !string.IsNullOrEmpty(d["Description"].ToString()) ? d["Description"] : "",
                                  Supplement = !string.IsNullOrEmpty(d["Supplement"].ToString()) ? d["Supplement"] : "",
                                  Reference = !string.IsNullOrEmpty(d["Reference"].ToString()) ? d["Reference"] : "",
                                  Time = !string.IsNullOrEmpty(d["Time"].ToString()) ? d["Time"] : "",
                                  TransactionDate = !string.IsNullOrEmpty(d["TransactionDate"].ToString()) ? d["TransactionDate"] : "",
                                  ReservationID = !string.IsNullOrEmpty(d["ReservationID"].ToString()) ? d["ReservationID"] : "",
                                  WinNo = !string.IsNullOrEmpty(d["WinNo"].ToString()) ? d["WinNo"] : "",
                                  CashierNo = !string.IsNullOrEmpty(d["CashierNo"].ToString()) ? d["CashierNo"] : "",
                                  ShiftID = !string.IsNullOrEmpty(d["ShiftID"].ToString()) ? d["ShiftID"] : "",
                                  InvoiceNo = !string.IsNullOrEmpty(d["InvoiceNo"].ToString()) ? d["InvoiceNo"] : "",
                                  Market = !string.IsNullOrEmpty(d["Market"].ToString()) ? d["Market"] : "",
                                  Source = !string.IsNullOrEmpty(d["Source"].ToString()) ? d["Source"] : "",
                                  Package = !string.IsNullOrEmpty(d["Package"].ToString()) ? d["Package"] : "",
                                  Origin = !string.IsNullOrEmpty(d["Origin"].ToString()) ? d["Origin"] : "",
                                  Company = !string.IsNullOrEmpty(d["Company"].ToString()) ? d["Company"] : "",
                                  Group = !string.IsNullOrEmpty(d["Group"].ToString()) ? d["Group"] : "",
                                  Agent = !string.IsNullOrEmpty(d["Agent"].ToString()) ? d["Agent"] : "",
                                  ReservationHolder = !string.IsNullOrEmpty(d["ReservationHolder"].ToString()) ? d["ReservationHolder"] : "",
                                  Country = !string.IsNullOrEmpty(d["Country"].ToString()) ? d["Country"] : "",
                                  Market_C = !string.IsNullOrEmpty(d["Market_C"].ToString()) ? d["Market_C"] : "",
                                  Source_C = !string.IsNullOrEmpty(d["Source_C"].ToString()) ? d["Source_C"] : "",
                                  Origin_C = !string.IsNullOrEmpty(d["Origin_C"].ToString()) ? d["Origin_C"] : "",
                                  Company_C = !string.IsNullOrEmpty(d["Company_C"].ToString()) ? d["Company_C"] : "",
                                  Group_C = !string.IsNullOrEmpty(d["Group_C"].ToString()) ? d["Group_C"] : "",
                                  Agent_C = !string.IsNullOrEmpty(d["Agent_C"].ToString()) ? d["Agent_C"] : "",
                                  ReservationHolder_C = !string.IsNullOrEmpty(d["ReservationHolder_C"].ToString()) ? d["ReservationHolder_C"] : "",
                                  HolderCode = !string.IsNullOrEmpty(d["HolderCode"].ToString()) ? d["HolderCode"] : "",
                                  Country_C = !string.IsNullOrEmpty(d["Country_C"].ToString()) ? d["Country_C"] : "",
                                  ProfitCenterCode = !string.IsNullOrEmpty(d["ProfitCenterCode"].ToString()) ? d["ProfitCenterCode"] : "",
                                  CreditVND = !string.IsNullOrEmpty(d["CreditVND"].ToString()) ? d["CreditVND"] : "",
                                  CreditUSD = !string.IsNullOrEmpty(d["CreditUSD"].ToString()) ? d["CreditUSD"] : "",
                                  DebitVND = !string.IsNullOrEmpty(d["DebitVND"].ToString()) ? d["DebitVND"] : "",
                                  DebitUSD = !string.IsNullOrEmpty(d["DebitUSD"].ToString()) ? d["DebitUSD"] : "",
                                  ArrivalDate = !string.IsNullOrEmpty(d["ArrivalDate"].ToString()) ? d["ArrivalDate"] : "",
                                  DepartureDate = !string.IsNullOrEmpty(d["DepartureDate"].ToString()) ? d["DepartureDate"] : "",
                                  Nationality = !string.IsNullOrEmpty(d["Nationality"].ToString()) ? d["Nationality"] : "",
                                  ProfileCode = !string.IsNullOrEmpty(d["ProfileCode"].ToString()) ? d["ProfileCode"] : "",


                              }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetCashierAudit(string userName, DateTime fromDate, DateTime toDate, string shiftID)

        {
            try
            {
                DataTable dataTable = _iCashieringService.CashierAudit(userName, fromDate, toDate, shiftID);


                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  Date = !string.IsNullOrEmpty(d["Date"].ToString()) ? d["Date"] : "",
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  ShiftNo = !string.IsNullOrEmpty(d["ShiftNo"].ToString()) ? d["ShiftNo"] : "",
                                  FullName = !string.IsNullOrEmpty(d["FullName"].ToString()) ? d["FullName"] : "",
                                  CashierNo = !string.IsNullOrEmpty(d["CashierNo"].ToString()) ? d["CashierNo"] : "",
                                  UserName = !string.IsNullOrEmpty(d["UserName"].ToString()) ? d["UserName"] : "",
                                  LoginTime = !string.IsNullOrEmpty(d["LoginTime"].ToString()) ? d["LoginTime"] : "",
                                  LogoutTime = !string.IsNullOrEmpty(d["LogoutTime"].ToString()) ? d["LogoutTime"] : "",
                                  AmountVND = !string.IsNullOrEmpty(d["AmountVND"].ToString()) ? d["AmountVND"] : "",
                                  AmountUSD = !string.IsNullOrEmpty(d["AmountUSD"].ToString()) ? d["AmountUSD"] : "",
                                  CountTransaction = !string.IsNullOrEmpty(d["CountTransaction"].ToString()) ? d["CountTransaction"] : "",
                                  Status = !string.IsNullOrEmpty(d["Status"].ToString()) ? d["Status"] : "",
                                  
                              }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        public IActionResult CashierAudit()
        {
            List<TransactionGroupModel> tlplist = PropertyUtils.ConvertToList<TransactionGroupModel>(TransactionGroupBO.Instance.FindAll());
            ViewBag.TransactionGroupList = tlplist; // Truyền danh sách TransactionGroupModel vào ViewBag để sử dụng trong View
            List<TransactionSubGroupModel> transg = PropertyUtils.ConvertToList<TransactionSubGroupModel>(TransactionSubGroupBO.Instance.FindAll());
            ViewBag.TransactionSubGroupList = transg; // Truyền danh sách TransactionSubGroupModel vào ViewBag để sử dụng trong View
            List<UsersModel> user = PropertyUtils.ConvertToList<UsersModel>(UsersBO.Instance.FindAll());
            ViewBag.UsersList = user;
            List<TransactionsModel> trans = PropertyUtils.ConvertToList<TransactionsModel>(TransactionsBO.Instance.FindAll());
            ViewBag.TransactionsList = trans;
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }      
       [HttpGet]
public IActionResult GetShiftDetail(int shiftID)
{
    try
    {
        // type=1: header
        DataTable headerTable = _iCashieringService.ShiftDetail(shiftID, 1);
        var header = (from d in headerTable.AsEnumerable()
                      select new
                      {
                          CashierNo = d["CashierNo"]?.ToString() ?? "",
                          ShiftNo = d["ShiftNo"]?.ToString() ?? "",
                          ShiftDate = d["ShiftDate"]?.ToString() ?? "",
                          UserName = d["UserName"]?.ToString() ?? "",
                          FullName = d["FullName"]?.ToString() ?? "",
                          LoginTime = d["LoginTime"]?.ToString() ?? "",
                          LogoutTime = d["LogoutTime"]?.ToString() ?? ""
                      }).FirstOrDefault();

        // type=0: detail grid
        DataTable detailTable = _iCashieringService.ShiftDetail(shiftID, 0);
        var details = (from d in detailTable.AsEnumerable()
                       select new
                       {
                           TransactionCode = d["TransactionCode"]?.ToString() ?? "",
                           Description = d["Description"]?.ToString() ?? "",
                           Amount = d["Amount"]?.ToString() ?? "",
                           TransactionDate = d["TransactionDate"]?.ToString() ?? "",
                           Reference = d["Reference"]?.ToString() ?? "",
                           Supplement = d["Supplement"]?.ToString() ?? "",
                           FolioID = d["FolioID"]?.ToString() ?? "",
                           RoomNo = d["RoomNo"]?.ToString() ?? "",
                           Account = d["Account"]?.ToString() ?? "",
                           PaymentType = d["*"]?.ToString() ?? "",
                           CurrencyID = d["CurrencyID"]?.ToString() ?? "",
                       }).ToList();

        return Ok(new { header, details });
    }
    catch (Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}
        [HttpGet]
        public IActionResult GetExchangeRate()

        {
            try
            {
                DataTable dataTable = _iCashieringService.ExchangeRate();


                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"] : "",
                                  DateTime = !string.IsNullOrEmpty(d["DateTime"].ToString()) ? d["DateTime"] : "",
                                  FromCurrencyID = !string.IsNullOrEmpty(d["FromCurrencyID"].ToString()) ? d["FromCurrencyID"] : "",
                                  ToCurrencyID = !string.IsNullOrEmpty(d["ToCurrencyID"].ToString()) ? d["ToCurrencyID"] : "",
                                  BuyRate = !string.IsNullOrEmpty(d["BuyRate"].ToString()) ? d["BuyRate"] : "",
                                  SellRate = !string.IsNullOrEmpty(d["SellRate"].ToString()) ? d["SellRate"] : "",
                                  ExChangeRateMin = !string.IsNullOrEmpty(d["ExChangeRateMin"].ToString()) ? d["ExChangeRateMin"] : "",
                                  DenominationMax = !string.IsNullOrEmpty(d["DenominationMax"].ToString()) ? d["DenominationMax"] : "",
                                  ExChangeRateMax = !string.IsNullOrEmpty(d["ExChangeRateMax"].ToString()) ? d["ExChangeRateMax"] : "",
                                  ExChangeRateMedium = !string.IsNullOrEmpty(d["ExChangeRateMedium"].ToString()) ? d["ExChangeRateMedium"] : "",
                                  DenominationMedium = !string.IsNullOrEmpty(d["DenominationMedium"].ToString()) ? d["DenominationMedium"] : "",
                                  DenominationMin = !string.IsNullOrEmpty(d["DenominationMin"].ToString()) ? d["DenominationMin"] : "",
                                  CreateBy = !string.IsNullOrEmpty(d["CreateBy"].ToString()) ? d["CreateBy"] : "",
                                  UpdateBy = !string.IsNullOrEmpty(d["UpdateBy"].ToString()) ? d["UpdateBy"] : "",
                                  CreateDate = !string.IsNullOrEmpty(d["CreateDate"].ToString()) ? d["CreateDate"] : "",
                                  UpdateDate = !string.IsNullOrEmpty(d["UpdateDate"].ToString()) ? d["UpdateDate"] : "",                          
                              }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        public IActionResult ExchangeRate()
        {
            List<CurrencyModel> crrlist = PropertyUtils.ConvertToList<CurrencyModel>(CurrencyBO.Instance.FindAll());
            ViewBag.CurrencyList = crrlist;        
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }
        [HttpPost]
        public IActionResult Insert(ExchangeRateModel model)
        {
            try
            {
                // Lấy UserID từ session login
                int? userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                {
                    return Unauthorized(new { error = "Session expired. Please login again." });
                }

                // Gán UserInsertID và UserUpdateID từ session
                model.UserInsertID = userId.Value;
                model.CreateDate = DateTime.Now;
                model.UserUpdateID = userId.Value;
                model.UpdateDate = DateTime.Now;

                string sql = @"
        EXEC sp_executesql N'
        INSERT INTO ExchangeRate
        (DateTime,FromCurrencyID,ToCurrencyID,
         ExChangeRate,ExChangeRateSell,
         UserInsertID,CreateDate,UserUpdateID,UpdateDate,
         ExChangeRateMax,DenominationMax,
         ExChangeRateMin,DenominationMin,
         ExChangeRateMedium,DenominationMedium)
        VALUES (@DateTime,@FromCurrencyID,@ToCurrencyID,
                @ExChangeRate,@ExChangeRateSell,
                @UserInsertID,@CreateDate,@UserUpdateID,@UpdateDate,
                @ExChangeRateMax,@DenominationMax,
                @ExChangeRateMin,@DenominationMin,
                @ExChangeRateMedium,@DenominationMedium)
        SELECT @@IDENTITY AS ''ID''',
        N'@DateTime datetime,@FromCurrencyID nvarchar(3),@ToCurrencyID nvarchar(3),
          @ExChangeRate decimal(18,4),@ExChangeRateSell decimal(18,4),
          @UserInsertID int,@CreateDate datetime,@UserUpdateID int,@UpdateDate datetime,
          @ExChangeRateMax decimal(18,4),@DenominationMax int,
          @ExChangeRateMin decimal(18,4),@DenominationMin int,
          @ExChangeRateMedium decimal(18,4),@DenominationMedium int',
        @DateTime=@DateTime,@FromCurrencyID=@FromCurrencyID,@ToCurrencyID=@ToCurrencyID,
        @ExChangeRate=@ExChangeRate,@ExChangeRateSell=@ExChangeRateSell,
        @UserInsertID=@UserInsertID,@CreateDate=@CreateDate,@UserUpdateID=@UserUpdateID,@UpdateDate=@UpdateDate,
        @ExChangeRateMax=@ExChangeRateMax,@DenominationMax=@DenominationMax,
        @ExChangeRateMin=@ExChangeRateMin,@DenominationMin=@DenominationMin,
        @ExChangeRateMedium=@ExChangeRateMedium,@DenominationMedium=@DenominationMedium";

                SqlParameter[] parameters = {
            new SqlParameter("@DateTime", model.DateTime ?? (object)DBNull.Value),
            new SqlParameter("@FromCurrencyID", model.FromCurrencyID ?? (object)DBNull.Value),
            new SqlParameter("@ToCurrencyID", model.ToCurrencyID ?? (object)DBNull.Value),
            new SqlParameter("@ExChangeRate", model.ExChangeRate),
            new SqlParameter("@ExChangeRateSell", model.ExChangeRateSell),
            new SqlParameter("@UserInsertID", model.UserInsertID),
            new SqlParameter("@CreateDate", model.CreateDate),
            new SqlParameter("@UserUpdateID", model.UserUpdateID),
            new SqlParameter("@UpdateDate", model.UpdateDate),
            new SqlParameter("@ExChangeRateMax", model.ExChangeRateMax),
            new SqlParameter("@DenominationMax", model.DenominationMax),
            new SqlParameter("@ExChangeRateMin", model.ExChangeRateMin),
            new SqlParameter("@DenominationMin", model.DenominationMin),
            new SqlParameter("@ExChangeRateMedium", model.ExChangeRateMedium),
            new SqlParameter("@DenominationMedium", model.DenominationMedium)
        };

                int newId = DataTableHelper.ExecuteInsertAndReturnId(sql, parameters);

                return Json(new { id = newId, inserted = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost]
        public IActionResult Update(ExchangeRateModel model)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                {
                    return Unauthorized(new { error = "Session expired. Please login again." });
                }

                model.UserUpdateID = userId.Value;
                model.UpdateDate = DateTime.Now;

                string sql = @"
            UPDATE ExchangeRate
            SET
                DateTime=@DateTime,
                FromCurrencyID=@FromCurrencyID,
                ToCurrencyID=@ToCurrencyID,
                ExChangeRate=@ExChangeRate,
                ExChangeRateSell=@ExChangeRateSell,
                UserUpdateID=@UserUpdateID,
                UpdateDate=@UpdateDate,
                ExChangeRateMax=@ExChangeRateMax,
                DenominationMax=@DenominationMax,
                ExChangeRateMin=@ExChangeRateMin,
                DenominationMin=@DenominationMin,
                ExChangeRateMedium=@ExChangeRateMedium,
                DenominationMedium=@DenominationMedium
            WHERE ID=@ID";

                SqlParameter[] parameters = {
            new SqlParameter("@DateTime", model.DateTime ?? (object)DBNull.Value),
            new SqlParameter("@FromCurrencyID", model.FromCurrencyID ?? (object)DBNull.Value),
            new SqlParameter("@ToCurrencyID", model.ToCurrencyID ?? (object)DBNull.Value),
            new SqlParameter("@ExChangeRate", model.ExChangeRate),
            new SqlParameter("@ExChangeRateSell", model.ExChangeRateSell),
            new SqlParameter("@UserUpdateID", model.UserUpdateID),
            new SqlParameter("@UpdateDate", model.UpdateDate),
            new SqlParameter("@ExChangeRateMax", model.ExChangeRateMax),
            new SqlParameter("@DenominationMax", model.DenominationMax),
            new SqlParameter("@ExChangeRateMin", model.ExChangeRateMin),
            new SqlParameter("@DenominationMin", model.DenominationMin),
            new SqlParameter("@ExChangeRateMedium", model.ExChangeRateMedium),
            new SqlParameter("@DenominationMedium", model.DenominationMedium),
            new SqlParameter("@ID", model.ID)
        };

                int rows = DataTableHelper.ExecuteNonQueryText(sql, parameters);

                return Json(new { id = model.ID, updated = rows });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM ExchangeRate WHERE ID=@ID";

                SqlParameter[] parameters = {
            new SqlParameter("@ID", id)
        };

                int rows = DataTableHelper.ExecuteNonQueryText(sql, parameters);

                return Json(new { deleted = rows });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }





    }
}




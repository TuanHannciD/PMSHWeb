using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Web.Internal;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Import.Doc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Reservation.Commons.Helpers;
using Reservation.Services.Interfaces;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Reservation.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReservationController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IReservationService _iReservationService;
        private readonly IFolioDetailService _iFolioDetailService;

        public ReservationController(ILogger<ReservationController> logger,
                IMemoryCache cache, IConfiguration configuration, IReservationService iReservationService,IFolioDetailService folioDetailService)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _iReservationService = iReservationService;
            _iFolioDetailService = folioDetailService;
        }
        public IActionResult SearchReservation()
        {
            List<BusinessDateModel> businessDateModel = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
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
            ViewBag.businesDate = businessDateModel[0].BusinessDate;
            ViewBag.cboItem = ListItemHelper.GetItemInventoryProvider();
            return View();
        }


        public IActionResult NewReservation()
        {
            List<BusinessDateModel> businessDateModel = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
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
            ViewBag.businesDate = businessDateModel[0].BusinessDate;
            ViewBag.cboItem = ListItemHelper.GetItemInventoryProvider();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetInfoProfile(int profileID)
        {
            try
            {

                ProfileModel profile = (ProfileModel)ProfileBO.Instance.FindByPrimaryKey(profileID);

                return Json(profile);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetInfoProfileIndividual(int id)
        {
            try
            {
                ReservationModel res = (ReservationModel)ReservationBO.Instance.FindByPrimaryKey(id);
                ProfileModel profile = (ProfileModel)ProfileBO.Instance.FindByPrimaryKey(res.ProfileIndividualId);

                return Json(profile);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRateCode(DateTime arrivalDate,DateTime departure,int adults,int roomType)
        {
            try
            {
                DataTable myData = _iReservationService.GetRateCode(arrivalDate, departure, adults,roomType);


                var result = (from d in myData.AsEnumerable()
                              select new
                              {
                                  RateCodeID = int.Parse(d["RateCodeID"].ToString()),
                                  RoomTypeID = d["RoomTypeID"].ToString(),
                                  RateCode =d["RateCode"].ToString(),
                                  Amount = d["Amount"].ToString(),
                                  AmountAfterTax = d["AmountAfterTax"].ToString()

                                  //FigureImage = d["FigureImage"].ToString(),

                              }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", detail = ex.Message });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms(DateTime fromDate, DateTime ToDate,string floor,string roomTypeID,string smoking,string foStatus,
            string hkStatus,string isDummy,string roomNo)
        {
            try
            {
                int roomID = 0;
                int type = 0;
                string hk = "";
                if (string.IsNullOrEmpty(roomNo))
                {
                    roomNo = "";
                }
                if (string.IsNullOrEmpty(floor))
                {
                    floor = "";
                }
                if (string.IsNullOrEmpty(roomTypeID))
                {
                    roomTypeID = "";
                }
                if (string.IsNullOrEmpty(smoking))
                {
                    smoking = "";
                }
                if (string.IsNullOrEmpty(foStatus))
                {
                    foStatus = "";
                }
                if (string.IsNullOrEmpty(hkStatus))
                {
                    hk = "";
                }
                else
                {
                    hk = string.Join("','", hkStatus.Split(','));
                }
                var list = PropertyUtils.ConvertToList<RoomModel>(RoomBO.Instance.FindAll()).Where(x => x.RoomNo == roomNo).ToList();
                if (list.Count > 0) {
                    roomID = list[0].ID;
                }

                DataTable myData = _iReservationService.GetRoomAvailable(fromDate, ToDate, floor, roomTypeID, smoking, foStatus, hk, isDummy, roomNo, roomID, type);
                var result = (from d in myData.AsEnumerable()
                              select new
                              {
                                  RoomID = d["RoomID"].ToString(),
                                  RoomNo = d["RoomNo"].ToString(),
                                  RoomType = d["RoomType"].ToString(),
                                  HKStatus = d["HKStatus"].ToString(),
                                  FO = d["FO"].ToString(),
                                  Floor = d["Floor"].ToString(),
                                  Connecting = d["Connecting"].ToString(),
                                  RoomTypeID = d["RoomTypeID"].ToString(),
                                  Dummy = d["Dummy"].ToString(),
                                  GuestCheckOut = d["GuestCheckOut"].ToString(),
                                  Balcony = d["Balcony"].ToString(),

                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllMarket()
        {
            try
            {
                List<MarketTypeModel> listMarketType = PropertyUtils.ConvertToList<MarketTypeModel>(MarketTypeBO.Instance.FindByAttribute("Inactive", 0));
                List<MarketModel> listMarket = PropertyUtils.ConvertToList<MarketModel>(MarketBO.Instance.FindAll());

                // Phẳng hóa dữ liệu
                var marketTypes = listMarketType.Select(mt => new
                {
                    Id = mt.ID,
                    Name = mt.Name,
                    Code = mt.Code,
                    ParentId = 0 // Root
                });

                var markets = listMarket.Select(m => new
                {
                    Id = m.ID,
                    Name = m.Name,
                    Code = m.Code,
                    ParentId = m.MarketTypeID // Là ID của market type
                });

                // Ghép lại thành một list duy nhất
                var treeData = marketTypes.Concat(markets).ToList();

                return Json(treeData);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllotmentSearch(string code, string marketID, string allotmentTypeID, string profileID,string isDefault)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    code = "";
                }
                if (string.IsNullOrEmpty(marketID))
                {
                    marketID = "";
                }
                if (string.IsNullOrEmpty(allotmentTypeID))
                {
                    allotmentTypeID = "";
                }
                if (string.IsNullOrEmpty(profileID))
                {
                    profileID = "";
                }
                if (string.IsNullOrEmpty(isDefault))
                {
                    isDefault = "";
                }
                DataTable myData = _iReservationService.GetAllotment(code, marketID, profileID, isDefault, allotmentTypeID);
                List<MarketModel> listMarketType = PropertyUtils.ConvertToList<MarketModel>(MarketBO.Instance.FindByAttribute("Inactive", 0));
                List<AllotmentTypeModel> listAllotmentType = PropertyUtils.ConvertToList<AllotmentTypeModel>(AllotmentTypeBO.Instance.FindByAttribute("Inactive", 0));

                var result = (from d in myData.AsEnumerable()
                              let marketIDs = d["MarketID"].ToString()
                              let allotmentTypeIDs = d["AllotmentTypeID"].ToString()
                              let matchedMarket = listMarketType.FirstOrDefault(m => m.ID.ToString() == marketID)
                              let allotmentType = listAllotmentType.FirstOrDefault(m => m.ID.ToString() == allotmentTypeIDs)

                              select new
                              {
                                  ID = d["ID"].ToString(),
                                  Code = d["Code"].ToString(),
                                  AllotmentName = d["AllotmentName"].ToString(),
                                  AccountName = d["AccountName"].ToString(),
                                  MarketID = d["MarketID"].ToString(),
                                  Market = matchedMarket != null ? matchedMarket.Code : "",
                                  AllotmentType = allotmentType != null ? allotmentType.Code : "",
                                  CuttOfDay = d["CuttOfDay"].ToString(),
                                  CuttOfDate = d["CuttOfDate"].ToString(),
                                  AllotmentTypeID = d["AllotmentTypeID"].ToString(),
                                  CreateBy = d["CreateBy"].ToString(),
                                  CreateDate = !string.IsNullOrEmpty(d["CreateDate"].ToString()) ? d["CreateDate"] : "",
                                  UpdateBy = d["UpdateBy"].ToString(),
                                  UpdateDate = !string.IsNullOrEmpty(d["UpdateDate"].ToString()) ? d["UpdateDate"] : "",
                                  ProfileID = d["ProfileID"].ToString(),
                                  IsDefault = d["IsDefault"].ToString(),

                              }).ToList();
                
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllotmentSearchDetail(int allotmentID)
        {
            try
            {
                List<RoomTypeModel>roomTypeModels = PropertyUtils.ConvertToList<RoomTypeModel>(RoomTypeBO.Instance.FindByAttribute("Inactive", 0));
                string allCodes = string.Join(",", roomTypeModels.Select(x => x.Code));
                DateTime date = new DateTime(1900, 1, 1);
                DataTable myData = _iReservationService.GetAllotmentDetail(allotmentID, allCodes, date);

                var result = (from d in myData.AsEnumerable()
                              select d.Table.Columns.Cast<DataColumn>()
                                  .Where(col => col.ColumnName != "AllotmentStageID" && col.ColumnName != "flag" && col.ColumnName != "Total") 
                                  .ToDictionary(
                                      col => col.ColumnName,
                                      col => d[col.ColumnName]?.ToString()
                                  )).ToList();


                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPreference(string code,int preferenceGroup)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    code = "";
                }
                DataTable myData = _iReservationService.GetReservationPreference(code, preferenceGroup);

                var result = (from d in myData.AsEnumerable()

                              select new
                              {
                                  PreferenceID = d["PreferenceID"].ToString(),
                                  Code = d["Code"].ToString(),
                                  Description = d["Description"].ToString(),
  

                              }).ToList();


                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ReservationRateQueryDetail(DateTime fromDate, DateTime toDate, int roomType, int adults, int noOfNight, int packageID, 
            int promotionID,int func,int display,int dayUse, int c1, int c2,int c3, int noOfRoom,string currency)
        {
            try
            {
                string tableName = "VwRatRateQuery";
                string onRows = "RateCode";
                string onRowsAlias = "RateCode";
                string onCols = "Code";
                string sumcol = "A2";

                DataTable myData = _iReservationService.ReservationRateQueryDetail( fromDate,  toDate,  roomType,  adults,  noOfNight,  packageID,
                 promotionID,  tableName,  onRows,  onRowsAlias,  onCols,  sumcol,  func,  currency,  display,dayUse,  c1,  c2,  c3,  noOfRoom);

                var result = (from d in myData.AsEnumerable()
                              select d.Table.Columns.Cast<DataColumn>()
                                  //.Where(col => col.ColumnName != "AllotmentStageID" && col.ColumnName != "flag" && col.ColumnName != "Total")
                                  .ToDictionary(
                                      col => col.ColumnName,
                                      col => d[col.ColumnName]?.ToString()
                                  )).ToList();


                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CaculateNet(DateTime fromDate,DateTime toDate,int rateCodeID,int roomTypeID,
            string currencyID,int packageID,int day,decimal price,string transactionCode,decimal discountPercent, decimal discountAmount)
        {
            try
            {
                if (rateCodeID != 0)
                {
                    var data = _iReservationService.ReservationGetRateQueryDetail(fromDate, toDate, rateCodeID, roomTypeID, currencyID, packageID, day);
                    transactionCode = data.Rows[0]["TransactionCode"].ToString();
                }
                else
                {

                    transactionCode = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindAll()).
                    Where(x => x.KeyName == "RoomCharge").ToList()[0].KeyValue;

                }
                var (originalPrice, priceAfter, priceDiscount, priceAfterDiscount) = _iReservationService.CalculateNet(price, transactionCode, discountAmount, discountPercent);

                // Tạo đối tượng JSON để trả về
                var result = new
                {
                    Price = originalPrice,
                    PriceAfter = priceAfter,
                    PriceDiscount = priceDiscount,
                    PriceAfterDiscount = priceAfterDiscount
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //[ValidateAntiForgeryToken]
        #region save reservation
        [HttpPost]
        public ActionResult SaveReservation()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                List<BusinessDateModel> businessDate = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
                int memberTypeID = 0; int roomTypeID = 0; int vipID = 0;
                if (string.IsNullOrEmpty(Request.Form["memberType"].ToString()))
                {
                    memberTypeID = 0;
                }
                if (string.IsNullOrEmpty(Request.Form["vipID"].ToString()))
                {
                    vipID = 0;
                }
                if (!string.IsNullOrEmpty(Request.Form["roomTypeID"].ToString()))
                {
                    roomTypeID = int.Parse(Request.Form["roomTypeID"].ToString());
                }
                if (string.IsNullOrEmpty(Request.Form["lastName"].ToString()))
                {
                    return Json(new { code = 1, msg = "Profile cannot be blank" });
                }
                if (string.IsNullOrEmpty(Request.Form["reservationTypeCode"].ToString()))
                {
                    return Json(new { code = 1, msg = "Reservation Type cannot be blank" });
                }
                if (decimal.Parse(Request.Form["rateAmount"].ToString()) == 0)
                {
                    return Json(new { code = 1, msg = "Rate cannot be blank " });

                }
                MemberTypeModel memberType = (MemberTypeModel)MemberTypeBO.Instance.FindByPrimaryKey(memberTypeID);
                VIPModel vip = (VIPModel)VIPBO.Instance.FindByPrimaryKey(vipID);
                RoomTypeModel roomType = (RoomTypeModel)RoomTypeBO.Instance.FindByPrimaryKey(roomTypeID);
                ReservationModel reservationModel = new ReservationModel();

                #region lưu reservation
                reservationModel.ConfirmationNo = (ReservationBO.GetTopConfirmationNo() + 1).ToString();
                reservationModel.ReservationNo = (ReservationBO.GetTopID() + 1).ToString();
                reservationModel.ReservationDate = businessDate[0].BusinessDate;
                reservationModel.ProfileAgentId = int.Parse(Request.Form["profileAgentID"].ToString());
                reservationModel.AgentName = Request.Form["agentName"].ToString();
                reservationModel.ProfileCompanyId = int.Parse(Request.Form["profileCompanyID"].ToString());
                reservationModel.CompanyName = Request.Form["companyName"].ToString();
                reservationModel.ProfileSourceId = 0;
                reservationModel.SourceName = "";
                reservationModel.ProfileGroupId = 0;
                reservationModel.GroupCode = Request.Form["groupCode"].ToString();
                reservationModel.GroupName = "";
                reservationModel.ProfileContactId = int.Parse(Request.Form["profileContactID"].ToString());
                reservationModel.ContactName = Request.Form["contactName"].ToString();
                reservationModel.ContactPhone = Request.Form["contactPhone"].ToString();
                reservationModel.ProfileComment = "";
                reservationModel.ProfileIndividualId = int.Parse(Request.Form["profileIndividualID"].ToString());
                reservationModel.LastName = Request.Form["lastName"].ToString();
                reservationModel.FirstName = Request.Form["firstName"].ToString();
                reservationModel.Title = Request.Form["title"].ToString();
                reservationModel.Phone = Request.Form["phone"].ToString();
                reservationModel.Email = Request.Form["email"].ToString();
                if (memberType != null)
                {
                    reservationModel.MemberType = memberType.Code;
                    reservationModel.MemberLevel = memberType.Level;
                }
                else
                {
                    reservationModel.MemberType = "";
                    reservationModel.MemberLevel = "";
                }
                reservationModel.MemberNo = Request.Form["memberNo"].ToString();
                if (vip != null)
                {
                    reservationModel.VipId = vip.ID;
                    reservationModel.Vip = vip.Code;
                }
                else
                {
                    reservationModel.VipId = 0;
                    reservationModel.Vip = "";
                }
                reservationModel.Address = Request.Form["address"].ToString();
                reservationModel.City = Request.Form["city"].ToString();
                reservationModel.Zip = "";
                reservationModel.State = "";
                reservationModel.Country = Request.Form["nationality"].ToString();
                reservationModel.Language = "";
                reservationModel.ArrivalDate = DateTime.Parse(Request.Form["arrival"].ToString());
                reservationModel.OriginalArrivalDate = DateTime.Parse(Request.Form["arrival"].ToString());
                reservationModel.NoOfNight = int.Parse(Request.Form["noOfNight"].ToString());
                reservationModel.DepartureDate = DateTime.Parse(Request.Form["departure"].ToString());
                reservationModel.OriginalDepartureDate = DateTime.Parse(Request.Form["departure"].ToString());
                reservationModel.NoOfAdult = int.Parse(Request.Form["noOfAdult"].ToString());
                reservationModel.NoOfChild = int.Parse(Request.Form["noOfChild"].ToString());
                reservationModel.NoOfChild1 = int.Parse(Request.Form["noOfChild1"].ToString());
                reservationModel.NoOfChild2 = int.Parse(Request.Form["noOfChild2"].ToString());
                reservationModel.NoOfRoom = int.Parse(Request.Form["noOfRoom"].ToString());
                if (roomType != null)
                {
                    reservationModel.RoomTypeId = roomType.ID;
                    reservationModel.RoomType = roomType.Code;
                }
                else
                {
                    reservationModel.RoomTypeId = 0;
                    reservationModel.RoomType = "";
                }
                reservationModel.RtcId = int.Parse(Request.Form["rtcID"].ToString());
                if (string.IsNullOrEmpty(Request.Form["roomNo"].ToString()))
                {
                    reservationModel.RoomId = 0;
                    reservationModel.RoomNo = "";
                }
                else
                {
                    reservationModel.RoomId = int.Parse(Request.Form["roomID"].ToString());
                    reservationModel.RoomNo = Request.Form["roomNo"].ToString();
                }

                reservationModel.BusinessBlockId = 0;
                reservationModel.BusinessBlockCode = "";
                reservationModel.Eta = !string.IsNullOrEmpty(Request.Form["eta"].ToString()) ? Request.Form["eta"].ToString() : "";
                reservationModel.CheckInDate = DateTime.Parse(Request.Form["arrival"].ToString());
                reservationModel.Etd = !string.IsNullOrEmpty(Request.Form["etd"].ToString()) ? Request.Form["etd"].ToString() : "";
                reservationModel.CheckOutDate = DateTime.Parse(Request.Form["arrival"].ToString());
                reservationModel.ReservationTypeId = int.Parse(Request.Form["reservationType"].ToString());
                reservationModel.ReservationTypeCode = Request.Form["reservationTypeCode"].ToString();
                if (string.IsNullOrEmpty(Request.Form["marketID"].ToString()))
                {
                    reservationModel.MarketId = 0;

                }
                else
                {
                    reservationModel.MarketId = int.Parse(Request.Form["marketID"].ToString());

                }
                reservationModel.MarketCode = Request.Form["marketCode"].ToString();
                reservationModel.SourceId = int.Parse(Request.Form["sourceID"].ToString());
                reservationModel.SourceCode = Request.Form["sourceCode"].ToString();
                reservationModel.OriginId = 0;
                reservationModel.OriginCode = "";
                reservationModel.CCHolder = "";
                if (string.IsNullOrEmpty(Request.Form["bookerID"].ToString()))
                {
                    reservationModel.BookerId = 0;
                    reservationModel.BookerName = "";
                }
                else
                {
                    reservationModel.BookerId = int.Parse(Request.Form["bookerID"].ToString());
                    reservationModel.BookerName = Request.Form["bookerName"].ToString();
                }

                reservationModel.BookerDetails = "";
                reservationModel.NoPost = false;
                reservationModel.PrintRate = true;
                reservationModel.ConfirmationStatus = true;
                reservationModel.VideoCheckOutStatus = false;
                reservationModel.CRSNo = "";
                reservationModel.DiscountAmount = decimal.Parse(Request.Form["discountAmount"].ToString());
                reservationModel.DiscountRate = decimal.Parse(Request.Form["discountRate"].ToString());
                reservationModel.DiscountReason = "";
                reservationModel.Comment = Request.Form["comment"].ToString();
                reservationModel.BalanceUSD = 0;
                reservationModel.BalanceVND = 0;
                reservationModel.ApprovalCode = "";
                reservationModel.ApprovalAmount = 0;
                reservationModel.SuiteWith = "";
                reservationModel.PaymentMethod = "";
                reservationModel.CreditCardNo = "";
                reservationModel.ExpirationDate = DateTime.Now;
                reservationModel.TaxTypeId = 0;
                reservationModel.ExemptNumber = "";
                reservationModel.PickupReqdId = int.Parse(Request.Form["pickedId"].ToString());
                reservationModel.PickupTransportType = Request.Form["pickUpTransportType"].ToString();
                reservationModel.PickupStationCode = Request.Form["pickUpStationCode"].ToString();
                reservationModel.PickupCarrierCode = Request.Form["pickUpCarrierCode"].ToString();
                reservationModel.PickupTime = Request.Form["pickUpTime"].ToString();
                reservationModel.PickupTransportNo = Request.Form["pickUpTransportNo"].ToString();
                reservationModel.PickupArrivalDate = DateTime.Parse(Request.Form["pickUpDate"].ToString());
                reservationModel.PickupDescription = Request.Form["pickUpDescription"].ToString();
                reservationModel.DropOffReqdId = int.Parse(Request.Form["dropOffId"].ToString());
                reservationModel.DropOffTransportType = Request.Form["dropOffTransportType"].ToString();
                reservationModel.DropOffStationCode = Request.Form["dropOffStationCode"].ToString();
                reservationModel.DropOffCarrierCode = Request.Form["dropOffCarrierCode"].ToString();
                reservationModel.DropOffTime = Request.Form["dropOffTime"].ToString();
                reservationModel.DropOffTransportNo = Request.Form["dropOffTransportNo"].ToString();
                reservationModel.DropOffDepartureDate = DateTime.Parse(Request.Form["dropOffDate"].ToString());
                reservationModel.DropOffDescription = Request.Form["dropOffDescription"].ToString();
                reservationModel.PackageId = int.Parse(Request.Form["packageID"].ToString());
                reservationModel.Packages = Request.Form["packages"].ToString();
                reservationModel.Relationship = ReservationBO.GetTopID() + 1;
                reservationModel.Status = 0;
                reservationModel.PostingMaster = false;
                reservationModel.MainGuest = true;
                if (string.IsNullOrEmpty(Request.Form["rateCode"].ToString()))
                {
                    reservationModel.RateCodeId = 0;
                    reservationModel.RateCode = "";
                }
                else
                {
                    reservationModel.RateCodeId = int.Parse(Request.Form["rateCodeID"].ToString());
                    reservationModel.RateCode = Request.Form["rateCode"].ToString();
                }
                reservationModel.Rate = decimal.Parse(Request.Form["rateAmount"].ToString());
                reservationModel.RateAfterTax = decimal.Parse(Request.Form["rateAfter"].ToString());
                reservationModel.FixedRate = false;
                reservationModel.TotalAmount = decimal.Parse(Request.Form["rateAmount"].ToString());
                reservationModel.CurrencyId = "VND";
                reservationModel.Party = "";
                reservationModel.PartyGuest = "";
                reservationModel.IsPasserBy = false;
                reservationModel.Color = Request.Form["color"].ToString();
                reservationModel.ARNo = "";
                reservationModel.ItemInventory = Request.Form["itemInventory"].ToString();
                reservationModel.Specials = Request.Form["specials"].ToString();
                reservationModel.ShareRoom = ReservationBO.GetTopID() + 1;
                reservationModel.NoShowStatus = false;
                reservationModel.ShareRoomName = "";
                reservationModel.AccompanyName = "";
                reservationModel.RoutingTransaction = "";
                reservationModel.RoutingToProfile = Request.Form["firstName"].ToString();
                reservationModel.FixedCharge = "";
                reservationModel.CommentGroup = "";
                reservationModel.IsWalkIn = false;
                reservationModel.UserInsertId = int.Parse(Request.Form["userID"].ToString());
                reservationModel.CreateDate = DateTime.Now;
                reservationModel.UserUpdateId = int.Parse(Request.Form["userID"].ToString());
                reservationModel.UpdateDate = DateTime.Now;
                reservationModel.CreateBy = Request.Form["userName"].ToString();
                reservationModel.UpdateBy = Request.Form["userName"].ToString();
                reservationModel.SpecialUpdateBy = Request.Form["userName"].ToString();
                reservationModel.SpecialUpdateDate = DateTime.Now;
                reservationModel.IsAdvanceBill = false;
                if (string.IsNullOrEmpty(Request.Form["allotmentID"].ToString()))
                {
                    reservationModel.AllotmentId = 0;
                    reservationModel.AllotmentCode = "";
                }
                else
                {
                    reservationModel.AllotmentId = int.Parse(Request.Form["allotmentID"].ToString());
                    reservationModel.AllotmentCode = Request.Form["allotmentCode"].ToString();
                }
                reservationModel.PinCode = (ReservationBO.GetTopID() + 1).ToString();
                reservationModel.PersonInChargeId = int.Parse(Request.Form["perrsonInCharge"].ToString());
                reservationModel.RoomNight = int.Parse(Request.Form["roomNight"].ToString());
                reservationModel.CardId = "";
                reservationModel.Breakfast = false;
                reservationModel.Dinner = false;
                reservationModel.Lunch = false;
                reservationModel.FixedMeal = false;
                reservationModel.VoucherId = "";
                long reservationID = ReservationBO.Instance.Insert(reservationModel);
                #endregion

                #region lưu reservation master
                if (int.Parse(Request.Form["profileAgentID"].ToString()) != 0 || int.Parse(Request.Form["profileCompanyID"].ToString()) != 0)
                {
                    ReservationModel reservationMaster = new ReservationModel();
                    reservationMaster = reservationModel;
                    reservationMaster.ReservationNo = "0";
                    reservationMaster.ProfileIndividualId = 0;
                    reservationMaster.LastName = "* " + reservationMaster.AgentName + ", Master *";
                    reservationMaster.FirstName = "Master *";
                    reservationMaster.NoOfAdult = 0;
                    reservationMaster.NoOfChild = reservationMaster.NoOfChild1 = reservationMaster.NoOfChild2 = 0;
                    reservationMaster.RoomTypeId = 8;
                    reservationMaster.RoomType = "DMR";
                    reservationMaster.RtcId = 8;
                    reservationMaster.RoomId = 0;
                    reservationMaster.RoomNo = "";
                    reservationMaster.PostingMaster = true;
                    reservationMaster.MainGuest = false;
                    reservationMaster.ShareRoom = reservationModel.ShareRoom + 1;
                    ReservationBO.Instance.Insert(reservationMaster);
                }
                #endregion

                #region lưu reservation item inventory
                string itemInventoryString  = Request.Form["itemInventory"].ToString(); 
                List<int> itemInventory = itemInventoryString.Split(',')
                                                    .Select(x => int.Parse(x)).Where(x => x != 0)
                                                    .ToList();
                if(itemInventory.Count > 0)
                {
                    foreach(var item in itemInventory)
                    {
                        ItemModel itemModel = (ItemModel)ItemBO.Instance.FindByPrimaryKey(item);
                        if(itemModel != null && itemModel.ID != 0)
                        {
                            ReservationItemInventoryModel reservationItemInventory = new ReservationItemInventoryModel();
                            reservationItemInventory.ReservationID = (int)reservationID;
                            reservationItemInventory.ItemID = itemModel.ID;
                            reservationItemInventory.Code = itemModel.Code;
                            reservationItemInventory.Name = itemModel.Name;
                            reservationItemInventory.BeginDate = reservationModel.ArrivalDate;
                            reservationItemInventory.EndDate = reservationModel.DepartureDate;
                            reservationItemInventory.Quantity = 1;
                            reservationItemInventory.RateCode = "";
                            reservationItemInventory.PackageID = reservationModel.PackageId;
                            reservationItemInventory.Package = reservationModel.Packages;
                            reservationItemInventory.ReservationFixedChargeID = 0;
                            reservationItemInventory.UserInsertID = reservationItemInventory.UserUpdateID = int.Parse(Request.Form["userID"].ToString());
                            reservationItemInventory.CreateDate = reservationItemInventory.UpdateDate = DateTime.Now;
                            ReservationItemInventoryBO.Instance.Insert(reservationItemInventory);
                        }

                    }
                }
                #endregion

                #region tạo folio
                FolioModel folioModel = new FolioModel();
                folioModel.ARNo = "";
                folioModel.FolioDate = reservationModel.ReservationDate;
                folioModel.FolioNo = 1;
                folioModel.ReservationID = reservationModel.ID;
                folioModel.ProfileID = reservationModel.ProfileIndividualId;
                folioModel.AccountName = reservationModel.LastName;
                folioModel.Status = true;
                folioModel.ConfirmationNo = reservationModel.ConfirmationNo;
                folioModel.BalanceUSD = folioModel.BalanceVND = 0;
                folioModel.CreateDate = folioModel.UpdateDate = DateTime.Now;
                folioModel.UserInsertID = folioModel.UserUpdateID = reservationModel.UserInsertId;
                FolioBO.Instance.Insert(folioModel);
                #endregion
                pt.CommitTransaction();
                return Json(new { code = 0, msg = "New reservation created successfully" });

            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { code = 1, msg = ex.Message });
            }
            finally
            {
                pt.CloseConnection();

            }
        }
        #endregion

        #region search reservation
        [HttpGet]
        public async Task<IActionResult> SearchReservation2(int searchType,string name,string firstName,string reservationHolder,string confirmationNo,
            string crsNo,string roomNo,string roomType,string package,string zone,DateTime arrivalFrom, DateTime arrivalTo,string roomSharer,string owner)
        {
            try
            {
                var data = _iReservationService.SearchReservation( searchType,  name,  firstName,  reservationHolder,  confirmationNo,
                crsNo,  roomNo,  roomType,  package,  zone,  arrivalFrom,  arrivalTo,  roomSharer,  owner);

                var result = (from d in data.AsEnumerable()
                              select d.Table.Columns.Cast<DataColumn>()
                                  //.Where(col => col.ColumnName != "AllotmentStageID" && col.ColumnName != "flag" && col.ColumnName != "Total")
                                  .ToDictionary(
                                      col => col.ColumnName,
                                      col => d[col.ColumnName]?.ToString()
                                  )).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetProfileIndividual()
        {
            try
            {
                List<ProfileModel> roomTypeModels = ReservationBO.GetProfileIndividual();
                return Json(roomTypeModels);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetReservationByID(int ID)
        {
            try
            {
                ReservationModel reservationModel = (ReservationModel)ReservationBO.Instance.FindByPrimaryKey(ID);
                return Json(reservationModel);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult EditReservation(int ID)
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                List<BusinessDateModel> businessDate = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
                int memberTypeID = 0; int roomTypeID = 0; int vipID = 0;
                if (string.IsNullOrEmpty(Request.Form["memberType"].ToString()))
                {
                    memberTypeID = 0;
                }
                if (string.IsNullOrEmpty(Request.Form["vipID"].ToString()))
                {
                    vipID = 0;
                }
                if (!string.IsNullOrEmpty(Request.Form["roomTypeID"].ToString()))
                {
                    roomTypeID = int.Parse(Request.Form["roomTypeID"].ToString());
                }
                if (string.IsNullOrEmpty(Request.Form["lastName"].ToString()))
                {
                    return Json(new { code = 1, msg = "Profile cannot be blank" });
                }
                if (string.IsNullOrEmpty(Request.Form["reservationTypeCode"].ToString()))
                {
                    return Json(new { code = 1, msg = "Reservation Type cannot be blank" });
                }
                if (decimal.Parse(Request.Form["rateAmount"].ToString()) == 0)
                {
                    return Json(new { code = 1, msg = "Rate cannot be blank " });

                }
                MemberTypeModel memberType = (MemberTypeModel)MemberTypeBO.Instance.FindByPrimaryKey(memberTypeID);
                VIPModel vip = (VIPModel)VIPBO.Instance.FindByPrimaryKey(vipID);
                RoomTypeModel roomType = (RoomTypeModel)RoomTypeBO.Instance.FindByPrimaryKey(roomTypeID);
                ReservationModel reservationModel = (ReservationModel)ReservationBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["rsvID"].ToString()));
                #region edit reservation
                reservationModel.ConfirmationNo = (ReservationBO.GetTopConfirmationNo() + 1).ToString();
                reservationModel.ReservationNo = (ReservationBO.GetTopID() + 1).ToString();
                reservationModel.ReservationDate = businessDate[0].BusinessDate;
                reservationModel.ProfileAgentId = int.Parse(Request.Form["profileAgentID"].ToString());
                reservationModel.AgentName = Request.Form["agentName"].ToString();
                reservationModel.ProfileCompanyId = int.Parse(Request.Form["profileCompanyID"].ToString());
                reservationModel.CompanyName = Request.Form["companyName"].ToString();
                reservationModel.ProfileSourceId = 0;
                reservationModel.SourceName = "";
                reservationModel.ProfileGroupId = 0;
                reservationModel.GroupCode = Request.Form["groupCode"].ToString();
                reservationModel.GroupName = "";
                reservationModel.ProfileContactId = int.Parse(Request.Form["profileContactID"].ToString());
                reservationModel.ContactName = Request.Form["contactName"].ToString();
                reservationModel.ContactPhone = Request.Form["contactPhone"].ToString();
                reservationModel.ProfileComment = "";
                reservationModel.ProfileIndividualId = int.Parse(Request.Form["profileIndividualID"].ToString());
                reservationModel.LastName = Request.Form["lastName"].ToString();
                reservationModel.FirstName = Request.Form["firstName"].ToString();
                reservationModel.Title = Request.Form["title"].ToString();
                reservationModel.Phone = Request.Form["phone"].ToString();
                reservationModel.Email = Request.Form["email"].ToString();
                if (memberType != null)
                {
                    reservationModel.MemberType = memberType.Code;
                    reservationModel.MemberLevel = memberType.Level;
                }
                else
                {
                    reservationModel.MemberType = "";
                    reservationModel.MemberLevel = "";
                }
                reservationModel.MemberNo = Request.Form["memberNo"].ToString();
                if (vip != null)
                {
                    reservationModel.VipId = vip.ID;
                    reservationModel.Vip = vip.Code;
                }
                else
                {
                    reservationModel.VipId = 0;
                    reservationModel.Vip = "";
                }
                reservationModel.Address = Request.Form["address"].ToString();
                reservationModel.City = Request.Form["city"].ToString();
                reservationModel.Zip = "";
                reservationModel.State = "";
                reservationModel.Country = Request.Form["nationality"].ToString();
                reservationModel.Language = "";
                reservationModel.ArrivalDate = DateTime.Parse(Request.Form["arrival"].ToString());
                reservationModel.OriginalArrivalDate = DateTime.Parse(Request.Form["arrival"].ToString());
                reservationModel.NoOfNight = int.Parse(Request.Form["noOfNight"].ToString());
                reservationModel.DepartureDate = DateTime.Parse(Request.Form["departure"].ToString());
                reservationModel.OriginalDepartureDate = DateTime.Parse(Request.Form["departure"].ToString());
                reservationModel.NoOfAdult = int.Parse(Request.Form["noOfAdult"].ToString());
                reservationModel.NoOfChild = int.Parse(Request.Form["noOfChild"].ToString());
                reservationModel.NoOfChild1 = int.Parse(Request.Form["noOfChild1"].ToString());
                reservationModel.NoOfChild2 = int.Parse(Request.Form["noOfChild2"].ToString());
                reservationModel.NoOfRoom = int.Parse(Request.Form["noOfRoom"].ToString());
                if (roomType != null)
                {
                    reservationModel.RoomTypeId = roomType.ID;
                    reservationModel.RoomType = roomType.Code;
                }
                else
                {
                    reservationModel.RoomTypeId = 0;
                    reservationModel.RoomType = "";
                }
                reservationModel.RtcId = int.Parse(Request.Form["rtcID"].ToString());
                if (string.IsNullOrEmpty(Request.Form["roomNo"].ToString()))
                {
                    reservationModel.RoomId = 0;
                    reservationModel.RoomNo = "";
                }
                else
                {
                    reservationModel.RoomId = int.Parse(Request.Form["roomID"].ToString());
                    reservationModel.RoomNo = Request.Form["roomNo"].ToString();
                }

                reservationModel.BusinessBlockId = 0;
                reservationModel.BusinessBlockCode = "";
                reservationModel.Eta = !string.IsNullOrEmpty(Request.Form["eta"].ToString()) ? Request.Form["eta"].ToString() : "";
                reservationModel.CheckInDate = DateTime.Parse(Request.Form["arrival"].ToString());
                reservationModel.Etd = !string.IsNullOrEmpty(Request.Form["etd"].ToString()) ? Request.Form["etd"].ToString() : "";
                reservationModel.CheckOutDate = DateTime.Parse(Request.Form["arrival"].ToString());
                reservationModel.ReservationTypeId = int.Parse(Request.Form["reservationType"].ToString());
                reservationModel.ReservationTypeCode = Request.Form["reservationTypeCode"].ToString();
                if (string.IsNullOrEmpty(Request.Form["marketID"].ToString()))
                {
                    reservationModel.MarketId = 0;

                }
                else
                {
                    reservationModel.MarketId = int.Parse(Request.Form["marketID"].ToString());

                }
                reservationModel.MarketCode = Request.Form["marketCode"].ToString();
                reservationModel.SourceId = int.Parse(Request.Form["sourceID"].ToString());
                reservationModel.SourceCode = Request.Form["sourceCode"].ToString();
                reservationModel.OriginId = 0;
                reservationModel.OriginCode = "";
                reservationModel.CCHolder = "";
                if (string.IsNullOrEmpty(Request.Form["bookerID"].ToString()))
                {
                    reservationModel.BookerId = 0;
                    reservationModel.BookerName = "";
                }
                else
                {
                    reservationModel.BookerId = int.Parse(Request.Form["bookerID"].ToString());
                    reservationModel.BookerName = Request.Form["bookerName"].ToString();
                }

                reservationModel.BookerDetails = "";
                reservationModel.NoPost = false;
                reservationModel.PrintRate = true;
                reservationModel.ConfirmationStatus = true;
                reservationModel.VideoCheckOutStatus = false;
                reservationModel.CRSNo = "";
                reservationModel.DiscountAmount = decimal.Parse(Request.Form["discountAmount"].ToString());
                reservationModel.DiscountRate = decimal.Parse(Request.Form["discountRate"].ToString());
                reservationModel.DiscountReason = "";
                reservationModel.Comment = Request.Form["comment"].ToString();
                reservationModel.BalanceUSD = 0;
                reservationModel.BalanceVND = 0;
                reservationModel.ApprovalCode = "";
                reservationModel.ApprovalAmount = 0;
                reservationModel.SuiteWith = "";
                reservationModel.PaymentMethod = "";
                reservationModel.CreditCardNo = "";
                reservationModel.ExpirationDate = DateTime.Now;
                reservationModel.TaxTypeId = 0;
                reservationModel.ExemptNumber = "";
                reservationModel.PickupReqdId = int.Parse(Request.Form["pickedId"].ToString());
                reservationModel.PickupTransportType = Request.Form["pickUpTransportType"].ToString();
                reservationModel.PickupStationCode = Request.Form["pickUpStationCode"].ToString();
                reservationModel.PickupCarrierCode = Request.Form["pickUpCarrierCode"].ToString();
                reservationModel.PickupTime = Request.Form["pickUpTime"].ToString();
                reservationModel.PickupTransportNo = Request.Form["pickUpTransportNo"].ToString();
                reservationModel.PickupArrivalDate = DateTime.Parse(Request.Form["pickUpDate"].ToString());
                reservationModel.PickupDescription = Request.Form["pickUpDescription"].ToString();
                reservationModel.DropOffReqdId = int.Parse(Request.Form["dropOffId"].ToString());
                reservationModel.DropOffTransportType = Request.Form["dropOffTransportType"].ToString();
                reservationModel.DropOffStationCode = Request.Form["dropOffStationCode"].ToString();
                reservationModel.DropOffCarrierCode = Request.Form["dropOffCarrierCode"].ToString();
                reservationModel.DropOffTime = Request.Form["dropOffTime"].ToString();
                reservationModel.DropOffTransportNo = Request.Form["dropOffTransportNo"].ToString();
                reservationModel.DropOffDepartureDate = DateTime.Parse(Request.Form["dropOffDate"].ToString());
                reservationModel.DropOffDescription = Request.Form["dropOffDescription"].ToString();
                reservationModel.PackageId = int.Parse(Request.Form["packageID"].ToString());
                reservationModel.Packages = Request.Form["packages"].ToString();
                reservationModel.Relationship = ReservationBO.GetTopID() + 1;
                reservationModel.Status = 0;
                reservationModel.PostingMaster = false;
                reservationModel.MainGuest = true;
                if (string.IsNullOrEmpty(Request.Form["rateCode"].ToString()))
                {
                    reservationModel.RateCodeId = 0;
                    reservationModel.RateCode = "";
                }
                else
                {
                    reservationModel.RateCodeId = int.Parse(Request.Form["rateCodeID"].ToString());
                    reservationModel.RateCode = Request.Form["rateCode"].ToString();
                }
                reservationModel.Rate = decimal.Parse(Request.Form["rateAmount"].ToString());
                reservationModel.RateAfterTax = decimal.Parse(Request.Form["rateAfter"].ToString());
                reservationModel.FixedRate = false;
                reservationModel.TotalAmount = decimal.Parse(Request.Form["rateAmount"].ToString());
                reservationModel.CurrencyId = "VND";
                reservationModel.Party = "";
                reservationModel.PartyGuest = "";
                reservationModel.IsPasserBy = false;
                reservationModel.Color = Request.Form["color"].ToString();
                reservationModel.ARNo = "";
                reservationModel.ItemInventory = Request.Form["itemInventory"].ToString();
                reservationModel.Specials = Request.Form["specials"].ToString();
                reservationModel.ShareRoom = ReservationBO.GetTopID() + 1;
                reservationModel.NoShowStatus = false;
                reservationModel.ShareRoomName = "";
                reservationModel.AccompanyName = "";
                reservationModel.RoutingTransaction = "";
                reservationModel.RoutingToProfile = Request.Form["firstName"].ToString();
                reservationModel.FixedCharge = "";
                reservationModel.CommentGroup = "";
                reservationModel.IsWalkIn = false;
                reservationModel.UserUpdateId = int.Parse(Request.Form["userID"].ToString());
                reservationModel.UpdateDate = DateTime.Now;
                reservationModel.UpdateBy = Request.Form["userName"].ToString();
                reservationModel.SpecialUpdateBy = Request.Form["userName"].ToString();
                reservationModel.SpecialUpdateDate = DateTime.Now;
                reservationModel.IsAdvanceBill = false;
                if (string.IsNullOrEmpty(Request.Form["allotmentID"].ToString()))
                {
                    reservationModel.AllotmentId = 0;
                    reservationModel.AllotmentCode = "";
                }
                else
                {
                    reservationModel.AllotmentId = int.Parse(Request.Form["allotmentID"].ToString());
                    reservationModel.AllotmentCode = Request.Form["allotmentCode"].ToString();
                }
                reservationModel.PinCode = (ReservationBO.GetTopID() + 1).ToString();
                reservationModel.PersonInChargeId = int.Parse(Request.Form["perrsonInCharge"].ToString());
                reservationModel.RoomNight = int.Parse(Request.Form["roomNight"].ToString());
                reservationModel.CardId = "";
                reservationModel.Breakfast = false;
                reservationModel.Dinner = false;
                reservationModel.Lunch = false;
                reservationModel.FixedMeal = false;
                reservationModel.VoucherId = "";
                ReservationBO.Instance.Update(reservationModel);
                #endregion

                #region lưu reservation item inventory
                string itemInventoryString = Request.Form["itemInventory"].ToString();
                List<int> itemInventory = itemInventoryString.Split(',')
                                                    .Select(x => int.Parse(x)).Where(x => x != 0)
                                                    .ToList();
                //if (itemInventory.Count > 0)
                //{
                //    foreach (var item in itemInventory)
                //    {
                //        ItemModel itemModel = (ItemModel)ItemBO.Instance.FindByPrimaryKey(item);
                //        if (itemModel != null && itemModel.ID != 0)
                //        {
                //            ReservationItemInventoryModel reservationItemInventory = new ReservationItemInventoryModel();
                //            reservationItemInventory.ReservationID = (int)reservationID;
                //            reservationItemInventory.ItemID = itemModel.ID;
                //            reservationItemInventory.Code = itemModel.Code;
                //            reservationItemInventory.Name = itemModel.Name;
                //            reservationItemInventory.BeginDate = reservationModel.ArrivalDate;
                //            reservationItemInventory.EndDate = reservationModel.DepartureDate;
                //            reservationItemInventory.Quantity = 1;
                //            reservationItemInventory.RateCode = "";
                //            reservationItemInventory.PackageID = reservationModel.PackageId;
                //            reservationItemInventory.Package = reservationModel.Packages;
                //            reservationItemInventory.ReservationFixedChargeID = 0;
                //            reservationItemInventory.UserInsertID = reservationItemInventory.UserUpdateID = int.Parse(Request.Form["userID"].ToString());
                //            reservationItemInventory.CreateDate = reservationItemInventory.UpdateDate = DateTime.Now;
                //            ReservationItemInventoryBO.Instance.Insert(reservationItemInventory);
                //        }

                //    }
                //}
                #endregion
                pt.CommitTransaction();
                return Json(new { code = 0, msg = "Update reservation created successfully" });

            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { code = 1, msg = ex.Message });
            }
            finally
            {
                pt.CloseConnection();

            }
        }

        [HttpPost]
        public ActionResult EditReservationRoomPlan(int ID)
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                List<BusinessDateModel> businessDate = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
                int memberTypeID = 0; int roomTypeID = 0; int vipID = 0;
                if (string.IsNullOrEmpty(Request.Form["memberType"].ToString()))
                {
                    memberTypeID = 0;
                }
                if (string.IsNullOrEmpty(Request.Form["vipID"].ToString()))
                {
                    vipID = 0;
                }
                if (!string.IsNullOrEmpty(Request.Form["roomTypeID"].ToString()))
                {
                    roomTypeID = int.Parse(Request.Form["roomTypeID"].ToString());
                }
                if (string.IsNullOrEmpty(Request.Form["lastName"].ToString()))
                {
                    return Json(new { code = 1, msg = "Profile cannot be blank" });
                }
                if (string.IsNullOrEmpty(Request.Form["reservationTypeCode"].ToString()))
                {
                    return Json(new { code = 1, msg = "Reservation Type cannot be blank" });
                }
                if (decimal.Parse(Request.Form["rateAmount"].ToString()) == 0)
                {
                    return Json(new { code = 1, msg = "Rate cannot be blank " });

                }
                MemberTypeModel memberType = (MemberTypeModel)MemberTypeBO.Instance.FindByPrimaryKey(memberTypeID);
                VIPModel vip = (VIPModel)VIPBO.Instance.FindByPrimaryKey(vipID);
                RoomTypeModel roomType = (RoomTypeModel)RoomTypeBO.Instance.FindByPrimaryKey(roomTypeID);
                ReservationModel reservationModel = (ReservationModel)ReservationBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["rsvID"].ToString()));
                #region edit reservation
              
                reservationModel.ProfileComment = "";
                reservationModel.ProfileIndividualId = int.Parse(Request.Form["profileIndividualID"].ToString());
                reservationModel.LastName = Request.Form["lastName"].ToString();
                reservationModel.FirstName = Request.Form["firstName"].ToString();
                reservationModel.Phone = Request.Form["phone"].ToString();
                reservationModel.Email = Request.Form["email"].ToString();
              
              
                reservationModel.ArrivalDate = DateTime.Parse(Request.Form["arrival"].ToString());
                reservationModel.OriginalArrivalDate = DateTime.Parse(Request.Form["arrival"].ToString());
                reservationModel.NoOfNight = int.Parse(Request.Form["noOfNight"].ToString());
                reservationModel.DepartureDate = DateTime.Parse(Request.Form["departure"].ToString());
                reservationModel.OriginalDepartureDate = DateTime.Parse(Request.Form["departure"].ToString());
                reservationModel.NoOfAdult = int.Parse(Request.Form["noOfAdult"].ToString());
                reservationModel.NoOfChild = int.Parse(Request.Form["noOfChild"].ToString());
                reservationModel.NoOfChild1 = int.Parse(Request.Form["noOfChild1"].ToString());
                reservationModel.NoOfChild2 = int.Parse(Request.Form["noOfChild2"].ToString());
                reservationModel.NoOfRoom = int.Parse(Request.Form["noOfRoom"].ToString());
                if (roomType != null)
                {
                    reservationModel.RoomTypeId = roomType.ID;
                    reservationModel.RoomType = roomType.Code;
                }
                else
                {
                    reservationModel.RoomTypeId = 0;
                    reservationModel.RoomType = "";
                }
                reservationModel.RtcId = int.Parse(Request.Form["rtcID"].ToString());
                if (string.IsNullOrEmpty(Request.Form["roomNo"].ToString()))
                {
                    reservationModel.RoomId = 0;
                    reservationModel.RoomNo = "";
                }
                else
                {
                    reservationModel.RoomId = int.Parse(Request.Form["roomID"].ToString());
                    reservationModel.RoomNo = Request.Form["roomNo"].ToString();
                }

                reservationModel.Eta = !string.IsNullOrEmpty(Request.Form["eta"].ToString()) ? Request.Form["eta"].ToString() : "";
                reservationModel.CheckInDate = DateTime.Parse(Request.Form["arrival"].ToString());
                reservationModel.Etd = !string.IsNullOrEmpty(Request.Form["etd"].ToString()) ? Request.Form["etd"].ToString() : "";
                reservationModel.CheckOutDate = DateTime.Parse(Request.Form["arrival"].ToString());
          
                if (string.IsNullOrEmpty(Request.Form["marketID"].ToString()))
                {
                    reservationModel.MarketId = 0;

                }
                else
                {
                    reservationModel.MarketId = int.Parse(Request.Form["marketID"].ToString());

                }
          
                if (string.IsNullOrEmpty(Request.Form["rateCode"].ToString()))
                {
                    reservationModel.RateCodeId = 0;
                    reservationModel.RateCode = "";
                }
                else
                {
                    reservationModel.RateCodeId = int.Parse(Request.Form["rateCodeID"].ToString());
                    reservationModel.RateCode = Request.Form["rateCode"].ToString();
                }
                reservationModel.Rate = decimal.Parse(Request.Form["rateAmount"].ToString());
                reservationModel.RateAfterTax = decimal.Parse(Request.Form["rateAfter"].ToString());
                reservationModel.FixedRate = false;
                reservationModel.TotalAmount = decimal.Parse(Request.Form["rateAmount"].ToString());
                reservationModel.CurrencyId = "VND";
                reservationModel.Party = "";
                reservationModel.DepositRequest = "";
                reservationModel.PartyGuest = "";
                reservationModel.IsPasserBy = false;
                //reservationModel.Color = Request.Form["color"].ToString();
                reservationModel.ARNo = "";
                reservationModel.ShareRoom = ReservationBO.GetTopID() + 1;
                reservationModel.NoShowStatus = false;
                reservationModel.ShareRoomName = "";
                reservationModel.AccompanyName = "";
                reservationModel.RoutingTransaction = "";
                reservationModel.RoutingToProfile = Request.Form["firstName"].ToString();
                reservationModel.FixedCharge = "";
                reservationModel.CommentGroup = "";
                reservationModel.IsWalkIn = false;
                reservationModel.UserUpdateId = int.Parse(Request.Form["userID"].ToString());
                reservationModel.UpdateDate = DateTime.Now;
                reservationModel.UpdateBy = Request.Form["userName"].ToString();
                reservationModel.SpecialUpdateBy = Request.Form["userName"].ToString();
                reservationModel.SpecialUpdateDate = DateTime.Now;
                reservationModel.IsAdvanceBill = false;
                if (string.IsNullOrEmpty(Request.Form["allotmentID"].ToString()))
                {
                    reservationModel.AllotmentId = 0;
                    reservationModel.AllotmentCode = "";
                }
                else
                {
                    reservationModel.AllotmentId = int.Parse(Request.Form["allotmentID"].ToString());
                    reservationModel.AllotmentCode = Request.Form["allotmentCode"].ToString();
                }
                reservationModel.PinCode = (ReservationBO.GetTopID() + 1).ToString();
                reservationModel.PersonInChargeId = int.Parse(Request.Form["perrsonInCharge"].ToString());
                reservationModel.RoomNight = int.Parse(Request.Form["roomNight"].ToString());
                reservationModel.CardId = "";
                reservationModel.Breakfast = false;
                reservationModel.Dinner = false;
                reservationModel.Lunch = false;
                reservationModel.FixedMeal = false;
                reservationModel.VoucherId = "";
                ReservationBO.Instance.Update(reservationModel);
                #endregion

                #region lưu reservation item inventory
                string itemInventoryString = Request.Form["itemInventory"].ToString();
                List<int> itemInventory = itemInventoryString.Split(',')
                                                    .Select(x => int.Parse(x)).Where(x => x != 0)
                                                    .ToList();
                //if (itemInventory.Count > 0)
                //{
                //    foreach (var item in itemInventory)
                //    {
                //        ItemModel itemModel = (ItemModel)ItemBO.Instance.FindByPrimaryKey(item);
                //        if (itemModel != null && itemModel.ID != 0)
                //        {
                //            ReservationItemInventoryModel reservationItemInventory = new ReservationItemInventoryModel();
                //            reservationItemInventory.ReservationID = (int)reservationID;
                //            reservationItemInventory.ItemID = itemModel.ID;
                //            reservationItemInventory.Code = itemModel.Code;
                //            reservationItemInventory.Name = itemModel.Name;
                //            reservationItemInventory.BeginDate = reservationModel.ArrivalDate;
                //            reservationItemInventory.EndDate = reservationModel.DepartureDate;
                //            reservationItemInventory.Quantity = 1;
                //            reservationItemInventory.RateCode = "";
                //            reservationItemInventory.PackageID = reservationModel.PackageId;
                //            reservationItemInventory.Package = reservationModel.Packages;
                //            reservationItemInventory.ReservationFixedChargeID = 0;
                //            reservationItemInventory.UserInsertID = reservationItemInventory.UserUpdateID = int.Parse(Request.Form["userID"].ToString());
                //            reservationItemInventory.CreateDate = reservationItemInventory.UpdateDate = DateTime.Now;
                //            ReservationItemInventoryBO.Instance.Insert(reservationItemInventory);
                //        }

                //    }
                //}
                #endregion
                pt.CommitTransaction();
                return Json(new { code = 0, msg = "Update reservation created successfully" });

            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { code = 1, msg = ex.Message });
            }
            finally
            {
                pt.CloseConnection();

            }
        }
        #endregion

        #region cancel reservation
        [HttpPost]
        public ActionResult CancelReservation()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                //validate form cancel
                if (string.IsNullOrEmpty(Request.Form["reasonCancellation"].ToString()))
                {
                    return Json(new { code = 1, msg = "Please choose reason cancel" });

                }
                ReservationModel rsv = (ReservationModel)ReservationBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["rsvID"].ToString()));
                if (rsv == null) {
                    return Json(new { code = 1, msg = "Can not find reservation" });
                }
                #region update status reservation
                rsv.Status = 3;
                rsv.UpdateDate = rsv.SpecialUpdateDate = DateTime.Now;
                rsv.UserUpdateId = int.Parse(Request.Form["userID"].ToString());
                rsv.UpdateBy = rsv.SpecialUpdateBy = Request.Form["userName"].ToString();
                ReservationBO.Instance.Update(rsv);
                #endregion

                string cancellationNo = ReservationCancellationBO.GetTopCancellatioNo();
                #region insert ReservationCancellation 
                ReservationCancellationModel reservationCancellation = new ReservationCancellationModel();
                reservationCancellation.ReservationID = rsv.ID;
                reservationCancellation.CancellationDate = DateTime.Now;
                reservationCancellation.CancellationNo = !string.IsNullOrEmpty(cancellationNo) ? cancellationNo : "0";
                reservationCancellation.ReasonCancellation = Request.Form["reasonCancellation"].ToString();
                reservationCancellation.Description = Request.Form["description"].ToString();
                reservationCancellation.CreateDate = reservationCancellation.UpdateDate = DateTime.Now;
                reservationCancellation.UserInsertID = reservationCancellation.UserUpdateID = int.Parse(Request.Form["userID"].ToString());
                ReservationCancellationBO.Instance.Insert(reservationCancellation);
                #endregion
                pt.CommitTransaction();
                return Json(new { code = 0, msg = "Cancel reservation successfully" });

            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { code = 1, msg = ex.Message });
            }
            finally
            {
                pt.CloseConnection();

            }
        }
        #endregion


        #region registration card
        [HttpGet]
        public async Task<IActionResult> GetRegistrationCard()
        {
            try
            {
                List<RegistrationCardModel> result = RegistrationCardBO.GetRegistrationCard();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        #endregion





        #region Reservation Accompanying
        [HttpGet]
        public async Task<IActionResult> GetReservationAccompanyingByReservationID(int reservationID)
        {
            try
            {

                var result = ReservationAccompanyBO.GetReservationAccompanyByReservationID(reservationID);

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult AttachAccompanying()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                if (string.IsNullOrEmpty(Request.Form["profileAgentID"].ToString()))
                {
                    return Json(new { code = 1, msg = "Could not find profile attached" });
                }
                if (string.IsNullOrEmpty(Request.Form["rsvID"].ToString()))
                {
                    return Json(new { code = 1, msg = "Please choose booking" });
                }
                var checkReservationAccompany = ReservationAccompanyBO.GetReservationAccompany(int.Parse(Request.Form["rsvID"].ToString()), int.Parse(Request.Form["profileAgentID"].ToString()));
                if(checkReservationAccompany.Count > 0)
                {
                    return Json(new { code = 1, msg = "This profile has been attached, please choose another profile" });

                }
                ReservationAccompanyModel model = new ReservationAccompanyModel();
                model.ReservationID = int.Parse(Request.Form["rsvID"].ToString());
                model.ProfileIndividualID = int.Parse(Request.Form["profileAgentID"].ToString());
                model.UserInsertID = model.UserUpdateID = int.Parse(Request.Form["userID"].ToString());
                model.UpdateDate = model.CreateDate = DateTime.Now;
                ReservationAccompanyBO.Instance.Insert(model);
                pt.CommitTransaction();
                return Json(new { code = 0, msg = "Profile was attacheđ successfully" });

            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { code = 1, msg = ex.Message });
            }
            finally
            {
                pt.CloseConnection();

            }
        }

        [HttpPost]
        public ActionResult DettachAccompanying(int id)
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                if (id == 0)
                {
                    return Json(new { code = 1, msg = "Please choose profile dettach" });
                }


                ReservationAccompanyBO.Instance.Delete(id);
                pt.CommitTransaction();
                return Json(new { code = 0, msg = "Profile was dettached successfully" });

            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { code = 1, msg = ex.Message });
            }
            finally
            {
                pt.CloseConnection();

            }
        }
        #endregion


        #region check in reservation
        [HttpPost]
        public ActionResult CheckInBooking()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                ReservationModel reservation = (ReservationModel)ReservationBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["rsvID"].ToString()));
                reservation.Status = 1;
                reservation.UpdateBy = Request.Form["userName"].ToString();
                reservation.UserUpdateId = int.Parse(Request.Form["userID"].ToString());
                reservation.SpecialUpdateBy = Request.Form["userName"].ToString();
                reservation.SpecialUpdateDate = reservation.UpdateDate =  DateTime.Now;
                ReservationBO.Instance.Update(reservation);
                pt.CommitTransaction();
                return Json(new { code = 0, msg = "Check in was successfully" });

            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { code = 1, msg = ex.Message });
            }
            finally
            {
                pt.CloseConnection();

            }
        }

        [HttpPost]
        public ActionResult PrintRegistrationCard(int id)
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                string url = "";
                ReservationModel reservation = (ReservationModel)ReservationBO.Instance.FindByPrimaryKey(id);
                XtraReport report = new WebApp.Templates.RegistrationCard.V_RegistrationCard();
                report.Parameters["ConfirmationNo"].Value = reservation.ConfirmationNo;
                report.Parameters["ArrivalDate"].Value = reservation.ArrivalDate.ToString();
                report.Parameters["DepartureDate"].Value = reservation.DepartureDate.ToString();
                report.CreateDocument();

                using (MemoryStream msPdf = new MemoryStream())
                {
                    report.ExportToPdf(msPdf);
                    string base64Pdf = Convert.ToBase64String(msPdf.ToArray());
                    url = $"data:application/pdf;base64,{base64Pdf}";

                }
                pt.CommitTransaction();
                return Json(url);
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return Json(new { code = 1, msg = ex.Message });
            }
            finally
            {
                pt.CloseConnection();

            }
        }
        #endregion


        #region reservation billing
        [HttpGet]
        public async Task<IActionResult> GetFolioDetailByFolioID(int reservationID, int mode)
        {
            try
            {
                int folioID = FolioBO.GetFolioIDByReservationID(reservationID);
                DataTable myData = _iFolioDetailService.GetFolioDetailByFolioID(folioID, mode);

                var result = (from d in myData.AsEnumerable()

                              select new
                              {
                                  Select = d["Select"].ToString(),
                                  CodePrefix = d["CodePrefix"].ToString(),
                                  ID = d["ID"].ToString(),
                                  FolioID = d["FolioID"].ToString(),
                                  GroupType = d["GroupType"].ToString(),
                                  GroupCode = d["GroupCode"].ToString(),
                                  SubgroupCode = d["SubgroupCode"].ToString(),
                                  PostType = d["PostType"].ToString(),
                                  RowState = d["RowState"].ToString(),
                                  IsSplit = d["IsSplit"].ToString(),
                                  InvoiceNo = d["InvoiceNo"].ToString(),
                                  TransactionNo = d["TransactionNo"].ToString(),
                                  Date = d["Date"].ToString(),
                                  Code = d["Code"].ToString(),
                                  Description = d["Description"].ToString(),
                                  Amount = d["Amount"].ToString(),
                                  Currency = d["Currency"].ToString(),
                                  Supplement = d["Supplement"].ToString(),
                                  Reference = d["Reference"].ToString(),
                                  UserName = d["UserName"].ToString(),
                                  ShiftID = d["ShiftID"].ToString(),
                                  ProfitCenterID = d["ProfitCenterID"].ToString(),
                                  ProfitCenterCode = d["ProfitCenterCode"].ToString(),
                                  RoomTypeID = d["RoomTypeID"].ToString(),
                                  RoomType = d["RoomType"].ToString(),
                                  Property = d["Property"].ToString(),
                                  CheckNo = d["CheckNo"].ToString(),


                              }).ToList();


                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        #endregion


        #region Reservation fixed charges
        [HttpGet]
        public async Task<IActionResult> GetFixedChargesByReservationID(int reservationID)
        {
            try
            {

                var result = ReservationFixedChargeBO.Instance.FindByAttribute("ReservationID", reservationID);
                ReservationModel reservation = (ReservationModel)ReservationBO.Instance.FindByPrimaryKey(reservationID);
                return Json(new {
                    reservationFixedCharge = result,
                    reservation = reservation
                });

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        #endregion
    }
}

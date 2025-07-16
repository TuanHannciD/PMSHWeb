using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
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

        public ReservationController(ILogger<ReservationController> logger,
             IMemoryCache cache, IConfiguration configuration, IReservationService iReservationService)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _iReservationService = iReservationService;
        }
        public IActionResult SearchReservation()
        {
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

        #region lưu reservation
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SaveReservation()
        {
            //ProcessTransactions pt = new ProcessTransactions();
            try
            {
                //pt.OpenConnection();
                //pt.BeginTransaction();
                List<BusinessDateModel> businessDate = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
                int memberTypeID = 0;int roomTypeID = 0; int vipID = 0;
                if (string.IsNullOrEmpty(Request.Form["memberType"].ToString()))
                {
                    memberTypeID = 0;
                }
                if (string.IsNullOrEmpty(Request.Form["vipID"].ToString()))
                {
                    vipID = 0;
                }
                if (string.IsNullOrEmpty(Request.Form["roomTypeID"].ToString()))
                {
                    roomTypeID = 0;
                }
                MemberTypeModel memberType = (MemberTypeModel)MemberTypeBO.Instance.FindByPrimaryKey(memberTypeID);
                VIPModel vip = (VIPModel)VIPBO.Instance.FindByPrimaryKey(vipID);
                RoomTypeModel roomType = (RoomTypeModel)RoomTypeBO.Instance.FindByPrimaryKey(roomTypeID);

                ReservationModel reservationModel = new ReservationModel();
                reservationModel.ConfirmationNo = "";
                reservationModel.ReservationNo = "";
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
                if(memberType != null)
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
                if(roomType != null)
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
                reservationModel.RoomId = int.Parse(Request.Form["roomID"].ToString());
                reservationModel.RoomNo = Request.Form["roomNo"].ToString();
                reservationModel.BusinessBlockId = 0;
                reservationModel.BusinessBlockCode = "";
                reservationModel.Eta = Request.Form["eta"].ToString();
                reservationModel.CheckInDate = DateTime.Parse(Request.Form["arrival"].ToString());
                reservationModel.Etd = Request.Form["etd"].ToString();
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
                reservationModel.BookerId = int.Parse(Request.Form["bookerID"].ToString());
                reservationModel.BookerName = Request.Form["bookerName"].ToString();
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
                reservationModel.Relationship = 0;
                reservationModel.Status = 0;
                reservationModel.PostingMaster = false;
                reservationModel.MainGuest = true;
                reservationModel.RateCodeId = int.Parse(Request.Form["rateCodeID"].ToString());
                reservationModel.RateCode = Request.Form["rateCode"].ToString();
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
                reservationModel.ShareRoom = 0;
                reservationModel.NoShowStatus = false;
                reservationModel.ShareRoomName = "";
                reservationModel.AccompanyName = "";
                reservationModel.RoutingTransaction = "";
                reservationModel.RoutingToProfile = Request.Form["firstName"].ToString();
                reservationModel.FixedCharge = "";
                reservationModel.CommentGroup = "";
                reservationModel.IsWalkIn = false;
                reservationModel.UserInsertId = 3;
                reservationModel.CreateDate = DateTime.Now;
                reservationModel.UserUpdateId = 3;
                reservationModel.UpdateDate = DateTime.Now;
                reservationModel.CreateBy = "admin1";
                reservationModel.UpdateBy = "admin2";
                reservationModel.SpecialUpdateBy = "manager1";
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
                reservationModel.PinCode = "";
                reservationModel.PersonInChargeId = int.Parse(Request.Form["perrsonInCharge"].ToString()); 
                reservationModel.RoomNight = int.Parse(Request.Form["roomNight"].ToString());
                reservationModel.CardId = "";
                reservationModel.Breakfast = false;
                reservationModel.Dinner = false;
                reservationModel.Lunch = false;
                reservationModel.FixedMeal = false;
                reservationModel.VoucherId = "";
                ReservationBO.Instance.Insert(reservationModel);
                //pt.CommitTransaction();
                return Json(new { code = 0, msg = "New reservation created successfully" });

            }
            catch (Exception ex)
            {
                //pt.RollBack();
                return Json(new { code = 1, msg = ex.Message });
            }
            //finally
            //{
            //    pt.CloseConnection();

            //}
        }
        #endregion
    }
}

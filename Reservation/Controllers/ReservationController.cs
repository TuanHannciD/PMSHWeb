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
    }
}

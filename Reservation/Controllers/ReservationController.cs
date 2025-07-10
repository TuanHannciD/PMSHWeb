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
using System.Text;
using System.Threading.Tasks;

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
    }
}

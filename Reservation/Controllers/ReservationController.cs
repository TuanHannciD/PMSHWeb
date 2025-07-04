using Microsoft.AspNetCore.Authorization;
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

            return View();
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
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FrontDesk.Services.Interfaces;
using FrontDesk.Services.Implements;
namespace FrontDesk.Controllers
{
    public class FrontDeskController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FrontDeskController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IFrontDeskService _iFrontDeskService;

        public FrontDeskController(ILogger<FrontDeskController> logger,
                IMemoryCache cache, IConfiguration configuration, IFrontDeskService iFrontDeskService)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _iFrontDeskService = iFrontDeskService;

        }
        public IActionResult TelephoneBook()
        {
            List<TelephoneBookCategoryModel> tlplist = PropertyUtils.ConvertToList<TelephoneBookCategoryModel>(TelephoneBookCategoryBO.Instance.FindAll());
            var sortedList = tlplist.OrderBy(x => x.Name).ToList();
            ViewBag.TelephoneBookCategoryList = sortedList;
            return View();
        }

        [HttpGet]
        public IActionResult GetTelephoneBook(string categoryId, string phoneSearch)
        {
            try
            {
                DataTable dt = _iFrontDeskService.GetTelephoneBookByCategory(categoryId, phoneSearch);

                var result = (from DataRow row in dt.Rows
                              select new
                              {
                                  Name = row["Name"]?.ToString(),
                                  Telephone = row["Telephone"]?.ToString(),
                                  Address = row["Address"]?.ToString(),
                                  Remark = row["Remark"]?.ToString()
                              }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

    }
}

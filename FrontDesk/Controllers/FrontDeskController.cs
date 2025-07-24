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
            ViewBag.TelephoneBookCategoryList = tlplist;
            return View();
        }
        [HttpGet]
        public IActionResult TelephoneBook(string categoryId, string categoryCode, string telephoneCode)
        {
            
            try
            {
                DataTable dataTable = _iFrontDeskService.TelephoneBook(categoryId, categoryCode, telephoneCode);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ID = !string.IsNullOrEmpty(d["ID"].ToString()) ? d["ID"].ToString() : "",
                                  Name = !string.IsNullOrEmpty(d["Name"].ToString()) ? d["Name"].ToString() : "",                           
                              }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

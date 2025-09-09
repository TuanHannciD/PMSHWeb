using Cashiering.Commons.Helpers;
using Cashiering.Services.Implements;
using Cashiering.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashiering.Controllers
{
    public class AccountingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountingController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IAccountingService _iAccountingService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountingController(ILogger<AccountingController> logger, 
                IMemoryCache cache, IConfiguration configuration, IAccountingService iAccountingService)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _iAccountingService = iAccountingService;
        }

        #region DatVp __ Accounting: Search
        public IActionResult AccountingSearch()
        {
            ViewBag.cboAccountType = ListItemHelper.GetARAccountType();
            return View(); // View này sẽ chứa DataGrid + script gọi API
        }


        [HttpGet]
        public IActionResult SearchAccounting(string accountName, string accountNo, int accountType, string balance)
        {
            try
            {

                DataTable resultExchangeData = _iAccountingService.AccountSearch(accountName, accountNo, accountType, balance);
                var resultExchange = (from d in resultExchangeData.AsEnumerable()
                                      select d.Table.Columns.Cast<DataColumn>()
                                          //.Where(col => col.ColumnName != "AllotmentStageID" && col.ColumnName != "flag" && col.ColumnName != "Total")
                                          .ToDictionary(
                                              col => col.ColumnName,
                                              col => d[col.ColumnName]?.ToString()
                                          )).ToList();
                return Json(resultExchange);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        #endregion
    }
}

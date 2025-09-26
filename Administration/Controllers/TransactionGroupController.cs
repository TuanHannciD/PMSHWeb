using Administration.Services.Interfaces;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
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

namespace Administration.Controllers
{
    public class TransactionGroupController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TransactionGroupController> _logger;
        private readonly IMemoryCache _cache;
        private readonly ITransactionGroupService _iTransactionGroupService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TransactionGroupController(ILogger<TransactionGroupController> logger,
                IMemoryCache cache, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITransactionGroupService iTransactionGroupService)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _iTransactionGroupService = iTransactionGroupService;
        }

        public IActionResult TransGroup()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SearchTransactionGroup(string groupCode,string description)
        {
            try
            {
                var data = _iTransactionGroupService.SearchTransactionGroup(groupCode ?? "", description?? "");

                var result = (from d in data.AsEnumerable()
                              select d.Table.Columns.Cast<DataColumn>()
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

        [HttpPost]
        public ActionResult SaveTransactionGroup()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();

                if (string.IsNullOrEmpty(Request.Form["code"].ToString()))
                {
                    return Json(new { code = 0, msg = "Group Code can not be blank" });
                }
                TransactionGroupModel transGroup = new TransactionGroupModel();
                transGroup.Code = Request.Form["code"].ToString();
                transGroup.Description = Request.Form["description"].ToString();
                transGroup.Type = int.Parse(Request.Form["description"].ToString());
                pt.CommitTransaction();
                return Json(new { code = 0, msg = "Group transaction was created successfully" });

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
    }
}

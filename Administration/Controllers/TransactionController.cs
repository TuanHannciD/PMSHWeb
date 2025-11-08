using Administration.Services.Implements;
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
    public class TransactionController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly ILogger<TransactionController> _logger;
        private readonly IMemoryCache _cache;
        private readonly ITransactionService _iTransactionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TransactionController(ILogger<TransactionController> logger,
                IMemoryCache cache, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITransactionService iTransactionService)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _iTransactionService = iTransactionService;
        }
        public IActionResult Search()
        {
            return View();
        }
        public IActionResult Transaction()
        {
            List<TransactionsModel> listTransac = PropertyUtils.ConvertToList<TransactionsModel>(TransactionsBO.Instance.FindAll());
            ViewBag.TransactionsList = listTransac;

            List<TransactionGroupModel> listTransacgroup  = PropertyUtils.ConvertToList<TransactionGroupModel>(TransactionGroupBO.Instance.FindAll());
            ViewBag.TransactionGroupList = listTransacgroup;

            List<TransactionSubGroupModel> listTransacsubgroup = PropertyUtils.ConvertToList<TransactionSubGroupModel>(TransactionSubGroupBO.Instance.FindAll());
            ViewBag.TransactionSubGroupList = listTransacsubgroup;

            List<CurrencyModel> listCurr = PropertyUtils.ConvertToList<CurrencyModel>(CurrencyBO.Instance.FindAll());
            ViewBag.CurrencyList = listCurr;

            List<VatTypeModel> listvattype = PropertyUtils.ConvertToList<VatTypeModel>(VatTypeBO.Instance.FindAll());
            ViewBag.VatTypeList = listvattype;

            List<TransactionTypeModel> listTransactionType = PropertyUtils.ConvertToList<TransactionTypeModel>(TransactionTypeBO.Instance.FindAll());
            ViewBag.TransactionTypeList = listTransactionType;

            List<ARAccountReceivableModel> listARAccount = PropertyUtils.ConvertToList<ARAccountReceivableModel>(ARAccountReceivableBO.Instance.FindAll());
            ViewBag.listARAccountList = listARAccount;
            return View();
        }
        [HttpGet]
        public IActionResult SearchTransaction(string code, string description,int groupID, int subGroupID)
        {
            try
            {
                var data = _iTransactionService.SearchTransaction(code ?? "", description ?? "",groupID, subGroupID);

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
    }
}

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

        #region Transaction 
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

        [HttpPost]
        public IActionResult TransactionListSave(TransactionsModel model)
        {
            ProcessTransactions pt = new ProcessTransactions();
            pt.OpenConnection();
            pt.BeginTransaction();

            model.CreatedBy = model.CreatedBy?.Replace("\"", "").Trim();
            List<BusinessDateModel> businessDateModel = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
            var businessDate = businessDateModel[0].BusinessDate;
            try
            {
                bool isNew = (model.ID == 0);


                if (isNew)
                {
                    model.CreateDate = businessDate;
                    model.UpdateDate = businessDate;

                    TransactionsBO.Instance.Insert(model);
                }
                else
                {
                    model = (TransactionsModel)TransactionsBO.Instance.FindByPrimaryKey(model.ID);
                    if (model == null)
                    {
                        throw new Exception($"Không tìm thấy Article có ID = {model.ID}");
                    }

                    //model.Code = codenew;
                    //model.Description = descriptionnew;

                    //model.DefaultPrice = dfprice;
                    //model.CurrencyID = currList;

                    //model.TransactionCode = transactionsListnew;
                    //model.Supplement = supplementNew;
                    //model.UpdateDate = businessDate;
                    //model.UserUpdateID = userID;

                    //ArticleBO.Instance.Update(model);
                    //#region Update thông tin bến bảng RestaurantClassArticleLnk
                    //model.Description = model.Description.Replace("'", "`");
                    //pt.UpdateCommand("Update RestaurantClassArticleLnk set ArticleDescription=N'" + model.Description + "' where ArticleCode= N'" + model.Code + "' ");
                    #endregion
                }

                pt.CommitTransaction();

                return Json(new
                {
                    success = true,
                    message = isNew ? "Insert success!" : "Update success!"
                });
            }
            catch (Exception ex)
            {
                pt.RollBack();
                return BadRequest(new { success = false, message = ex.Message });
            }
            finally
            {
                pt.CloseConnection();
            }
            //return BadRequest(new { success = false });
        }

    }

}

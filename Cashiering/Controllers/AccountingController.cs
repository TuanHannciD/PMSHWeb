using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
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
            ViewBag.cboCountry = ListItemHelper.GetCountry();
            ViewBag.cboCity = ListItemHelper.GetCity();

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

        #region DatVP __ Accounting: Add
        [HttpPost]
        public ActionResult AccountReceivableAdd()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                ProfileModel profile = (ProfileModel)ProfileBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["profile"].ToString()));
                if (profile == null || profile.ID == 0)
                {
                    return Json(new { code = 1, msg = "Could not find profile" });

                }
                if (int.Parse(Request.Form["id"].ToString()) == 0)
                {
                    ARAccountReceivableModel model = new ARAccountReceivableModel();
                    model.AccountNo = Request.Form["accountNumber"].ToString();
                    model.AccountTypeID = int.Parse(Request.Form["accountType"].ToString());
                    model.CreditLimit = string.IsNullOrEmpty(Request.Form["creditLimit"].ToString()) ? 0 : int.Parse(Request.Form["creditLimit"].ToString());
                    model.CurrencyID = "VND";
                    model.ProfileID = profile.ID;
                    model.AccountName = profile.Account;
                    model.ContactName = string.IsNullOrEmpty(Request.Form["contact"].ToString()) ? "" : Request.Form["contact"].ToString();
                    model.TelePhone = string.IsNullOrEmpty(Request.Form["phone"].ToString()) ? "" : Request.Form["phone"].ToString();
                    model.Fax = string.IsNullOrEmpty(Request.Form["fax"].ToString()) ? "" : Request.Form["fax"].ToString();
                    model.Email = string.IsNullOrEmpty(Request.Form["email"].ToString()) ? "" : Request.Form["email"].ToString();
                    model.Address1 = string.IsNullOrEmpty(Request.Form["address1"].ToString()) ? "" : Request.Form["address1"].ToString();
                    model.Address2 = string.IsNullOrEmpty(Request.Form["address2"].ToString()) ? "" : Request.Form["address2"].ToString();
                    model.Address3 = string.IsNullOrEmpty(Request.Form["address3"].ToString()) ? "" : Request.Form["address3"].ToString();
                    model.CityID = int.Parse(Request.Form["city"].ToString());
                    model.PostalCode = string.IsNullOrEmpty(Request.Form["postalCode"].ToString()) ? "" : Request.Form["postalCode"].ToString();
                    model.CountryID = int.Parse(Request.Form["country"].ToString());
                    model.State = "";
                    model.Description = string.IsNullOrEmpty(Request.Form["description"].ToString()) ? "" : Request.Form["description"].ToString();
                    model.StatusFlagged = Request.Form["flagged"].ToString() == "1" ? true : false;
                    model.StatusInactive = Request.Form["inactive"].ToString() == "1" ? true : false;
                    model.PaymentDueDays = string.IsNullOrEmpty(Request.Form["paymentDue"].ToString()) ? 0 : int.Parse(Request.Form["paymentDue"].ToString());
                    model.CreatedBy = model.UpdatedBy = Request.Form["userName"].ToString();
                    model.CreatedDate = DateTime.Now;

                    model.UpdatedDate = DateTime.Now;
                    ARAccountReceivableBO.Instance.Insert(model);
                }
                else
                {
                    ARAccountReceivableModel model = (ARAccountReceivableModel)ARAccountReceivableBO.Instance.FindByPrimaryKey(int.Parse(Request.Form["id"].ToString()));
                    if (model == null || model.ID == 0)
                    {
                        return Json(new { code = 1, msg = "Could not find AR Account Receivable" });

                    }
                    model.AccountNo = Request.Form["accountNumber"].ToString();
                    model.AccountTypeID = int.Parse(Request.Form["accountType"].ToString());
                    model.CreditLimit = string.IsNullOrEmpty(Request.Form["creditLimit"].ToString()) ? 0 : int.Parse(Request.Form["creditLimit"].ToString());
                    model.CurrencyID = "VND";
                    model.ProfileID = profile.ID;
                    model.AccountName = profile.Account;
                    model.ContactName = string.IsNullOrEmpty(Request.Form["contact"].ToString()) ? "" : Request.Form["contact"].ToString();
                    model.TelePhone = string.IsNullOrEmpty(Request.Form["phone"].ToString()) ? "" : Request.Form["phone"].ToString();
                    model.Fax = string.IsNullOrEmpty(Request.Form["fax"].ToString()) ? "" : Request.Form["fax"].ToString();
                    model.Email = string.IsNullOrEmpty(Request.Form["email"].ToString()) ? "" : Request.Form["email"].ToString();
                    model.Address1 = string.IsNullOrEmpty(Request.Form["address1"].ToString()) ? "" : Request.Form["address1"].ToString();
                    model.Address2 = string.IsNullOrEmpty(Request.Form["address2"].ToString()) ? "" : Request.Form["address2"].ToString();
                    model.Address3 = string.IsNullOrEmpty(Request.Form["address3"].ToString()) ? "" : Request.Form["address3"].ToString();
                    model.CityID = int.Parse(Request.Form["city"].ToString());
                    model.PostalCode = string.IsNullOrEmpty(Request.Form["postalCode"].ToString()) ? "" : Request.Form["postalCode"].ToString();
                    model.CountryID = int.Parse(Request.Form["country"].ToString());
                    model.State = "";
                    model.Description = string.IsNullOrEmpty(Request.Form["description"].ToString()) ? "" : Request.Form["description"].ToString();
                    model.StatusFlagged = Request.Form["flagged"].ToString() == "1" ? true : false;
                    model.StatusInactive = Request.Form["inactive"].ToString() == "1" ? true : false;
                    model.PaymentDueDays = string.IsNullOrEmpty(Request.Form["paymentDue"].ToString()) ? 0 : int.Parse(Request.Form["paymentDue"].ToString());
                    model.UpdatedBy = Request.Form["userName"].ToString();
                    model.UpdatedDate = DateTime.Now;
                    ARAccountReceivableBO.Instance.Update(model);
                }

                pt.CommitTransaction();
                return Json(new { code = 0, msg = "AR Account Available was created successfully" });

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
        #endregion

        #region DatVP __ Accounting: Maintaince
        [HttpGet]
        public async Task<IActionResult> GetAccountReceivable(int id)
        {
            try
            {

                ARAccountReceivableModel model = (ARAccountReceivableModel)ARAccountReceivableBO.Instance.FindByPrimaryKey(id);
                return Json(model);
            }
            catch (Exception ex)
            {
                return Json(new ARAccountTypeModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchMaintenance(int arID, string folioNo,string isActive,string paymentOnly,string print,DateTime fromDate, DateTime toDate)
        {
            try
            {

                var data = _iAccountingService.AccountMaintence( arID,  folioNo ?? "",  isActive ?? "",  paymentOnly ?? "",  print??"",  fromDate,  toDate);
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
        #endregion
    }
}

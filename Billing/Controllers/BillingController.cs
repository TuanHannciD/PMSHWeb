using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using Billing.Dto;
using Billing.Services.Interfaces;
using DevExpress.Office.Utils;
using DevExpress.Web.Internal;
using DevExpress.XtraReports.Design;
using DevExpress.XtraRichEdit.Fields;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using User.Services.Interfaces;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Billing.Controllers
{
    public class BillingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BillingController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IPostService _iPostService;
        private readonly IUserService _iUserService;

        public BillingController(ILogger<BillingController> logger,
                IMemoryCache cache, IConfiguration configuration, IPostService iPostService, IUserService iUserService)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _iPostService = iPostService;
            _iUserService = iUserService;
        }


        #region DatVP __ Billing: Common
        [HttpGet]
        public async Task<IActionResult> GetInforService()
        {
            try
            {

                var groupTransaction = TransactionGroupBO.GetList();
                var groupSubTransaction = TransactionSubGroupBO.GetList();
                var transactions = TransactionsBO.GetList();
                var articles = ArticleBO.GetList();

                return Json(new
                {
                    groupTransaction = groupTransaction,
                    groupSubTransaction = groupSubTransaction,
                    transactions = transactions,
                    articles = articles
                });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

    //    [HttpPost]
    //    public async Task<IActionResult> CrashierLogin()
    //    {
    //        try
    //        {
    //            string loginName = Request.Form["LoginName"].ToString();
    //            string password = Request.Form["Password"].ToString();
    //            var result = _iUserService.Login(loginName, password);
    //            if(result == null || result.ID == 0)
    //            {
    //                return Json(new { code = 1, msg = "Could not find account. Please login again!" });

    //            }
    //            var businessDate = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
                

    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new { code = 1, msg = ex.Message });
    //        }

    //    }
    //}
        #endregion

        #region DatVP __ Billing: Post
        [HttpPost]
        public ActionResult PostArticle()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                int postType = int.Parse(Request.Form["postType"].ToString());
                string listItemJson = Request.Form["listItem"];

                if (string.IsNullOrEmpty(listItemJson))
                {
                    return Json(new { code = 1, msg = "Could not find Transaction!" });
                }
                var itemList = JsonSerializer.Deserialize<List<ItemPost>>(listItemJson);
                if(itemList.Count < 1)
                {
                    return Json(new { code = 1, msg = "Could not find Transaction!" });

                }

                // tìm invoice lớn nhất 
                string invoiceNo = (FolioDetailBO.GetTopInvoiceNo() + 1).ToString();
                foreach (var itemTrans in itemList)
                {
                    string transactionNo = (FolioDetailBO.GetTopTransactioNo() + 1).ToString();

                    string tranCode = itemTrans.transCode;
                    if (string.IsNullOrEmpty(tranCode))
                    {
                        return Json(new { code = 1, msg = "Please choose Transaction/Article!" });

                    }
                    List<TransactionsModel> trans = PropertyUtils.ConvertToList<TransactionsModel>(TransactionsBO.Instance.FindByAttribute("Code", tranCode));
                    if (trans.Count < 1)
                    {
                        return Json(new { code = 1, msg = "Could not find Transaction!" });

                    }


                    // tìm folio của reservation
                    List<FolioModel> folio = PropertyUtils.ConvertToList<FolioModel>(FolioBO.Instance.FindByAttribute("ReservationID", int.Parse(Request.Form["rsvID"].ToString())));
                    if (folio.Count < 1)
                    {
                        return Json(new { code = 1, msg = $"Could not find Folio. Please check Folio" });

                    }


                    #region lưu transaction chính vào folio detail
                    // kiểm tra xem transaction chọn để post có article không
                    string articleCode = itemTrans.articleCode;
                    FolioDetailModel folioArticle = new FolioDetailModel();
                    folioArticle.UserID = folioArticle.ShiftID = int.Parse(Request.Form["userID"].ToString());
                    folioArticle.UserName = folioArticle.CashierNo = Request.Form["userID"].ToString();
                    folioArticle.ReservationID = folioArticle.OriginReservationID = int.Parse(Request.Form["rsvID"].ToString());
                    folioArticle.FolioID = folioArticle.OriginFolioID = folio[0].ID;
                    folioArticle.InvoiceNo =  invoiceNo;
                    folioArticle.TransactionNo = transactionNo;
                    folioArticle.ReceiptNo = "";
                    folioArticle.TransactionDate = DateTime.Now;
                    folioArticle.ProfitCenterID = 2;
                    folioArticle.ProfitCenterCode = "0";
                    folioArticle.TransactionGroupID = trans[0].TransactionGroupID;
                    folioArticle.TransactionSubgroupID = trans[0].TransactionSubGroupID;
                    folioArticle.GroupCode = trans[0].GroupCode;
                    folioArticle.SubgroupCode = trans[0].SubgroupCode;
                    folioArticle.GroupType = trans[0].GroupType;
                    folioArticle.TransactionCode = tranCode;
                    if (!string.IsNullOrEmpty(articleCode))
                    {
                        folioArticle.ArticleCode = articleCode;
                        string articleName = !string.IsNullOrEmpty(itemTrans.articleName) ? itemTrans.articleName : string.Empty;
                        folioArticle.Reference = $"A[{articleCode}]-{articleName}";
                    }
                    else
                    {
                        folioArticle.ArticleCode = "";
                    }
                    if (!string.IsNullOrEmpty(Request.Form["referencePost"].ToString()))
                    {
                        folioArticle.Reference = Request.Form["referencePost"].ToString();

                    }
                    folioArticle.Status = false;
                    if(postType == 1)
                    {
                        folioArticle.RowState = 1;
                        folioArticle.PostType = 2;
                    }
                    else
                    {
                        folioArticle.RowState = 2;
                        folioArticle.PostType = 3;
                    }
                    folioArticle.IsSplit = true;
                    folioArticle.Quantity = int.Parse(!string.IsNullOrEmpty(itemTrans.quantity) ? itemTrans.quantity : "0");
                    folioArticle.Price = decimal.Parse(!string.IsNullOrEmpty(itemTrans.priceNet) ? itemTrans.priceNet : "0");
                    folioArticle.Amount = decimal.Parse(!string.IsNullOrEmpty(itemTrans.amountNet) ? itemTrans.amountNet : "0");
                    folioArticle.CurrencyID = folioArticle.CurrencyMaster = "VND";
                    folioArticle.AmountMaster = decimal.Parse(!string.IsNullOrEmpty(itemTrans.amountNet) ? itemTrans.amountNet : "0");
                    folioArticle.Description = Request.Form["description"].ToString();
                    folioArticle.AmountBeforeTax = folioArticle.AmountMasterBeforeTax = decimal.Parse(!string.IsNullOrEmpty(itemTrans.amount) ? itemTrans.amount : "0");
                    folioArticle.AmountGross = folioArticle.AmountMasterGross = decimal.Parse(!string.IsNullOrEmpty(itemTrans.amountNet) ? itemTrans.amountNet : "0"); ;
                    folioArticle.RoomType = "";
                    folioArticle.RoomTypeID = 0;
                    folioArticle.UserInsertID = folioArticle.UserUpdateID = int.Parse(Request.Form["userID"].ToString());
                    folioArticle.CreateDate = folioArticle.UpdateDate = DateTime.Now;
                    folioArticle.RoomID = int.Parse(Request.Form["roomID"].ToString());
                    folioArticle.Property = folioArticle.CheckNo = folioArticle.OriginARNo = "";
                    folioArticle.IsPostedAR = false;
                    folioArticle.ARTransID = 0;
                    folioArticle.IsTransfer = false;
                    FolioDetailBO.Instance.Insert(folioArticle);
                    #endregion

                    #region lưu transaction từ generate transaction và folio detail
                    List<GenerateTransactionModel> generateTransaction = PropertyUtils.ConvertToList<GenerateTransactionModel>(GenerateTransactionBO.Instance.FindByAttribute("TransactionCode", tranCode));
                    if (generateTransaction.Count > 0)
                    {
                        bool isVat = false;
                        bool isSvc = false;
                        int indexVat = -1;
                        int indexSvc = -1;
                        // Kiểm tra xem generate transaction có Tax không
                        for (int i = 0; i < generateTransaction.Count; i++)
                        {
                            if (generateTransaction[i].GroupCode == "Tax" && generateTransaction[i].SubgroupCode == "Tax")
                            {
                                isVat = true;
                                indexVat = i;
                                break;
                            }
                        }
                        // Kiểm tra xem generate transaction có Svc không
                        for (int i = 0; i < generateTransaction.Count; i++)
                        {
                            if (generateTransaction[i].GroupCode == "Tax" && generateTransaction[i].SubgroupCode == "SVC")
                            {
                                isSvc = true;
                                indexSvc = i;
                                break;
                            }
                        }
                        foreach (var item in generateTransaction)
                        {
                            if (item.GroupCode == "Tax" && item.SubgroupCode == "Tax")
                            {
                                FolioDetailModel folioSub = new FolioDetailModel();
                                folioSub.UserID = folioSub.ShiftID = int.Parse(Request.Form["userID"].ToString());
                                folioSub.UserName = folioSub.CashierNo = Request.Form["userID"].ToString();
                                folioSub.ReservationID = folioSub.OriginReservationID = int.Parse(Request.Form["rsvID"].ToString());
                                folioSub.FolioID = folioSub.OriginFolioID = folio[0].ID;
                                folioSub.InvoiceNo  = invoiceNo;
                                folioSub.TransactionNo = transactionNo;
                                folioSub.ReceiptNo = "";
                                folioSub.TransactionDate = DateTime.Now;
                                folioSub.ProfitCenterID = 2;
                                folioSub.ProfitCenterCode = "0";
                                folioSub.TransactionGroupID = item.TransactionGroupID;
                                folioSub.TransactionSubgroupID = item.TransactionSubGroupID;
                                folioSub.GroupCode = item.GroupCode;
                                folioSub.SubgroupCode = item.SubgroupCode;
                                folioSub.GroupType = item.GroupType;
                                folioSub.TransactionCode = item.TransactionCodeDetail;
                                folioSub.ArticleCode = "";
                                folioSub.Status = false;
                                if (postType == 1)
                                {
                                    folioArticle.RowState = 2;
                                    folioArticle.PostType = 2;
                                }
                                else
                                {
                                    folioArticle.RowState = 3;
                                    folioArticle.PostType = 3;
                                }
                                folioSub.IsSplit = false;
                                folioSub.Quantity = int.Parse(!string.IsNullOrEmpty(itemTrans.quantity) ? itemTrans.quantity : "0"); ;
                                if (item.GroupCode == "Tax" && item.GroupCode == "Tax")
                                {
                                    folioSub.Price = decimal.Parse(!string.IsNullOrEmpty(itemTrans.priceNet) ? itemTrans.priceNet : "0") * (item.Percentage / 100) / (1 + (item.Percentage / 100));
                                }
                                folioSub.Amount = folioSub.AmountMaster = folioSub.AmountBeforeTax = folioSub.AmountMasterBeforeTax = folioSub.AmountGross = folioSub.AmountMasterGross = folioSub.Price * folioSub.Quantity;
                                folioSub.CurrencyID = folioSub.CurrencyMaster = "VND";
                                folioSub.Description = item.Description;
                                folioSub.Reference = "";
                                folioSub.RoomType = "";
                                folioSub.RoomTypeID = 0;
                                folioSub.UserInsertID = folioSub.UserUpdateID = int.Parse(Request.Form["userID"].ToString());
                                folioSub.CreateDate = folioSub.UpdateDate = DateTime.Now;
                                folioSub.RoomID = int.Parse(Request.Form["roomID"].ToString());
                                folioSub.Property = folioSub.CheckNo = folioSub.OriginARNo = "";
                                folioSub.IsPostedAR = false;
                                folioSub.ARTransID = 0;
                                folioSub.IsTransfer = false;
                                FolioDetailBO.Instance.Insert(folioSub);
                            }

                            else if (item.GroupCode == "Tax" && item.SubgroupCode == "SVC")
                            {
                                decimal priceVat = 0;
                                if (isVat == true)
                                {
                                    decimal percent = generateTransaction.Where(x => x.GroupCode == "Tax" && x.SubgroupCode == "Tax").FirstOrDefault().Percentage;
                                    priceVat = decimal.Parse(!string.IsNullOrEmpty(itemTrans.priceNet) ? itemTrans.priceNet : "0") * (percent / 100) / (1 + (percent / 100));

                                }
                                FolioDetailModel folioSub = new FolioDetailModel();
                                folioSub.UserID = folioSub.ShiftID = int.Parse(Request.Form["userID"].ToString());
                                folioSub.UserName = folioSub.CashierNo = Request.Form["userID"].ToString();
                                folioSub.ReservationID = folioSub.OriginReservationID = int.Parse(Request.Form["rsvID"].ToString());
                                folioSub.FolioID = folioSub.OriginFolioID = folio[0].ID;
                                folioSub.InvoiceNo = invoiceNo;
                                folioSub.TransactionNo = transactionNo;
                                folioSub.ReceiptNo = "";
                                folioSub.TransactionDate = DateTime.Now;
                                folioSub.ProfitCenterID = 2;
                                folioSub.ProfitCenterCode = "0";
                                folioSub.TransactionGroupID = item.TransactionGroupID;
                                folioSub.TransactionSubgroupID = item.TransactionSubGroupID;
                                folioSub.GroupCode = item.GroupCode;
                                folioSub.SubgroupCode = item.SubgroupCode;
                                folioSub.GroupType = item.GroupType;
                                folioSub.TransactionCode = item.TransactionCodeDetail;
                                folioSub.ArticleCode = "";
                                folioSub.Status = false;
                                if (postType == 1)
                                {
                                    folioArticle.RowState = 2;
                                    folioArticle.PostType = 2;
                                }
                                else
                                {
                                    folioArticle.RowState = 3;
                                    folioArticle.PostType = 3;
                                }
                                folioSub.IsSplit = false;
                                folioSub.Quantity = int.Parse(!string.IsNullOrEmpty(itemTrans.quantity) ? itemTrans.quantity : "0");
                                if (item.GroupCode == "Tax" && item.GroupCode == "Tax")
                                {
                                    folioSub.Price = (decimal.Parse(!string.IsNullOrEmpty(itemTrans.priceNet) ? itemTrans.priceNet : "0") - priceVat) * (item.Percentage / 100) / (1 + (item.Percentage / 100));
                                }
                                folioSub.Amount = folioSub.AmountMaster = folioSub.AmountBeforeTax = folioSub.AmountMasterBeforeTax = folioSub.AmountGross = folioSub.AmountMasterGross = folioSub.Price * folioSub.Quantity;
                                folioSub.CurrencyID = folioSub.CurrencyMaster = "VND";
                                folioSub.Description = item.Description;
                                folioSub.Reference = "";
                                folioSub.RoomType = "";
                                folioSub.RoomTypeID = 0;
                                folioSub.UserInsertID = folioSub.UserUpdateID = int.Parse(Request.Form["userID"].ToString());
                                folioSub.CreateDate = folioSub.UpdateDate = DateTime.Now;
                                folioSub.RoomID = int.Parse(Request.Form["roomID"].ToString());
                                folioSub.Property = folioSub.CheckNo = folioSub.OriginARNo = "";
                                folioSub.IsPostedAR = false;
                                folioSub.ARTransID = 0;
                                folioSub.IsTransfer = false;
                                FolioDetailBO.Instance.Insert(folioSub);
                            }

                            else
                            {
                                decimal priceVat = 0;
                                decimal priceSvc = 0;
                                if (isVat == true)
                                {
                                    decimal percent = generateTransaction[indexVat].Percentage;
                                    priceVat = decimal.Parse(!string.IsNullOrEmpty(itemTrans.priceNet) ? itemTrans.priceNet : "0") * (percent / 100) / (1 + (percent / 100));
                                }
                                if (isSvc == true)
                                {
                                    decimal percent = generateTransaction[indexSvc].Percentage;
                                    priceSvc = (decimal.Parse(!string.IsNullOrEmpty(itemTrans.priceNet) ? itemTrans.priceNet : "0") - priceVat) * (percent / 100) / (1 + (percent / 100));
                                }
                                FolioDetailModel folioSub = new FolioDetailModel();
                                folioSub.UserID = folioSub.ShiftID = int.Parse(Request.Form["userID"].ToString());
                                folioSub.UserName = folioSub.CashierNo = Request.Form["userID"].ToString();
                                folioSub.ReservationID = folioSub.OriginReservationID = int.Parse(Request.Form["rsvID"].ToString());
                                folioSub.FolioID = folioSub.OriginFolioID = folio[0].ID;
                                folioSub.InvoiceNo =  invoiceNo;
                                folioSub.TransactionNo = transactionNo;
                                folioSub.ReceiptNo = "";
                                folioSub.TransactionDate = DateTime.Now;
                                folioSub.ProfitCenterID = 2;
                                folioSub.ProfitCenterCode = "0";
                                folioSub.TransactionGroupID = item.TransactionGroupID;
                                folioSub.TransactionSubgroupID = item.TransactionSubGroupID;
                                folioSub.GroupCode = item.GroupCode;
                                folioSub.SubgroupCode = item.SubgroupCode;
                                folioSub.GroupType = item.GroupType;
                                folioSub.TransactionCode = item.TransactionCodeDetail;
                                folioSub.ArticleCode = "";
                                folioSub.Status = false;
                                if (postType == 1)
                                {
                                    folioArticle.RowState = 2;
                                    folioArticle.PostType = 2;
                                }
                                else
                                {
                                    folioArticle.RowState = 3;
                                    folioArticle.PostType = 3;
                                }
                                folioSub.IsSplit = false;
                                folioSub.Quantity = int.Parse(!string.IsNullOrEmpty(itemTrans.quantity) ? itemTrans.quantity : "0");
                                if (isVat == false && isSvc == false)
                                {
                                    folioSub.Price = decimal.Parse(!string.IsNullOrEmpty(itemTrans.priceNet) ? itemTrans.priceNet : "0") - decimal.Parse(!string.IsNullOrEmpty(itemTrans.priceNet) ? itemTrans.priceNet : "0") * (item.Percentage / 100);

                                }
                                else
                                {
                                    folioSub.Price = decimal.Parse(!string.IsNullOrEmpty(itemTrans.priceNet) ? itemTrans.priceNet : "0") - priceVat - priceSvc;
                                }
                                folioSub.Amount = folioSub.AmountMaster = folioSub.AmountBeforeTax = folioSub.AmountMasterBeforeTax = folioSub.AmountGross = folioSub.AmountMasterGross = folioSub.Price * folioSub.Quantity;
                                folioSub.CurrencyID = folioSub.CurrencyMaster = "VND";
                                folioSub.Description = item.Description;
                                folioSub.Reference = "";
                                folioSub.RoomType = "";
                                folioSub.RoomTypeID = 0;
                                folioSub.UserInsertID = folioSub.UserUpdateID = int.Parse(Request.Form["userID"].ToString());
                                folioSub.CreateDate = folioSub.UpdateDate = DateTime.Now;
                                folioSub.RoomID = int.Parse(Request.Form["roomID"].ToString());
                                folioSub.Property = folioSub.CheckNo = folioSub.OriginARNo = "";
                                folioSub.IsPostedAR = false;
                                folioSub.ARTransID = 0;
                                folioSub.IsTransfer = false;
                                FolioDetailBO.Instance.Insert(folioSub);
                            }
                        }
                    }
                    #endregion
                }

                pt.CommitTransaction();
                return Json(new { code = 0, msg = "New reservation created successfully" });

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

        [HttpGet]
        public async Task<IActionResult> CalculatePrice(string transactionCode, string price)
        {
            try
            {
                if (string.IsNullOrEmpty(transactionCode))
                {
                    price = "0";
                }
                decimal net = _iPostService.CalculatePrice(transactionCode, decimal.Parse(price));

                return Json(net);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> CalculateNet(string transactionCode, string price)
        {
            try
            {
                if(string.IsNullOrEmpty(transactionCode))
                {
                    price = "0";
                }
                decimal net = _iPostService.CalculateNet(transactionCode, decimal.Parse(price));

                return Json(net);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        #endregion

        #region DatVP __ Billing: Edit Posting
        [HttpGet]
        public async Task<IActionResult> GetFolioDetailMaster(string transactionNo)
        {
            try
            {
                var result = FolioDetailBO.GetFolioDetailMaster(transactionNo);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }


        [HttpPost]
        public ActionResult EditPosting()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                List<FolioDetailModel> trans = PropertyUtils.ConvertToList<FolioDetailModel>(FolioDetailBO.Instance.FindByAttribute("TransactionNo", Request.Form["transactionNo"].ToString()));
                if(trans.Count < 1)
                {
                    return Json(new { code = 1, msg = "Could not find!" });

                }
                string transCode = "";
                for(int i = 0;i< trans.Count; i++)
                {
                    if (trans[i].IsSplit == true && trans[i].RowState != trans[i].PostType)
                    {
                        transCode = trans[i].TransactionCode;
                        break;
                    }
                }
                List<GenerateTransactionModel> generateTransaction = PropertyUtils.ConvertToList<GenerateTransactionModel>(GenerateTransactionBO.Instance.FindByAttribute("TransactionCode", transCode));
                decimal priceVat = 0;
                decimal priceSvc = 0;
                if (generateTransaction.Count > 0)
                {

                    // Kiểm tra xem generate transaction có Tax không
                    for (int i = 0; i < generateTransaction.Count; i++)
                    {
                        if (generateTransaction[i].GroupCode == "Tax" && generateTransaction[i].SubgroupCode == "Tax")
                        {
                            priceVat = decimal.Parse(Request.Form["amount"].ToString()) * (generateTransaction[i].Percentage / 100) / (1 + (generateTransaction[i].Percentage / 100));
                            break;
                        }
                    }
                    // Kiểm tra xem generate transaction có Svc không
                    for (int i = 0; i < generateTransaction.Count; i++)
                    {
                        if (generateTransaction[i].GroupCode == "Tax" && generateTransaction[i].SubgroupCode == "SVC")
                        {
                            priceSvc = (decimal.Parse(Request.Form["amount"].ToString()) - priceVat) * (generateTransaction[i].Percentage / 100) / (1 + (generateTransaction[i].Percentage / 100));
                            
                            break;
                        }
                    }
                }
                foreach (var item in trans)
                {
                    FolioDetailModel folio = (FolioDetailModel)FolioDetailBO.Instance.FindByPrimaryKey(item.ID);
                    if(folio.IsSplit == true)
                    {
                        folio.Price = decimal.Parse(Request.Form["price"].ToString());
                        folio.Quantity = int.Parse(Request.Form["quantity"].ToString());
                        folio.Amount = folio.AmountMaster = folio.AmountGross = folio.AmountMasterGross = folio.Price * folio.Quantity;
                        folio.AmountBeforeTax = folio.AmountMasterBeforeTax = folio.Amount - priceVat - priceSvc;
                    }
                    else
                    {
                        if(folio.SubgroupCode == "Tax")
                        {
                            folio.Quantity = int.Parse(Request.Form["quantity"].ToString());

                            folio.Price = priceVat / folio.Quantity;
                            folio.Amount = folio.AmountMaster = folio.AmountGross = folio.AmountMasterGross = folio.AmountBeforeTax = folio.AmountMasterBeforeTax = priceVat;
                        }
                        else if(folio.SubgroupCode == "Svc")
                        {
                            folio.Quantity = int.Parse(Request.Form["quantity"].ToString());

                            folio.Price = priceSvc / folio.Quantity;
                            folio.Amount = folio.AmountMaster = folio.AmountGross = folio.AmountMasterGross = folio.AmountBeforeTax = folio.AmountMasterBeforeTax = priceSvc;
                        }
                        else
                        {
                            decimal priceMain = decimal.Parse(Request.Form["amount"].ToString()) - priceVat - priceSvc;
                            folio.Quantity = int.Parse(Request.Form["quantity"].ToString());

                            folio.Price = priceMain / folio.Quantity;
                            folio.Amount = folio.AmountMaster = folio.AmountGross = folio.AmountMasterGross = folio.AmountBeforeTax = folio.AmountMasterBeforeTax = priceMain;
                        }
                    }
                    FolioDetailBO.Instance.Update(folio);
                }

                pt.CommitTransaction();
                return Json(new { code = 0, msg = "Edit Posting was successfully" });

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
    }
}

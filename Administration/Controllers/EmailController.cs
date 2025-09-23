using Administration.Services.Implements;
using Administration.Services.Interfaces;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using DevExpress.Office.Utils;
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
    public class EmailController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _iEmailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmailController(ILogger<EmailController> logger,
                IMemoryCache cache, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IEmailService iEmailService)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _iEmailService = iEmailService;
        }
        public IActionResult SetupEmail()
        {
            return View();
        }

        public IActionResult SendEmail()
        {
            return View();
        }

        #region DatVP __ SetupEmail: Get config email in config system
        [HttpGet]

        public IActionResult GetEmailConfig()
        {
            try
            {
                string senderName = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindByAttribute("KeyName", "MAIL_FROMNAME")).FirstOrDefault().KeyValue;
                string host = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindByAttribute("KeyName", "MAIL_SERVERNAME")).FirstOrDefault().KeyValue;
                string port = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindByAttribute("KeyName", "MAIL_SERVERPORT")).FirstOrDefault().KeyValue;
                string userName = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindByAttribute("KeyName", "MAIL_FROM")).FirstOrDefault().KeyValue;
                string password = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindByAttribute("KeyName", "MAIL_PASSWORD")).FirstOrDefault().KeyValue;
                string ssl = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindByAttribute("KeyName", "MAIL_USE_SSL")).FirstOrDefault().KeyValue;

                return Json(new
                {
                    senderName = senderName,
                    host = host,
                    port = port,
                    userName = userName,
                    password = password,
                    ssl = ssl
                });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        #endregion

        #region DatVP __ SetUpEmail: Save config email
        [HttpPost]
        public ActionResult SaveConfigEmail()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                #region lưu sender name
                ConfigSystemModel senderName = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindByAttribute("KeyName", "MAIL_FROMNAME")).FirstOrDefault();
                if (senderName == null)
                {
                    senderName.KeyName = "MAIL_FROMNAME";
                    senderName.KeyValue = Request.Form["senderName"];
                    senderName.Desciption = "Sender Name";
                    senderName.UserInsertID = senderName.UserUpdateID = int.Parse(Request.Form["userID"]);
                    senderName.UpdateDate = senderName.CreateDate = DateTime.Now;
                    ConfigSystemBO.Instance.Insert(senderName);
                }
                else
                {
                    senderName.KeyValue = Request.Form["senderName"];
                    senderName.UserUpdateID = int.Parse(Request.Form["userID"]);
                    senderName.UpdateDate = DateTime.Now;
                    ConfigSystemBO.Instance.Update(senderName);

                }

                #endregion


                #region lưu host
                ConfigSystemModel host = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindByAttribute("KeyName", "MAIL_SERVERNAME")).FirstOrDefault();
                if (host == null)
                {
                    host.KeyName = "MAIL_SERVERNAME";
                    host.KeyValue = Request.Form["host"];
                    host.Desciption = "Server host";
                    host.UserInsertID = host.UserUpdateID = int.Parse(Request.Form["userID"]);
                    host.UpdateDate = host.CreateDate = DateTime.Now;
                    ConfigSystemBO.Instance.Insert(host);
                }
                else
                {
                    host.KeyValue = Request.Form["host"];
                    host.UserUpdateID = int.Parse(Request.Form["userID"]);
                    host.UpdateDate = DateTime.Now;
                    ConfigSystemBO.Instance.Update(host);

                }
                #endregion

                #region  lưu port
                ConfigSystemModel port = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindByAttribute("KeyName", "MAIL_SERVERPORT")).FirstOrDefault();
                if (port == null)
                {
                    port.KeyName = "MAIL_SERVERPORT";
                    port.KeyValue = Request.Form["port"];
                    port.Desciption = "Server port";
                    port.UserInsertID = port.UserUpdateID = int.Parse(Request.Form["userID"]);
                    port.UpdateDate = port.CreateDate = DateTime.Now;
                    ConfigSystemBO.Instance.Insert(port);
                }
                else
                {
                    port.KeyValue = Request.Form["port"];
                    port.UserUpdateID = int.Parse(Request.Form["userID"]);
                    port.UpdateDate = DateTime.Now;
                    ConfigSystemBO.Instance.Update(port);

                }
                #endregion

                #region lưu user name
                ConfigSystemModel userName = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindByAttribute("KeyName", "MAIL_FROM")).FirstOrDefault();
                if (userName == null)
                {
                    userName.KeyName = "MAIL_FROM";
                    userName.KeyValue = Request.Form["userNameEmail"];
                    userName.Desciption = "user name";
                    userName.UserInsertID = userName.UserUpdateID = int.Parse(Request.Form["userID"]);
                    userName.UpdateDate = userName.CreateDate = DateTime.Now;
                    ConfigSystemBO.Instance.Insert(userName);
                }
                else
                {
                    userName.KeyValue = Request.Form["userNameEmail"];
                    userName.UserUpdateID = int.Parse(Request.Form["userID"]);
                    userName.UpdateDate = DateTime.Now;
                    ConfigSystemBO.Instance.Update(userName);

                }
                #endregion

                #region lưu password
                ConfigSystemModel password = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindByAttribute("KeyName", "MAIL_PASSWORD")).FirstOrDefault();
                if (password == null)
                {
                    password.KeyName = "MAIL_PASSWORD";
                    password.KeyValue = Request.Form["password"];
                    password.Desciption = "password";
                    password.UserInsertID = password.UserUpdateID = int.Parse(Request.Form["userID"]);
                    password.UpdateDate = password.CreateDate = DateTime.Now;
                    ConfigSystemBO.Instance.Insert(password);
                }
                else
                {
                    password.KeyValue = Request.Form["password"];
                    password.UserUpdateID = int.Parse(Request.Form["userID"]);
                    password.UpdateDate = DateTime.Now;
                    ConfigSystemBO.Instance.Update(password);

                }
                #endregion

                #region lưu ssl
                ConfigSystemModel ssl = PropertyUtils.ConvertToList<ConfigSystemModel>(ConfigSystemBO.Instance.FindByAttribute("KeyName", "MAIL_USE_SSL")).FirstOrDefault();
                if (ssl == null)
                {
                    ssl.KeyName = "MAIL_USE_SSL";
                    ssl.KeyValue = Request.Form["ssl"];
                    ssl.Desciption = "ssl";
                    ssl.UserInsertID = ssl.UserUpdateID = int.Parse(Request.Form["userID"]);
                    ssl.UpdateDate = ssl.CreateDate = DateTime.Now;
                    ConfigSystemBO.Instance.Insert(ssl);
                }
                else
                {
                    ssl.KeyValue = Request.Form["ssl"];
                    ssl.UserUpdateID = int.Parse(Request.Form["userID"]);
                    ssl.UpdateDate = DateTime.Now;
                    ConfigSystemBO.Instance.Update(ssl);

                }
                #endregion


                pt.CommitTransaction();
                return Json(new { code = 0, msg = $"Saved config system" });

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

        #region DatVP __ SetupEmail: Get all parameter
        [HttpGet]
        public IActionResult GetAllParameter()
        {
            try
            {
                var senderName = PropertyUtils.ConvertToList<ParameterMailModel>(ParameterMailBO.Instance.FindAll()).ToList();


                return Json(senderName);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        #endregion

        #region DatVP __ SetupEmail: Save param
        [HttpPost]
        public ActionResult SaveParam()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                #region lưu sender name
                ParameterMailModel param = PropertyUtils.ConvertToList<ParameterMailModel>(ParameterMailBO.Instance.FindByAttribute("Parameter", Request.Form["param"])).FirstOrDefault();
                if (param != null)
                {
                    return Json(new { code = 1, msg = "Param invalid" });

                }
                ParameterMailModel paramSave = new ParameterMailModel();
                paramSave.Parameter = Request.Form["param"];
                ParameterMailBO.Instance.InsertStringNoneId(paramSave);
                #endregion



                pt.CommitTransaction();
                return Json(new { code = 0, msg = $"Saved param" });

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

        #region DatVP __  SetupEmail: Get all template email
        [HttpGet]
        public IActionResult GetAllTemplateEmail()
        {
            try
            {
                var senderName = PropertyUtils.ConvertToList<ContactEmailTemplateModel>(ContactEmailTemplateBO.Instance.FindAll()).ToList();


                return Json(senderName);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        #endregion

        #region DatVP __ SetupEmail: Save contact template email
        [HttpPost]
        public ActionResult SaveContactTemplateEmail()
        {
            ProcessTransactions pt = new ProcessTransactions();
            try
            {
                pt.OpenConnection();
                pt.BeginTransaction();
                ContactEmailTemplateModel model = new ContactEmailTemplateModel();
                model.Name = Request.Form["subject"];
                model.Content = Request.Form["content"];
                model.Language = 1;
                ContactEmailTemplateBO.Instance.Insert(model);


                pt.CommitTransaction();
                return Json(new { code = 0, msg = $"Saved contact email template" });

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

        #region DatVP __ SendEmail: Get Email of guest
        [HttpGet]
        public IActionResult GetAllEmailOfGuest(DateTime fromDate,DateTime toDate, int status)
        {
            try
            {
                var data = _iEmailService.GetAllEmailOfGuest(fromDate,toDate, status);

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

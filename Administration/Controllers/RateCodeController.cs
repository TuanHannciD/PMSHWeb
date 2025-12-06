using System.Data;
using Administration.Services.Interfaces;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers
{
    [Route("/Administration/RateCode")]
    public class RateCodeController(IRateCodeService rateCodeService) : Controller
    {
        private readonly IRateCodeService _rateCodeSer = rateCodeService;

        [HttpGet("")]
        public IActionResult RateCode()
        {
            // List<RoomModel> listroom = PropertyUtils.ConvertToList<RoomModel>(RoomBO.Instance.FindAll());
            // ViewBag.RoomList = listroom;
            // List<ZoneModel> listzone = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            // ViewBag.ZoneList = listzone;
            // List<RoomClassModel> listrclass = PropertyUtils.ConvertToList<RoomClassModel>(RoomClassBO.Instance.FindAll());
            // ViewBag.RoomClassList = listrclass;
            // List<CommentModel> listcmt = PropertyUtils.ConvertToList<CommentModel>(CommentBO.Instance.FindAll());
            // ViewBag.CommentList = listcmt;
            List<RateCodeModel> listRateCode = PropertyUtils.ConvertToList<RateCodeModel>(RateCodeBO.Instance.FindAll());
            List<RateCategoryModel> listRateCate = PropertyUtils.ConvertToList<RateCategoryModel>(RateCategoryBO.Instance.FindAll());
            List<RateClassModel> listRateClass = PropertyUtils.ConvertToList<RateClassModel>(RateClassBO.Instance.FindAll());
            ViewBag.RateCodeList = listRateCode;
            ViewBag.RateCateList = listRateCate;
            ViewBag.RateClass = listRateClass;
            return View("~/Views/Administration/RateCode/RateCode.cshtml");
        }
        [HttpGet("GetAllRateCode")]
        public IActionResult GetAllRateCode(string rateCode, string rateCategory)
        {
            try
            {
                DataTable dataTable = _rateCodeSer.GetAllRateCode(rateCode, rateCategory);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ID = d.Field<int>("ID") == 0 ? "" : d.Field<int>("ID").ToString(),
                                  Sequence = d.Field<int>("Sequence") == 0 ? "" : d.Field<int>("Sequence").ToString(),
                                  RateCode = d.Field<string>("RateCode") ?? "",
                                  Category = d.Field<string>("Category") ?? "",
                                  Description = d.Field<string>("Description") ?? ""
                              }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost("Save")]
        public IActionResult Save(RateCodeDto model)
        {
            try
            {
                // Lấy BusinessDate từ BO (giống controller mẫu của bạn)
                List<BusinessDateModel> businessDates =
                    PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());

                // Tạo model để insert/update
                RateCodeModel entity = new()
                {
                    // Map dữ liệu từ DTO sang entity
                    ID = model.Id,
                    RateCode = model.RateCode,
                    Descripton = model.Description,
                    RateCategoryID = model.RateCategory,
                    RateClassID = model.RateClass,
                    Sequence = model.Sequence,
                    DefaultDisplay = (byte)Math.Clamp(model.Display, 0, 255),
                    DayUse = model.DayUse == 1,
                    Status = model.Active,
                    Negotiated = model.Negotiated,
                    IndividualOnly = model.IndividualOnly,
                    IsModify = model.IsModifiable
                };

                // Insert hoặc Update
                if (model.Id > 0)
                {
                    // UPDATE
                    entity.UserUpdateID = model.UserID;
                    entity.UpdateDate = businessDates[0].BusinessDate;

                    RateCodeBO.Instance.Update(entity);
                }
                else
                {
                    // INSERT
                    entity.UserUpdateID = model.UserID;
                    entity.CreateDate = businessDates[0].BusinessDate;

                    entity.UserUpdateID = model.UserID;
                    entity.UpdateDate = entity.CreateDate;

                    RateCodeBO.Instance.Insert(entity);
                }

                return Json(new
                {
                    success = true,
                    message = "Success"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        public class RateCodeDto
        {
            public int UserID { get; set; }
            public int Id { get; set; }
            public string RateCode { get; set; } = "";
            public string Description { get; set; } = "";
            public int RateClass { get; set; }
            public int RateCategory { get; set; }
            public int Sequence { get; set; }
            public int Display { get; set; }
            public int DayUse { get; set; }
            public bool Active { get; set; }
            public bool Negotiated { get; set; }
            public bool IndividualOnly { get; set; }
            public bool IsModifiable { get; set; }
        }


    }
}

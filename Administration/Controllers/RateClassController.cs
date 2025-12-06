using System.Data;
using Administration.Services.Interfaces;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers
{
    [Route("/Administration/RateCode")]
    public class RateClassController : Controller
    {
        private readonly IRateClassService _svRateClass;
        public RateClassController(IRateClassService rateClass)
        {
            _svRateClass = rateClass;
        }
        [HttpGet("RateClass")] // Truyeenf DataGrid , script api
        public IActionResult RateClass()
        {
            return View("~/Views/Administration/RateCode/RateClass.cshtml");
            // Truyền đường dẫn chuẩn vào để tìm đúng
        }



        [HttpGet("GetAllRateClass")]
        public async Task<IActionResult> RateClassGetAll(int inactive = 0)
        {
            try
            {
                DataTable dataTable = await _svRateClass.RateClassTypeData(inactive);
                var result = (from d in dataTable.AsEnumerable()
                              select new
                              {
                                  ID = d["ID"]?.ToString() ?? "",
                                  Code = d["Code"]?.ToString() ?? "",
                                  Description = d["Description"]?.ToString() ?? "",
                                  CreatedDate = d["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(d["CreatedDate"]) : (DateTime?)null,
                                  UpdatedDate = d["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(d["UpdatedDate"]) : (DateTime?)null,
                                  CreatedBy = d["CreatedBy"]?.ToString() ?? "",
                                  UpdatedBy = d["UpdatedBy"]?.ToString() ?? "",
                                  Inactive = d["Inactive"] != DBNull.Value ? Convert.ToInt32(d["Inactive"]) : 0,

                              }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {

                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPost("RateClassSave")]
        public async Task<IActionResult> RateClassSave(string idRateClass, string codeClass, string descriptionaccty, string user, string inactive)
        {
            try
            {
                List<BusinessDateModel> businessDates = PropertyUtils.ConvertToList<BusinessDateModel>(BusinessDateBO.Instance.FindAll());
                RateClassModel _Model = new();
                user = user.Trim().Trim('"');
                _Model.Code = codeClass;
                _Model.Description = descriptionaccty;
                _Model.Inactive = inactive == "1" ? true : false;
                if (!string.IsNullOrWhiteSpace(idRateClass) && idRateClass != "0")
                {
                    _Model.UpdatedBy = user;
                    _Model.UpdatedDate = businessDates[0].BusinessDate;
                    _Model.ID = int.Parse(idRateClass);
                    RateClassBO.Instance.Update(_Model);
                }
                else
                {
                    _Model.UpdatedBy = user;
                    _Model.CreatedBy = user;
                    _Model.CreatedDate = businessDates[0].BusinessDate;
                    _Model.UpdatedDate = _Model.CreatedDate;
                    RateClassBO.Instance.Insert(_Model);
                }
                return Json(new { success = true, message = "Success" });
            }
            catch (Exception ex)
            {

                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPost("RateClassDelete")]
        public IActionResult RateClassDelete(int id)
        {
            try
            {
                RateClassBO.Instance.Delete(id);

                return Json(new { success = true, message = $"Success Delete! {id}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}


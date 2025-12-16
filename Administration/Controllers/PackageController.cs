using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers
{
    [Route("/Administration/RateCode")]
    public class PackageController : Controller
    {
        [HttpGet("Package")] // Truyeenf DataGrid , script api
        public IActionResult Package()
        {
            List<PackageModel> listPackage = PropertyUtils.ConvertToList<PackageModel>(PackageBO.Instance.FindAll());
            List<PackageForecastGroupModel> listPackageForecastGroup = PropertyUtils.ConvertToList<PackageForecastGroupModel>(PackageForecastGroupBO.Instance.FindAll());
            ViewBag.PackageList = listPackage;
            ViewBag.PackageForecastGroup = listPackageForecastGroup;
            return View("~/Views/Administration/RateCode/Package.cshtml");
            // Truyền đường dẫn chuẩn vào để tìm đúng
        }
        [HttpGet("GetByIDPackage")]
        public async Task<IActionResult> GetByIDPackage(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid ID"
                    });
                }
                var pack = PackageBO.Instance.FindByPrimaryKey(id) as PackageModel;
                if (pack == null || pack.ID == 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Rate Code not found (ID = {id})"
                    });
                }

                return Json(new
                {
                    success = true,
                    message = "Success",
                    data = pack
                });
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, $"GetByID failed with ID {id}");
                return Json(new
                {
                    success = false,
                    message = "Error: " + ex.Message
                });
            }
        }
    }
}

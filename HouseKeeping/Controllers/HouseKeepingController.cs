using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using HouseKeeping.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace HouseKeeping.Controllers
{
    public class HouseKeepingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HouseKeepingController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IHouseKeepingService _iHouseKeepingService;
        public HouseKeepingController(ILogger<HouseKeepingController> logger,
             IMemoryCache cache, IConfiguration configuration, IHouseKeepingService iHouseKeepingService)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _iHouseKeepingService = iHouseKeepingService;
        }
        public IActionResult RoomControlPanel()
        {
            List<ZoneModel> listzo = PropertyUtils.ConvertToList<ZoneModel>(ZoneBO.Instance.FindAll());
            ViewBag.ZoneList = listzo;
            return View();
        }

        [HttpGet]
        public IActionResult RoomControlPanelData(DateTime fromDate, DateTime toDate, string zone)
        {
            try
            {
                DataTable dataTable = _iHouseKeepingService.RoomControlPanelData(fromDate, toDate, zone);

                // Tạo danh sách các ngày trong khoảng fromDate -> toDate
                var dateRange = Enumerable.Range(0, (toDate - fromDate).Days + 1)
                                          .Select(offset => fromDate.AddDays(offset))
                                          .ToList();

                var result = dataTable.AsEnumerable().Select(d =>
                {
                    var rowData = new Dictionary<string, object>
                    {
                        ["Statisticname"] = d["Statisticname"]?.ToString() ?? "",
                        ["Index"] = d["Index"]?.ToString() ?? ""
                    };

                    foreach (var date in dateRange)
                    {
                        string columnName = date.ToString("yyyy-MM-dd"); // đồng bộ với JS
                        string dtColName = date.ToString("yyyy/MM/dd");   // tên cột trong DataTable

                        if (dataTable.Columns.Contains(dtColName))
                        {
                            var cellValue = d[dtColName];
                            rowData[columnName] = cellValue == DBNull.Value ? "" : cellValue;
                        }
                        else
                        {
                            rowData[columnName] = ""; // nếu không có cột thì cũng để ""
                        }
                    }

                    return rowData;
                }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }


    }
}

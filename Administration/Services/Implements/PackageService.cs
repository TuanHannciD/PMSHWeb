using System.Data;
using Administration.Services.Interfaces;
using BaseBusiness.util;
using Microsoft.Data.SqlClient;

namespace Administration.Services.Implements
{
    public class PackageService : IPackageService
    {
        public Task<DataTable> RateCategoryTypeData(string? strPackageCode)
        {
            throw new NotImplementedException();
        }

        public async Task<DataTable> PackageDataByID(int? PackageDataByID)
        {
            throw new NotImplementedException();
        }
    }
}

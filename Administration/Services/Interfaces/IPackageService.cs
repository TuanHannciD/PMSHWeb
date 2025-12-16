using System.Data;

namespace Administration.Services.Interfaces
{
    public interface IPackageService
    {
        Task<DataTable> RateCategoryTypeData(string? strPackageCode);
        Task<DataTable> PackageDataByID(int? PackageID);

    }
}

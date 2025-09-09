using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashiering.Services.Interfaces
{
    public interface IAccountingService
    {
        /// <summary>
        /// DatVP: Lây danh sách  account receivable
        /// </summary>


        /// <returns>Data table chứa danh sách account receivable</returns>
        DataTable AccountSearch(string accountName,string accountNo,int accountType,string balance);
    }
}

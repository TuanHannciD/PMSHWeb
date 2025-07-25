using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.util;
using FrontDesk.Services.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;

namespace FrontDesk.Services.Implements
{
    public class FrontDeskService : IFrontDeskService
    {

        public DataTable TelephoneBook(string categoryId, string categoryCode, string telephoneCode)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Action", false),

                new SqlParameter("@TelephoneBookCategoryID", categoryId ?? ""),
                new SqlParameter("@TelephoneBookCategoryCode", categoryCode ?? ""),
                new SqlParameter("@TelephoneBookCode", telephoneCode ?? "")
            };

            return DataTableHelper.getTableData("spTelephoneBookSearch", param);
        }


    }
}

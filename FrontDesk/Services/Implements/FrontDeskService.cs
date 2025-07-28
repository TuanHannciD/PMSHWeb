using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.Model;
using BaseBusiness.util;
using FrontDesk.Services.Interfaces;
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

        public DataTable GetTelephoneBookByCategory(string categoryId, string searchTerm)
        {
            string sql = @"SELECT ID, Name, Telephone, Address, Remark, Color 
                   FROM TelephoneBook 
                   WHERE ID > 0";

            if (!string.IsNullOrEmpty(categoryId) && categoryId != "25")
                sql += $" AND TelephoneBookCategoryID = {categoryId}";

            if (!string.IsNullOrEmpty(searchTerm))
                sql += $" AND Telephone LIKE N'%{searchTerm}%'";

            sql += " ORDER BY Name";
            Console.WriteLine("Final SQL passed to SP:");
            Console.WriteLine(sql);

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@sqlCommand", sql)
            };

            return DataTableHelper.getTableData("spSearchAllForTrans", param);
        }



    }
}

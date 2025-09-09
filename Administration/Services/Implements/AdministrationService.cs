using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Administration.Services.Interfaces;
using BaseBusiness.util;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Administration.Services.Implements
{
    public class AdministrationService : IAdministrationService
    {
        public DataTable MemberList(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmMemberTypeSearch", param);
            return myTable;
        }
        public DataTable MemberCategory(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmMemberCategorySearch", param);
            return myTable;
        }
        public DataTable City(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmCitySearch", param);
            return myTable;
        }
        public DataTable Country(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmCountrySearch", param);
            return myTable;
        }
        public DataTable Language(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmLanguageSearch", param);
            return myTable;
        }
        public DataTable Nationality(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmNationalitySearch", param);
            return myTable;
        }
        public DataTable Title(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmTitleSearch", param);
            return myTable;
        }
        public DataTable Territory(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmTerritorySearch", param);
            return myTable;
        }
        public DataTable State(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmStateSearch", param);
            return myTable;
        }
        public DataTable VIP(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmVIPSearch", param);
            return myTable;
        }
        public DataTable Market(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmMarketSearch", param);
            return myTable;
        }
    }
}

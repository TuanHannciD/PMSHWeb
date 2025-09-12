using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Administration.Services.Interfaces;
using BaseBusiness.util;
using Microsoft.Data.SqlClient;
using static DevExpress.DataProcessing.InMemoryDataProcessor.AddSurrogateOperationAlgorithm;

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
        public DataTable MarketType(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmMarketTypeSearch", param);
            return myTable;
        }
        public DataTable PickupDropPlace(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmPickupDropPlaceSearch", param);
            return myTable;
        }
        public DataTable TransportType(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmTransportTypeSearch", param);
            return myTable;
        }
        public DataTable Reason(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmReasonSearch", param);
            return myTable;
        }
        public DataTable Origin(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmOriginSearch", param);
            return myTable;
        }
        public DataTable Source(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmSourceSearch", param);
            return myTable;
        }
        public DataTable AlertsSetup(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmAlertsSetupSearch", param);
            return myTable;
        }
        public DataTable Comment(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmCommentSearch", param);
            return myTable;
        }
        public DataTable CommentType(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmCommentTypeSearch", param);
            return myTable;
        }
        public DataTable Season(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmSeasonSearch", param);
            return myTable;
        }
        public DataTable Zone(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmZoneSearch", param);
            return myTable;
        }
        public DataTable Department(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmDepartmentSearch", param);
            return myTable;
        }
        public DataTable Owner(string code, string name, int inactive)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Name", name ?? ""),
                new SqlParameter("@Inactive", inactive)
            };

            DataTable myTable = DataTableHelper.getTableData("spFrmOwnerSearch", param);
            return myTable;
        }
        public DataTable PropertyType(string code, string description, int sequence)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Code", code ?? ""),
                new SqlParameter("@Description", description ?? ""),
                new SqlParameter("@Sequence", sequence)
            };

            DataTable myTable = DataTableHelper.getTableData("spPropertyTypeSearch", param);
            return myTable;
        }
        public DataTable ReservationType()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@sqlCommand",
                    @"SELECT Code,
                             Name,
                             CASE WHEN Deduct = 1 THEN 'x' ELSE '' END AS Deduct,
                             CASE WHEN ArrivalTimeRequired = 1 THEN 'x' ELSE '' END AS ArrivalTimeRequired,
                             CASE WHEN CreditCardRequired = 1 THEN 'x' ELSE '' END AS CreditCardRequired,
                             CASE WHEN DepositRequired = 1 THEN 'x' ELSE '' END AS DepositRequired,
                             CASE WHEN Inactive = 1 THEN 'x' ELSE '' END AS Inactive,
                             Sequence,
                             ID
                      FROM ReservationType WITH (NOLOCK)
                      ORDER BY Sequence ASC")
                    };

                    DataTable myTable = DataTableHelper.getTableData("spSearchAllForTrans", param);
                    return myTable;
                }
        public DataTable Currency()
        {
            SqlParameter[] param = new SqlParameter[]
            {
        new SqlParameter("@sqlCommand",
            @"SELECT a.ID,
                     (CASE a.MasterStatus WHEN 0 THEN '' WHEN 1 THEN 'X' END) AS IsMaster,
                     (CASE a.Inactive WHEN 0 THEN '' WHEN 1 THEN 'X' END) AS Inactive,
                     (b.Code + ' - ' + b.Description) AS Trans,
                     a.Description
              FROM Currency a
              LEFT JOIN Transactions b ON a.TransactionCode = b.Code
              WHERE 1 = 1
              ORDER BY a.ID DESC")
            };

            DataTable myTable = DataTableHelper.getTableData("spSearchAllForTrans", param);
            return myTable;
        }


    }
}

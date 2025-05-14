using BaseBusiness.bc;
using BaseBusiness.Facade;
using BaseBusiness.util;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class ProfileBO : BaseBO
    {
        private ProfileFacade facade = ProfileFacade.Instance;
        protected static ProfileBO instance = new ProfileBO();

        protected ProfileBO()
        {
            this.baseFacade = facade;
        }

        public static ProfileBO Instance
        {
            get { return instance; }
        }
        public static DataTable GetAllProfile(string code, string account, string firstName, string keyWord, string city, int type, bool showSaleInCharge)
        {
            if (string.IsNullOrEmpty(code))
            {
                code = "";
            }
            if (string.IsNullOrEmpty(code))
            {
                account = "";
            }
            if (string.IsNullOrEmpty(code))
            {
                firstName = "";
            }
            if (string.IsNullOrEmpty(code))
            {
                keyWord = "";
            }
            if (string.IsNullOrEmpty(code))
            {
                city = "";
            }

            string typeS = "";
            if (type == 0)
            {
                typeS = "1";
            }
            //Company
            else if (type == 1)
            {
                typeS = "2";
            }
            //Source
            else if (type == 2)
            {
                typeS = "3";
            }
            //Individual
            else if (type == 3)
            {
                typeS = "0";
            }
            //Group
            else if (type == 4)
            {
                typeS = "4";
            }
            //Contact
            else if (type == 5)
            {
                typeS = "5";
            }
            //All
            else if (type == 6)
            {
                typeS = "";
            }
            string _saleincharge = "";
            if (showSaleInCharge == true)
                _saleincharge = "true";
            SqlParameter[] param = new SqlParameter[]
                         {
                    new SqlParameter("@Code", code),
                    new SqlParameter("@Account", account),
                    new SqlParameter("@FirstName", firstName),
                    new SqlParameter("@Keyword", keyWord),
                    new SqlParameter("@City", city),
                    new SqlParameter("@Type", typeS),
                    new SqlParameter("@ShowSaleInCharge", _saleincharge),
            };
            DataTable myTable = DataTableHelper.getTableData("spProfileSearch_ALL", param);
            return myTable;
        }
    }
}

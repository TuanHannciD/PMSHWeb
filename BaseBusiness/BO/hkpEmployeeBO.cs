using BaseBusiness.bc;
using BaseBusiness.Facade;
using BaseBusiness.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class hkpEmployeeBO : BaseBO
    {
        private hkpEmployeeFacade facade = hkpEmployeeFacade.Instance;
        protected static hkpEmployeeBO instance = new hkpEmployeeBO();

        protected hkpEmployeeBO()
        {
            this.baseFacade = facade;
        }

        public static hkpEmployeeBO Instance
        {
            get { return instance; }
        }

        public static List<hkpEmployeeModel> GetEmployee()
        {
            string query = "Select * from hkpEmployee where ((ID like N'%') or (Name like N'%')) AND Inactive = 0 Order by ID";

          

            return instance.GetList<hkpEmployeeModel>(query);
        }
    }
}

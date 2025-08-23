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
    public class hkpAttendantBO : BaseBO
    {
        private hkpAttendantFacade facade = hkpAttendantFacade.Instance;
        protected static hkpAttendantBO instance = new hkpAttendantBO();

        protected hkpAttendantBO()
        {
            this.baseFacade = facade;
        }

        public static hkpAttendantBO Instance
        {
            get { return instance; }
        }
        public static List<hkpAttendantModel> GethkpAttendantbySect(string _ListSection)
        {


            string query = $@"SELECT ID FROM dbo.hkpAttendant WITH (NOLOCK) WHERE SectionID IN ('" + _ListSection + "') ";

            return instance.GetList<hkpAttendantModel>(query);
        }
        public static List<hkpAttendantModel> GethkpAttendantProcessSource(int  _secID,string _ListAttendant)
        {


            string query = $@"SELECT DISTINCT ID FROM dbo.hkpAttendant WITH (NOLOCK) WHERE SectionID = " + _secID + " AND ID IN ('" + _ListAttendant + "') ";

            return instance.GetList<hkpAttendantModel>(query);
        }
    }
}

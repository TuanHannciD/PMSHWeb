using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class NationalityBO: BaseBO
    {
        private NationalityFacade facade = NationalityFacade.Instance;
        protected static NationalityBO instance = new NationalityBO();

        protected NationalityBO()
        {
            this.baseFacade = facade;
        }

        public static NationalityBO Instance
        {
            get { return instance; }
        }
    }
}

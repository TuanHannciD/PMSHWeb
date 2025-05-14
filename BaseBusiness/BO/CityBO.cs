using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class CityBO : BaseBO
    {
        private CityFacade facade = CityFacade.Instance;
        protected static CityBO instance = new CityBO();

        protected CityBO()
        {
            this.baseFacade = facade;
        }

        public static CityBO Instance
        {
            get { return instance; }
        }
    }
}

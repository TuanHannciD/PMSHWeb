using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class TransportTypeBO : BaseBO
    {
        private TransportTypeFacade facade = TransportTypeFacade.Instance;
        protected static TransportTypeBO instance = new TransportTypeBO();

        protected TransportTypeBO()
        {
            this.baseFacade = facade;
        }

        public static TransportTypeBO Instance
        {
            get { return instance; }
        }
    }
}

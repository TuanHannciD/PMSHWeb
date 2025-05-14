using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class DepartmentBO : BaseBO
    {
        private DepartmentFacade facade = DepartmentFacade.Instance;
        protected static DepartmentBO instance = new DepartmentBO();

        protected DepartmentBO()
        {
            this.baseFacade = facade;
        }

        public static DepartmentBO Instance
        {
            get { return instance; }
        }
    }
}

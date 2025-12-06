using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.bc;
using BaseBusiness.Facade;
using BaseBusiness.Model;
using Microsoft.Data.SqlClient;

namespace BaseBusiness.BO
{
    using Dapper;

    public class RateCategoryBO : BaseBO
    {
        private RateCategoryFacade facade = RateCategoryFacade.Instance;
        protected static RateCategoryBO instance = new RateCategoryBO();


        protected RateCategoryBO()
        {
            this.baseFacade = facade;
        }

        public static RateCategoryBO Instance
        {
            get { return instance; }
        }

        public long InsertRateCategory(RateCategoryModel model)
        {
            return facade.Insert(model);
        }
        public long UpdateRateCategory(RateCategoryModel model)
        {
            return facade.Update(model);
        }
    }
}

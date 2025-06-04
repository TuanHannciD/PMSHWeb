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
    public class TransactionsBO: BaseBO
    {
        private TransactionsFacade facade = TransactionsFacade.Instance;
        protected static TransactionsBO instance = new TransactionsBO();

        protected TransactionsBO()
        {
            this.baseFacade = facade;
        }

        public static TransactionsBO Instance
        {
            get { return instance; }
        }
    }
}

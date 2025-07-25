using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace FrontDesk.Services.Interfaces
{
    public interface IFrontDeskService
    {

        public DataTable TelephoneBook(string categoryId, string categoryCode, string telephoneCode);
    }
}

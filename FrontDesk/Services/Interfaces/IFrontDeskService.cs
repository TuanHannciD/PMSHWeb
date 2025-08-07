using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.Model;

namespace FrontDesk.Services.Interfaces
{

    public interface IFrontDeskService
    {


        public DataTable TelephoneBook(string categoryId, string categoryCode, string telephoneCode);
        public DataTable GetTelephoneBookByCategory(string categoryId, string searchTerm);
        public DataTable TelephoneSwitch(string roomNo, int foStatus);
        public DataTable DialingInformation(DateTime fromDate, DateTime toDate, string phoneNo, int view);
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.Model;

namespace RoomManagement.Services.Interfaces
{
    public interface IRoomManagementService
    {
        DataTable Discrepancy(int sleep, int skip, int person, string floor, string room, string zone, string roomClass);
        DataTable ItemDailyInventory(
                int groupId,
                DateTime firstDate,
                DateTime secondDate,
                DateTime thirthDate,
                DateTime fourthDate,
                DateTime fifthDate,
                DateTime sixthDate,
                DateTime seventhDate,
                DateTime eighthDate,
                DateTime ninthDate,
                DateTime tenthDate,
                DateTime eleventhDate,
                DateTime twelvethDate,
                DateTime thirtheenDate,
                DateTime fourtheenDate,
                DateTime fiftheenDate);
        bool UpdateItemInventory(InventoryUpdateRequest model);
        DataTable ItemInventoryAvailable(
                int groupId,
                DateTime firstDate,
                DateTime secondDate,
                DateTime thirthDate,
                DateTime fourthDate,
                DateTime fifthDate,
                DateTime sixthDate,
                DateTime seventhDate,
                DateTime eighthDate,
                DateTime ninthDate,
                DateTime tenthDate,
                DateTime elevenDate,
                DateTime twelveDate,
                DateTime thirtheenDate,
                DateTime fourtheenDate,
                DateTime fiftheenDate);
        DataTable ItemResvDetail(int itemID, DateTime day);


    }
    }

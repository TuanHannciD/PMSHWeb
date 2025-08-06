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
    public class RoomBO : BaseBO
    {
        private RoomFacade facade = RoomFacade.Instance;
        protected static RoomBO instance = new RoomBO();

        protected RoomBO()
        {
            this.baseFacade = facade;
        }

        public static RoomBO Instance
        {
            get { return instance; }
        }
        public static List<RoomModel> GetRoomZone(string  roomtype, string zone)
        {
            int roomtypeInt = int.TryParse(roomtype, out var temp) ? temp : 0;
            // Chuyển zone thành chuỗi "1,2,3" => "'1','2','3'" (nếu là chuỗi)
            var zoneList = zone.Split(',').Select(z => $"'{z.Trim()}'");
            var zoneInClause = string.Join(",", zoneList);

            string query = $@"
        SELECT count(ID) as zone
        FROM Room WITH(NOLOCK) 
        WHERE ({roomtypeInt} = 0 OR RoomTypeID = {roomtypeInt})
          AND (ZoneID IN ({zoneInClause}) OR '{zone}' = '0')  AND HKStatusID = 7";

            return instance.GetList<RoomModel>(query);
        }
        public static List<RoomModel> GetRoomCountPlan()
        {
          
            string query = $@"Select * from Room WITH (NOLOCK) where RoomTypeCode<>'XXX'";

            return instance.GetList<RoomModel>(query);
        }
        public static List<RoomModel> GetFloorPlan(string block, string suffix, string name)
        {
            string condition = "";

            if (suffix == "-A")
            {
                condition = "CONVERT(Int, RoomNo) < 40000";
            }
            else
            {
                condition = "CONVERT(Int, RoomNo) > 39999";
            }

            string query = $@"
        SELECT * 
        FROM Room 
        WHERE {condition}
          AND RoomTypeID != 8
          AND BlockID = N'{block}'
          AND floor + '{suffix}' = N'{name}'
        ORDER BY CONVERT(Int, RoomNo)
    ";

            return instance.GetList<RoomModel>(query);
        }



    }
}

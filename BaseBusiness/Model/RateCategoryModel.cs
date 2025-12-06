using BaseBusiness.bc;

namespace BaseBusiness.Model
{
    public class RateCategoryModel : BaseModel
    {
        public int ID { get; set; }                     // IDENTITY, NOT NULL
        public string Code { get; set; }                // nvarchar(20), NOT NULL
        public string Name { get; set; }                // nvarchar(100), NULL
        public string Description { get; set; }         // nvarchar(150), NULL
        public int ParentID { get; set; }              // int, NULL
        public int UserInsertID { get; set; }          // int, NULL
        public DateTime CreateDate { get; set; }       // datetime, NULL
        public int UserUpdateID { get; set; }          // int, NULL
        public DateTime UpdateDate { get; set; }       // datetime, NULL
        public bool Inactive { get; set; }             // bit, NULL
        public string CreatedBy { get; set; }           // nvarchar(50), NULL
        public DateTime CreatedDate { get; set; }      // datetime, NULL
        public string UpdatedBy { get; set; }           // nvarchar(50), NULL
        public DateTime UpdatedDate { get; set; }      // datetime, NULL

        // constructor để set giá trị mặc định giống SQL

    }
}

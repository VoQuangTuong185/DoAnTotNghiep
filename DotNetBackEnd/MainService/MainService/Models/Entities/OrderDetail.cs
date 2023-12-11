using WebAppAPI.Models.Bases;

namespace WebAppAPI.Models.Entities
{
    namespace WebAppAPI.Models.Entities
    {
        public class OrderDetail :DbEntity
        {
            public int ProductId { get; set; }
            public Double Price { get; set; }
            public int Discount { get; set; }
            public int OrderId { get; set; }
            public int Quantity { get; set; }
            public virtual Product product { get; set; }
            public virtual Order orders { get; set; }
        }
    }
}

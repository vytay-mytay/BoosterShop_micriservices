using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop.Domain.Entities
{
    public class OrderProduct : IEntity
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        #endregion

        #region Navigation properties

        [ForeignKey(nameof(OrderId))]
        [InverseProperty("OrderProducts")]
        public virtual Order Order { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("OrderProducts")]
        public virtual Product Product { get; set; }

        #endregion
    }
}

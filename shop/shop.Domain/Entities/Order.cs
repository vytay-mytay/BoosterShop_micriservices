using shop.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop.Domain.Entities
{
    public class Order : IEntity
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public double SumPrice { get; set; }

        #endregion

        #region Navigation properties

        [InverseProperty("Order")]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

        #endregion

        #region Ctor

        public Order()
        {
            OrderProducts = OrderProducts.Empty();
        }

        #endregion
    }
}

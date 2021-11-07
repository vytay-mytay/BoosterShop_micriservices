using shop.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop.Domain.Entities
{
    public class Product : IEntity
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        #endregion

        #region Navigation properties

        [InverseProperty("Product")]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

        #endregion

        #region Ctor

        public Product()
        {
            OrderProducts = OrderProducts.Empty();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.Entities
{
    public class PromoCode
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public double DiscountPercentage { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsValid { get; set; }

        public bool IsExpired() => DateTime.Now > ExpiryDate;
    }
}

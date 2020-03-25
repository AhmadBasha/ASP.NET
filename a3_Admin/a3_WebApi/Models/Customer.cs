using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace a3_WebApi.Models
{
    public class Customer
    {
        
        [Key, Required]
        public int CustomerID { get; set; }

        [Required , StringLength(50)]
        public string  CustomerName { get; set; }

        [RegularExpression(@"[0-9]{11}$", ErrorMessage = "Not a valid TFN")]
        [StringLength(11)]
        public string TFN { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(40)]
        public string City  { get; set; }

        // DOUBT : 3 LETTER or 20
        [RegularExpression(@"VIC|NSW|WA|ACT$", ErrorMessage = "Not a valid State")]
        [StringLength(3)]
        public string State { get; set; }

        [RegularExpression(@"[0-9]{4}$", ErrorMessage = "Not a valid Post Code")]
        [StringLength(4)]
        public string PostCode { get; set; }

        [Required , StringLength(15)]
        [RegularExpression(@"\(61\)- [0-9]{4} [0-9]{4}$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }

        //public virtual Login Login { get; set; }

        public virtual List<BaseAccount> Accounts { get; set; }

        public Customer()
        {
        }
    }
}

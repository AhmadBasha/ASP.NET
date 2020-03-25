using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a2_s3644668_s3643929.Models
{
    public class Payee
    {

        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayeeID { get; set; }

        [Required, StringLength(50)]
        public string PayeeName { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        // DOUBT : 3 LETTER or 20
        [RegularExpression(@"VIC|NSW|WA|ACT$", ErrorMessage = "Not a valid State")]
        [StringLength(3)]
        public string State { get; set; }

        [RegularExpression(@"[0-9]{4}$", ErrorMessage = "Not a valid Post Code")]
        [StringLength(4)]
        public string PostCode { get; set; }

        [Required, StringLength(15)]
        [RegularExpression(@"\(61\)- [0-9]{4} [0-9]{4}$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }

        [DataType(DataType.Currency)]
        public double Balance { get; set; }

        public virtual List<BillPay> BillPays { get; set; }

        public Payee()
        {
        }
    }
}

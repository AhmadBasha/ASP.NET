using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Primitives;

namespace a3_WebApi.Models
{
    public enum AccountType
    {
        Checking = 'C',
        Saving = 'S'
    }

    public class BaseAccount
    {

        [Key, Range(1000, 9999), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountNumber { get; set; }

        [Required, StringLength(1)]
        public AccountType AccountType { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        [Required , DataType(DataType.Date)]
        public DateTime ModifyDate { get; set; }

        //[Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public double Balance { get; set; }

        public virtual List<Transaction> Transactions { get; set; }

        public virtual List<BillPay> BillPays { get; set; }

        [Required]
        public int transactCount { get; set; }



        public BaseAccount()
        { }

    }
}

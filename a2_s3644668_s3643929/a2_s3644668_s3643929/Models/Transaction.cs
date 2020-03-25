using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a2_s3644668_s3643929.Models
{

    public enum TransactionType
    {
        Deposit = 'D',
        Withdraw = 'W',
        Transfer = 'T',
        Service = 'S',
        BillPay = 'B'
    }

    public class Transaction
    {


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        [Required, StringLength(1)]
        public TransactionType TransactionType { get; set; }

        [Required]
        public int AccountNumber { get; set; }

        [ForeignKey("AccountNumber")]
        public virtual BaseAccount SourceAccount { get; set; }

        public int? DestAccount { get; set; }

        [ForeignKey("DestAccount")]
        public virtual BaseAccount DestAccountObj { get; set; }

        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }


        [DataType(DataType.Date)]
        public DateTime ModifyDate { get; set; }


        public Transaction()
        {
        }
    }
}

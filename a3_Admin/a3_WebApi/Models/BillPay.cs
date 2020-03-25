using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a3_WebApi.Models
{
    public enum PeriodType
    {
        Monthly = 'M',
        Quarterly = 'Q',
        Annually = 'Y',
        Once = 'S'
    }

    public class BillPay
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BillPayID { get; set; }

        [Required]
        public int AccountNumber { get; set; }

        [ForeignKey("AccountNumber")]
        public virtual BaseAccount Account { get; set; }

        [Required, DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Required]
        public int PayeeID { get; set; }

        [ForeignKey("PayeeID")]
        public virtual Payee Payee { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime ScheduleDate { get; set; }

        [Required, StringLength(1)]
        public PeriodType Period { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime ModifyDate { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public bool Blocked { get; set; }


        public BillPay()
        {
        }
    }
}

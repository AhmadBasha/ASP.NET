using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using SimpleHashing;

namespace a3_Client.Models
{
    public class Login
    {

        [Key ,Required]
        public int UserID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        // virtual is just can be loaded from the memory 
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime ModifyDate { get; set; }

        [Range(0,3)]
        public int? FailedCtr { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LockoutEnd { get; set; }

    
    }
}

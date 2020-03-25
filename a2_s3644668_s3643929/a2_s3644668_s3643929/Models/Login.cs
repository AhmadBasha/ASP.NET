using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SimpleHashing;

namespace a2_s3644668_s3643929.Models
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

        private void CheckLogin(string plainPass)
        {
            if (FailedCtr == null)
                FailedCtr = 0;

            if (!PBKDF2.Verify(Password, plainPass))
            {
                ++FailedCtr;
                if (FailedCtr == 3)
                {
                    //LockoutEnd = new DateTimeOffset(DateTime.UtcNow, TimeSpan.FromMinutes(1.0)).da;
                    LockoutEnd = DateTime.UtcNow.AddMinutes(1);
                    throw new Exception("Three Failed Attempts! ACCOUNT NOW LOCKED");
                }
                else
                {
                    throw new Exception(FailedCtr + " failed attempt of 3 allowed.");
                }
            }
            else
            {
                FailedCtr = 0;
            }
            
        }

        //A3
        public void AttemptLogin(string plainPass)
        {

            if( LockoutEnd == null )
            {
                CheckLogin(plainPass);
            }
            else
            {
                if ( DateTime.UtcNow > LockoutEnd )
                {
                    FailedCtr = 0;
                    LockoutEnd = null;
                    CheckLogin(plainPass);
                }
                else
                {
                    throw new Exception("Come back after 1 minute");
                }
            }
        }

        internal void ChangePassword(string oldPass, string newPass)
        {
            if(PBKDF2.Verify(Password, oldPass))
            {
                Password = PBKDF2.Hash(newPass);
            }
            else
            {
                throw new Exception("Wrong password entered!");
            }
        }
    }
}

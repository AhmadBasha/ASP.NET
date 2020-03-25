using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using a2_s3644668_s3643929.Exceptions;
using Microsoft.Extensions.Primitives;

namespace a2_s3644668_s3643929.Models
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

        private readonly double WITHDRAW_CHARGE = 0.10;
        private readonly double TRANSFER_CHARGE = 0.20;

        private readonly int NUM_FREE_TRANSACT = 4;

        private readonly double LOWEST_BALANCE_C = 200.00;
        private readonly double LOWEST_BALANCE_S = 00.00;
        private readonly double OPENING_BALANCE_C = 500.00;
        private readonly double OPENING_BALANCE_S = 100.00;


        public BaseAccount()
        { }

        // when testing account creation
        public BaseAccount(double openBalance)
        {
            if (AccountType == AccountType.Checking)
            {
                if (openBalance < OPENING_BALANCE_C)
                {
                    throw new OpeningBalenceException(OPENING_BALANCE_C);
                }
            }
            else
            {
                if (openBalance < OPENING_BALANCE_S)
                {
                    throw new OpeningBalenceException(OPENING_BALANCE_S);
                }
            }
        }

        public void Deposit(double amount, string comment)
        {
            decimal value = Convert.ToDecimal(amount);

            if (amount <= 0)
                throw new Exception("Amount must be positive.");

            if (decimal.Round(value, 2) != value)
                throw new Exception("Amount cannot have more than 2 decimal places.");

            Balance += amount;

            Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    Amount = amount,
                    ModifyDate = DateTime.UtcNow,
                    Comment = comment
                }) ;

        }

        internal void BillPay(double amount, Payee payee, string comment)
        {
            //decimal value = Convert.ToDecimal(amount);

            if (payee == null)
                throw new Exception("Invalid deposit account number!");

            //if (amount <= 0)
            //    throw new Exception("Amount must be positive.");

            //if (decimal.Round(value, 2) != value)
            //    throw new Exception("Amount cannot have more than 2 decimal places.");

            if (Balance - amount < 0)
                throw new Exception("EXCEPT: ACCOUNT BALANCE!!");

            Balance -= amount;

            payee.Balance += amount;

            var nowTime = DateTime.UtcNow;

            Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.BillPay,
                    Amount = amount,
                    ModifyDate = nowTime,
                    Comment = comment
                });
        }

        public void Withdraw(double amount, StringValues comment)
        {
            decimal value = Convert.ToDecimal(amount);

            if (amount <= 0)
                throw new Exception("Amount must be positive.");

            if (decimal.Round(value, 2) != value)
                throw new Exception("Amount cannot have more than 2 decimal places.");

            if (AccountType == AccountType.Checking)
            {
                if (Balance - (amount + (transactCount > NUM_FREE_TRANSACT ? WITHDRAW_CHARGE : 0.0)) < LOWEST_BALANCE_C)
                {
                    throw new LowBalenceException(LOWEST_BALANCE_C);
                }
            }
            else
            {
                if (Balance - (amount + (transactCount > NUM_FREE_TRANSACT ? WITHDRAW_CHARGE : 0.0)) < LOWEST_BALANCE_S)
                {
                    throw new LowBalenceException(LOWEST_BALANCE_S);
                }
            }
            
            Balance -= amount;

            transactCount++;
            // comparing to check the charge 

            if (transactCount > NUM_FREE_TRANSACT)
            {
                serviceCharge(WITHDRAW_CHARGE);
            }

            Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    Amount = amount,
                    ModifyDate = DateTime.UtcNow,
                    Comment = comment
                });

        }

        internal void Transfer(double amount, BaseAccount depositAcc, StringValues comment)
        {
            decimal value = Convert.ToDecimal(amount);

            if (depositAcc == null)
                throw new Exception("Invalid deposit account number!");

            if (depositAcc.AccountNumber == this.AccountNumber)
                throw new Exception("You Can't transfer to the same account!");

            if (amount <= 0)
                throw new Exception("Amount must be positive.");

            if (decimal.Round(value, 2) != value)
                throw new Exception("Amount cannot have more than 2 decimal places.");

            if (AccountType == AccountType.Checking)
            {
                if (Balance - (amount + (transactCount > NUM_FREE_TRANSACT ? TRANSFER_CHARGE : 0.0)) < LOWEST_BALANCE_C)
                {
                    throw new LowBalenceException(LOWEST_BALANCE_C);
                }
            }
            else
            {
                if (Balance - (amount + (transactCount > NUM_FREE_TRANSACT ? TRANSFER_CHARGE : 0.0)) < LOWEST_BALANCE_S)
                {
                    throw new LowBalenceException(LOWEST_BALANCE_S);
                }
            }

            Balance -= amount;

            depositAcc.Balance += amount;

            var nowTime = DateTime.UtcNow;

            transactCount++;

            // comparing to check the charge 
            if (transactCount > NUM_FREE_TRANSACT)
            {
                serviceCharge(TRANSFER_CHARGE);
            }

            Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Transfer,
                    Amount = amount,
                    ModifyDate = nowTime,
                    Comment = comment,
                    DestAccount = depositAcc.AccountNumber,
                });

            depositAcc.Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    Amount = amount,
                    ModifyDate = nowTime,
                    Comment = comment,
                    DestAccount = this.AccountNumber
                });
        }

        public void serviceCharge(double amount)
        {
            Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Service,
                    Amount = amount,
                    ModifyDate = DateTime.UtcNow,
                    Comment = "Service Charge"
                });

            Balance -= amount;
        }
    }
}

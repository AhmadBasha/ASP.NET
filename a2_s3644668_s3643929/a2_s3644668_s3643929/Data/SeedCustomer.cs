using System;
using a2_s3644668_s3643929.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace a2_s3644668_s3643929.Data
{
    public static class SeedCustomer
    {

        public static void Seed_Customer(this ModelBuilder builder)
        {

            builder.Entity<Customer>().HasData
            (
                new Customer
                {
                    CustomerID = 2100,
                    CustomerName = "Matthew Bolger",
                    Address = "123 Fake Street",
                    City = "Melbourne",
                    PostCode = "3000",
                    Phone = "(61)- 1234 5678"
                },
                new Customer
                {
                    CustomerID = 2200,
                    CustomerName = "Rodney Cocker",
                    Address = "456 Real Road",
                    City = "Melbourne",
                    PostCode = "3005",
                    Phone = "(61)- 1234 5679"
                },
                new Customer
                {
                    CustomerID = 2300,
                    CustomerName = "Shekhar Kalra",
                    Phone = "(61)- 1234 5680"
                }
            ) ;

            builder.Entity<BaseAccount>().HasData
            (
                new BaseAccount
                {
                    AccountNumber = 4100,
                    AccountType = AccountType.Saving,
                    CustomerID = 2100,
                    Balance = 100.00,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", "dd/MM/yyyy hh:mm:ss tt", null),
                    transactCount = 0
                },

                new BaseAccount
                {
                    AccountNumber = 4101,
                    AccountType = AccountType.Checking,
                    CustomerID = 2100,
                    Balance = 500.00,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:30:00 PM", "dd/MM/yyyy hh:mm:ss tt", null),
                    transactCount = 0
                },

                new BaseAccount
                {
                    AccountNumber = 4200,
                    AccountType = AccountType.Saving,
                    CustomerID = 2200,
                    Balance = 500.95,
                    ModifyDate = DateTime.ParseExact("19/12/2019 09:00:00 PM", "dd/MM/yyyy hh:mm:ss tt", null),
                    transactCount = 0
                },

                new BaseAccount
                {
                    AccountNumber = 4300,
                    AccountType = AccountType.Checking,
                    CustomerID = 2300,
                    Balance = 1250.50,
                    ModifyDate = DateTime.ParseExact("19/12/2019 10:00:00 PM", "dd/MM/yyyy hh:mm:ss tt", null),
                    transactCount = 0
                }
            );

            builder.Entity<Transaction>().HasData
            (
                new Transaction
                {
                    TransactionID = 1,
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 100.00,
                    Comment = "Acc Creation",
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", "dd/MM/yyyy hh:mm:ss tt", null)
                },
                new Transaction
                {
                    TransactionID = 2,
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4101,
                    Amount = 500.00,
                    Comment = "Acc Creation",
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:30:00 PM", "dd/MM/yyyy hh:mm:ss tt", null)
                },
                new Transaction
                {
                    TransactionID = 3,
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4200,
                    Amount = 500.95,
                    Comment = "Acc Creation",
                    ModifyDate = DateTime.ParseExact("19/12/2019 09:00:00 PM", "dd/MM/yyyy hh:mm:ss tt", null)
                },
                new Transaction
                {
                    TransactionID = 4,
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4300,
                    Amount = 1250.50,
                    Comment = "Acc Creation",
                    ModifyDate = DateTime.ParseExact("19/12/2019 10:00:00 PM", "dd/MM/yyyy hh:mm:ss tt", null)
                }
            ) ;
        }
    }
}

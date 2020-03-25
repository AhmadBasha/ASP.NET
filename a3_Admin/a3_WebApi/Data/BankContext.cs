using System;
using a3_WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace a3_WebApi.Data
{
    public class BankContext : DbContext // here for options 
    {
        //bool loadSeed = true;
        // base constructor 
        public BankContext(DbContextOptions<BankContext> options)
            : base(options)
        {

        }

        // here we need to change the name to Login because of the seed.
        //Dbset will do all the heavy duty 
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Login> Logins { get; set; }

        public DbSet<BaseAccount> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BillPay> BillPays { get; set; }
        public DbSet<Payee> Payees { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Customer>().HasOne(c => c.Login).WithOne(l => l.Customer);



            builder.Entity<Transaction>().HasOne(a => a.SourceAccount).WithMany(t => t.Transactions);


            //if (loadSeed)
            //{
            //    loadSeed = false;
            //    builder.Seed_Customer();
            //    builder.Seed_Login();

            //}

            //else
            //{
            //    return;
            //}
        }
    }
}

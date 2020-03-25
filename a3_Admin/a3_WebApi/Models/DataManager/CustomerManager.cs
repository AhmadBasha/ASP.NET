using System;
using System.Collections.Generic;
using System.Linq;
using a3_WebApi.Data;
using a3_WebApi.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace a3_WebApi.Models.DataManager
{
    public class CustomerManager : IDataRepository<Customer, int>
    {
        private readonly BankContext _context;

        public CustomerManager(BankContext context)
        {
            _context = context;
        }

        public Customer Get(int id)
        {
            var customer = _context.Customers.Find(id);

            customer.Accounts = _context.Accounts.Where(x => x.CustomerID == id).ToList();

            return customer;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();

            //return _context.Customers.Include(x => x.Accounts).ToList();
        }

        public int Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer.CustomerID;
        }

        public int Delete(int id)
        {
            _context.Customers.Remove(_context.Customers.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Customer customer)
        {
            _context.Update(customer);
            _context.SaveChanges();

            return id;
        }

        public int Lockout(int id)
        {

            var login = _context.Logins.Where(x => x.CustomerID == id).FirstOrDefault();

            login.LockoutEnd = DateTime.UtcNow.AddMinutes(1);

            _context.SaveChanges();

            return id;
        }

        public BaseAccount GetAcc(int accId)
        {
            var account = _context.Accounts.Find(accId);

            account.Transactions = _context.Transactions.Where(x => x.AccountNumber == accId).ToList();

            return account;
        }

        public BaseAccount FilterTransactions(int accId, long from, long to, string key)
        {
            var account = _context.Accounts.Find(accId);

            //all the transaction
            var aTT = _context.Transactions.Where(x => x.AccountNumber == accId).AsEnumerable();
            //long int
            aTT = aTT.Where(x => x.ModifyDate.ToBinary() > from);
            // modify lower than to 
            aTT = aTT.Where(x => x.ModifyDate.ToBinary() < to);

            // if blank or null 
            if(!string.IsNullOrEmpty(key))
            {
                aTT = aTT.Where(x => x.Comment.Contains(key));
            }

            account.Transactions = aTT.ToList();

            return account;
        }
    }
}

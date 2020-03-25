using System;
using System.Collections.Generic;
using System.Linq;
using a3_WebApi.Data;
using a3_WebApi.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace a3_WebApi.Models.DataManager
{
    public class TransactionManager : IDataRepository<Transaction, int>
    {
        private readonly BankContext _context;

        public TransactionManager(BankContext context)
        {
            _context = context;
        }

        public Transaction Get(int id)
        {
            return _context.Transactions.Find(id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions
                .ToList();

            //return _context.Customers.Include(x => x.Accounts).ToList();
        }

        public int Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return transaction.TransactionID;
        }

        public int Delete(int id)
        {
            _context.Transactions.Remove(_context.Transactions.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();

            return id;
        }
    }
}

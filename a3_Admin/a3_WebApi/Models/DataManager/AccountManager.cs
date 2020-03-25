using System;
using System.Collections.Generic;
using System.Linq;
using a3_WebApi.Data;
using a3_WebApi.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace a3_WebApi.Models.DataManager
{
    public class AccountManager : IDataRepository<BaseAccount, int>
    {
        private readonly BankContext _context;

        public AccountManager(BankContext context)
        {
            _context = context;
        }

        //public BaseAccount Get(int id)
        //{
        //    return _context.Accounts.Find(id);
        //}

        public BaseAccount Get(int id)
        {
            var account = _context.Accounts.Find(id);

            account.Transactions = _context.Transactions.Where(x => x.AccountNumber == id).ToList();

            return account;
        }

        public IEnumerable<BaseAccount> GetAll()
        {
            return _context.Accounts
                .ToList();

            //return _context.Customers.Include(x => x.Accounts).ToList();
        }

        public int Add(BaseAccount account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account.AccountNumber;
        }

        public int Delete(int id)
        {
            _context.Accounts.Remove(_context.Accounts.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, BaseAccount account)
        {
            _context.Update(account);
            _context.SaveChanges();

            return id;
        }


    }
}

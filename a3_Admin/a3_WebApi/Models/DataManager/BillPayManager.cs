using System;
using System.Collections.Generic;
using System.Linq;
using a3_WebApi.Data;
using a3_WebApi.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace a3_WebApi.Models.DataManager
{
    public class BillPayManager : IDataRepository<BillPay, int>
    {
        private readonly BankContext _context;

        public BillPayManager(BankContext context)
        {
            _context = context;
        }

        public BillPay Get(int id)
        {
            return _context.BillPays.Find(id);
        }

        public IEnumerable<BillPay> GetAll()
        {
            return _context.BillPays
                .Include(x => x.Payee)
                .Include(x => x.Account)
                    .ThenInclude(account => account.Customer)
                .ToList();

            //return _context.Customers.Include(x => x.Accounts).ToList();
        }

        public int Add(BillPay billPay)
        {
            _context.BillPays.Add(billPay);
            _context.SaveChanges();

            return billPay.BillPayID;
        }

        public int Delete(int id)
        {
            _context.BillPays.Remove(_context.BillPays.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, BillPay billPay)
        {
            _context.Update(billPay);
            _context.SaveChanges();

            return id;
        }

        public int ToggleBlock(int id)
        {

            var billPay = _context.BillPays.Find(id);

            billPay.Blocked = !billPay.Blocked;

            _context.SaveChanges();

            return id;
        }
    }
}

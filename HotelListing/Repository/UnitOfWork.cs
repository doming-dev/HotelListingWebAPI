using HotelListing.Data;
using HotelListing.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<Country> countries;

        private IGenericRepository<Hotel> hotels;

        public IGenericRepository<Country> Countries => countries ??= new GenericRepository<Country>(_context);

        public IGenericRepository<Hotel> Hotels => hotels ??= new GenericRepository<Hotel>(_context);

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}

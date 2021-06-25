using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
                new Country() { Id = 1, Name = "United States", ShortName = "US" },
                new Country() { Id = 2, Name = "Jamaica", ShortName = "HM" },
                new Country() { Id = 3, Name = "Bahamas", ShortName = "BS" },
                new Country() { Id = 4, Name = "Cayman Island", ShortName = "CI" });

            builder.Entity<Hotel>().HasData(
                new Hotel() { Id = 1, Name = "Sandals Resort and Spa", Address = "Negril", CountryId = 1, Rating = 4.5 },
                new Hotel() { Id = 2, Name = "Grand Palldium", Address = "Nassua", CountryId = 2, Rating = 4 },
                new Hotel() { Id = 3, Name = "Comfort Suites", Address = "George Town", CountryId = 3, Rating = 4.3 });
        }

    }
}

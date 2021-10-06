using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiCountries.Model
{
    public class CountriesDbContext : DbContext
    {
        public CountriesDbContext()
        {
        }
        public CountriesDbContext(DbContextOptions<CountriesDbContext> options) 
        : base(options)
        {
        }

        public virtual DbSet<Countries> countries { get; set; }

        public virtual DbSet<SubDivision> subDivision { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer("Data Source=PCM-78523Z2\\SQLEXPRESS;Initial Catalog=DBCountries;User ID=Gregor;Password=Greg576&; MultipleActiveResultSets=True;Connect Timeout=100;Encrypt=False;");
            //}
        }


    }

}



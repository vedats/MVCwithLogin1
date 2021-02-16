using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCwithLogin1.Models;

namespace MVCwithLogin1.Data
{
    public class HRDBContext : DbContext
    {
        public HRDBContext (DbContextOptions<HRDBContext> options)
            : base(options)
        {
        }

        public DbSet<MVCwithLogin1.Models.Person> Person { get; set; }
    }
}

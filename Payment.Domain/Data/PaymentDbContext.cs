using Microsoft.EntityFrameworkCore;
using Payment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<PaymentState> PaymentStates { get; set; }


    }
}

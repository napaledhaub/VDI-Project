using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using VDIProject.Model;
using VDIProject.Models;

namespace VDIProject.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<TotalTransaction> Totals { get; set; }

    }
}

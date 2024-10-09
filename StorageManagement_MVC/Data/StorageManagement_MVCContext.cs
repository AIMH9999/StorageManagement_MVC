using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorageManagement.Models;
using StorageManagement_MVC.Models;

namespace StorageManagement_MVC.Data
{
    public class StorageManagement_MVCContext : DbContext
    {
        public StorageManagement_MVCContext (DbContextOptions<StorageManagement_MVCContext> options)
            : base(options)
        {
        }

        public DbSet<StorageManagement.Models.Product> Product { get; set; } = default!;
        public DbSet<StorageManagement.Models.productInventory> productInventory { get; set; } = default!;
        public DbSet<StorageManagement.Models.Receipt> Receipt { get; set; } = default!;
        public DbSet<StorageManagement.Models.ReceiptDetail> ReceiptDetail { get; set; } = default!;
        public DbSet<StorageManagement_MVC.Models.User> User { get; set; } = default!;
    }
}

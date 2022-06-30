using Touba.WebApis.IdentityServer.DataLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Touba.WebApis.DataLayer.Models.Product;

namespace Touba.WebApis.IdentityServer.DataLayer
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        public new DbSet<User> Users { get; set; }

        public DbSet<Message> Message { get; set; }

        public DbSet<MailReceiverLogEntity> MailReceiverLogs { get; set; }

        public DbSet<ProductTest> ProductTest { get; set; }
    }
}

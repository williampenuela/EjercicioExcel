using ExcelUploadReadDataSaveExampleCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelUploadReadDataSaveExampleCore.Context
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectiontring = string.Format(@"Data Source=NIX\BIT_DESA;Initial Catalog=William;Integrated Security=True");
            options.UseSqlServer(connectiontring);
        }

        public DbSet<Student> Students { get; set; }
    }
}

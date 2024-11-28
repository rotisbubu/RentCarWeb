using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RentCarWeb.Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public override int SaveChanges()
        {
            SetCustomerAndDriverLicenseIds();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetCustomerAndDriverLicenseIds();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetCustomerAndDriverLicenseIds()
        {
            var newUsers = ChangeTracker.Entries<User>()
                .Where(e => e.State == EntityState.Added && (string.IsNullOrEmpty(e.Entity.UserId) || string.IsNullOrEmpty(e.Entity.DriverLicenseNumber)))
                .Select(e => e.Entity)
                .ToList();

            if (!newUsers.Any()) return;

            var lastCustomerId = Users.OrderByDescending(u => u.UserId).Select(u => u.UserId).FirstOrDefault();
            var lastDriverLicenseNumber = Users
                .Where(u => u.DriverLicenseNumber.StartsWith("DLN"))
                .OrderByDescending(u => int.Parse(u.DriverLicenseNumber.Substring(3)))
                .Select(u => u.DriverLicenseNumber)
                .FirstOrDefault();

            int lastCustomerNumber = 0;
            int lastLicenseNumber = 0;

            if (lastCustomerId != null && lastCustomerId.StartsWith("CUS"))
            {
                lastCustomerNumber = int.Parse(lastCustomerId.Substring(3));
            }
            if (lastDriverLicenseNumber != null && lastDriverLicenseNumber.StartsWith("DLN"))
            {
                lastLicenseNumber = int.Parse(lastDriverLicenseNumber.Substring(3));
            }

            foreach (var user in newUsers)
            {
                lastCustomerNumber++;
                lastLicenseNumber++;
                user.UserId = $"CUS{lastCustomerNumber:D3}";
                user.DriverLicenseNumber = $"DLN{lastLicenseNumber:D3}";
            }
        }

        public string GetNextCustomerId()
        {
            var lastCustomerId = Users
                .OrderByDescending(u => u.UserId)
                .Select(u => u.UserId)
                .FirstOrDefault();

            int lastNumber = 0;
            if (lastCustomerId != null && lastCustomerId.StartsWith("CUS"))
            {
                lastNumber = int.Parse(lastCustomerId.Substring(3));
            }

            return $"CUS{(lastNumber + 1):D3}";
        }

        public string GetNextDriverLicenseNumber()
        {
            var lastDriverLicenseNumber = Users
                .OrderByDescending(u => u.UserId)
                .Select(u => u.DriverLicenseNumber)
                .FirstOrDefault();

            int lastNumber = 0;
            if (lastDriverLicenseNumber != null && lastDriverLicenseNumber.StartsWith("DLN"))
            {
                lastNumber = int.Parse(lastDriverLicenseNumber.Substring(3));
            }

            return $"DLN{(lastNumber + 1):D3}";
        }

    }
}

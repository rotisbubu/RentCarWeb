using Microsoft.EntityFrameworkCore;

namespace RentCarWeb.Models
{
    public class CarHomeContext : DbContext
    {
        public CarHomeContext(DbContextOptions<CarHomeContext> options) : base(options) { }
        public DbSet<Car> Cars { get; set; }
        public DbSet<RiwayatSewa> Rental { get; set; }
        public DbSet<CarImage> Images { get; set; }

        private void SetRentalIds()
        {
            var newRentals = ChangeTracker.Entries<RiwayatSewa>()
                .Where(e => e.State == EntityState.Added && string.IsNullOrEmpty(e.Entity.RentalId))
                .Select(e => e.Entity)
                .ToList();

            if (!newRentals.Any()) return;

            var lastRentalId = Rental
                .Where(r => r.RentalId.StartsWith("RTD"))
                .OrderByDescending(r => int.Parse(r.RentalId.Substring(3)))
                .Select(r => r.RentalId)
                .FirstOrDefault();

            int lastRentalNumber = 0;

            if (lastRentalId != null && lastRentalId.StartsWith("RTD"))
            {
                lastRentalNumber = int.Parse(lastRentalId.Substring(3));
            }
            
            foreach (var rental in newRentals)
            {
                lastRentalNumber++;
                rental.RentalId = $"RTD{lastRentalNumber:D3}";
            }
        }
        public string GetNextRentalId()
        {
            var lastRentalId = Rental
        .Where(r => r.RentalId.StartsWith("RTD"))
        .AsEnumerable()
        .OrderByDescending(r => int.Parse(r.RentalId.Substring(3)))
        .Select(r => r.RentalId)
        .FirstOrDefault();

            int lastNumber = 0;
            if (lastRentalId != null && lastRentalId.StartsWith("RTD"))
            {
                lastNumber = int.Parse(lastRentalId.Substring(3));
            }
            return $"RTD{(lastNumber + 1):D3}";
        }
    }
}

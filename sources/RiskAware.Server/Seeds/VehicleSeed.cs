using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class VehicleSeed
    {
        public static void Seed(AppDbContext context)
        {
            var vehiclesToSeed = new Vehicle[]
            {
            new()
            {
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                OwnerId = Guid.Parse("12345678-1234-1234-1234-1234567890ab")
            },
            new()
            {
                Make = "Honda",
                Model = "Civic",
                Year = 2019,
                OwnerId = Guid.Parse("87654321-4321-4321-4321-ba0987654321")
            },
            new()
            {
                Make = "Ford",
                Model = "Focus",
                Year = 2021,
                OwnerId = Guid.Parse("12345678-1234-1234-1234-1234567890ab")
            }
            };

            foreach (var vehicle in vehiclesToSeed)
            {
                if (!context.Vehicles.Any(v => v.OwnerId == vehicle.OwnerId))
                {
                    context.Vehicles.Add(vehicle);
                }
            }
            context.SaveChanges();
        }
    }
}

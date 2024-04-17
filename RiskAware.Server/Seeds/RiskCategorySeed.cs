using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class RiskCategorySeed
    {
        public static void Seed(AppDbContext context)
        {
            ICollection<string> names = ["Finanční rizika", "Lidská rizika", "Operační rizika", "Legislativní rizika", "Technická rizika"];
            foreach (var name in names)
            {
                var newRiskCategory = new RiskCategory
                {
                    Name = name,
                    RiskProjectId = 1
                };
                context.RiskCategories.Add(newRiskCategory);
            }

            context.SaveChanges();
        }
    }
}

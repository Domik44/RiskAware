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
            for (int i = 1; i < 4+1; i++)
            {
                foreach (var name in names)
                {
                    var newRiskCategory = new RiskCategory
                    {
                        Name = name,
                        RiskProjectId = i
                    };
                    context.RiskCategories.Add(newRiskCategory);
                }
            }

            context.SaveChanges();
        }
    }
}

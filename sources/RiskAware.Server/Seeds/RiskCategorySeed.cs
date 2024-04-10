using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class RiskCategorySeed
    {
        public static void Seed(AppDbContext context)
        {
            var categoriesToBeAdded = new RiskCategory[]
            {
                new()
                {
                    Name = "Kategorie rizika 1",
                    Description = "Popis kategorie rizika 1",
                    RiskProjectId = 1
                },
                new()
                {
                    Name = "Kategorie rizika 2",
                    Description = "Popis kategorie rizika 2",
                    RiskProjectId = 1
                }
            };

            foreach (var category in categoriesToBeAdded)
            {
                if(!context.RiskCategories.Any(c => c.Id == category.Id))
                {
                    context.RiskCategories.Add(category);
                }
            }
            context.SaveChanges();
        }
    }
}

using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class RiskCategorySeed
    {
        public static void Seed(AppDbContext context)
        {
            var CategoriesToBeAdded = new RiskCategory[]
            {
                
            };

            foreach (var category in CategoriesToBeAdded)
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

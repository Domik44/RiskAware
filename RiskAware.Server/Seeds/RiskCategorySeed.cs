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

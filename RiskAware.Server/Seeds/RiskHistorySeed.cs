using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class RiskHistorySeed
    {
        public static void Seed(AppDbContext context)
        {
            var RiskHistoryToBeAdded = new RiskHistory[]
            {

            };

            foreach(var riskHistory in RiskHistoryToBeAdded)
            {
                if(!context.RiskHistory.Any(h => h.Id == riskHistory.Id))
                {
                    context.RiskHistory.Add(riskHistory);
                }
            }
            context.SaveChanges();
        }
    }
}

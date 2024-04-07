using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class RiskHistorySeed
    {
        public static void Seed(AppDbContext context)
        {
            var riskHistoryToBeAdded = new RiskHistory[]
            {

            };

            foreach(var riskHistory in riskHistoryToBeAdded)
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

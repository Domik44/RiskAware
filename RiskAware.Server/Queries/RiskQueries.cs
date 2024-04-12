using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.RiskDTOs;

namespace RiskAware.Server.Queries
{
    public class RiskQueries
    {
        private readonly AppDbContext _context;

        public RiskQueries(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RiskDto>> GetRiskProjectRisksAsync(int id)
        {
            var risks = await _context.Risks
                .AsNoTracking()
                .Where(r => r.RiskProjectId == id)
                .Include(r => r.RiskCategory)
                .Select(r => new RiskDto
                {
                    Id = r.Id,
                    Title = r.RiskHistory
                        .OrderByDescending(h => h.Created)
                        .Select(h => h.Title)
                        .FirstOrDefault(),
                    CategoryName = r.RiskCategory.Name,
                    Severity = r.RiskHistory
                        .OrderByDescending(h => h.Created)
                        .Select(h => h.Probability * h.Impact)
                        .FirstOrDefault(),
                    State = r.RiskHistory
                        .OrderByDescending(h => h.Created)
                        .Select(h => h.Status)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return risks;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.DTOs.RiskDTOs;

namespace RiskAware.Server.Queries
{
    public class ProjectPhaseQueries
    {
        private readonly AppDbContext _context;

        public ProjectPhaseQueries(AppDbContext context) 
        { 
            _context = context;
        }

        public async Task<IEnumerable<ProjectPhaseDto>> GetRiskProjectPhasesAsync(int id)
        {
            var projectPhases = await _context.ProjectPhases
                .AsNoTracking()
                .Where(pp => pp.RiskProjectId == id)
                .Include(pp => pp.Risks)
                .OrderBy(pp => pp.Order)
                .Select(pp => new ProjectPhaseDto
                {
                    Id = pp.Id,
                    Order = pp.Order,
                    Name = pp.Name,
                    Start = pp.Start,
                    End = pp.End,
                    Risks = pp.Risks.Select(r => new RiskUnderPhaseDto
                    {
                        Id = r.Id,
                        Title = r.RiskHistory.OrderByDescending(h => h.Created).FirstOrDefault().Title, // TODO -> mby add Title to Risk entity, in this case the redudancy will be minimal and it will be easier to get the title
                    })

                })
                .ToListAsync();

            return projectPhases;
        }
    }
}

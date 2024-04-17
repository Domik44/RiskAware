using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.DTOs.UserDTOs;

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

        public async Task<ProjectPhaseDetailDto> GetProjectPhaseDetailAsync(int id)
        {
            var projectPhase = await _context.ProjectPhases
                .AsNoTracking()
                .Where(pp => pp.Id == id)
                //.Include(pp => pp.Risks)
                //.ThenInclude(r => r.RiskCategory)
                //.Include(pp => pp.ProjectRole)
                //.ThenInclude(rh => rh.User)
                .Select(pp => new ProjectPhaseDetailDto
                {
                    Id = pp.Id,
                    Order = pp.Order,
                    Name = pp.Name,
                    Start = pp.Start,
                    End = pp.End,
                    Risks = pp.Risks.Select(r => new RiskDto
                    {
                        Id = r.Id,
                        Title = r.RiskHistory.OrderByDescending(h => h.Created).FirstOrDefault().Title,
                        CategoryName = r.RiskCategory.Name,
                        Severity = r.RiskHistory
                            .OrderByDescending(h => h.Created)
                            .Select(h => h.Probability * h.Impact)
                            .FirstOrDefault(),
                        State = r.RiskHistory
                            .OrderByDescending(h => h.Created)
                            .Select(h => h.Status)
                            .FirstOrDefault()
                    }),
                    //AssignedUser = new UserDto
                    //{
                    //    Id = pp.ProjectRole.User.Id,
                    //    FullName = pp.ProjectRole.User.FirstName + " " + pp.ProjectRole.User.LastName,
                    //    Email = pp.ProjectRole.User.Email,
                    //}
                })
                .FirstOrDefaultAsync();
            
            return projectPhase;
        }
    }
}

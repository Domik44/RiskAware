using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.DatatableDTOs;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.DTOs.RiskDTOs;
using System.Linq.Dynamic.Core;

namespace RiskAware.Server.Queries
{
    /// <summary>
    /// Class containing queries for project phases.
    /// </summary>
    /// <author> Dominik Pop </author>
    public class ProjectPhaseQueries
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor for ProjectPhaseQueries.
        /// </summary>
        /// <param name="context"> Application DB context. </param>
        public ProjectPhaseQueries(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for getting project phases of a risk project with the given id.
        /// </summary>
        /// <param name="id"> Id of risk project. </param>
        /// <returns> List of DTOs containing basic info about project phase. </returns>
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
                    Risks = pp.Risks.Where(r => r.RiskHistory.OrderByDescending(h => h.LastModif).FirstOrDefault().IsValid).Select(r => new RiskUnderPhaseDto
                    {
                        Id = r.Id,
                        Title = r.RiskHistory.OrderByDescending(h => h.LastModif).FirstOrDefault().Title,
                    })
                })
                .ToListAsync();
            return projectPhases;
        }

        /// <summary>
        /// Method for getting project phases of a risk project with the given id.
        /// Filtered by the given datatable parameters.
        /// </summary>
        /// <param name="projectId"> Id of risk project. </param>
        /// <param name="dtParams"> Datatable parametres. </param>
        /// <returns> Returns a list of DTOs containing basic info about project phase. </returns>
        /// <author> David Drtil </author>
        public IQueryable<ProjectPhaseDto> QueryProjectPhases(int projectId, DtParamsDto dtParams)
        {
            var query = _context.ProjectPhases
                .AsNoTracking()
                .Where(phase => phase.RiskProjectId == projectId)
                .Select(phase => new ProjectPhaseDto
                {
                    Id = phase.Id,
                    Order = phase.Order,
                    Name = phase.Name,
                    Start = phase.Start,
                    End = phase.End,
                });

            if (dtParams.Sorting.Any())
            {
                Sorting sorting = dtParams.Sorting.First();
                query = query.OrderBy($"{sorting.Id} {sorting.Dir}");
            }
            else
            {
                query = query.OrderBy(phase => phase.Order);
            }
            return query;
        }
    }
}

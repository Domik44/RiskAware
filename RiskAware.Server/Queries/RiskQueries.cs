using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.DatatableDTOs;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.DTOs.RiskProjectDTOs;
using RiskAware.Server.Models;
using System.Linq.Dynamic.Core;

namespace RiskAware.Server.Queries
{
    public class RiskQueries
    {
        private readonly AppDbContext _context;

        public RiskQueries(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RiskDetailDto> GetRiskDetailAsync(int id)
        {
            var recentRiskHistory = await _context.RiskHistory
                .AsNoTracking()
                .Where(h => h.RiskId == id)
                .OrderByDescending(h => h.LastModif)
                .FirstOrDefaultAsync();

            var riskDto = await _context.Risks
                .AsNoTracking()
                .Where(r => r.Id == id)
                .Select(r => new RiskDetailDto // TODO -> handle null values
                {
                    Id = r.Id,
                    Title = recentRiskHistory.Title,
                    Description = recentRiskHistory.Description,
                    Probability = recentRiskHistory.Probability,
                    Impact = recentRiskHistory.Impact,
                    Severity = recentRiskHistory.Probability * recentRiskHistory.Impact,
                    Threat = recentRiskHistory.Threat,
                    Indicators = recentRiskHistory.Indicators,
                    Prevention = recentRiskHistory.Prevention,
                    Status = recentRiskHistory.Status,
                    PreventionDone = recentRiskHistory.PreventionDone,
                    RiskEventOccured = recentRiskHistory.RiskEventOccured,
                    End = recentRiskHistory.End,
                    LastModif = recentRiskHistory.LastModif,
                    Created = r.Created,
                    StatusLastModif = recentRiskHistory.StatusLastModif,
                    ProjectPhaseName = r.ProjectPhase.Name,
                    RiskCategoryName = r.RiskCategory.Name,
                    IsVaid = recentRiskHistory.IsValid,
                    IsApproved = recentRiskHistory.IsApproved,
                    UserFullName = r.User.FirstName + " " + r.User.LastName
                })
                .FirstOrDefaultAsync();

            return riskDto;
        }

        public IQueryable<RiskDto> QueryProjectRisks(int projectId, DtParamsDto dtParams)
        {
            var query = _context.Risks
                .AsNoTracking()
                //.Where(r => r.RiskProjectId == projectId) // Original
                .Where(r => r.RiskProjectId == projectId && r.RiskHistory.OrderByDescending(h => h.LastModif).FirstOrDefault().IsValid)
                //.Include(r => r.RiskCategory)
                .Select(r => new RiskDto
                {
                    Id = r.Id,
                    Title = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Title)
                        .FirstOrDefault(),
                    CategoryName = r.RiskCategory.Name,
                    Severity = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Probability * h.Impact)
                        .FirstOrDefault(),
                    Probability = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Probability)
                        .FirstOrDefault(),
                    Impact = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Impact)
                        .FirstOrDefault(),
                    State = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Status)
                        .FirstOrDefault()
                });

            foreach (var filter in dtParams.Filters)
            {
                query = filter.PropertyName switch
                {
                    nameof(RiskDto.Title) =>
                        query.Where(r => r.Title.StartsWith(filter.Value)),                 // string property
                    nameof(RiskDto.CategoryName) =>
                        query.Where(r => r.CategoryName.StartsWith(filter.Value)),
                    nameof(RiskDto.Severity) =>
                        query.Where(r => r.Severity.ToString().StartsWith(filter.Value)),   // numeric property
                    nameof(RiskDto.Probability) =>
                        query.Where(r => r.Probability.ToString().StartsWith(filter.Value)),
                    nameof(RiskDto.Impact) =>
                        query.Where(r => r.Impact.ToString().StartsWith(filter.Value)),
                    nameof(RiskDto.State) =>
                        query.Where(r => r.State.StartsWith(filter.Value)),
                    _ => query      // Default case - do not apply any filter
                };
            }

            if (dtParams.Sorting.Any())
            {
                Sorting sorting = dtParams.Sorting.First();
                query = query.OrderBy($"{sorting.Id} {sorting.Dir}");
            }
            else
            {
                query = query.OrderBy(phase => phase.Id);
            }
            return query;
        }

        public async Task<IEnumerable<RiskDto>> GetRiskProjectRisksAsync(int id)
        {
            var risks = await _context.Risks
            .AsNoTracking()
                //.Where(r => r.RiskProjectId == id)
                .Where(r => r.RiskProjectId == id && r.RiskHistory.OrderByDescending(h => h.LastModif).FirstOrDefault().IsValid)
                .Select(r => new RiskDto
                {
                    Id = r.Id,
                    Title = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Title)
                        .FirstOrDefault(),
                    CategoryName = r.RiskCategory.Name,
                    Severity = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Probability * h.Impact)
                        .FirstOrDefault(),
                    Probability = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Probability)
                        .FirstOrDefault(),
                    Impact = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Impact)
                        .FirstOrDefault(),
                    State = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Status)
                        .FirstOrDefault()
                })
                .ToListAsync();

            // TODO -> mby check if null return empty list?

            return risks;
        }

        public async Task<IEnumerable<RiskDto>> GetProjectPhaseRisksAsync(int id)
        {
            var risks = await _context.Risks
                .AsNoTracking()
                .Where(r => r.ProjectPhaseId == id && r.RiskHistory.OrderByDescending(h => h.LastModif).FirstOrDefault().IsValid)
                //.Where(r => r.ProjectPhaseId == id)
                //.Include(r => r.RiskCategory)
                .Select(r => new RiskDto
                {
                    Id = r.Id,
                    Title = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Title)
                        .FirstOrDefault(),
                    CategoryName = r.RiskCategory.Name,
                    Severity = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Probability * h.Impact)
                        .FirstOrDefault(),
                    State = r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Status)
                        .FirstOrDefault()
                })
                .ToListAsync();

            // TODO -> mby check if null return empty list?

            return risks;
        }

        public async Task<bool> AddRiskAsync(RiskCreateDto riskDto, string userId, int riskProjectId, int riskCategoryId, bool isApproved)
        {
            var risk = new Risk
            {
                Created = DateTime.Now,
                UserId = userId,
                RiskProjectId = riskProjectId,
                ProjectPhaseId = riskDto.ProjectPhaseId,
                RiskCategoryId = riskCategoryId
            };
            _context.Risks.Add(risk);
            await _context.SaveChangesAsync();

            var riskHistory = new RiskHistory
            {
                RiskId = risk.Id,
                UserId = userId,
                Created = risk.Created,
                Title = riskDto.Title,
                Description = riskDto.Description,
                Probability = riskDto.Probability,
                Impact = riskDto.Impact,
                Threat = riskDto.Threat,
                Indicators = riskDto.Indicators,
                Prevention = riskDto.Prevention,
                Status = riskDto.Status,
                PreventionDone = (DateTime)riskDto.PreventionDone,
                RiskEventOccured = (DateTime)riskDto.RiskEventOccured,
                LastModif = DateTime.Now,
                StatusLastModif = DateTime.Now,
                End = riskDto.End,
                IsValid = true,
                IsApproved = isApproved
            };

            _context.RiskHistory.Add(riskHistory);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> EditRiskAsync(RiskCreateDto riskDto, string userId, Risk risk, int riskCategoryId, bool isApproved)
        {
            if(risk.RiskCategoryId != riskCategoryId || risk.ProjectPhaseId != riskDto.ProjectPhaseId)
            {
                risk.ProjectPhaseId = riskDto.ProjectPhaseId;
                risk.RiskCategoryId = riskCategoryId;
                await _context.SaveChangesAsync();
            }

            var oldRiskHistory = await _context.RiskHistory
                .Where(h => h.RiskId == risk.Id)
                .OrderByDescending(h => h.LastModif)
                .FirstOrDefaultAsync();

            var riskHistory = new RiskHistory
            {
                RiskId = risk.Id,
                UserId = userId,
                Created = oldRiskHistory.Created,
                Title = riskDto.Title,
                Description = riskDto.Description,
                Probability = riskDto.Probability,
                Impact = riskDto.Impact,
                Threat = riskDto.Threat,
                Indicators = riskDto.Indicators,
                Prevention = riskDto.Prevention,
                Status = riskDto.Status,
                PreventionDone = (DateTime)riskDto.PreventionDone,
                RiskEventOccured = (DateTime)riskDto.RiskEventOccured,
                LastModif = DateTime.Now,
                StatusLastModif = oldRiskHistory.StatusLastModif,
                End = riskDto.End,
                IsValid = oldRiskHistory.IsValid,
                IsApproved = oldRiskHistory.IsApproved // TODO
            };

            if(riskHistory.Status != oldRiskHistory.Status)
            {
                riskHistory.StatusLastModif = DateTime.Now;
            }

            _context.RiskHistory.Add(riskHistory);
            _context.SaveChanges();

            return true;
        }
    }
}

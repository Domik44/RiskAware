using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs;
using RiskAware.Server.DTOs.DatatableDTOs;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.DTOs.RiskProjectDTOs;
using RiskAware.Server.Models;
using System.Linq.Dynamic.Core;
using System.Reflection.Metadata.Ecma335;

namespace RiskAware.Server.Queries
{
    /// <summary>
    /// Class containing queries for risks.
    /// </summary>
    /// <author> Dominik Pop </author>
    public class RiskQueries
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor for RiskQueries.
        /// </summary>
        /// <param name="context"> Application DB context. </param>
        public RiskQueries(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for getting risk detail with the given id.
        /// </summary>
        /// <param name="id"> Id of risk. </param>
        /// <returns> Returns DTO containing all important info about risk. </returns>
        public async Task<RiskDetailDto> GetRiskDetailAsync(int id)
        {
            var recentRiskHistory = await _context.RiskHistory
                .AsNoTracking()
                .Where(h => h.RiskId == id)
                .OrderByDescending(h => h.LastModif)
                .Include(h => h.User)
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
                    UserFullName = r.User.FirstName + " " + r.User.LastName,
                    EditedBy = recentRiskHistory.User.FirstName + " " + recentRiskHistory.User.LastName
                })
                .FirstOrDefaultAsync();

            return riskDto;
        }

        /// <summary>
        /// Method for editing value back to the original FE value based on scale.
        /// </summary>
        /// <param name="scale"> Scale of risk project. </param>
        /// <param name="value"> Value to be edited. </param>
        /// <returns> Returns new value based on given scale. </returns>
        public int EditValueBack(int scale, int value)
        {
            switch (scale)
            {
                case 3 when value == 3:
                    return 5;
                case 3 when value == 2:
                    return 3;
                default:
                    return value;
            }
        }

        /// <summary>
        /// Method for getting risk information needed for edit modal in FE.
        /// </summary>
        /// <param name="id"> Id of risk. </param>
        /// <param name="scale"> Scale of risk project risk is in. </param>
        /// <returns> Returns DTO containing info needed for risk edit. </returns>
        public async Task<RiskCreateDto> GetRiskEditAsync(int id, int scale)
        {
            var recentRiskHistory = await _context.RiskHistory
                .AsNoTracking()
                .Where(h => h.RiskId == id)
                .OrderByDescending(h => h.LastModif)
                .FirstOrDefaultAsync();

            var probability = EditValueBack(scale, recentRiskHistory.Probability);
            var impact = EditValueBack(scale, recentRiskHistory.Impact);

            var risk = await _context.Risks
                .AsNoTracking()
                .Where(r => r.Id == id)
                .Select(r => new RiskCreateDto
                {
                    Title = recentRiskHistory.Title,
                    Description = recentRiskHistory.Description,
                    Probability = probability,
                    Impact = impact,
                    Threat = recentRiskHistory.Threat,
                    Indicators = recentRiskHistory.Indicators,
                    Prevention = recentRiskHistory.Prevention,
                    Status = recentRiskHistory.Status,
                    PreventionDone = recentRiskHistory.PreventionDone,
                    RiskEventOccured = recentRiskHistory.RiskEventOccured,
                    End = recentRiskHistory.End,
                    ProjectPhaseId = r.ProjectPhaseId,
                    RiskCategory = new RiskCategoryDto
                    {
                        Id = r.RiskCategory.Id,
                        Name = r.RiskCategory.Name
                    },
                })
                .FirstOrDefaultAsync();

            return risk;
        }

        /// <summary>
        /// Method for getting risks of a project with the given id.
        /// Filtered by the given datatable parameters.
        /// </summary>
        /// <param name="projectId"> Id of risk project. </param>
        /// <param name="dtParams"> Datatable parametres. </param>
        /// <returns> Return collection of DTOs containing basic info about risk. </returns>
        public IQueryable<RiskDto> QueryProjectRisks(int projectId, DtParamsDto dtParams)
        {
            var filterHistoryDate = dtParams.Filters
                .Where(f => f.PropertyName == "RiskHistoryDate")
                .First();
            var historyDate = DtParamsDto.ParseClientDate(filterHistoryDate.Value, DateTime.Now);

            var query = _context.Risks
                .AsNoTracking()
                //.Where(r => r.RiskProjectId == projectId) // Original
                .Where(r => r.RiskProjectId == projectId
                    && r.Created <= historyDate
                    && r.RiskHistory
                        .OrderByDescending(h => h.LastModif)
                        .FirstOrDefault()
                        .IsValid)
                //.Include(r => r.RiskCategory)
                .Select(r => new RiskDto
                {
                    Id = r.Id,
                    Title = r.RiskHistory
                        .Where(h => h.LastModif <= historyDate)
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Title)
                        .FirstOrDefault(),
                    CategoryName = r.RiskCategory.Name,
                    Severity = r.RiskHistory
                        .Where(h => h.LastModif <= historyDate)
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Probability * h.Impact)
                        .FirstOrDefault(),
                    Probability = r.RiskHistory
                        .Where(h => h.LastModif <= historyDate)
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Probability)
                        .FirstOrDefault(),
                    Impact = r.RiskHistory
                        .Where(h => h.LastModif <= historyDate)
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Impact)
                        .FirstOrDefault(),
                    State = r.RiskHistory
                        .Where(h => h.LastModif <= historyDate)
                        .OrderByDescending(h => h.LastModif)
                        .Select(h => h.Status)
                        .FirstOrDefault(),
                    ProjectPhase = new ProjectPhaseSimpleDto
                    {
                        Id = r.ProjectPhase.Id,
                        Name = r.ProjectPhase.Name
                    }
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

        /// <summary>
        /// Method for getting risks of risk project with the given id.
        /// </summary>
        /// <param name="id"> Id of risk project. </param>
        /// <returns> Return collection of DTOs containing basic info about risk. </returns>
        public async Task<IEnumerable<RiskDto>> GetRiskProjectRisksAsync(int id)
        {
            var risks = await _context.Risks
            .AsNoTracking()
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
                        .FirstOrDefault(),
                    ProjectPhase = new ProjectPhaseSimpleDto
                    {
                        Id = r.ProjectPhase.Id,
                        Name = r.ProjectPhase.Name
                    }
                })
                .ToListAsync();

            return risks;
        }

        /// <summary>
        /// Method for getting risks of project phase with the given id.
        /// </summary>
        /// <param name="id"> Id of project  phase. </param>
        /// <returns> Returns collection of DTOs containing basic info about risk. </returns>
        public async Task<IEnumerable<RiskDto>> GetProjectPhaseRisksAsync(int id)
        {
            var risks = await _context.Risks
                .AsNoTracking()
                .Where(r => r.ProjectPhaseId == id && r.RiskHistory.OrderByDescending(h => h.LastModif).FirstOrDefault().IsValid)
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
                        .FirstOrDefault(),
                    ProjectPhase = new ProjectPhaseSimpleDto
                    {
                        Id = r.ProjectPhase.Id,
                        Name = r.ProjectPhase.Name
                    }
                })
                .ToListAsync();

            return risks;
        }

        /// <summary>
        /// Method for adding new risk to the database.
        /// </summary>
        /// <param name="riskDto"> DTO containing info about new risk. </param>
        /// <param name="userId"> Id of user creating risk. </param>
        /// <param name="riskProjectId"> Id of risk project risk belongs to. </param>
        /// <param name="riskCategoryId"> Id of risk category assigned to risk. </param>
        /// <param name="isApproved"> Boolean deciding if risk should be approved or not. </param>
        /// <returns> Returns true if action was successful. </returns>
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

        /// <summary>
        /// Method for editing risk in the database.
        /// </summary>
        /// <param name="riskDto"> DTO containing info what should be edited on risk. </param>
        /// <param name="userId"> Id of user editing risk. </param>
        /// <param name="risk"> Entity representing risk in DB.1 </param>
        /// <param name="riskCategoryId"> Id of risk category assigned to risk. </param>
        /// <param name="isApproved"> Boolean deciding if risk should be approved or not. </param>
        /// <returns> Returns true if action was successful. </returns>
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

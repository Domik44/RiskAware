﻿using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.Models;

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
                .OrderByDescending(h => h.Created)
                .FirstOrDefaultAsync();
            // TODO -> check if recentRiskHistory is null handle

            var risk = await _context.Risks
                .AsNoTracking()
                .Where(r => r.Id == id)
                //.Include(r => r.RiskCategory)
                .Select(r => new RiskDetailDto // TODO -> handle null values
                {
                    Id = r.Id,
                    Title = recentRiskHistory.Title,
                    Description = recentRiskHistory.Description,
                    CategoryName = r.RiskCategory.Name,
                    Probability = recentRiskHistory.Probability,
                    Impact = recentRiskHistory.Impact,
                    Severity = recentRiskHistory.Probability * recentRiskHistory.Impact,
                    State = recentRiskHistory.Status,
                    Threat = recentRiskHistory.Threat,
                    Indicators = recentRiskHistory.Indicators,
                    Prevention = recentRiskHistory.Prevention
                })
                .FirstOrDefaultAsync();

            return risk;
        }

        public async Task<IEnumerable<RiskDto>> GetRiskProjectRisksAsync(int id)
        {
            var risks = await _context.Risks
                .AsNoTracking()
                .Where(r => r.RiskProjectId == id)
                //.Include(r => r.RiskCategory)
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
                    Probability = r.RiskHistory
                        .OrderByDescending(h => h.Created)
                        .Select(h => h.Probability)
                        .FirstOrDefault(),
                    Impact = r.RiskHistory
                        .OrderByDescending(h => h.Created)
                        .Select(h => h.Impact)
                        .FirstOrDefault(),
                    State = r.RiskHistory
                        .OrderByDescending(h => h.Created)
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
                .Where(r => r.ProjectPhaseId == id)
                //.Include(r => r.RiskCategory)
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
            _context.SaveChanges();

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
                PreventionDone = riskDto.PreventionDone,
                RiskEventOccured = riskDto.RiskEventOccured,
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
    }
}

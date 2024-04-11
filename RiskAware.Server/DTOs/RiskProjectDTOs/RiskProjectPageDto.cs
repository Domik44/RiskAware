﻿using RiskAware.Server.DTOs.ProjectPhase;

namespace RiskAware.Server.DTOs.RiskProjectDTOs
{
    /// <summary>
    /// Data transfer object for a page of a RiskProject entity.
    /// </summary>
    public class RiskProjectPageDto
    {
        public RiskProjectDetailDto Detail { get; set; }
        //public IEnumerable<ProjectPhasesPanelDto> PhasesPanel { get; set; }
        //public IEnumerable<ProjectPhaseDto> PhasesTable { get; set; }
        //public IEnumerable<RiskDto> RisksTable { get; set; }
        public IEnumerable<ProjectRoleDto> Members { get; set; }

        public RiskProjectPageDto()
        {
            //RiskProjectDetail = new RiskProjectDetailDto(riskProject);
            //PhasesPanel = riskProject.ProjectPhases.Select(pp => new ProjectPhasesPanelDto(pp)).ToList();
            //PhasesTable = riskProject.ProjectPhases.Select(pp => new ProjectPhaseDto(pp)).ToList();
            //RisksTable = riskProject.Risks.Select(r => new RiskDto(r)).ToList();
            //MembersTable = riskProject.ProjectRoles.Select(pr => new RiskProjectMembersDto(pr)).ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaccinbevisVerifiering.Services.CWT.Certificates;
using VaccinbevisVerifiering.Services.DGC.ValueSet;
using VaccinbevisVerifiering.Services.Vaccinregler.ValueSet;

namespace VaccinbevisVerifiering.Services
{
    public interface IRestService
    {
        Task<DSC_TL> RefreshTrustListAsync();
        Task<Dictionary<string, string>> RefreshValueSetAsync();
        Task<VaccinRules> RefreshVaccinRulesAsync();
    }
}

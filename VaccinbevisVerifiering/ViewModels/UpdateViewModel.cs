using System;
using System.Windows.Input;
using Xamarin.Forms;
using VaccinbevisVerifiering.Views;
using Xamarin.Essentials;
using VaccinbevisVerifiering.Resources;
using VaccinbevisVerifiering.Services;
using VaccinbevisVerifiering.Services.Vaccinregler.ValueSet;

namespace VaccinbevisVerifiering.ViewModels
{
    public class UpdateViewModel
    {
        public UpdateViewModel()
        {
        }

        public String CurrentVersion
        {
            get { return VersionTracking.CurrentVersion; }
        }
        public String LatestVersion
        {
            get { return App._latestVersion; }
        }
    }
}

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using VaccinbevisVerifiering.Views;
using VaccinbevisVerifiering.Services.CWT.Certificates;
using VaccinbevisVerifiering.Services;

namespace VaccinbevisVerifiering
{
    public partial class App : Application
    {
        public static CertificateManager CertificateManager { get; private set; }

        public App()
        {
            InitializeComponent();
            CertificateManager = new CertificateManager(new RestService());
            MainPage = new NavigationPage(new MainPage ()){ BarBackgroundColor = Color.White };
            Xamarin.Essentials.Preferences.Set("NoVerificationMode", false);
            Xamarin.Essentials.Preferences.Set("ProductionMode", true);
        }

        protected override void OnStart()
        {
            CertificateManager.LoadCertificates();
            CertificateManager.LoadValueSets();
            CertificateManager.LoadVaccineRules();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            CertificateManager.LoadCertificates();
            CertificateManager.LoadValueSets();
            CertificateManager.LoadVaccineRules();
        }
    }
}

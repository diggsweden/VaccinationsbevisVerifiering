using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using VaccinbevisVerifiering.Views;
using VaccinbevisVerifiering.Services.CWT.Certificates;
using VaccinbevisVerifiering.Services;
using Xamarin.Essentials;

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

        protected override async void OnStart()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                var storageReadPermission = await Permissions.RequestAsync<Permissions.StorageRead>();
                var storageWritePermission = await Permissions.RequestAsync<Permissions.StorageWrite>();
                if (storageReadPermission != PermissionStatus.Granted ||
                    storageWritePermission != PermissionStatus.Granted)
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }

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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using VaccinbevisVerifiering.Models;
using VaccinbevisVerifiering.Resources;
using VaccinbevisVerifiering.Services;
using VaccinbevisVerifiering.Services.DGC;
using VaccinbevisVerifiering.Services.DGC.V1;
using VaccinbevisVerifiering.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VaccinbevisVerifiering.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        private ICommand scanCommand;
        private ICommand settingsCommand;
        private ICommand aboutCommand;

        public MainViewModel()
        {
            MessagingCenter.Subscribe<Xamarin.Forms.Application>(Xamarin.Forms.Application.Current, "Cancel", async (sender) =>
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            });
        }

        public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        public ICommand SettingsCommand => settingsCommand ??
                (settingsCommand = new Command(async () =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
                }));

        public ICommand AboutCommand => aboutCommand ??
                (aboutCommand = new Command(async () =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
                }));

        public ICommand ScanCommand
        {
            get
            {
                return scanCommand ??
                (scanCommand = new Command(async () => await Scan()));
            }
        }

        public async Task Scan() {
            try
            {
                var scanner = DependencyService.Get<IQRScanningService>();
                var result = await scanner.ScanAsync();

                if( result !=null)
                {
                    ResultViewModel resultModel = new ResultViewModel();
                    resultModel.UpdateFields(result);
                    ResultPage resultPage = new ResultPage();
                    resultPage.BindingContext = resultModel;
                    await Application.Current.MainPage.Navigation.PushModalAsync(resultPage);
                }
            }
            catch (Exception ex)
            {
            }
        }

       

    }

    
}
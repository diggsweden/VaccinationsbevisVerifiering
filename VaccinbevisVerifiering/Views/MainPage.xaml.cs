using VaccinbevisVerifiering.Resources;
using Xamarin.Forms;

namespace VaccinbevisVerifiering.Views
{

    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<Xamarin.Forms.Application>(Xamarin.Forms.Application.Current, "DisplayPublicKeysError",
                (sender) =>
                {
                    DisplayAlert(labelValidKeysText.Text, AppResources.KeyModalErrorMessage, "OK");

                });

            // Title = AppResources.HeaderText;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VaccinbevisVerifiering.Views
{
    public partial class UpdatePage : ContentPage
    {


        public UpdatePage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            string url = string.Empty;
            var location = RegionInfo.CurrentRegion.Name.ToLower();
            await Application.Current.MainPage.Navigation.PopAsync();
            if (Device.RuntimePlatform == Device.Android)
                url = "https://play.google.com/store/apps/details?id=se.digg.dccvalidator";
            else if (Device.RuntimePlatform == Device.iOS)
                url = "https://apps.apple.com/" + location + "/app/vaccinationsbevis-verifiering/id1597745749";
            await Browser.OpenAsync(url, BrowserLaunchMode.External);
            
        }
    }
}

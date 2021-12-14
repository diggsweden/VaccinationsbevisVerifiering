using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using ZXing.Mobile;

namespace VaccinbevisVerifiering.Droid.Services
{
    public static class CameraResolutionSelector
    {
        public static CameraResolution SelectLowestResolutionMatchingDisplayAspectRatio(List<CameraResolution> availableResolutions)
        {
            const double aspectTolerance = 0.1;

            //calculating our targetRatio
            var targetRatio = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Height;

            var result = availableResolutions
                .OrderBy(r => r.Height * r.Width)
                .FirstOrDefault(r => (double)r.Height / r.Width - targetRatio < aspectTolerance);

            return result;
        }
    }
}
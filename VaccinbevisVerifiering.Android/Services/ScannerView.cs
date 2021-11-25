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
using Android.Content.PM;
using Android.Graphics;
using Android.Text;
using ZXing.Mobile;

namespace VaccinbevisVerifiering.Droid.Services
{
    public class ScannerView : View
    {
        int[] SCANNER_ALPHA = { 0, 64, 128, 192, 255, 192, 128, 64 };
        const long ANIMATION_DELAY = 80L;
        const int CURRENT_POINT_OPACITY = 0xA0;
        const int MAX_RESULT_POINTS = 20;
        const int POINT_SIZE = 6;

        Paint paint;
        Bitmap resultBitmap;
        Color maskColor;
        Color resultColor;
        Color frameColor;
        Color laserColor;

        int scannerAlpha;
        List<ZXing.ResultPoint> possibleResultPoints;
        private Paint defaultPaint;
        private Paint pressedPaint;
        private Android.Graphics.Bitmap litTorchIcon;
        private Android.Graphics.Bitmap unlitTorchIcon;
        private Rect torchIconDimRect;
        private bool hasTorch = false;
        private bool torchOn = false;
        private bool cancelPressed = false;
        readonly string scanText;
        private Context context;
        private MobileBarcodeScanner scanner;

        public ScannerView(Context context, string scanText, MobileBarcodeScanner scanner) : base(context)
        {
            this.scanner = scanner;
            // Initialize these once for performance rather than calling them every time in onDraw().
            paint = new Paint(PaintFlags.AntiAlias);
            maskColor = Color.Gray; // resources.getColor(R.color.viewfinder_mask);
            resultColor = Color.Red; // resources.getColor(R.color.result_view);
            frameColor = Color.Black; // resources.getColor(R.color.viewfinder_frame);
            laserColor = Color.Red; //  resources.getColor(R.color.viewfinder_laser);
            scannerAlpha = 0;
            possibleResultPoints = new List<ZXing.ResultPoint>(5);

            SetDisplayValues();
        }

        private void SetDisplayValues()
        {
            //Determine if device has flash/torch
            hasTorch = this.Context.PackageManager.HasSystemFeature(PackageManager.FeatureCameraFlash);
            if (hasTorch)
            {
                //Load button icons
                var metrics = Resources.DisplayMetrics;
                int iconHeight = 248;
                int iconWidth = 248;
                torchIconDimRect = new Rect(0, 0, iconWidth, iconHeight);
                litTorchIcon = ImageHelper.DecodeSampledBitmapFromResource(Resources, Resource.Drawable.flash, iconWidth, iconHeight);
                unlitTorchIcon = ImageHelper.DecodeSampledBitmapFromResource(Resources, Resource.Drawable.flash_off, iconWidth, iconHeight);
            }
            // Initialize these once for performance rather than calling them every time in onDraw()
            defaultPaint = new Paint(PaintFlags.AntiAlias);
            pressedPaint = new Paint(PaintFlags.AntiAlias);
            pressedPaint.Color = new Color(96, 97, 104);

            SetBackgroundColor(Color.Transparent);
        }

        Rect GetFramingRect(Canvas canvas)
        {
            var width = canvas.Width * 15 / 16;

            var height = canvas.Height * 4 / 10;

            var leftOffset = (canvas.Width - width) / 2;
            var topOffset = (canvas.Height - height) / 2;
            var framingRect = new Rect(leftOffset, topOffset, leftOffset + width, topOffset + height);

            return framingRect;
        }

        Rect GetTorchIconDimRect()
        {
            return torchIconDimRect;
        }

        //The Rect used to draw the icon
        Rect GetTorchIconRect()
        {
            var metrics = Resources.DisplayMetrics;
            int height = metrics.HeightPixels / 8;
            int width = height;
            int leftOffset = metrics.WidthPixels / 2 - width / 2;
            int topOffset = metrics.HeightPixels / 3 * 2;
            var torchRect = new Rect(leftOffset, topOffset, leftOffset + width, topOffset + height);

            return torchRect;
        }

        //The Rect used to detect touch
        Rect GetTorchButtonRect()
        {
            var metrics = Resources.DisplayMetrics;
            int height = metrics.HeightPixels / 8;
            int width = height;
            int leftOffset = metrics.WidthPixels / 2 - width / 2;
            int topOffset = metrics.HeightPixels / 3 * 2;
            var torchRect = new Rect(leftOffset, topOffset, leftOffset + width, topOffset + height);

            return torchRect;
        }

        protected override void OnDraw(Canvas canvas)
		{

			var scale = this.Context.Resources.DisplayMetrics.Density;

			var frame = GetFramingRect(canvas);
			if (frame == null)
				return;

			var width = canvas.Width;
			var height = canvas.Height;

            //var cancelBtn = GetCancelButtonRect();
            var torchBtn = GetTorchButtonRect();

            //Draw buttons
            defaultPaint.Color = Color.Black;
            defaultPaint.Alpha = 255;
            pressedPaint.Color = new Color(96, 97, 104);

            if (hasTorch)
            {
                //canvas.DrawRect(torchBtn, torchOn ? pressedPaint : defaultPaint);
                //Draw button icons
                canvas.DrawBitmap(torchOn ? litTorchIcon : unlitTorchIcon, GetTorchIconDimRect(), GetTorchIconRect(), defaultPaint);
            }

            base.OnDraw(canvas);
		}

        public void DrawResultBitmap(Android.Graphics.Bitmap barcode)
        {
            resultBitmap = barcode;
            Invalidate();
        }

        public void AddPossibleResultPoint(ZXing.ResultPoint point)
        {
            var points = possibleResultPoints;

            lock (points)
            {
                points.Add(point);
                var size = points.Count;
                if (size > MAX_RESULT_POINTS)
                {
                    points.RemoveRange(0, size - MAX_RESULT_POINTS / 2);
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent me)
        {
            if (me.Action == MotionEventActions.Down)
            {
                //if (GetCancelButtonRect().Contains((int)me.RawX, (int)me.RawY))
                //{
                //    cancelPressed = true;
                //    this.Invalidate();
                //    OnUnload();
                //    scanner.Cancel();
                //}
                if (GetTorchButtonRect().Contains((int)me.RawX, (int)me.RawY))
                {
                    scanner.ToggleTorch();
                    torchOn = !torchOn;
                    this.Invalidate();
                }
                //else if (GetProblemButtonRect().Contains((int)me.RawX, (int)me.RawY))
                //{
                //    problemPressed = true;
                //    this.Invalidate();

                //    if (parentActivity == null)
                //    {
                //        Intent intent = new Intent();
                //        intent.SetClass(context, typeof(ManualEntryActivity));
                //        context.StartActivity(intent);
                //    }
                //    else
                //    {
                //        parentActivity.GetManualEntry();
                //    }
                //    OnUnload();
                //    scanner.Cancel();
                //}
                return true;
            }
            else
                return false;
        }
    }
}
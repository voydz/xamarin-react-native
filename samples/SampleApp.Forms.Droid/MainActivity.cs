using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace SampleApp.Forms.Droid
{
    [Activity(Label = "SampleApp.Forms.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

#if DEBUG
            var debug = true;
#else
            var debug = false;
#endif
            ReactNative.Forms.Droid.Renderers.ReactViewRenderer.Init(this, debug);

            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            ReactNative.Forms.Droid.Renderers.ReactViewRenderer.OnPermissionResult(requestCode, (int)resultCode, data);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();

            ReactNative.Forms.Droid.Renderers.ReactViewRenderer.Instance.BackPressed();
        }

        protected override void OnResume()
        {
            base.OnResume();

            ReactNative.Forms.Droid.Renderers.ReactViewRenderer.Instance.Resume();
        }

        protected override void OnPause()
        {
            base.OnPause();

            ReactNative.Forms.Droid.Renderers.ReactViewRenderer.Instance.Pause();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            ReactNative.Forms.Droid.Renderers.ReactViewRenderer.Instance.Destroy();
        }
    }
}

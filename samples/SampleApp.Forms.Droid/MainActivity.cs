using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SampleApp.Forms.Droid
{
    [Activity(Label = "SampleApp.Forms.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::ReactNative.Forms.Droid.ReactFormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        protected override bool IsReactNativeDebugging()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}

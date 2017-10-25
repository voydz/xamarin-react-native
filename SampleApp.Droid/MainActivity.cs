using Android.App;
using Android.OS;
using Com.Facebook.React;
using Com.Facebook.React.Shell;
using Com.Facebook.React.Common;
using Android.Views;
using Com.Facebook.React.Modules.Core;
using Android.Support.V7.App;

namespace SampleApp.Droid
{
    [Activity(Label = "SampleApp.Droid", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity, IDefaultHardwareBackBtnHandler
    {
        ReactRootView mReactRootView;

        ReactInstanceManager mReactInstanceManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ReactNativeBinding.Init();

            mReactRootView = new ReactRootView(this);
            mReactInstanceManager = ReactInstanceManager.Builder()
                    .SetApplication(Application)
                    .SetBundleAssetName("index.android.bundle")
                    .SetJSMainModulePath("index")
                    .AddPackage(new MainReactPackage())
                    .SetUseDeveloperSupport(BuildConfig.Debug)
                    .SetInitialLifecycleState(LifecycleState.Resumed)
                    .Build();

            mReactRootView.StartReactApplication(mReactInstanceManager, "MyReactNativeApp", null);

            SetContentView(mReactRootView);
        }

        protected override void OnPause()
        {
            base.OnPause();

            if (mReactInstanceManager != null)
            {
                mReactInstanceManager.OnHostPause(this);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (mReactInstanceManager != null)
            {
                mReactInstanceManager.OnHostResume(this, this);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (mReactInstanceManager != null)
            {
                mReactInstanceManager.OnHostDestroy(this);
            }
        }

        public override void OnBackPressed()
        {
            if (mReactInstanceManager != null)
            {
                mReactInstanceManager.OnBackPressed();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public void InvokeDefaultOnBackPressed()
        {
            base.OnBackPressed();
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Menu && mReactInstanceManager != null)
            {
                mReactInstanceManager.ShowDevOptionsDialog();
                return true;
            }

            return base.OnKeyUp(keyCode, e);
        }
    }
}


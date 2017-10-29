using ReactNative.Forms.Droid.Renderers;
using ReactNative.Forms.Views;
using Xamarin.Forms;
using Com.Facebook.React;
using Xamarin.Forms.Platform.Android;
using Com.Facebook.React.Shell;
using Com.Facebook.React.Common;
using System.ComponentModel;
using Android.OS;
using Android.Provider;
using Android.Content;
using Android.Net;
using Com.Facebook.React.Modules.Core;
using Android.Views;
using System.Collections.Generic;

[assembly: ExportRenderer(typeof(ReactView), typeof(ReactViewRenderer))]
namespace ReactNative.Forms.Droid.Renderers
{
    public class ReactViewRenderer : ViewRenderer<ReactView, ReactRootView>, IDefaultHardwareBackBtnHandler
    {
        private const int OVERLAY_PERMISSION_REQ_CODE = 1945;

        #region static fields

        private static Android.App.Application _application;

        private static Android.App.Activity _activity;

        private static bool _debugMode;

        #endregion

        #region fields

        private ReactRootView _rootView;

        private ReactInstanceManager _instanceManager;

        #endregion

        #region singleton

        private static ReactViewRenderer _instance;

        public static ReactViewRenderer Instance => _instance;

        #endregion

        public ReactViewRenderer()
        {
            _instance = this;
        }

        public static void Init(Android.App.Activity activity, bool debugMode)
        {
            _debugMode = debugMode;
            _activity = activity;
            _application = activity.Application;

            if (_debugMode && Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (!Settings.CanDrawOverlays(activity))
                {
                    var intent = new Intent(Settings.ActionManageOverlayPermission, Uri.Parse("package:" + activity.PackageName));
                    activity.StartActivityForResult(intent, OVERLAY_PERMISSION_REQ_CODE);
                }
            }
        }

        #region android control api

        public void Pause()
        {
            _instanceManager?.OnHostPause(_activity);
        }

        public void Resume()
        {
            _instanceManager?.OnHostResume(_activity, this);
        }

        public void Destroy()
        {
            _instanceManager?.OnHostDestroy(_activity);
        }

        public void BackPressed()
        {
            if (_instanceManager != null)
            {
                _instanceManager.OnBackPressed();
            }
            //else
            //{
            //    base.OnBackPressed();
            //}
        }

        public void InvokeDefaultOnBackPressed()
        {
            _activity.OnBackPressed();
        }

        public bool KeyUp(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Menu && _instanceManager != null)
            {
                _instanceManager.ShowDevOptionsDialog();
                return true;
            }

            return base.OnKeyUp(keyCode, e);
        }

        #endregion

        #region renderer

        protected override void OnElementChanged(ElementChangedEventArgs<ReactView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                CreateReactView();
                SetNativeControl(_rootView);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Destroy();

            // We have to force recreate the whole view.
            CreateReactView();
            SetNativeControl(_rootView);
        }

        private void CreateReactView()
        {
            _rootView = new ReactRootView(Context);
            _instanceManager = ReactInstanceManager.Builder()
                .SetApplication(_application)
                .SetBundleAssetName(Element.BundleName)
                .SetJSMainModulePath(Element.ModulePath)
                .AddPackage(new MainReactPackage())
                .SetUseDeveloperSupport(_debugMode)
                .SetInitialLifecycleState(LifecycleState.Resumed)
                .Build();

            // convert dictionary to bundle
            var props = new Bundle();
            foreach (KeyValuePair<string, object> entry in Element.Properties)
            {
                props.PutString(entry.Key, (string)entry.Value);
            }

            _rootView.StartReactApplication(_instanceManager, Element.ModuleName, props);
        }

        #endregion
    }
}

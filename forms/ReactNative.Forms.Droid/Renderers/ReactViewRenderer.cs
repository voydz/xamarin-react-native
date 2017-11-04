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
using Com.Facebook.React.Bridge;

[assembly: ExportRenderer(typeof(ReactView), typeof(ReactViewRenderer))]
namespace ReactNative.Forms.Droid.Renderers
{
    public class ReactViewRenderer : ViewRenderer<ReactView, ReactRootView>, IDefaultHardwareBackBtnHandler, ReactInstanceManager.IReactInstanceEventListener
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

        public static void OnPermissionResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == OVERLAY_PERMISSION_REQ_CODE)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    if (Settings.CanDrawOverlays(_activity))
                    {
                        // Success, now create the view if we have an instance.
                        _instance?.CreateReactView();
                    }
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
                // View is added upon react context was initialized.
                CreateReactView();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(ReactView.PackagerUrl):
                case nameof(ReactView.BundleName):
                case nameof(ReactView.ModulePath):
                case nameof(ReactView.ModuleName):
                case nameof(ReactView.Properties):

                    // Kill everything.
                    _instanceManager.DetachRootView(_rootView);
                    _instanceManager.Destroy();

                    _instanceManager = null;
                    _rootView = null;

                    // Recreate view.
                    CreateReactView();
                    break;
            }
        }

        private void CreateReactView()
        {
            if (_debugMode && !Settings.CanDrawOverlays(_activity))
            {
                // Debug mode without overlay permissions not supported.
                System.Console.WriteLine("[ReactNative.Forms] Debug mode without overlay permissions not supported.");
                return;
            }

            _rootView = new ReactRootView(Context);
            _instanceManager = ReactInstanceManager.Builder()
                .SetApplication(_application)
                .SetBundleAssetName(Element.BundleName)
                .SetJSMainModulePath(Element.ModulePath)
                .AddPackage(new MainReactPackage())
                .SetUseDeveloperSupport(_debugMode)
                .SetInitialLifecycleState(LifecycleState.Resumed)
                .Build();

            _instanceManager.AddReactInstanceEventListener(this);

            // convert dictionary to bundle
            var props = new Bundle();
            foreach (KeyValuePair<string, object> entry in Element.Properties)
            {
                props.PutString(entry.Key, (string)entry.Value);
            }

            _rootView.StartReactApplication(_instanceManager, Element.ModuleName, props);
        }

        public void OnReactContextInitialized(ReactContext p0)
        {
            // Adding the view here will ensure it already
            // has a view id managed by react native.
            // Forms will take on this id and does not generate
            // a new one upon adding without an id.
            SetNativeControl(_rootView);
        }

        #endregion
    }
}

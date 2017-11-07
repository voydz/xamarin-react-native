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
using Com.Facebook.React.Modules.Core;
using Android.Views;
using System.Collections.Generic;
using Com.Facebook.React.Bridge;
using Android.App;

[assembly: ExportRenderer(typeof(ReactView), typeof(ReactViewRenderer))]
namespace ReactNative.Forms.Droid.Renderers
{
    public class ReactViewRenderer : ViewRenderer<ReactView, ReactRootView>, ReactInstanceManager.IReactInstanceEventListener
    {
        #region fields

        private ReactRootView _rootView;

        private ReactInstanceManager _instanceManager;

        #endregion

        public ReactViewRenderer()
        {
            RendererManager.Register(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RendererManager.Unregister(this);
            }

            base.Dispose(disposing);
        }

        #region internal api

        internal void RerenderReactView()
        {
            CreateReactView();
        }

        internal void Resume(IDefaultHardwareBackBtnHandler handler)
        {
            _instanceManager?.OnHostResume(RendererManager.Activity, handler);
        }

        internal void Pause()
        {
            _instanceManager?.OnHostPause(RendererManager.Activity);
        }

        internal void Destroy()
        {
            _instanceManager?.OnHostDestroy(RendererManager.Activity);
        }

        internal bool BackPressed()
        {
            if (_instanceManager == null)
            {
                return false;
            }
            else
            {
                _instanceManager.OnBackPressed();
                return true;
            }
        }

        internal void ShowDevOptionsDialog()
        {
            _instanceManager?.ShowDevOptionsDialog();
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
            if (RendererManager.Debug && !Settings.CanDrawOverlays(RendererManager.Activity))
            {
                // Debug mode without overlay permissions not supported.
                System.Console.WriteLine("[ReactNative.Forms] Debug mode without overlay permissions not supported.");
                return;
            }

            _rootView = new ReactRootView(Context);
            _instanceManager = ReactInstanceManager.Builder()
                .SetApplication(RendererManager.Activity.Application)
                .SetBundleAssetName(Element.BundleName)
                .SetJSMainModulePath(Element.ModulePath)
                .AddPackage(new MainReactPackage())
                .SetUseDeveloperSupport(RendererManager.Debug)
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

using System.ComponentModel;
using System.IO;
using System.Linq;
using Foundation;
using ReactNative.Forms.iOS.Renderers;
using ReactNative.Forms.Views;
using ReactNative.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ReactView), typeof(ReactViewRenderer))]
namespace ReactNative.Forms.iOS.Renderers
{
    public class ReactViewRenderer : ViewRenderer<ReactView, RCTRootView>
    {
        private RCTRootView _rootView;

        #region renderer

        protected override void OnElementChanged(ElementChangedEventArgs<ReactView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                _rootView = CreateReactView();
                SetNativeControl(_rootView);
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
                    // We have to force recreate the whole view.
                    _rootView = CreateReactView();
                    SetNativeControl(_rootView);
                    break;
            }
        }

        private RCTRootView CreateReactView()
        {
            // convert dictionary to nsdictionary
            var props = NSDictionary.FromObjectsAndKeys(
                Element.Properties.Values.ToArray(),
                Element.Properties.Keys.ToArray()
            );

            // TODO options not implemented for now
            var options = new NSDictionary();

            NSUrl url;
            if (string.IsNullOrEmpty(Element.ModulePath))
            {
                // using bundled asset
                var bundleName = Element.BundleName;
                url = NSBundle.MainBundle.GetUrlForResource(
                    Path.GetFileNameWithoutExtension(bundleName),
                    Path.GetExtension(bundleName)
                );
            }
            else
            {
                // using react packager
                url = NSUrl.FromString($"{Element.PackagerUrl}/{Element.ModulePath}.bundle?platform=ios");
            }

            return new RCTRootView(url, new NSString(Element.ModuleName), props, options);
        }

        #endregion
    }
}

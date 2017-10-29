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
        private RCTRootView _reactView;

        public static void Init()
        {
        }

        #region renderer

        protected override void OnElementChanged(ElementChangedEventArgs<ReactView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                _reactView = CreateReactView();
                SetNativeControl(_reactView);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            // We have to force recreate the whole view.
            _reactView = CreateReactView();
            SetNativeControl(_reactView);
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

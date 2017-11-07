using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Xamarin.Forms.Platform.Android;

namespace ReactNative.Forms.Droid
{
    public class ReactFormsAppCompatActivity : FormsAppCompatActivity
    {
        /// <summary>
        /// The renderer manager.
        /// </summary>
        private RendererManager _rendererManager;

        #region configure functionality

        protected virtual bool IsReactNativeDebugging()
        {
            return false;
        }

        #endregion

        #region lifecycle functions

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _rendererManager = new RendererManager(this, IsReactNativeDebugging());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            _rendererManager.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnResume()
        {
            base.OnResume();

            _rendererManager.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();

            _rendererManager.OnPause();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _rendererManager.OnDestroy();
        }

        public override void OnBackPressed()
        {
            if (!_rendererManager.OnBackPressed())
            {
                base.OnBackPressed();
            }
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Menu)
            {
                _rendererManager.OnKeyUp(keyCode, e);
                return true;
            }

            return base.OnKeyUp(keyCode, e);
        }

        #endregion
    }
}

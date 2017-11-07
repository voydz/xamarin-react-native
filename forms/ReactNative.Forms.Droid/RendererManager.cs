using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Views;
using Com.Facebook.React.Modules.Core;
using ReactNative.Forms.Droid.Renderers;

namespace ReactNative.Forms.Droid
{
    public class RendererManager : Java.Lang.Object, IDefaultHardwareBackBtnHandler
    {
        private const int OVERLAY_PERMISSION_REQ_CODE = 1945;

        /// <summary>
        /// The forms activity.
        /// </summary>
        internal static Activity Activity;

        /// <summary>
        /// The debug mode.
        /// </summary>
        internal static bool Debug;

        /// <summary>
        /// The renderers.
        /// </summary>
        private static List<ReactViewRenderer> _renderers = new List<ReactViewRenderer>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReactNative.Forms.Droid.RendererManager"/> class.
        /// </summary>
        /// <param name="activity">Activity.</param>
        /// <param name="debug">Debug.</param>
        public RendererManager(Activity activity, bool debug)
        {
            Activity = activity;
            Debug = debug;

            if (debug && Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (!Settings.CanDrawOverlays(activity))
                {
                    var intent = new Intent(Settings.ActionManageOverlayPermission, Uri.Parse("package:" + activity.PackageName));
                    activity.StartActivityForResult(intent, OVERLAY_PERMISSION_REQ_CODE);
                }
            }
        }

        #region public interface

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == OVERLAY_PERMISSION_REQ_CODE)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    if (Settings.CanDrawOverlays(Activity))
                    {
                        // Success, now create the view if we have an instance.
                        foreach (var renderer in _renderers)
                        {
                            renderer.RerenderReactView();
                        }
                    }
                }
            }
        }

        public void OnResume()
        {
            foreach (var renderer in _renderers)
            {
                renderer.Resume(this);
            }
        }

        public void OnPause()
        {
            foreach (var renderer in _renderers)
            {
                renderer.Pause();
            }
        }

        public void OnDestroy()
        {
            foreach (var renderer in _renderers)
            {
                renderer.Destroy();
            }
        }

        public bool OnBackPressed()
        {
            var result = false;
            foreach (var renderer in _renderers)
            {
                if (renderer.BackPressed())
                    result = true;
            }
            return result;
        }

        public bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Menu)
            {
                foreach (var renderer in _renderers)
                {
                    renderer.ShowDevOptionsDialog();
                }
                return true;
            }

            return false;
        }

        #endregion

        #region interface implementation

        /// <summary>
        /// Invokes the default on back pressed.
        /// </summary>
        public void InvokeDefaultOnBackPressed()
        {
            Activity.OnBackPressed();
        }

        #endregion

        #region internal interface

        /// <summary>
        /// Register the specified renderer.
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="renderer">Renderer.</param>
        internal static void Register(ReactViewRenderer renderer)
        {
            _renderers.Add(renderer);
        }

        /// <summary>
        /// Unregister the specified renderer.
        /// </summary>
        /// <returns>The unregister.</returns>
        /// <param name="renderer">Renderer.</param>
        internal static void Unregister(ReactViewRenderer renderer)
        {
            _renderers.Remove(renderer);
        }

        #endregion
    }
}

using System;

namespace Com.Facebook.React
{
    public partial class ReactInstanceManager
    {
        /// <summary>
        /// Initializes the <see cref="T:Com.Facebook.React.ReactInstanceManager"/> class.
        /// Load necessary jni libraries via static ctor.
        /// </summary>
        static ReactInstanceManager()
        {
            // Load jni libraries.
            Java.Lang.JavaSystem.LoadLibrary("imagepipeline");
            Java.Lang.JavaSystem.LoadLibrary("reactnativejni");
        }
    }
}

using System;

namespace Com.Facebook.React
{
    public class ReactNativeBinding
    {
        public static void Init()
        {
            // Load jni libraries.
            Java.Lang.JavaSystem.LoadLibrary("imagepipeline");
            Java.Lang.JavaSystem.LoadLibrary("reactnativejni");
        }
    }
}

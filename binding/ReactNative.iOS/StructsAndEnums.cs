using System;
using System.Runtime.InteropServices;
using Foundation;

namespace ReactNative.iOS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RCTMethodInfo
    {
        public string jsName;
        public string objcName;
        public bool isSync;
    }

    public static class CFunctions
    {
        [DllImport ("__Internal")]
        public static extern void RCTRegisterModule(IntPtr module);
    }
}


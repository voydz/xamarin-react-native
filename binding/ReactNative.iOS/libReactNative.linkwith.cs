using System;
using ObjCRuntime;

[assembly: LinkWith(
    "libReactNative.a",
    LinkTarget.ArmV7 | LinkTarget.Arm64 | LinkTarget.i386 | LinkTarget.x86_64,
    Frameworks = "JavaScriptCore",
    IsCxx = true,
    SmartLink = false /* if set to true it will unlink both RCTBatchedBridge and RCTCxxBridge */,
    ForceLoad = true)]

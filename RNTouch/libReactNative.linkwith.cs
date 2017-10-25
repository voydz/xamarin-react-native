using System;
using ObjCRuntime;

[assembly: LinkWith(
    "libReactNative.a",
    LinkTarget.ArmV7 | LinkTarget.Arm64 | LinkTarget.i386 | LinkTarget.x86_64,
    Frameworks = "JavaScriptCore",
    IsCxx = true, SmartLink = true, ForceLoad = true)]

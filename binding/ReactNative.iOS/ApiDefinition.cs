using System;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace ReactNative.iOS
{
    [BaseType(typeof(UIView))]
    interface RCTRootView
    {
        [ExportAttribute("initWithBundleURL:moduleName:initialProperties:launchOptions:")]
        IntPtr Constructor(NSUrl bundleUrl, NSString moduleName, NSDictionary initialProperties, NSDictionary launchOptions);
    }

    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface RCTBridgeModule { }

    [BaseType(typeof(NSObject))]
    interface RCTBundleURLProvider
    {
        [Static]
        [Export("sharedSettings")]
        RCTBundleURLProvider SharedSettings();

        [Export("jsBundleURLForBundleRoot:fallbackResource:")]
        NSUrl JsBundleURLForBundleRoot(string bundleRoot, [NullAllowed] string resourceName);
    }
}

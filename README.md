# Xamarin + react-native binding & sample project [![Build Status](https://www.bitrise.io/app/b3d5e7123fea85f8/status.svg?token=kL4Osd_HE45B1xGIH9wqVA&branch=master)](https://www.bitrise.io/app/b3d5e7123fea85f8)
This project is inspired and based on https://github.com/cluxton/xamarin-react-native. Credits to him, for his good work! :+1:

#### Whats in the box?
This example includes a working proof-of-concept for both *Xamarin.Android* and *Xamarin.iOS*. Also there are renderers for *Xamarin.Forms*.

#### So, why would you do that?
This is mainly a proof-of-concept and a challange to make transitioning projects from Xamarin to react-native a lot easier.

## Integrate in your project
To get startet just install the NuGet packages you need for your project. It depends on which project type you are dealing with. The version of the NuGet package indicates in which version of react-native is targeted. For example the version `1.0.**50**` targets react-native `^0.50.0`.

#### Xamarin.iOS and Xamarin.Droid
* Xamarin.Droid - https://www.nuget.org/packages/ReactNative.Droid/
* Xamarin.iOS - https://www.nuget.org/packages/ReactNative.iOS/
* Xamarin.iOS for Debugging (see notes in 1. Getting your hands dirty) https://www.nuget.org/packages/ReactNative.iOS.Debug/

#### Xamarin.Forms
* Xamarin.Forms.Droid - https://www.nuget.org/packages/ReactNative.Forms.Droid/
* Xamarin.Forms.iOS - https://www.nuget.org/packages/ReactNative.Forms.iOS/

(you can and should use the `ReactNative.iOS.Debug` package in exchange for `ReactNative.iOS` for debugging purposes)

## A word about linking react-native components
There are a lot of react-native components which need [native linking](https://facebook.github.io/react-native/docs/linking-libraries-ios.html). This binding already contains a precompiled and linked version of react-native. In theroy every component which need native linking should work if bound to C# seperatly. Make sure it is referencing this base react-native binding.

## Build the samples and sources
### iOS
#### 1. Getting your hands dirty
To build the application you will first need to download React Native & build the static library for Xamarin to use.

It is crucial to understand, that since the library is compiled and linked statically you have to ship seperate `*.dlls` for release and debug. For example a release build of `libReactNative.a` won't contain the [DevSupport](https://facebook.github.io/react-native/docs/debugging.html) tools. See the commands below on how you can change the build configuration.

After checking out the project run the following commands:

```bash
# inside of ./binding/ReactNative.iOS
# install all node dependencies
yarn install

# build static react native library and the binding
make build CONFIGURATION=Debug # or Release respectively (case sensitive), default: Debug
```

#### 2. Either use the react packager
This will only work for debug builds. Run the following command and check that your javascript bundle is available on `http://localhost:8081/index.bundle`

If you deploy to a physical device make sure you update the url inside `SampleApp.iOS/AppDelegate.cs` with your local ip address, so that the device can reach the packager in your local network.

```bash
# inside of ./binding
# run react native packager
yarn start
```

#### 2. Or use the embeddable javascript bundle
This is recommended for release builds. You will need to update the javascript source inside `samples/SampleApp.iOS/AppDelegate.cs` or `samples/SampleApp.Forms/Mainpage.xaml` to the bundled asset.

```bash
# inside of ./binding
# bundle javascript to embeddable main.jsbundle
yarn bundle-ios
```

#### 3. Firing it up
After you have done this, you can open the project `samples/SampleApp.sln` and deploy it to a device or simulator.

### Android
#### 1. Getting your hands dirty
After checking out the project run the following commands:

```bash
# inside of ./binding/ReactNative.Droid
# install all node dependencies
yarn install

# build the android binding
make build
```

#### 2. Bundle the embeddable javascript
This is recommended for release builds.

```bash
# inside of ./binding/
# bundle javascript to embeddable index.android
yarn bundle-android
```

#### (OPTIONAL) 3.1. Use the react packager
Using the react packager is only possible once the app already started and loaded it's bundle from `Assets/`. See *Known Issues*.

This will only work for debug builds. Run the following command and check that your javascript bundle is available on `http://localhost:8081/index.bundle`

```bash
# inside of ./binding
# run react native packager
yarn start
```

Open the react dev support menu and `Refresh` the view or `Enable hot reloading` to check if everything works.

#### 3. Firing it up
After you have done this, you can open the project `samples/SampleApp.sln` and deploy it to a device or simulator.

## Known Issues
* The precompiled `ReactNative.Droid` assembly references the `Square.Okio` package. This will cause build errors in the DEXer build step if you are using `modernhttpclient`. This is because `modernhttpclient` ships with its own prebundled version of `okhttp`.
    * **Workaround:** You have to compile `ReactNative.Droid` by yourself and remove the duplicated native references. Alternatively you can use a fork of `modernhttpclient` which does not embed its own version of `okhttp`.
* The Android sample application does not initially load from the react packager. **Or is this the intended behavior?**
    * **Workaround:** Instead you have to `yarn bundle` and include the `index.android.bundle` in the android `Assets/` directory.

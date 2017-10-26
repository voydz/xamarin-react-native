# Xamarin + react-native binding & sample project
This project is inspired and based on https://github.com/cluxton/xamarin-react-native. Credits to him, for his good work! :+1:

#### Whats in the box?
This example includes a working proof-of-concept for both *Xamarin.Android* and *Xamarin.iOS*.

#### So, why would you do that?
This is mainly a proof-of-concept and a challange to make transitioning projects from Xamarin to react-native a lot easier.

## Getting started
### iOS
#### 1. Getting your hands dirty
To build the application you will first need to download React Native & build the static library for Xamarin to use.

It is crucial to understand, that since the library is compiled and linked statically you have to ship seperate `*.dlls` for release and debug. For example a release build of `libReactNative.a` won't contain the DevSupport tools. See the commands below on how you can change the build configuration.

After checking out the project run the following commands:

```bash
cd ./RNTouch

# install all node dependencies
yarn install

# build static react native library
yarn build Debug # or Release respectively (case sensitive), default: Debug
```

#### 2. Either use the react packager
This will only work for debug builds. Run the following command and check that your javascript bundle is available on `http://localhost:8081/index.bundle`

```bash
# run react native packager
yarn start
```

#### 2. Or use the embeddable javascript bundle
This is recommended for release builds. You will need to update the url inside `SampleApp.iOS/AppDelegate.cs` to the bundle asset.

```bash
# bundle javascript to embeddable main.jsbundle
yarn bundle
```

#### 3. Firing it up
After you have done this, you can open the project `XamarinReactNative.sln` and deploy it to a device or simulator.

### Android
#### 1. Getting your hands dirty
After checking out the project run the following commands:

```bash
cd ./RNDroid

# install all node dependencies
yarn install
```

#### 2. Bundle the embeddable javascript
This is recommended for release builds.

```bash
# bundle javascript to embeddable index.android
yarn bundle
```

#### 3. Firing it up
After you have done this, you can open the project `XamarinReactNative.sln` and deploy it to a device or simulator.

#### (OPTIONAL) 3.1. Use the react packager
Using the react packager is only possible once the app already started and loaded it's bundle from `Assets/`. See *Known Issues*.

This will only work for debug builds. Run the following command and check that your javascript bundle is available on `http://localhost:8081/index.bundle`

```bash
# run react native packager
yarn start
```

Open the react dev support menu and `Refresh` the view or `Enable hot reloading` to check if everything works.

## Known Issues
* The iOS sample application only works on a simulator. Physical devices run into an dynamic code generation error.
    * **No workaround** known, yet.


* The Android sample application does not initially load from the react packager. **Or is this the intended behavior?**
    * **Workaround:** Instead you have to `yarn bundle` and include the `index.android.bundle` in the android `Assets/` directory.

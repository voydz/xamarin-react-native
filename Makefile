all: build

build:
	$(MAKE) -C binding/ReactNative.Droid
	$(MAKE) -C binding/ReactNative.iOS

clean:
	$(MAKE) clean -C binding/ReactNative.Droid
	$(MAKE) clean -C binding/ReactNative.iOS

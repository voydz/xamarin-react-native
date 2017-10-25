#!/bin/bash
DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd)

function buildLib {
  # prepare project params
  BASE="$DIR/node_modules/react-native"
  PROJECT="$BASE/$1"

  if [[ -n $3 ]]; then
    HEADER_SIMULATOR="HEADER_SEARCH_PATHS=\"$BASE/$3/build/Release-iphonesimulator/include\""
    HEADER_IPHONE="HEADER_SEARCH_PATHS=\"$BASE/$3/build/Release-iphoneos/include\""
  else
    HEADER_SIMULATOR=""
    HEADER_IPHONE=""
  fi

  # display some resolved vars
  echo $PROJECT
  echo $HEADER_SIMULATOR
  echo $HEADER_IPHONE

  rm -Rf "$PROJECT/**/*.a"
  cd $PROJECT

  # build xcode
  xcodebuild \
    -project "$2.xcodeproj" \
    -target $2 \
    -sdk iphonesimulator \
    -arch i386 \
    -configuration Release clean build \
    $HEADER_SIMULATOR
  xcodebuild \
    -project "$2.xcodeproj" \
    -target $2 \
    -sdk iphonesimulator \
    -arch x86_64 \
    -configuration Release clean build \
    TARGET_BUILD_DIR='./build-x86_64' \
    BUILT_PRODUCTS_DIR='./build-x86_64' \
    $HEADER_SIMULATOR
  xcodebuild \
    -project "$2.xcodeproj" \
    -target $2 \
    -sdk iphoneos \
    -arch armv7 \
    -configuration Release clean build \
    $HEADER_IPHONE
  xcodebuild \
    -project "$2.xcodeproj" \
    -target $2 \
    -sdk iphoneos \
    -arch arm64 \
    -configuration Release clean build \
    TARGET_BUILD_DIR='./build-arm64' \
    BUILT_PRODUCTS_DIR='./build-arm64' \
    $HEADER_IPHONE

  # link archs together
  cd $DIR
  lipo -create -output "$DIR/bin/react/lib$2.a" \
    "$PROJECT/build/Release-iphoneos/lib$2.a" \
    "$PROJECT/build-arm64/lib$2.a" \
    "$PROJECT/build/Release-iphonesimulator/lib$2.a" \
    "$PROJECT/build-x86_64/lib$2.a"
}

# clean .a directory
rm -Rf "$DIR/bin/react/**/*.a"
mkdir "$DIR/bin/react/"

# actually build the react submodules
buildLib React React
buildLib Libraries/Text RCTText React
buildLib Libraries/Network RCTNetwork React
buildLib Libraries/WebSocket RCTWebSocket React # needed for debugging

# link everything together
cd $DIR
rm -f "$DIR/libReactNative.a"
libtool -static -o libReactNative.a \
  "$DIR/bin/react/libReact.a" \
  "$DIR/bin/react/libRCTText.a" \
  "$DIR/bin/react/libRCTNetwork.a" \
  "$DIR/bin/react/libRCTWebSocket.a"

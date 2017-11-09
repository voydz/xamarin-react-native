#!/bin/bash
DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd)
CONFIGURATION=${1:-${CONFIGURATION:-Debug}}

function buildLib {
  # prepare project params
  BASE="$DIR/../node_modules/react-native"
  PROJECT="$BASE/$1"

  if [[ -n $4 ]]; then
    HEADER_SIMULATOR="HEADER_SEARCH_PATHS=\"$BASE/$4/build/$3-iphonesimulator/include\""
    HEADER_IPHONE="HEADER_SEARCH_PATHS=\"$BASE/$4/build/$3-iphoneos/include\""
  else
    HEADER_SIMULATOR=""
    HEADER_IPHONE=""
  fi

  # build xcode
  xcodebuild \
    -project "$PROJECT/$2.xcodeproj" \
    -target $2 \
    -sdk iphonesimulator \
    -arch i386 \
    -configuration $3 clean build \
    $HEADER_SIMULATOR
  xcodebuild \
    -project "$PROJECT/$2.xcodeproj" \
    -target $2 \
    -sdk iphonesimulator \
    -arch x86_64 \
    -configuration $3 clean build \
    TARGET_BUILD_DIR="$PROJECT/build-x86_64" \
    BUILT_PRODUCTS_DIR="$PROJECT/build-x86_64" \
    $HEADER_SIMULATOR
  xcodebuild \
    -project "$PROJECT/$2.xcodeproj" \
    -target $2 \
    -sdk iphoneos \
    -arch armv7 \
    -configuration $3 clean build \
    $HEADER_IPHONE
  xcodebuild \
    -project "$PROJECT/$2.xcodeproj" \
    -target $2 \
    -sdk iphoneos \
    -arch arm64 \
    -configuration $3 clean build \
    TARGET_BUILD_DIR="$PROJECT/build-arm64" \
    BUILT_PRODUCTS_DIR="$PROJECT/build-arm64" \
    $HEADER_IPHONE

  # link archs together
  lipo -create -output "$DIR/bin/react/$3/lib$2.a" \
    "$PROJECT/build/$3-iphoneos/lib$2.a" \
    "$PROJECT/build-arm64/lib$2.a" \
    "$PROJECT/build/$3-iphonesimulator/lib$2.a" \
    "$PROJECT/build-x86_64/lib$2.a"
}

# clean .a directory
rm -Rf "$DIR/bin/react/$CONFIGURATION/"
mkdir "$DIR/bin/react/$CONFIGURATION/"

# actually build the react submodules
buildLib React React "$CONFIGURATION"
buildLib Libraries/Text RCTText "$CONFIGURATION" React
buildLib Libraries/Network RCTNetwork "$CONFIGURATION" React
buildLib Libraries/WebSocket RCTWebSocket "$CONFIGURATION" React # needed for debugging

# link everything together
rm -f "$DIR/libReactNative.a"
libtool -static -o "$DIR/libReactNative.a" \
  "$DIR/bin/react/$CONFIGURATION/libReact.a" \
  "$DIR/bin/react/$CONFIGURATION/libRCTText.a" \
  "$DIR/bin/react/$CONFIGURATION/libRCTNetwork.a" \
  "$DIR/bin/react/$CONFIGURATION/libRCTWebSocket.a"

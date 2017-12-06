#!/bin/bash
DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd)
CONFIGURATION=${1:-${CONFIGURATION:-Debug}}

# xcpretty is required to run this script
if ! [ -x "$(command -v xcpretty)" ]; then
  echo 'Error: xcpretty is not installed.' >&2
  exit 1
fi

function buildLib {
  # prepare project params
  BASE="$DIR/../node_modules/react-native"
  PROJECT="$BASE/$1"

  if [[ -n $4 ]]; then
    HEADER_I386="HEADER_SEARCH_PATHS=\"$BASE/$4/build-i386/include\""
    HEADER_X86_64="HEADER_SEARCH_PATHS=\"$BASE/$4/build-x86_64/include\""
    HEADER_ARM64="HEADER_SEARCH_PATHS=\"$BASE/$4/build-arm64/include\""
    HEADER_ARMV7="HEADER_SEARCH_PATHS=\"$BASE/$4/build-armv7/include\""
  else
    HEADER_I386=""
    HEADER_X86_64=""
    HEADER_ARM64=""
    HEADER_ARMV7=""
  fi

  # build xcode
  xcodebuild \
    -project "$PROJECT/$2.xcodeproj" \
    -target $2 \
    -sdk iphonesimulator \
    -arch i386 \
    -configuration $3 clean build \
    TARGET_BUILD_DIR="$PROJECT/build-i386" \
    BUILT_PRODUCTS_DIR="$PROJECT/build-i386" \
    $HEADER_I386 | xcpretty
  xcodebuild \
    -project "$PROJECT/$2.xcodeproj" \
    -target $2 \
    -sdk iphonesimulator \
    -arch x86_64 \
    -configuration $3 clean build \
    TARGET_BUILD_DIR="$PROJECT/build-x86_64" \
    BUILT_PRODUCTS_DIR="$PROJECT/build-x86_64" \
    $HEADER_X86_64 | xcpretty
  xcodebuild \
    -project "$PROJECT/$2.xcodeproj" \
    -target $2 \
    -sdk iphoneos \
    -arch armv7 \
    -configuration $3 clean build \
    TARGET_BUILD_DIR="$PROJECT/build-armv7" \
    BUILT_PRODUCTS_DIR="$PROJECT/build-armv7" \
    $HEADER_ARMV7 | xcpretty
  xcodebuild \
    -project "$PROJECT/$2.xcodeproj" \
    -target $2 \
    -sdk iphoneos \
    -arch arm64 \
    -configuration $3 clean build \
    TARGET_BUILD_DIR="$PROJECT/build-arm64" \
    BUILT_PRODUCTS_DIR="$PROJECT/build-arm64" \
    $HEADER_ARM64 | xcpretty

  # link archs together
  lipo -create -output "$DIR/bin/react/$3/lib$2.a" \
    "$PROJECT/build-armv7/lib$2.a" \
    "$PROJECT/build-arm64/lib$2.a" \
    "$PROJECT/build-i386/lib$2.a" \
    "$PROJECT/build-x86_64/lib$2.a"
}

# clean .a directory
OUTPUT="$DIR/bin/react/$CONFIGURATION/"
rm -Rf "$OUTPUT"
mkdir -p "$OUTPUT"

# actually build the react submodules
buildLib React React "$CONFIGURATION"
buildLib Libraries/Text RCTText "$CONFIGURATION" React
buildLib Libraries/Network RCTNetwork "$CONFIGURATION" React
buildLib Libraries/WebSocket RCTWebSocket "$CONFIGURATION" React # needed for debugging

# link everything together
ASSEMBLY="$DIR/bin/react/$CONFIGURATION/libReactNative.a"
rm -f "$ASSEMBLY"
libtool -static -o "$ASSEMBLY" \
  "$DIR/bin/react/$CONFIGURATION/libReact.a" \
  "$DIR/bin/react/$CONFIGURATION/libRCTText.a" \
  "$DIR/bin/react/$CONFIGURATION/libRCTNetwork.a" \
  "$DIR/bin/react/$CONFIGURATION/libRCTWebSocket.a"

# copy target assembly
cp "$ASSEMBLY" "$DIR/libReactNative.a"

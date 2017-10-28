'use strict';

import React from 'react';
import {
    AppRegistry,
    StyleSheet,
    Text,
    View
} from 'react-native';

class RNHelloWorld extends React.Component {
    render() {
        return (
            <View style={styles.container}>
                <Text style={styles.title}>
                    Hello from Xamarin.iOS!
                </Text>
            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#FFFFFF',
    },
    title: {
        fontSize: 20,
        textAlign: 'center',
        margin: 10,
    },
});

// Module name
AppRegistry.registerComponent('MyReactNativeApp', () => RNHelloWorld);

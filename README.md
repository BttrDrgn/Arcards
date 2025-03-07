# Arcards
An arcade card management app for Android!

# Features
- 💳 Tap your cards to your NFC capable device to quickly add them to your list
    - Currently fully supports Eamuse and Eamuse IC cards, however, any RFID/NFC card can be tapped and added.
- ⚙️ Generate cards directly in the app to use on unofficial networks, some official networks may work

# Installation
- Download the latest release apk [here](https://github.com/BttrDrgn/Arcards/releases/latest)
- Install it!

# Building
## Requirements
- Visual Studio 2022
    - C# modules, .NET MAUI
    - Android SDK
## Instructions
- Open the project solution
- Build
## Release Build
- Generate a keystore with `Generate Keystore.bat {filename} {alias}`
- Fill out the information it prompts
- Run `dotnet publish -f net8.0-android -c Release -p:AndroidKeyStore=true -p:AndroidSigningKeyStore={filename}.keystore -p:AndroidSigningKeyAlias={alias} -p:AndroidSigningKeyPass={password} -p:AndroidSigningStorePass={password}` from the `App` folder
- A signed and non signed APK will be in `./App/bin/Release/net8.0-android/publish`
@echo off

IF "%1"=="" (
    echo Error: No filename provided.
    echo Usage: %~n0 filename alias
    exit /b 1
)

IF "%2"=="" (
    echo Error: No alias provided.
    echo Usage: %~n0 filename alias
    exit /b 1
)

SET filename=%1
SET alias=%2

REM Run the keytool command
keytool -genkeypair -v -keystore %filename%.keystore -alias %alias% -keyalg RSA -keysize 2048 -validity 10000
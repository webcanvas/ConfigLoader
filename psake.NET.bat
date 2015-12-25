@ECHO OFF

SET BATCH_FILE_PATH=%~dp0
:: Override here psake.NET properties setting the related environment variables
:: e.g. SET PSAKE_NET_DEFAULT_PROJECT_DIR=%BATCH_FILE_PATH:~0,-1%\foo\bar
SET PSAKE_NET_RUN_OPENCOVER=true

CMD /C "%BATCH_FILE_PATH%\.build\psake.NET.bat" %*

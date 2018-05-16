@echo off
for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"

set "datestamp=%YYYY%%MM%%DD%"


"C:\Program Files\7-Zip\7z.exe" a "C:\inetpub\wwwroot\%datestamp%-jenkinstest.7z" "C:\inetpub\wwwroot\jenkinstest"

REM robocopy "C:\inetpub\wwwroot\jenkinstest" "C:\inetpub\wwwroot\%datestamp%" /MIR
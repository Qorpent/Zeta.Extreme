..\..\.build\bin\all\bsc.exe --log .output/.new/.log --out .output/.new  --loglevel Info
..\..\.build\bin\all\bsc.exe --log .output/.old/.log --out .output/.old --set-OLDONLY --set-USEALL --loglevel Info
..\..\.build\bin\all\bsc.exe --log .output/.all/.log --out .output/.all --set-USEALL --loglevel Info
..\..\.build\bin\all\bsc.exe --log .output/.demo/.log --out .output/.demo --set-DEMO --loglevel Info
pause
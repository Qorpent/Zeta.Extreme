..\..\.build\bin\all\bsc.exe --log .output/.new/.log --out .output/.new  --loglevel Error
..\..\.build\bin\all\bsc.exe --log .output/.old/.log --out .output/.old --set-OLDONLY --set-USEALL --loglevel Error
..\..\.build\bin\all\bsc.exe --log .output/.all/.log --out .output/.all --set-USEALL --loglevel Error
..\..\.build\bin\all\bsc.exe --log .output/.demo/.log --out .output/.demo --set-DEMO --loglevel Error
pause
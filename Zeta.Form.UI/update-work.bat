set app=\\192.168.26.133\c$\codebase\base\test\zefs

xcopy "images\*.*" "%app%\images\" /s/d/y
xcopy "scripts\*.*" "%app%\scripts\" /s/d/y
xcopy "styles\*.*" "%app%\styles\" /s/d/y
xcopy "favicon.png" "%app%\" /s/d/y
xcopy "zefs-forms.html" "%app%\" /s/d/y
xcopy "zefs-test.html" "%app%\" /s/d/y

set app=\\192.168.26.132\c$\codebase\base\test\zefs

xcopy "images\*.*" "%app%\images\" /s/d/y
xcopy "scripts\*.*" "%app%\scripts\" /s/d/y
xcopy "styles\*.*" "%app%\styles\" /s/d/y
xcopy "favicon.png" "%app%\" /s/d/y
xcopy "zefs-forms.html" "%app%\" /s/d/y
xcopy "zefs-test.html" "%app%\" /s/d/y
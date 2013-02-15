set app=\\192.168.26.133\c$\codebase\base\test\zefs

xcopy "img\*.*" "%app%\img\" /s/d/y
xcopy "js\*.*" "%app%\js\" /s/d/y
xcopy "css\*.*" "%app%\css\" /s/d/y
xcopy "favicon.png" "%app%\" /s/d/y
xcopy "zefs-forms.html" "%app%\" /s/d/y
xcopy "zefs-test.html" "%app%\" /s/d/y
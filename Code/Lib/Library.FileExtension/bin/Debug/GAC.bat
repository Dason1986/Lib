@echo MultiLingual
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -u MultiLingual
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -i %CD%\MultiLingual.dll


@echo Library.Core
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -u Library.Core
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -i %CD%\Library.Core.dll


@echo
@echo Library.DBProvider
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -u Library.DBProvider
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -i %CD%\Library.DBProvider.dll

 


@echo
@echo Library.Design
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -u Library.Design
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -i %CD%\Library.Design.dll



@echo
@echo Library
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -u Library
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -i %CD%\Library.dll



@echo
@echo Library.Draw
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -u Library.Draw
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -i %CD%\Library.Draw.dll


@echo
@echo Library.Web
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -u Library.Web
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -i %CD%\Library.Web.dll



@echo
@echo Library.Win.Controls
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -u Library.Win.Controls
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -i %CD%\Library.Win.Controls.dll


@echo
@echo Library.Win
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -u Library.Win
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" -i %CD%\Library.Win.dll



@echo
@echo Copy Library.DialogDebugger
 copy  /y %CD%\Library.DialogDebugger.dll "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Packages\Debugger\Visualizers"

@pause 
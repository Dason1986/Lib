Debug時支持對象彈出窗口（如DataSet的效果）。需要把Dll複製到以下目錄


C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Packages\Debugger\Visualizers


以下為別一種方式，只有本登陸帳號使用VS才能使用。
 ----------
 Copy the contents of Chrome\src\tools\win\DebugVisualizers\ to %USERPROFILE%\My Documents\Visual Studio 2013\Visualizers\
Start the debugger, and be amazed at the fancy new way it displays your favorite objects. When you edit the file, you shouldn't have to restart all of Visual Studio - it will get re-loaded when you start the debugger.
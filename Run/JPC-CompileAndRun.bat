CALL CompileHelloWorld.bat
..\bin\Mosa.Tools.CreateBootImage.exe IMG\mosaboot-img.config build\bootimage.img
CD jpc
CALL LaunchJPC.bat

..\bin\Mosa.Tools.CreateBootImage.exe VirtualPC\mosaboot-vhd.config build\bootimage.vhd
..\bin\Mosa.Tools.CreateBootImage.exe Vmware\mosaboot-vhd.config build\bootimage.vhd
..\bin\Mosa.Tools.CreateBootImage.exe VirtualBox\mosaboot-vdi.config build\bootimage.vdi
..\bin\Mosa.Tools.CreateBootImage.exe IMG\mosaboot-img.config build\bootimage.img
rem ..\bin\Mosa.Tools.CreateBootImage.exe Floppy\mosaboot-floppy.config build\floppy144.img

copy VirtualPC\MOSA.vmc build\MOSA.vmc
copy VMware\MOSA.vmx build\MOSA.vmx

CALL MakeISOImage.bat

pause

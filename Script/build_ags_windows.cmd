@echo off
setlocal enabledelayedexpansion

echo Build environment: & set

%VS140COMNTOOLS%\vsvars32.bat

echo Build environment (after vcvars32): & set

set AGS=%SYSTEMDRIVE%\AGS
set CACHE=%AGS%\Cache
set BUILD=%AGS%\Build
set FCIV=%AGS%\fciv.exe
set WGET=%AGS%\wget.exe
set BITSADMIN=bitsadmin /transfer bootstrap /download /priority FOREGROUND
set SEVENZIP="C:\Program Files (x86)\7-Zip\7z.exe"
set MSBUILD="C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
set UNINSTALL=HKLM\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall

if not exist %CACHE% mkdir %CACHE%
if not exist %BUILD% mkdir %BUILD%

if not exist %FCIV% (
	echo Downloading fciv...
	%BITSADMIN% https://download.microsoft.com/download/c/f/4/cf454ae0-a4bb-4123-8333-a1b6737712f7/Windows-KB841290-x86-ENU.exe %CACHE%\Windows-KB841290-x86-ENU.exe
	%CACHE%\Windows-KB841290-x86-ENU.exe /Q /T:%AGS% && del /f %AGS%\readme.txt
	if not exist %FCIV% exit /b 1
)

if not exist %WGET% (
	echo Downloading wget...
	%BITSADMIN% http://eternallybored.org/misc/wget/1.19.4/32/wget.exe %CACHE%\wget.exe
	call :VERIFY 3dadb6e2ece9c4b3e1e322e617658b60 %CACHE%\wget.exe && move %CACHE%\wget.exe %AGS%
	if not exist %WGET% exit /b 1
)

set DOWNLOADER=%WGET% --no-check-certificate --quiet -N -P %CACHE%

set INSTALL[0][name]=7-Zip 18.05
set INSTALL[0][version]=*
set INSTALL[0][url]=https://www.7-zip.org/a/7z1805.msi
set INSTALL[0][md5]=83b2e31c6534de4b119ef32c7ab97773
set INSTALL[0][cmd]=msiexec /i %CACHE%\7z1805.msi /qb

set INSTALL[1][name]=Microsoft .NET Framework 1.1
set INSTALL[1][version]=1.1.4322
set INSTALL[1][url]=https://download.microsoft.com/download/a/a/c/aac39226-8825-44ce-90e3-bf8203e74006/dotnetfx.exe
set INSTALL[1][md5]=52456ac39bbb4640930d155c15160556
set INSTALL[1][cmd]=start /wait %CACHE%\dotnetfx.exe /q:a /c:"install.exe /qb /l"

set INSTALL[2][name]=Microsoft DirectX SDK (August 2007)
set INSTALL[2][version]=9.20.1057
set INSTALL[2][url]=https://download.microsoft.com/download/3/3/f/33f1af6e-c61b-4f14-a0de-3e9096ed4b3a/dxsdk_aug2007.exe
set INSTALL[2][md5]=e866e58a5cbfc98b3880261b5ae78529
set INSTALL[2][cmd]=call :INSTALLDIRECTXSDK

set INSTALL[3][name]=Microsoft Build Tools 14.0 (x86)
set INSTALL[3][version]=14.0.25420
set INSTALL[3][url]=http://download.microsoft.com/download/5/f/7/5f7acaeb-8363-451f-9425-68a90f98b238/visualcppbuildtools_full.exe
set INSTALL[3][md5]=8d4afd3b226babecaa4effb10d69eb2e
set INSTALL[3][cmd]=start /wait %CACHE%\visualcppbuildtools_full.exe /Passive /Full

for /l %%n in (0,1,3) do (
	echo Checking installation: !INSTALL[%%n][name]!
	call :ISINSTALLED "!INSTALL[%%n][name]!" "!INSTALL[%%n][version]!" || (
		for %%i in (!INSTALL[%%n][url]!) do (
			if not exist %CACHE%\%%~nxi %DOWNLOADER% !INSTALL[%%n][url]!
			call :VERIFY !INSTALL[%%n][md5]! %CACHE%\%%~nxi || exit /b 1
			echo --^> installing...
			!INSTALL[%%n][cmd]!
			call :ISINSTALLED "!INSTALL[%%n][name]!" "!INSTALL[%%n][version]!" || exit /b 1
		)
	)
)

set DOWNLOAD[0][name]=OpenGL 1.2 and above compatibility profile and extension interfaces
set DOWNLOAD[0][url]=https://www.khronos.org/registry/OpenGL/api/GL/glext.h
set DOWNLOAD[0][md5]=51062c2411be2934e940f74cfd09653f
set DOWNLOAD[0][testpath]=%BUILD%\OpenGL\GL\glext.h
set DOWNLOAD[0][cmd]=robocopy %CACHE% %BUILD%\OpenGL\GL\ glext.h

set DOWNLOAD[1][name]=OpenGL core profile and ARB extension interfaces
set DOWNLOAD[1][url]=https://www.khronos.org/registry/OpenGL/api/GL/glcorearb.h
set DOWNLOAD[1][md5]=d6aec607e4e41f0969d9e874e9a4a3b8
set DOWNLOAD[1][testpath]=%BUILD%\OpenGL\GL\glcorearb.h
set DOWNLOAD[1][cmd]=robocopy %CACHE% %BUILD%\OpenGL\GL\ glcorearb.h

set DOWNLOAD[2][name]=GLX 1.3 and above API and GLX extension interfaces
set DOWNLOAD[2][url]=https://www.khronos.org/registry/OpenGL/api/GL/glxext.h
set DOWNLOAD[2][md5]=0fba05d6c852da8ec911e05eab5892b2
set DOWNLOAD[2][testpath]=%BUILD%\OpenGL\GL\glxext.h
set DOWNLOAD[2][cmd]=robocopy %CACHE% %BUILD%\OpenGL\GL\ glxext.h

set DOWNLOAD[3][name]=WGL extension interfaces
set DOWNLOAD[3][url]=https://www.khronos.org/registry/OpenGL/api/GL/wglext.h
set DOWNLOAD[3][md5]=789262fd13fc9009e4606130b2ab0c80
set DOWNLOAD[3][testpath]=%BUILD%\OpenGL\GL\wglext.h
set DOWNLOAD[3][cmd]=robocopy %CACHE% %BUILD%\OpenGL\GL\ wglext.h

set DOWNLOAD[4][name]=KHR platform
set DOWNLOAD[4][url]=https://www.khronos.org/registry/EGL/api/KHR/khrplatform.h
set DOWNLOAD[4][md5]=66cc4235af5b34a2bab8064904e80855
set DOWNLOAD[4][testpath]=%BUILD%\OpenGL\KHR\khrplatform.h
set DOWNLOAD[4][cmd]=robocopy %CACHE% %BUILD%\OpenGL\KHR\ khrplatform.h

for /l %%n in (0,1,4) do (
	echo Checking download: !DOWNLOAD[%%n][name]!
	if not exist !DOWNLOAD[%%n][testpath]! (
		for %%i in (!DOWNLOAD[%%n][url]!) do (
			%DOWNLOADER% !DOWNLOAD[%%n][url]!
rem call :VERIFY !DOWNLOAD[%%n][md5]! %CACHE%\%%~nxi || exit /b 1
			!DOWNLOAD[%%n][cmd]!
		)
	)
)

set CLONE[0][name]=vcpkg
set CLONE[0][url]=https://github.com/Microsoft/vcpkg
set CLONE[0][branch]=master

set CLONE[1][name]=Allegro 4.4.2 (patched)
set CLONE[1][url]=https://github.com/adventuregamestudio/lib-allegro.git
set CLONE[1][branch]=allegro-4.4.2-agspatch

set CLONE[2][name]=Alfont (patched)
set CLONE[2][url]=https://github.com/adventuregamestudio/lib-alfont
set CLONE[2][branch]=alfont-1.9.1-agspatch

set CLONE[3][name]=AGS
set CLONE[3][url]=https://github.com/adventuregamestudio/ags
set CLONE[3][branch]=master

for /l %%n in (0,1,3) do (
	echo Checking git clone: !CLONE[%%n][name]!
	pushd %BUILD% && git clone -b !CLONE[%%n][branch]! !CLONE[%%n][url]!
	popd
)

rem THIS ENV IS NOT SET ON AZURE FOR SOME REASON
rem if not defined VCTargetsPath (
rem 	echo.
rem 	echo Build environment variables are not set!
rem 	echo re-run from Visual C++ build command prompt
rem 	goto :END
rem )

set UseEnv=true
set INCLUDE=%BUILD%\OpenGL;%BUILD%\lib-allegro\include;%BUILD%\lib-allegro\build\VS2015\include;%INCLUDE%
set LIB=%BUILD%\Xiph;%BUILD%\lib-alfont\lib\msvc\vs2015\static;%BUILD%\lib-allegro\build\VS2015\lib;%LIB%;C:\Program Files (x86)\Microsoft DirectX SDK (August 2007)\Lib\x86\

echo building Xiph libraries...
if not exist %BUILD%\Xiph mkdir %BUILD%\Xiph
pushd %BUILD%\vcpkg
if not exist vcpkg.exe call bootstrap-vcpkg.bat
vcpkg.exe install libogg:x86-windows-static libtheora:x86-windows-static libvorbis:x86-windows-static
copy /Y installed\x86-windows-static\lib\ogg.lib %BUILD%\Xiph\libogg_static.lib
copy /Y installed\x86-windows-static\lib\theora.lib %BUILD%\Xiph\libtheora_static.lib
copy /Y installed\x86-windows-static\lib\vorbis.lib %BUILD%\Xiph\libvorbis_static.lib
copy /Y installed\x86-windows-static\lib\vorbisfile.lib %BUILD%\Xiph\libvorbisfile_static.lib
popd

echo building Allegro...
pushd %BUILD%\lib-allegro\build\VS2015
%MSBUILD% ALLEGRO.sln /p:PlatformToolset=v140 /p:Configuration="Release MD" /p:Platform=Win32 /maxcpucount /nologo
%MSBUILD% ALLEGRO.sln /p:PlatformToolset=v140 /p:Configuration="Release MT" /p:Platform=Win32 /maxcpucount /nologo
%MSBUILD% ALLEGRO.sln /p:PlatformToolset=v140 /p:Configuration="Debug MD" /p:Platform=Win32 /maxcpucount /nologo
%MSBUILD% ALLEGRO.sln /p:PlatformToolset=v140 /p:Configuration="Debug MT" /p:Platform=Win32 /maxcpucount /nologo
popd

echo building Alfont...
pushd %BUILD%\lib-alfont\build\VS2015
%MSBUILD% alfont_static.vcxproj /p:PlatformToolset=v140 /p:Configuration="MT" /p:Platform=Win32 /maxcpucount /nologo
%MSBUILD% alfont_static.vcxproj /p:PlatformToolset=v140 /p:Configuration="Debug MT" /p:Platform=Win32 /maxcpucount /nologo
popd

pushd %BUILD%\ags\Solutions
%MSBUILD% Engine.sln /p:PlatformToolset=v140 /p:Configuration=Release /p:Platform=Win32 /maxcpucount /nologo
popd

goto :END

:INSTALLDIRECTXSDK
%SEVENZIP% e -o%TEMP% -y %CACHE%\dxsdk_aug2007.exe && %SEVENZIP% x -o%TEMP%\dxsdk_aug2007 -y %TEMP%\dxsdk_aug2007.exe
msiexec /i %TEMP%\dxsdk_aug2007\Microsoft_DirectX_SDK.msi /qb 
exit /b 0

:VERIFY
echo --^> verifying %2

for /f "tokens=1 skip=3" %%j in ('%FCIV% %2') do (
	if not %1==%%j (
		echo --^> verification failed md5:%%j
			exit /b 1
		)
	)
)

echo --^> verification passed
exit /b 0

:ISINSTALLED
echo --^> looking for %1 version %2

for /f "tokens=1,2,*" %%i in ('reg query %UNINSTALL% /s /f Display /t REG_SZ') do (
	if %%i==DisplayName (
		set DISPLAYNAME=%%k
	) else if %%i==DisplayVersion (
		set DISPLAYVERSION=%%k
	) else (
		set DISPLAYNAME=
		set DISPLAYVERSION=
	)

	if defined DISPLAYNAME if defined DISPLAYVERSION (
		if %1=="!DISPLAYNAME!" if %2=="!DISPLAYVERSION!" (
			echo --^> installation found
			exit /b 0
		)
	)

	if defined DISPLAYNAME if %1=="!DISPLAYNAME!" if %2=="*" (
		echo --^> installation found - any
		exit /b 0
	)
)

echo --^> no installation was found
exit /b 1

:END
endlocal

echo loading vcvarsall from VS2015 installation path

echo if you want to pack the code with VS2013 you must change the path to:
echo "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\vcvarsall.bat"

call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat" 
echo.

echo ----------------------------------- CORE -----------------------------------------

echo.
echo -- Core DEBUG --
MsBuild Core/core.csproj /property:Configuration=Debug /property:Platform=AnyCPU /t:Clean,Build /verbosity:minimal
echo.
echo -- Core RELEASE --
MsBuild Core/core.csproj /property:Configuration=Release /property:Platform=AnyCPU /t:Clean,Build /verbosity:minimal

nuget pack Core/core.csproj
move Core/*.nupkg /
echo --Core packed--


echo ----------------------------------- WPF -----------------------------------------

if not exist "WpfView/bin" mkdir WpfView/bin

echo.
echo -- WPF DEBUG --
if not exist "WpfView/bin/Debug" mkdir WpfView/bin/net403
MsBuild WpfView/wpfview.csproj /property:Configuration=Debug;Platform=AnyCPU;OutputPath=bin/Debug /t:Clean,Build /verbosity:minimal

echo.
echo -- WPF 403 --
if not exist "WpfView/bin/net403" mkdir WpfView/bin/net403
MsBuild WpfView/wpfview.csproj /property:Configuration=net40;Platform=AnyCPU;OutputPath=bin/net403 /t:Clean,Build /verbosity:minimal

echo.
echo -- WPF 45 --
if not exist "WpfView/bin/net45" mkdir WpfView/bin/net45
MsBuild WpfView/wpfview.csproj /property:Configuration=net45;Platform=AnyCPU;OutputPath=bin/net45 /t:Clean,Build /verbosity:minimal

echo.
echo -- WPF 451 --
if not exist "WpfView/bin/net451" mkdir WpfView/bin/net451
MsBuild WpfView/wpfview.csproj /property:Configuration=net451;Platform=AnyCPU;OutputPath=bin/net451 /t:Clean,Build /verbosity:minimal

echo.
echo -- WPF 452 --
if not exist "WpfView/bin/net452" mkdir WpfView/bin/net452
MsBuild WpfView/wpfview.csproj /property:Configuration=net452;Platform=AnyCPU;OutputPath=bin/net452 /t:Clean,Build /verbosity:minimal

echo.
echo -- WPF 46 --
if not exist "WpfView/bin/net46" mkdir WpfView/bin/net46
MsBuild WpfView/wpfview.csproj /property:Configuration=net46;Platform=AnyCPU;OutputPath=bin/net46 /t:Clean,Build /verbosity:minimal

echo.
echo -- WPF 461 --
if not exist "WpfView/bin/net461" mkdir WpfView/bin/net461
MsBuild WpfView/wpfview.csproj /property:Configuration=net461;Platform=AnyCPU;OutputPath=bin/net461 /t:Clean,Build /verbosity:minimal

nuget pack WpfView/WpfView.csproj -IncludeReferencedProjects
move WpfView/*.nupkg /
echo --WPF packed--


echo ----------------------------------- WinForms -----------------------------------------

if not exist "WinFormsView/bin" mkdir WinFormsView/bin

echo.
echo -- WinForms DEBUG --
if not exist "WinFormsView/bin/Debug" mkdir WinFormsView/bin/net403
MsBuild WinFormsView/winformsview.csproj /property:Configuration=Debug;Platform=AnyCPU;OutputPath=bin/Debug /t:Clean,Build /verbosity:minimal

echo.
echo -- WinForms 403 --
if not exist "WinFormsView/bin/net403" mkdir WinFormsView/bin/net403
MsBuild WinFormsView/winformsview.csproj /property:Configuration=net40;Platform=AnyCPU;OutputPath=bin/net403 /t:Clean,Build /verbosity:minimal

echo.
echo -- WinForms 45 --
if not exist "WinFormsView/bin/net45" mkdir WinFormsView/bin/net45
MsBuild WinFormsView/winformsview.csproj /property:Configuration=net45;Platform=AnyCPU;OutputPath=bin/net45 /t:Clean,Build /verbosity:minimal

echo.
echo -- WinForms 451 --
if not exist "WinFormsView/bin/net451" mkdir WinFormsView/bin/net451
MsBuild WinFormsView/winformsview.csproj /property:Configuration=net451;Platform=AnyCPU;OutputPath=bin/net451 /t:Clean,Build /verbosity:minimal

echo.
echo -- WinForms 452 --
if not exist "WinFormsView/bin/net452" mkdir WinFormsView/bin/net452
MsBuild WinFormsView/winformsview.csproj /property:Configuration=net452;Platform=AnyCPU;OutputPath=bin/net452 /t:Clean,Build /verbosity:minimal

echo.
echo -- WinForms 46 --
if not exist "WinFormsView/bin/net46" mkdir WinFormsView/bin/net46
MsBuild WinFormsView/winformsview.csproj /property:Configuration=net46;Platform=AnyCPU;OutputPath=bin/net46 /t:Clean,Build /verbosity:minimal

echo.
echo -- WinForms 461 --
if not exist "WinFormsView/bin/net461" mkdir WinFormsView/bin/net461
MsBuild WinFormsView/winformsview.csproj /property:Configuration=net461;Platform=AnyCPU;OutputPath=bin/net461 /t:Clean,Build /verbosity:minimal

nuget pack WinFormsView/winformsview.csproj -IncludeReferencedProjects
move WinForms/*.nupkg /
echo --WPF packed--

echo.
echo -- LiveCharts is packed!--
echo.

pause

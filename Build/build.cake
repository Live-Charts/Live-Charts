//Addins
#addin Cake.VersionReader
#addin Cake.FileHelpers
#addin Cake.Git

//Variables
var buildType = Argument("Configuration", "Release");
var target = Argument ("target", "Default");
var configuration = "AnyCPU";

Task("OutputArguments")
    .Does(() => 
    {
        Information("Target: " + target);
        Information("Build Type: " + buildType);
    });

Task("Core")
    .Does(() =>
    {
        Information("Building Core.PCL...");
        BuildProject("./Core/Core.csproj", "./bin/Release", buildType, configuration, "v4.5");

        Information("Building Core.Net40...");
        BuildProject("./Core40/Core40.csproj", "./bin/Net40", buildType, configuration, "v4.0");
        
		Information("Building Core.Net45...");
        BuildProject("./Core40/Core40.csproj", "./bin/Net45", buildType, configuration, "v4.5");

        Information("Packing Core...");
        NugetPack("./Core/Core.nuspec", "./Core/bin/Release/LiveCharts.dll");

        Information("-- Core Packed --");
    });

Task("WPF")
    .Does(() =>
    {
        var wpfPath = "./WpfView/wpfview.csproj";

        Information("Building Wpf.Debug...");
        BuildProject(wpfPath, "./bin/Debug", "Debug", configuration, "v4.0");

        Information("Building Wpf.Net40...");
        BuildProject(wpfPath, "./bin/net403", "Release", configuration, "v4.0");

        Information("Building Wpf.Debug...");
        BuildProject(wpfPath, "./bin/net45", "Release", configuration, "v4.5");

        Information("Packing Wpf...");
        NugetPack("./WpfView/WpfView.nuspec", "./WpfView/bin/net403/LiveCharts.Wpf.dll");
        
        Information("-- WPF Packed --");
    });

Task("WinForms")
    .Does(() => 
    {
        var formsPath = "./WinFormsView/WinFormsView.csproj";

        Information("Building WinForms.Debug...");
        BuildProject(formsPath, "./bin/Debug", "Debug", configuration, "v4.0");

        Information("Building WinForms.Net40...");
        BuildProject(formsPath, "./bin/net403", "Release", configuration, "v4.0");

        Information("Building WinForms.Debug...");
        BuildProject(formsPath, "./bin/net45", "Release", configuration, "v4.5");

        Information("Packing WinForms...");
        NugetPack("./WinFormsView/WinFormsView.nuspec", "./WinFormsView/bin/net403/LiveCharts.WinForms.dll");

        Information("-- WinForms Packed --");
    });

Task("UWP")
    .Does(() =>
    {
        Information("Building UWP...");        
        BuildProject("./UwpView/UwpView.csproj", "./bin/AnyCPU", buildType, "AnyCPU");

        Information("Packing UWP...");
        NugetPack("./UwpView/UwpView.nuspec", "./UwpView/bin/AnyCPU/LiveCharts.Uwp.dll");

        Information("-- UWP Packed --");
    });

Task("Default")
    .IsDependentOn("OutputArguments")
	.IsDependentOn("Core")
    .IsDependentOn("WPF")
    .IsDependentOn("WinForms")
	.IsDependentOn("UWP");

//Entry point for Cake build
RunTarget (target);

//Helper Methods

//Build a project
public void BuildProject(string path, string outputPath, string configuration, 
								string platform, string targetVersion = null)
{
    Information("Building " + path);
    try
    {
        DotNetBuild(path, settings => 
		{ 
			settings.SetConfiguration(configuration)
				.WithProperty("Platform", platform)
				.WithTarget("Clean,Build")
				.WithProperty("OutputPath", outputPath)
                .SetVerbosity(Cake.Core.Diagnostics.Verbosity.Minimal);

            if (targetVersion != null) 
				settings.WithProperty("TargetFrameworkVersion", targetVersion);
        });
    }
    catch(Exception ex)
    {
        Error("An error occurred while trying to build {0} with {1}", path, configuration);
    }

    Information("Build completed");
}

//Pack into Nuget package
public void NugetPack(string nuspecPath, string mainBinaryPath)
{
    Information("Packing " + nuspecPath);
    var binary = MakeAbsolute(File(mainBinaryPath));
    var binaryVersion = GetFullVersionNumber(binary);
    ReplaceRegexInFiles(nuspecPath, "0.0.0.0", binaryVersion);
    
    NuGetPack(nuspecPath, new NuGetPackSettings{
        Verbosity = NuGetVerbosity.Quiet,
        OutputDirectory = "./"
    });

    //We revert the nuspec file to the check out one, otherwise we cannot build it again with a new version
    //This should rather use XmlPoke but cannot yet get it to work
    var fullNuspecPath = MakeAbsolute(File(nuspecPath));
    GitCheckout("./", fullNuspecPath);

    Information("Packing completed");
}
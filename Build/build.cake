//Addins
#addin Cake.VersionReader
#addin Cake.FileHelpers
#addin Cake.Git

//Variables
var buildType = Argument("Configuration", "Release");
var target = Argument ("target", "Default");
var configuration = "AnyCPU";
var releaseDir = "../Dist/";
var sourceDir = "../Src/";
var nugetOutDir = "../";

Task("OutputArguments")
    .Does(() => 
    {
        Information("Target: " + target);
        Information("Build Type: " + buildType);
    });

Task("Core")
    .Does(() =>
    {
		var frameworks = new [] {"net40", "net45", "net46", "netstandard2.0"};
		string version = string.Empty;

		for (var i = 0; i < frameworks.Length; i++) 
		{
			var framework = frameworks[i];
			DotNetCoreBuild(sourceDir + "LiveCharts/LiveCharts.csproj", 
				new DotNetCoreBuildSettings
				{
					Framework = framework,
					Configuration = "Release",
					OutputDirectory = releaseDir + framework
				});

			if (version == string.Empty) 
			{
				version = GetFullVersionNumber(releaseDir + framework + "/LiveCharts.dll");
			}
			Information("-- " + framework + " Built --");
		}

		NuGetPack("./Nuget/Core.nuspec", new NuGetPackSettings
		{
			Verbosity = NuGetVerbosity.Quiet,
			OutputDirectory = nugetOutDir,
			Version = version
		});

        Information("-- LiveCharts Packed --");
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

        Information("-- WinForms Packed --");
    });

Task("UWP")
    .Does(() =>
    {
        Information("Building UWP...");        
        BuildProject("./UwpView/UwpView.csproj", "./bin/AnyCPU", buildType, "AnyCPU");

        Information("Packing UWP...");

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
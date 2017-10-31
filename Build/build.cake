//Addins
#addin Cake.VersionReader
#addin Cake.FileHelpers

//Variables
var buildType = Argument("Configuration", "Release");
var target = Argument ("target", "Default");
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
		var frameworks = new [] {"net40", "net45", "netstandard2.0"};

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

			Information("\n===== LiveCharts(" + framework + ") Built ======");
		}

		NuGetPack("./Nuget/Core.nuspec", new NuGetPackSettings
		{
			Verbosity = NuGetVerbosity.Quiet,
			OutputDirectory = nugetOutDir,
			Version = GetFullVersionNumber(releaseDir + frameworks[0] + "/LiveCharts.dll")
		});

        Information("\n===== LiveCharts Packed =====");
    });

Task("WPF")
    .Does(() =>
    {
		var frameworks = new [] {"net40", "net45"};

		for (var i = 0; i < frameworks.Length; i++)
		{
			var framework = frameworks[i];
			DotNetBuild(sourceDir + "LiveCharts.Wpf/LiveCharts.Wpf.csproj", 
				settings => 
				{ 
					settings.SetConfiguration(buildType)
						.WithTarget("Clean,Build")
						.WithProperty("Platform", "AnyCPU")
						.WithProperty("OutputPath", "../" + releaseDir + framework)
						.WithProperty("TargetFrameworkVersion", MapFrameworkToCompiler(framework))
						.WithProperty("DocumentationFile", "../" + releaseDir + framework + "/LiveCharts.Wpf.xml")
						.SetVerbosity(Cake.Core.Diagnostics.Verbosity.Minimal);
				});

			Information("\n===== LiveCharts.Wpf(" + framework + ") Built ======");
		}
        
		NuGetPack("./Nuget/Wpf.nuspec", new NuGetPackSettings
		{
			Verbosity = NuGetVerbosity.Quiet,
			OutputDirectory = nugetOutDir,
			Version = GetFullVersionNumber(releaseDir + frameworks[0] + "/LiveCharts.Wpf.dll")
		});

		Information("\n===== LiveCharts.Wpf Packed =====");
    });

Task("WinForms")
    .Does(() => 
    {
        var formsPath = "./WinFormsView/WinFormsView.csproj";

        Information("Building WinForms.Debug...");
        BuildProject(formsPath, "./bin/Debug", "Debug", "AnyCPU", "v4.0");

        Information("Building WinForms.Net40...");
        BuildProject(formsPath, "./bin/net403", "Release", "AnyCPU", "v4.0");

        Information("Building WinForms.Debug...");
        BuildProject(formsPath, "./bin/net45", "Release", "AnyCPU", "v4.5");

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

public string MapFrameworkToCompiler(string framework) 
{
	switch (framework) 
	{
		case "net40":
			return "v4.0";
		case "net45":
			return "v4.5";
		default:
			return "v4.0";
	}
}

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
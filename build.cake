//Addins
#addin Cake.VersionReader
#addin Cake.FileHelpers
#addin Cake.Git

//Variables
var target = Argument ("target", "Default");
var buildType = Argument("Configuration", "Release");

var corePath = "./Core/Core.csproj";
var coreSpec = "./Core/Core.nuspec";
var coreBinary = "./Core/bin/Release/LiveCharts.dll";

//Main Tasks

Task("OutputArguments")
    .Does(() =>
    {
        Information("Target: " + target);
        Information("Build Type: " + buildType);
    });

Task("Core")
    .Does(() =>
    {
        Information("-- Core - " + buildType.ToUpper() + " --");
        BuildProject(corePath);
        
        if(buildType == "Release")
        {
            NugetPack(coreSpec, coreBinary);
        }
        Information("-- Core Packed --");
    });

Task("Default")
    .IsDependentOn("OutputArguments")
	.IsDependentOn("Core");

//Entry point for Cake build
RunTarget (target);

//Helper Methods

//Build a project
public void BuildProject(string path)
{
    Information("Building " + path);
    DotNetBuild(path, settings =>
    settings.SetConfiguration(buildType)
    .WithProperty("Platform", "AnyCPU")
    .WithTarget("Clean,Build")
    .SetVerbosity(Cake.Core.Diagnostics.Verbosity.Minimal)
    );
    Information("Build completed");
}

//Pack into Nuget package
public void NugetPack(string nuspecPath, string mainBinaryPath)
{
    Information("Packing " + nuspecPath);
    var binary = MakeAbsolute(File(mainBinaryPath));
    var binaryVersion = GetVersionNumber(binary);
    ReplaceRegexInFiles(nuspecPath, "0.0.0", binaryVersion);
    
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
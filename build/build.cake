#tool "nuget:?package=GitVersion.CommandLine"
#addin "nuget:?package=Cake.Incubator"
#addin "nuget:?package=Cake.Npm"
#tool "nuget:?package=NUnit.ConsoleRunner"
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var verbosity = Argument("Verbosity", "Normal");
var outputPath = Argument("OutputPath","../output/");
var incrementalValue = Argument("buildId",1);

var relativeOutputPathOfPackage = $"../bin/{configuration}/";
GitVersion gitVersion = new GitVersion();


///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   EnsureDirectoryExists(outputPath);
   CleanDirectory(new DirectoryPath(outputPath));
   CleanDirectory(new DirectoryPath(relativeOutputPathOfPackage));
   Information($"Output Path: {outputPath}");
   Information($"Package Path: {relativeOutputPathOfPackage}");   
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
Task("Build")
.IsDependentOn("Copy to Output");

Task("GitVersion")
.Does(() => {
    gitVersion = GitVersion();
    Information("GitVersion Task Succcessfull");
    Information(gitVersion.Dump());
});
Task("Restore Solution")
.IsDependentOn("GitVersion")
    .Does(() =>
{
    NuGetRestore("Ninject.AzureFunctions.sln");
    DotNetCoreRestore("../source/Ninject.AzureFunctions/Ninject.AzureFunctions.csproj");
});

Task("Build Solution")
.IsDependentOn("Restore Solution")
.Does(() => {
    var settings =  new MSBuildSettings(){
       Configuration = configuration,
       Verbosity = (Verbosity)Enum.Parse(typeof(Verbosity),verbosity),
       ToolVersion = MSBuildToolVersion.VS2017
   };
   Information("Verbosity: " + settings.Verbosity);
   settings.Properties["PackageVersion"] = new []{gitVersion.NuGetVersion + "-" + incrementalValue};
   settings.Properties["ContinueOnError"] = new []{"false"};
   MSBuild("../source/Ninject.AzureFunctions/Ninject.AzureFunctions.csproj",settings);
});

Task("Test Solution")
.IsDependentOn("Build Solution")
    .Does(() =>
{
    // var settings =  new MSBuildSettings(){
    //    Configuration = configuration,
    //    Verbosity = (Verbosity)Enum.Parse(typeof(Verbosity),verbosity),
    //    ToolVersion = MSBuildToolVersion.VS2017,
    //    PlatformTarget = PlatformTarget.x64
    // };
    // settings.Properties["ContinueOnError"] = new []{"false"};
    // MSBuild("../source/Ninject.AzureFunctions.Tests/Ninject.AzureFunctions.Tests.csproj",settings);
    // NUnit3($"../source/Ninject.AzureFunctions.Tests/bin/x64/Release/netstandard2.0/Ninject.AzureFunctions.Tests.dll");
});

Task("Copy to Output")
.IsDependentOn("Test Solution")
.Does(() => {
    Information(relativeOutputPathOfPackage + "*.nupkg");
    CopyFiles(relativeOutputPathOfPackage  + "*.nupkg",outputPath);
    DeleteFiles(outputPath+"*.0.0.0.*");
});
RunTarget(target);

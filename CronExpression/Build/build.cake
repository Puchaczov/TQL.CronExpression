///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var version = Argument<string>("version", string.Empty);

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutions = GetFiles(@"../*.sln");
var solutionPaths = solutions.Select(solution => solution.GetDirectory());
var tempDirName = ".autobuild";
var tempDirPath = string.Format(@"{0}\Build\{1}", solutionPaths.First(), tempDirName);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
});

Teardown(() =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Description("Cleans all directories that are used during the build process.")
    .Does(() =>
{
    // Clean solution directories.
    foreach(var path in solutionPaths)
    {
        Information("Cleaning {0}", path);
        CleanDirectories(path + "/**/bin/" + configuration);
        CleanDirectories(path + "/**/obj/" + configuration);
    }
});

Task("Restore")
    .Description("Restores all the NuGet packages that are used by the specified solution.")
    .Does(() =>
{
    // Restore all NuGet packages.
    foreach(var solution in solutions)
    {
        Information("Restoring {0}...", solution);
        NuGetRestore(solution);
    }
});

Task("CreateFolderForBinaries")
	.Description("Moving generated files to folder")
	.Does(() =>{
		CreateDirectory(tempDirPath);
		CleanDirectory(tempDirPath);
	});

Task("Build")
    .Description("Builds all the different parts of the project.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("CreateFolderForBinaries")
    .Does(() =>
{
    // Build all solutions.
    foreach(var solution in solutions)
    {
        Information("Building {0}", solution);
        MSBuild(solution, settings =>
            settings.SetPlatformTarget(PlatformTarget.MSIL)
                .WithProperty("TreatWarningsAsErrors","false")
				.WithProperty(@"OutputPath", tempDirPath)
                .WithTarget("Build")
                .SetConfiguration(configuration));
    }
	DeleteFiles(string.Format(@"{0}\*.exe", tempDirPath));
	DeleteFiles(string.Format(@"{0}\*.pdb", tempDirPath));
	DeleteFiles(string.Format(@"{0}\*.Tests.dll", tempDirPath));
	DeleteFiles(string.Format(@"{0}\*.CONFIG", tempDirPath));
});

///////////////////////////////////////////////////////////////////////////////
// NUGET
///////////////////////////////////////////////////////////////////////////////

Task("Create Nuget Package")
	.Description("Create package for nuget")
	.IsDependentOn("Build")
	.Does(()=>{
		var nugetSettings = new NuGetPackSettings{
			Id = "Cron.Converter",
			Authors = new[] {"Jakub Pucha³a"},
			Owners = new[] {"Jakub Pucha³a"},
			Description = "Contains parser and evaluator for cron expressions",
			ProjectUrl = new Uri("https://github.com/Puchaczov/CronExpression"),
			LicenseUrl = new Uri("https://github.com/Puchaczov/CronExpression/blob/master/LICENSE"),
			Copyright = "Jakub Pucha³a",
			Tags = new [] { "Cron", "Parser", "Expressions", "Evaluator" },
			RequireLicenseAcceptance = true,
			Symbols = false,
			BasePath = "./.autobuild",
			Files = new[] {
				new NuSpecContent{Source = ".autobuild/Cron.Converter.dll", Target = ".autobuild"}
			},
			OutputDirectory = "./.autobuild"
		};

		NuGetPack("./Cron.Converter.nuspec", nugetSettings);
	});

///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .Description("This is the default task which will be ran if no specific target is passed in.")
    .IsDependentOn("Build");
	
///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
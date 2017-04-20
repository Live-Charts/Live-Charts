
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("LiveCharts.Wpf")]
[assembly: AssemblyDescription("LiveCharts view for wpf")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("LiveCharts.Wpf")]
[assembly: AssemblyCopyright("Copyright © 2016 Alberto Rodríguez Orozco")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

//In order to begin building localizable applications, set 
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly:ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                             //(used if a resource is not found in the page, 
                             // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                      //(used if a resource is not found in the page, 
                                      // app, or any theme specific resource dictionaries)
)]


// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.9.6")]
[assembly: AssemblyFileVersion("0.9.6")]

#if DEBUG

[assembly: InternalsVisibleTo("LiveCharts.Geared")]
[assembly: InternalsVisibleTo("LiveCharts.WinForms")]

#endif

#if TRACE

[assembly: InternalsVisibleTo("LiveCharts.Geared,PublicKey=0024000004800000940000000602000000240000525341310004000001000100dd31828d6e6f53eebd68f2c94ddb06ffb377517cbc8a0ff1c5d580c0a9b961b868e3354190f4b07d46f551ec050bf19eb88ca0d40abe7881d5fa1407d53ac2c0148950e1cc763178a1e739ebc9bbcc9794f76b8636bfbcd0b0dbcb61b9468b9f66ffc17695bfea71e199db6c58ce8d8dda923cb43ef9f7e7a3d3d5e6188a9dc9")]
[assembly: InternalsVisibleTo("LiveCharts.WinForms,PublicKey=0024000004800000940000000602000000240000525341310004000001000100dd31828d6e6f53eebd68f2c94ddb06ffb377517cbc8a0ff1c5d580c0a9b961b868e3354190f4b07d46f551ec050bf19eb88ca0d40abe7881d5fa1407d53ac2c0148950e1cc763178a1e739ebc9bbcc9794f76b8636bfbcd0b0dbcb61b9468b9f66ffc17695bfea71e199db6c58ce8d8dda923cb43ef9f7e7a3d3d5e6188a9dc9")]

#endif
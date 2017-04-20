using System.Resources;
using System.Reflection;
using System.Runtime.CompilerServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("LiveCharts")]
[assembly: AssemblyDescription("Core library for LiveCharts")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("LiveCharts")]
[assembly: AssemblyCopyright("Copyright © 2016 Alberto Rodríguez Orozco")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en")]

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

[assembly: InternalsVisibleTo("LiveCharts.Wpf")]
[assembly: InternalsVisibleTo("LiveCharts.Uwp")]
[assembly: InternalsVisibleTo("LiveCharts.Geared")]

#endif

#if TRACE

[assembly: InternalsVisibleTo("LiveCharts.Uwp,PublicKey=0024000004800000940000000602000000240000525341310004000001000100dd31828d6e6f53eebd68f2c94ddb06ffb377517cbc8a0ff1c5d580c0a9b961b868e3354190f4b07d46f551ec050bf19eb88ca0d40abe7881d5fa1407d53ac2c0148950e1cc763178a1e739ebc9bbcc9794f76b8636bfbcd0b0dbcb61b9468b9f66ffc17695bfea71e199db6c58ce8d8dda923cb43ef9f7e7a3d3d5e6188a9dc9")]
[assembly: InternalsVisibleTo("LiveCharts.Wpf,PublicKey=0024000004800000940000000602000000240000525341310004000001000100dd31828d6e6f53eebd68f2c94ddb06ffb377517cbc8a0ff1c5d580c0a9b961b868e3354190f4b07d46f551ec050bf19eb88ca0d40abe7881d5fa1407d53ac2c0148950e1cc763178a1e739ebc9bbcc9794f76b8636bfbcd0b0dbcb61b9468b9f66ffc17695bfea71e199db6c58ce8d8dda923cb43ef9f7e7a3d3d5e6188a9dc9")]
[assembly: InternalsVisibleTo("LiveCharts.Geared,PublicKey=0024000004800000940000000602000000240000525341310004000001000100dd31828d6e6f53eebd68f2c94ddb06ffb377517cbc8a0ff1c5d580c0a9b961b868e3354190f4b07d46f551ec050bf19eb88ca0d40abe7881d5fa1407d53ac2c0148950e1cc763178a1e739ebc9bbcc9794f76b8636bfbcd0b0dbcb61b9468b9f66ffc17695bfea71e199db6c58ce8d8dda923cb43ef9f7e7a3d3d5e6188a9dc9")]

#endif



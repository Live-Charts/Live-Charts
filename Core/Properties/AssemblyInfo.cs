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

[assembly: AssemblyKeyFile(@"..\LVCSign.snk")]
[assembly: InternalsVisibleTo("LiveCharts.Uwp,PublicKey=002400000480000094000000060200000024000052534131000400000100010009132ae1a474e2ecf9903c1ef8945a2119aa0b9a3b4e40c43f6cb66233669e3007b4109d5c37957c2d0c5cfe7fce34366150210f83c618c18cdc8d7b763bff60419837a2185df1867c73f679e05f82b861e6ca764612eabc36d71858260b262bb9c3ad546c9692fe6379a520b6c5fc701e0ee6d071b52e9f20166fc0752ff894")]
[assembly: InternalsVisibleTo("LiveCharts.Wpf,PublicKey=002400000480000094000000060200000024000052534131000400000100010009132ae1a474e2ecf9903c1ef8945a2119aa0b9a3b4e40c43f6cb66233669e3007b4109d5c37957c2d0c5cfe7fce34366150210f83c618c18cdc8d7b763bff60419837a2185df1867c73f679e05f82b861e6ca764612eabc36d71858260b262bb9c3ad546c9692fe6379a520b6c5fc701e0ee6d071b52e9f20166fc0752ff894")]
[assembly: InternalsVisibleTo("LiveCharts.Geared,PublicKey=002400000480000094000000060200000024000052534131000400000100010009132ae1a474e2ecf9903c1ef8945a2119aa0b9a3b4e40c43f6cb66233669e3007b4109d5c37957c2d0c5cfe7fce34366150210f83c618c18cdc8d7b763bff60419837a2185df1867c73f679e05f82b861e6ca764612eabc36d71858260b262bb9c3ad546c9692fe6379a520b6c5fc701e0ee6d071b52e9f20166fc0752ff894")]

#endif



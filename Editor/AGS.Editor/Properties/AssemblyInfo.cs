using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("AGS Editor for Windows")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("AGS")]
[assembly: AssemblyProduct("Adventure Game Studio")]
[assembly: AssemblyCopyright(AGS.Types.Version.AGS_EDITOR_COPYRIGHT)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("10e5db99-0fc7-4a9c-b68b-72a3244bcc8f")]

[assembly: AssemblyVersion(AGS.Types.Version.AGS_EDITOR_VERSION)]
[assembly: AssemblyFileVersion(AGS.Types.Version.AGS_EDITOR_VERSION)]

// Make internals available to the test project
[assembly: InternalsVisibleTo("Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001001928fff7aae4735931d94028240c068f6e48c162de51a23e60d1fbd702acfa22c0e7fe476c578147311cfc31eafc0eb3439833376206013fde5518129df439eff4531dc45b2222605a26815d6da885e19bcead6bcc5e644807e56f31b07cf25f075ecb3b47cb7acba7cc0373583a61fc1e540a1ab62922e8cd50d16de50ba6b2")]

using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace CssTools
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [Guid("fd12c213-77b8-4e48-b372-17b83b533789")]
    public sealed class CssToolsPackage : AsyncPackage
    {
    }
}

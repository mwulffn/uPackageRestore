using System.Xml.Linq;

namespace uPackageRestore
{
  interface IPackage
  {
    string PackageName { get; }
    string PackageUrl { get; }
    string PackageFolder { get; }
    XDocument PackageXml { get; }

    void ExtractPackage(string workingDirectory);
    void OpenPackageFolder(string packageFolder);
    void CopyPackageToDirectory(string path);
  }
}

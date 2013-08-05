using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uPackageRestore.Test.Fakes
{
  class FakePackage : IPackage
  {
    public bool ExtractPackageCalled = false;
    public bool OpenPackageFolderCalled = false;
    public bool CopyPackageToDirectoryCalled = false;
    
    public string PackageName
    {
      get { return PackageTests.TestPackageName; }
    }

    public string PackageUrl
    {
      get { return PackageTests.TestPackageUrl; }
    }

    public string PackageFolder
    {
      get { return Path.Combine(Path.GetDirectoryName(PackageTests.TestPackageFile), PackageTests.TestPackageName); }
    }

    public System.Xml.Linq.XDocument PackageXml
    {
      get { return null; }
    }

    public void ExtractPackage(string workingDirectory)
    {
      ExtractPackageCalled = true;
    }

    public void OpenPackageFolder(string packageFolder)
    {
      OpenPackageFolderCalled = true;
    }

    public void CopyPackageToDirectory(string path)
    {
      CopyPackageToDirectoryCalled = true;
    }
  }
}

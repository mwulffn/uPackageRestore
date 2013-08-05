using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace uPackageRestore.Test
{
  [TestClass]
  public class PackageRestoreTests
  {
    private Fakes.FakePackage _package = new Fakes.FakePackage();
    
    [TestMethod]
    public void TestConstructorAndRun()
    {
      Assert.IsFalse(_package.CopyPackageToDirectoryCalled);

      string workingDirectory = PackageTests.SetupWorkingDirectory();
      var packageRestore = new PackageRestore(_package, Path.Combine(workingDirectory, "Umbraco"));
      packageRestore.Run();

      Assert.IsTrue(_package.CopyPackageToDirectoryCalled);
    }
  }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using uPackageRestore;

namespace uPackageRestore.Test
{
  [TestClass]
  public class PackageTests
  {
    public const string TestPackageFile = "TestFiles\\Document_Type_Picker_1.0.zip";
    public const string TestPackageName = "Test Package";
    public const string TestPackageUrl = "http://localhost/file.zip";

    private IPackage _package;

    [TestMethod]
    public void TestConstructor()
    {
      var package = new Package(TestPackageUrl, TestPackageName);
      Assert.AreEqual(TestPackageUrl, package.PackageUrl);
      Assert.AreEqual(TestPackageName, package.PackageName);
    }

    [TestMethod]
    public void TestPackageExists()
    {
      Assert.IsTrue(File.Exists(TestPackageFile), "Could not find the test package file: " + TestPackageFile);
    }

    [TestMethod]
    public void TestExtractPackage()
    {
      string workingDirectory = SetupPackage();

      Assert.IsTrue(File.Exists(Path.Combine(workingDirectory, TestPackageName, "fa27b457-2d5f-4ea4-b57d-f05684cba07a.dll")));
      Assert.IsTrue(File.Exists(Path.Combine(workingDirectory, TestPackageName, "Package.xml")));
    }

    [TestMethod]
    public void TestOpenPackageFolder()
    {
      string workingDirectory = SetupPackage();
      _package.OpenPackageFolder(Path.Combine(workingDirectory, TestPackageName));
      Assert.IsNotNull(_package.PackageXml);
    }

    [TestMethod]
    public void TestCopyPackageToDirectory()
    {
      string workingDirectory = SetupPackage();
      _package.OpenPackageFolder(Path.Combine(workingDirectory, TestPackageName));
      _package.CopyPackageToDirectory(Path.Combine(workingDirectory, "Umbraco"));
      Assert.IsTrue(File.Exists(Path.Combine(workingDirectory, "Umbraco\\bin\\Auros.DocumentTypePicker.dll")));
    }

    private string SetupPackage()
    {
      var webClient = new Fakes.FakeWebClient();
      _package = new Package(TestPackageUrl, TestPackageName, webClient);
      string workingDirectory = SetupWorkingDirectory();
      _package.ExtractPackage(workingDirectory);
      return workingDirectory;
    }

    public static string SetupWorkingDirectory()
    {
      string workingDirectory = Path.GetDirectoryName(TestPackageFile);
      if (!Directory.Exists(workingDirectory))
        Directory.CreateDirectory(workingDirectory);

      string packageDirectory = Path.Combine(workingDirectory, TestPackageName);
      if (Directory.Exists(packageDirectory))
        Directory.Delete(packageDirectory, true);

      string outputDirectory = Path.Combine(workingDirectory, "Umbraco");
      if (Directory.Exists(outputDirectory))
        Directory.Delete(outputDirectory, true);
      Directory.CreateDirectory(outputDirectory);

      return workingDirectory;
    }
  }
}

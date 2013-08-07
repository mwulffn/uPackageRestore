using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using uPackageRestore;

namespace uPackageRestore.Test
{
  [TestClass]
  public class PackageTests
  {
    // Test package for normal http operation
    public const string TestPackageFile = "TestFiles\\Document_Type_Picker_1.0.zip";
    public const string TestPackageName = "Test Package";
    public const string TestPackageUrl = "http://localhost/Document_Type_Picker_1.0.zip";

    
    // Test package for file-based operation
    public const string TestFileBasedPackageFile = "TestFiles\\custom.zip";
    public const string TestFileBasedPackageName = "Custom Package";
    public const string TestFileBasedPackageUrl = "Testfiles\\custom.zip";

    private IPackage _package;

    [TestMethod]
    public void TestConstructor()
    {
      var package = new Package(TestPackageUrl, TestPackageName);
      Assert.AreEqual(TestPackageUrl, package.PackageUrl);
      Assert.AreEqual(TestPackageName, package.PackageName);
    }

    [TestMethod]
    public void TestPackagesExists()
    {
      Assert.IsTrue(File.Exists(TestPackageFile), "Could not find the test package file: " + TestPackageFile);
      Assert.IsTrue(File.Exists(TestFileBasedPackageFile), "Could not find the test package file: " + TestFileBasedPackageFile);
    }

    [TestMethod]
    public void TestExtractPackage()
    {
      string workingDirectory = SetupPackage();

      Assert.IsTrue(File.Exists(Path.Combine(workingDirectory, TestPackageName, "fa27b457-2d5f-4ea4-b57d-f05684cba07a.dll")));
      Assert.IsTrue(File.Exists(Path.Combine(workingDirectory, TestPackageName, "Package.xml")));
    }

    [TestMethod]
    public void TestExtractFileBasedPackage()
    {
      string workingDirectory = SetupFileBasedPackage();

      Assert.IsTrue(File.Exists(Path.Combine(workingDirectory, TestFileBasedPackageName, "3593d8e7-8b35-47b9-beda-5e830ca8c93c.dll")));
      Assert.IsTrue(File.Exists(Path.Combine(workingDirectory, TestFileBasedPackageName, "Package.xml")));
    }

    [TestMethod]
    public void TestExtractPackageWithEmptyPackageDirectory()
    {
      var webClient = new Fakes.FakeWebClient();
      _package = new Package(TestPackageUrl, TestPackageName, webClient);
      string workingDirectory = SetupWorkingDirectory();
      string packageDirectory = Path.Combine(workingDirectory, TestPackageName);
      Directory.CreateDirectory(packageDirectory);

      _package.ExtractPackage(workingDirectory);

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

    private string SetupFileBasedPackage()
    {
      var webClient = new Fakes.FakeWebClient();
      _package = new Package(TestFileBasedPackageUrl, TestFileBasedPackageName, webClient);
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
      
      packageDirectory = Path.Combine(workingDirectory, TestFileBasedPackageName);
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

using System.IO;

namespace uPackageRestore
{
  class PackageRestore : uPackageRestore.IPackageRestore
  {
    public string DestinationDirectory { get; protected set; }
    public string WorkingDirectory { get; protected set; }
    public IPackage Package { get; protected set; }

    public PackageRestore(IPackage package, string destinationDirectory)
    {
      DestinationDirectory = destinationDirectory;
      WorkingDirectory = Path.Combine(DestinationDirectory, ".uPackageRestore");
      Package = package;
      Setup();
    }
    
    private void Setup()
    {
      CheckParameters();
      CreateWorkingDirectory();
      Package.ExtractPackage(WorkingDirectory);
    }

      private void CheckParameters()
      {
        if (!Directory.Exists(DestinationDirectory))
          throw new DirectoryNotFoundException(string.Format("The directory '{0}' does not exist", DestinationDirectory));
      }
    
      private void CreateWorkingDirectory()
      {
        if (!Directory.Exists(WorkingDirectory))
          Directory.CreateDirectory(WorkingDirectory);
      }

    public void Run()
    {
      Package.CopyPackageToDirectory(DestinationDirectory);
    }
  }
}

using System;

namespace uPackageRestore
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length != 3)
        Usage();
      else
        RunProgram(args[0], args[1], args[2]);

#if DEBUG
      PauseForInput();
#endif
    }

    private static void Usage()
    {
      Console.WriteLine("Usage: uPackageRestore.exe <source_package_url> <destination_folder> <package_name>");
      Console.WriteLine("");
    }

    private static void RunProgram(string sourcePackageUrl, string destinationFolder, string packageName)
    {
      var package = new Package(sourcePackageUrl, packageName);
      var packageRestore = new PackageRestore(package, destinationFolder);
      packageRestore.Run();
    }

#if DEBUG
    private static void PauseForInput()
    {
      Console.WriteLine();
      Console.Write("Press any key to continue.");
      Console.ReadKey();
    }
#endif

  }
}

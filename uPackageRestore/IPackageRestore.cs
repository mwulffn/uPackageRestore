namespace uPackageRestore
{
  interface IPackageRestore
  {
    string DestinationDirectory { get; }
    IPackage Package { get; }
    void Run();
    string WorkingDirectory { get; }
  }
}

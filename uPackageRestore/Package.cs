using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.Zip;

namespace uPackageRestore
{
  public class Package : uPackageRestore.IPackage
  {
    private IWebClient _webClient;

    public string PackageName { get; protected set; }
    public string PackageUrl { get; protected set; }
    public string PackageFolder { get; protected set; }
    public XDocument PackageXml { get; protected set; }

    public Package(string packageUrl, string packageName, IWebClient webClient = null)
    {
      PackageUrl = packageUrl;
      PackageName = packageName;
      _webClient = (webClient == null) ? new LiveWebClient() : webClient;
    }

    public void ExtractPackage(string workingDirectory)
    {
      string packageFolder = Path.Combine(workingDirectory, PackageName);

      if (!Directory.Exists(packageFolder))
      {
        string packageFile = Path.GetTempFileName();
        _webClient.DownloadFile(PackageUrl, packageFile);

        DecompressToDirectory(packageFile, packageFolder);

        File.Delete(packageFile);
      }

      OpenPackageFolder(packageFolder);
    }


    public void OpenPackageFolder(string packageFolder)
    {
      PackageFolder = packageFolder;
      if (!File.Exists(Path.Combine(PackageFolder, "package.xml")))
        throw new Exception(string.Format("The folder '{0}' does not contain a package xml file.", packageFolder));

      PackageXml = XDocument.Load(Path.Combine(PackageFolder, "package.xml"));
    }


    public void CopyPackageToDirectory(string path)
    {
      var files = PackageXml.Descendants("file");

      int copied = 0;
      int skipped = 0;

      foreach (var file in files)
      {
        string src = file.Descendants("guid").First().Value;

        string currentSourceFile = Path.Combine(PackageFolder, src);
        if (!File.Exists(currentSourceFile))
        {
          skipped++;
          continue;
        }

        string dest = file.Descendants("orgName").First().Value;
        string destPath = file.Descendants("orgPath").First().Value;

        if (destPath.StartsWith("/"))
          destPath = destPath.Substring(1);

        destPath = destPath.Replace('/', '\\');


        string finalDestination = Path.Combine(path, Path.Combine(destPath, dest));

        if (File.Exists(finalDestination))
        {
          skipped++;
          continue;
        }

        if (!Directory.Exists(Path.GetDirectoryName(finalDestination)))
          Directory.CreateDirectory(Path.GetDirectoryName(finalDestination));

        Console.WriteLine("Copy '{0}' to '{1}'", src, finalDestination);
        File.Copy(currentSourceFile, finalDestination);
        copied++;

      }

      Console.WriteLine("Skipped {0} files, copied {1} files", skipped, copied);

    }


    private void DecompressToDirectory(string sourceFile, string targetPath, string pwd = "")
    {
      FileStream source = new FileStream(sourceFile, FileMode.Open);
      targetPath = Path.GetFullPath(targetPath);

      using (ZipInputStream decompressor = new ZipInputStream(source))
      {
        if (!string.IsNullOrEmpty(pwd))
        {
          decompressor.Password = pwd;
        }

        ZipEntry entry;

        while ((entry = decompressor.GetNextEntry()) != null)
        {

          string filePath = Path.Combine(targetPath, Path.GetFileName(entry.Name));

          string directoryPath = Path.GetDirectoryName(filePath);


          if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
          {
            Directory.CreateDirectory(directoryPath);
          }

          if (entry.IsDirectory)
          {
            continue;
          }

          byte[] data = new byte[2048];
          using (FileStream streamWriter = File.Create(filePath))
          {
            int bytesRead;
            while ((bytesRead = decompressor.Read(data, 0, data.Length)) > 0)
            {
              streamWriter.Write(data, 0, bytesRead);
            }
          }
        }
      }
    }


  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;
using System.Xml.Linq;

namespace uPackageRestore
{
    class Package
    {

        public string PackageFolder { get; set; }


        public XDocument PackageXml { get; set; }

        public Package(string packageFolder)
        {
            PackageFolder = packageFolder;
            if(!File.Exists(Path.Combine(PackageFolder, "package.xml")))
                throw new Exception(string.Format("The folder '{0}' does not contain a package xml file.",packageFolder));

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


        public static Package FromWeb(string path)
        {
            string packageName = Program.PackageName;
            string packageDir = Path.Combine(Program.WorkingDirectory, packageName);

            if (!Directory.Exists(packageDir))
            {

                //Download and unpack
                try
                {
                    WebClient client = new WebClient();

                    string tmpfile = Path.GetTempFileName();

                    client.DownloadFile(path, tmpfile);

                    FileStream stream = new FileStream(tmpfile, FileMode.Open);

                    DecompressToDirectory(stream, packageDir, "");


                    File.Delete(tmpfile);

                }
                catch (Exception)
                {

                    throw;
                }
            }



            return new Package(packageDir);
        }


        public static void DecompressToDirectory(Stream source, string targetPath, string pwd)
        {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace uPackageRestore
{
    class Program
    {

        public static string DestinationDirectory { get; protected set; }
        public static string WorkingDirectory { get; protected set; }
        public static string PackageName { get; set; }

        static void Main(string[] args)
        {


            if (args.Length != 3)
            {
                Usage();
                return;
            }


            //Check parameters
            if (!Directory.Exists(args[1]))
            {
                Usage(string.Format("The directory '{0}' does not exist",args[1]));
                return;
            }

            DestinationDirectory = args[1];
            WorkingDirectory = Path.Combine(DestinationDirectory, ".uPackageRestore");

            //Check if our cache directory exists
            if (!Directory.Exists(WorkingDirectory))
            {
                try
                {
                    Directory.CreateDirectory(WorkingDirectory);
                }
                catch (Exception ex)
                {
                    Usage(ex.Message);
                    return;                  
                }
            }

            PackageName = args[2];

            Package p = Package.FromWeb(args[0]);

            p.CopyPackageToDirectory(DestinationDirectory);

        }

        private static void Usage(string extraMessage = "")
        {
            Console.WriteLine("Usage: uPackageRestore.exe <source_package_url> <destination_folder> <package_name>");
            Console.WriteLine("");
            if (extraMessage != "")
            {
                Console.WriteLine(extraMessage);
                Console.WriteLine("");
            }
        }

    }
}

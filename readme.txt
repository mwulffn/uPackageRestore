uPackage Restore is a command line tool that will download an Umbraco package and install all files into a folder. 

It is is primarily used for recreating package files inside an umbraco folder, thus eliminating the need to check packagefiles into a source repository.

The syntax is simple:

uPackageRestore <source_package_url> <path_to_umbraco_folder> <packagename>

the software will create a .uPackageRestore folder in the umbraco folder, this folder can safely be ignored.

Examples of usage:

UpackageRestore.exe "http://our.umbraco.org/FileDownload?id=4302" src\Umbraco Courier

This will install all files from courier into the src\Umbraco folder. Also note that the destination folder is not required to be an Umbraco folder. It will work fine with an empty folder.

Furthermore uPackageRestore will never overwrite a file on your system. If a file already exists in place this file will be preserved.

This tools has been developed as part of my work at FRIE Funktionærer and is free software.
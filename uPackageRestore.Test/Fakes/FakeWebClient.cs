using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uPackageRestore.Test.Fakes
{
  class FakeWebClient : IWebClient
  {
    public void DownloadFile(string address, string fileName)
    {
      File.Copy(PackageTests.TestPackageFile, fileName, true);
    }
  }
}

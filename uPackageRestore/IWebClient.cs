using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uPackageRestore
{
  public interface IWebClient
  {
    void DownloadFile(string address, string fileName);
  }
}

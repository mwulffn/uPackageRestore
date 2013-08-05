using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace uPackageRestore
{
  class LiveWebClient : IWebClient
  {
    public void DownloadFile(string address, string fileName)
    {
      WebClient client = new WebClient();
      client.DownloadFile(address, fileName);
    }
  }
}

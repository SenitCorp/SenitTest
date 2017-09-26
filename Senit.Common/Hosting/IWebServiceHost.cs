using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senit.Common.Hosting
{
    public interface IWebServiceHost
    {
        IWebHost GetHost();
    }
}

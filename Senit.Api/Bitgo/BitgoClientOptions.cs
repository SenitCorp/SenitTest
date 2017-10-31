using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senit.Api.Bitgo
{
    public class BitgoClientOptions
    {
        public string BaseUrl { get; set; }
        public string Environment { get; set; }
        public string Enterprise { get; set; }
        public string AccessToken { get; set; }
        public string BackupXpubProvider { get; set; }
    }
}

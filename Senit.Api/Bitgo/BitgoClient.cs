using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Senit.Api.Bitgo
{
    public class BitgoClient
    {
        private readonly BitgoClientOptions _options;

        private readonly ILogger _logger;

        private readonly HttpClient _httpClient;

        private IDataProtector _dataProtector;

        const string DataProtectorPurpose = "Account";

        public BitgoClient(IOptionsSnapshot<BitgoClientOptions> options, IDataProtectionProvider provider, ILoggerFactory loggerFactory)
        {
            _options = options.Value;

            _dataProtector = provider.CreateProtector(DataProtectorPurpose);

            _logger = loggerFactory.CreateLogger<BitgoClient>();

            _httpClient = new HttpClient();

            _httpClient.BaseAddress = new Uri($"{_options.BaseUrl}");

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Value.AccessToken);
        }

        public void CreateWallet(string label, string passphrase)
        {

        }

        public void GetWallet(string id)
        {

        }

        public void GetWalletTransfer()
        {

        }

        public void GetWalletAddress()
        {

        }

        public void CreateWalletAddress()
        {

        }

        public void SendTransaction()
        {

        }

        public void GetTransaction()
        {

        }

        public void AddWalletWebhook()
        {

        }

        public void RemoveWalletWebhook()
        {

        }
    }
}

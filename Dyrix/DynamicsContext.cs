using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.OData.Client;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;

namespace Dyrix
{
    public class DynamicsContext : DataServiceContext
    {
        private readonly string _resource;
        private readonly AuthenticationContext _authenticationContext;
        private readonly ClientCredential _clientCredential;

        public DynamicsContext(IOptions<DynamicsContextOptions> options)
        {
            var optionsValue = options?.Value ?? throw new ArgumentNullException(nameof(options));

            var apiVersion = string.IsNullOrWhiteSpace(optionsValue.ApiVersion)
                ? throw new ArgumentNullException(nameof(optionsValue.ApiVersion))
                : optionsValue.ApiVersion;
            var clientId = string.IsNullOrWhiteSpace(optionsValue.ClientId)
                ? throw new ArgumentNullException(nameof(optionsValue.ClientId))
                : optionsValue.ClientId;
            var clientSecret = string.IsNullOrWhiteSpace(optionsValue.ClientSecret)
                ? throw new ArgumentNullException(nameof(optionsValue.ClientSecret))
                : optionsValue.ClientSecret;
            var directoryId = string.IsNullOrWhiteSpace(optionsValue.DirectoryId)
                ? throw new ArgumentNullException(nameof(optionsValue.DirectoryId))
                : optionsValue.DirectoryId;
            _resource = string.IsNullOrWhiteSpace(optionsValue.Resource)
               ? throw new ArgumentNullException(nameof(optionsValue.Resource))
               : optionsValue.Resource;

            _authenticationContext = new AuthenticationContext($"https://login.windows.net/{directoryId}");
            _clientCredential = new ClientCredential(clientId, clientSecret);

            BaseUri = new Uri($"{_resource}/api/data/v{apiVersion}/");
            BuildingRequest += OnBuildingRequest;

            Format.LoadServiceModel = LoadServiceModel;
            Format.UseJson();

        }

        private IEdmModel LoadServiceModel()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());

                var edmxString = httpClient.GetStringAsync(GetMetadataUri())
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                using (var stringReader = new StringReader(edmxString))
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    return CsdlReader.Parse(xmlReader);
                }
            }
        }

        private void OnBuildingRequest(object sender, BuildingRequestEventArgs e)
        {
            var accessToken = GetAccessToken();

            // e.Headers.Add("OData-Version", "4.0");

            e.Headers.Add("Authorization", $"Bearer {accessToken}");
        }

        private string GetAccessToken() => _authenticationContext.AcquireTokenAsync(_resource, _clientCredential)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult()
            .AccessToken;
    }
}

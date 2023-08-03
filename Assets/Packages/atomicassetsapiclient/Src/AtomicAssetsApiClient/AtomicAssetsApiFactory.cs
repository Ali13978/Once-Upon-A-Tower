using AtomicAssetsApiClient.Accounts;
using AtomicAssetsApiClient.Assets;
using AtomicAssetsApiClient.Burns;
using AtomicAssetsApiClient.Collections;
using AtomicAssetsApiClient.Config;
using AtomicAssetsApiClient.Offers;
using AtomicAssetsApiClient.Schemas;
using AtomicAssetsApiClient.Templates;
using AtomicAssetsApiClient.Transfers;

namespace AtomicAssetsApiClient
{
    public class AtomicAssetsApiFactory
    {
        private readonly string _baseUrl;
        private const string Version1BaseUrl = "https://wax.api.atomicassets.io/atomicassets/v1";

        private AtomicAssetsApiFactory(string baseUrl) => _baseUrl = baseUrl;

        public static AtomicAssetsApiFactory Version1 => new AtomicAssetsApiFactory(Version1BaseUrl);

        public AccountsApi AccountsApi => new AccountsApi(_baseUrl, new HttpHandler());

        public AssetsApi AssetsApi => new AssetsApi(_baseUrl, new HttpHandler());

        public BurnsApi BurnsApi => new BurnsApi(_baseUrl, new HttpHandler());

        public CollectionsApi CollectionsApi => new CollectionsApi(_baseUrl, new HttpHandler());

        public ConfigApi ConfigApi => new ConfigApi(_baseUrl, new HttpHandler());

        public OffersApi OffersApi => new OffersApi(_baseUrl, new HttpHandler());

        public SchemasApi SchemasApi => new SchemasApi(_baseUrl, new HttpHandler());

        public TemplatesApi TemplatesApi => new TemplatesApi(_baseUrl, new HttpHandler());

        public TransfersApi TransfersApi => new TransfersApi(_baseUrl, new HttpHandler());
    }
}

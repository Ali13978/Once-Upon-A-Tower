using System.Text;

namespace AtomicAssetsApiClient.Transfers
{
    //TODO use the correct parameters
    public class TransfersUriParameterBuilder
    {
        private string _account;
        private string _sender;
        private string _recipient;
        private string _assetId;
        private string _templateId;
        private string _schemaName;
        private string _collectionName;
        private string _collectionBlacklist;
        private string _collectionWhitelist;
        private int? _page;
        private int? _limit;
        private SortStrategy? _sortStrategy;
        private string _sort;

        public TransfersUriParameterBuilder WithAccount(string account)
        {
            _account = account;
            return this;
        }

        public TransfersUriParameterBuilder WithRecipient(string recipient)
        {
            _recipient = recipient;
            return this;
        }

        public TransfersUriParameterBuilder WithAssetId(string assetId)
        {
            _assetId = assetId;
            return this;
        }

        public TransfersUriParameterBuilder WithSender(string sender)
        {
            _sender = sender;
            return this;
        }

        public TransfersUriParameterBuilder WithCollectionName(string collectionName)
        {
            _collectionName = collectionName;
            return this;
        }

        public TransfersUriParameterBuilder WithSchemaName(string schemaName)
        {
            _schemaName = schemaName;
            return this;
        }

        public TransfersUriParameterBuilder WithTemplateId(string templateId)
        {
            _templateId = templateId;
            return this;
        }

        public TransfersUriParameterBuilder WithCollectionBlacklist(string[] collectionBlacklist)
        {
            _collectionBlacklist = string.Join(",", collectionBlacklist);
            return this;
        }

        public TransfersUriParameterBuilder WithCollectionWhitelist(string[] collectionWhitelist)
        {
            _collectionWhitelist = string.Join(",", collectionWhitelist);
            return this;
        }

        public TransfersUriParameterBuilder WithPage(int page)
        {
            _page = page;
            return this;
        }

        public TransfersUriParameterBuilder WithLimit(int limit)
        {
            _limit = limit;
            return this;
        }

        public TransfersUriParameterBuilder WithOrder(SortStrategy sorting)
        {
            _sortStrategy = sorting;
            return this;
        }

        public TransfersUriParameterBuilder WithSort(string sort)
        {
            _sort = sort;
            return this;
        }

        public string Build()
        {
            var parameterString = new StringBuilder("?");
            if (!string.IsNullOrEmpty(_account))
            {
                parameterString.Append($"&account={_account}");
            }
            if (!string.IsNullOrEmpty(_sender))
            {
                parameterString.Append($"&sender={_sender}");
            }
            if (!string.IsNullOrEmpty(_recipient))
            {
                parameterString.Append($"&recipient={_recipient}");
            }
            if (!string.IsNullOrEmpty(_assetId))
            {
                parameterString.Append($"&asset_id={_assetId}");
            }
            if (!string.IsNullOrEmpty(_templateId))
            {
                parameterString.Append($"&template_id={_templateId}");
            }
            if (!string.IsNullOrEmpty(_collectionName))
            {
                parameterString.Append($"&collection_name={_collectionName}");
            }
            if (!string.IsNullOrEmpty(_collectionBlacklist))
            {
                parameterString.Append($"&collection_blacklist={_collectionBlacklist}");
            }
            if (!string.IsNullOrEmpty(_collectionWhitelist))
            {
                parameterString.Append($"&collection_whitelist={_collectionWhitelist}");
            }
            if (_page.HasValue)
            {
                parameterString.Append($"&page={_page}");
            }
            if (_limit.HasValue)
            {
                parameterString.Append($"&limit={_limit}");
            }
            if (_sortStrategy.HasValue)
            {
                switch (_sortStrategy)
                {
                    case SortStrategy.Ascending:
                        parameterString.Append("&order=asc");
                        break;
                    case SortStrategy.Descending:
                        parameterString.Append("&order=desc");
                        break;
                }
            }
            if (!string.IsNullOrEmpty(_sort))
            {
                parameterString.Append($"&sort={_sort}");
            }
            if (!string.IsNullOrEmpty(_schemaName))
            {
                parameterString.Append($"&schema_name={_schemaName}");
            }

            return parameterString.ToString();
        }
    }
}

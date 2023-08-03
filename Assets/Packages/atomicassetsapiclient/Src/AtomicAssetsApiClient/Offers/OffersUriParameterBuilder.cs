using System.Text;

namespace AtomicAssetsApiClient.Offers
{
    public class OffersUriParameterBuilder
    {
        private string _account;
        private string _sender;
        private string _recipient;
        private string _state;
        private bool? _isRecipientContract;
        private string _assetId;
        private string _templateId;
        private string _schemaName;
        private string _collectionName;
        private string _accountWhitelist;
        private string _accountBlacklist;
        private string _senderAssetWhitelist;
        private string _senderAssetBlacklist;
        private string _recipientAssetWhitelist;
        private string _recipientAssetBlacklist;
        private string _collectionBlacklist;
        private string _collectionWhitelist;
        private string _ids;
        private string _lowerBound;
        private string _upperBound;
        private int? _before;
        private int? _after;
        private int? _page;
        private int? _limit;
        private SortStrategy? _sortStrategy;
        private string _sort;

        public OffersUriParameterBuilder WithAccountWhitelist(string accountWhitelist)
        {
            _accountWhitelist = accountWhitelist;
            return this;
        }

        public OffersUriParameterBuilder WithAccountBlacklist(string accountBlacklist)
        {
            _accountBlacklist = accountBlacklist;
            return this;
        }

        public OffersUriParameterBuilder WithSenderAssetWhitelist(string senderAssetwhitelist)
        {
            _senderAssetWhitelist = senderAssetwhitelist;
            return this;
        }

        public OffersUriParameterBuilder WithSenderAssetBlacklist(string senderAssetBlacklist)
        {
            _senderAssetBlacklist = senderAssetBlacklist;
            return this;
        }

        public OffersUriParameterBuilder WithRecipientAssetWhitelist(string recipientAssetwhitelist)
        {
            _recipientAssetWhitelist = recipientAssetwhitelist;
            return this;
        }

        public OffersUriParameterBuilder WithRecipientAssetBlacklist(string recipientAssetBlacklist)
        {
            _recipientAssetBlacklist = recipientAssetBlacklist;
            return this;
        }

        public OffersUriParameterBuilder WithAccount(string account)
        {
            _account = account;
            return this;
        }

        public OffersUriParameterBuilder WithSender(string sender)
        {
            _sender = sender;
            return this;
        }

        public OffersUriParameterBuilder WithRecipient(string recipient)
        {
            _recipient = recipient;
            return this;
        }

        public OffersUriParameterBuilder WithState(string state)
        {
            _state = state;
            return this;
        }

        public OffersUriParameterBuilder WithIsRecipientContract(bool isRecipientContract)
        {
            _isRecipientContract = isRecipientContract;
            return this;
        }

        public OffersUriParameterBuilder WithAssetId(string assetId)
        {
            _assetId = assetId;
            return this;
        }

        public OffersUriParameterBuilder WithTemplateId(string temlpateId)
        {
            _templateId = temlpateId;
            return this;
        }

        public OffersUriParameterBuilder WithCollectionName(string collectionName)
        {
            _collectionName = collectionName;
            return this;
        }

        public OffersUriParameterBuilder WithSchemaName(string schemaName)
        {
            _schemaName = schemaName;
            return this;
        }

        public OffersUriParameterBuilder WithCollectionBlacklist(string[] collectionBlacklist)
        {
            _collectionBlacklist = string.Join(",", collectionBlacklist);
            return this;
        }

        public OffersUriParameterBuilder WithCollectionWhitelist(string[] collectionWhitelist)
        {
            _collectionWhitelist = string.Join(",", collectionWhitelist);
            return this;
        }

        public OffersUriParameterBuilder WithIds(string[] ids)
        {
            _ids = string.Join(",", ids);
            return this;
        }

        public OffersUriParameterBuilder WithLowerBound(string lowerBound)
        {
            _lowerBound = lowerBound;
            return this;
        }

        public OffersUriParameterBuilder WithUpperBound(string upperBound)
        {
            _upperBound = upperBound;
            return this;
        }

        public OffersUriParameterBuilder WithBefore(int before)
        {
            _before = before;
            return this;
        }

        public OffersUriParameterBuilder WithAfter(int after)
        {
            _after = after;
            return this;
        }

        public OffersUriParameterBuilder WithPage(int page)
        {
            _page = page;
            return this;
        }

        public OffersUriParameterBuilder WithLimit(int limit)
        {
            _limit = limit;
            return this;
        }

        public OffersUriParameterBuilder WithOrder(SortStrategy sorting)
        {
            _sortStrategy = sorting;
            return this;
        }

        public OffersUriParameterBuilder WithSort(string sort)
        {
            _sort = sort;
            return this;
        }

        public string Build()
        {
            var parameterString = new StringBuilder("?");
            if (!string.IsNullOrEmpty(_account))
            {
                parameterString.Append($"account={_account}");
            }
            if (!string.IsNullOrEmpty(_sender))
            {
                parameterString.Append($"sender={_sender}");
            }
            if (!string.IsNullOrEmpty(_recipient))
            {
                parameterString.Append($"recipient={_recipient}");
            }
            if (!string.IsNullOrEmpty(_state))
            {
                parameterString.Append($"state={_state}");
            }
            if (_isRecipientContract.HasValue)
            {
                parameterString.Append($"is_recipient_contract={_isRecipientContract}");
            }
            if (!string.IsNullOrEmpty(_assetId))
            {
                parameterString.Append($"asset_id={_assetId}");
            }
            if (!string.IsNullOrEmpty(_templateId))
            {
                parameterString.Append($"template_id={_templateId}");
            }
            if (!string.IsNullOrEmpty(_accountWhitelist))
            {
                parameterString.Append($"account_whitelist={_accountWhitelist}");
            }
            if (!string.IsNullOrEmpty(_accountBlacklist))
            {
                parameterString.Append($"account_blacklist={_accountBlacklist}");
            }
            if (!string.IsNullOrEmpty(_senderAssetWhitelist))
            {
                parameterString.Append($"sender_asset_whitelist={_senderAssetWhitelist}");
            }
            if (!string.IsNullOrEmpty(_senderAssetBlacklist))
            {
                parameterString.Append($"sender_asset_blacklist={_senderAssetBlacklist}");
            }
            if (!string.IsNullOrEmpty(_recipientAssetWhitelist))
            {
                parameterString.Append($"recipient_asset_whitelist={_recipientAssetWhitelist}");
            }
            if (!string.IsNullOrEmpty(_recipientAssetBlacklist))
            {
                parameterString.Append($"recipient_asset_blacklist={_recipientAssetBlacklist}");
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
            if (!string.IsNullOrEmpty(_ids))
            {
                parameterString.Append($"&ids={_ids}");
            }
            if (!string.IsNullOrEmpty(_lowerBound))
            {
                parameterString.Append($"&lower_bound={_lowerBound}");
            }
            if (!string.IsNullOrEmpty(_upperBound))
            {
                parameterString.Append($"&upper_bound={_upperBound}");
            }
            if (_before.HasValue)
            {
                parameterString.Append($"&before={_before}");
            }
            if (_after.HasValue)
            {
                parameterString.Append($"&after={_after}");
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

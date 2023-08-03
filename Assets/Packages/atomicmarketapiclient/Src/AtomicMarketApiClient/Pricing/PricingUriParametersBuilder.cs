using System.Text;

namespace AtomicMarketApiClient.Pricing
{
    public class PricingUriParametersBuilder : IUriParameterBuilder
    {
/* A private variable that is used to store the value of the symbol parameter. */
        private string _symbol;
/* A nullable boolean specfying burned assets. */
        private bool? _burned;
/* A private variable that is used to store the value of the collectionName parameter. */
        private string _collectionName;
/* A private variable that is used to store the value of the schemaName parameter. */
        private string _schemaName;
/* A private variable that is used to store the value of the templateId parameter. */
        private string _templateId;
/* A nullable boolean specfying transferable assets. */
        private bool? _isTransferable;
/* A nullable boolean specfying burnable assets. */
        private bool? _isBurnable;


/// <summary>
/// `WithSymbol` sets the `symbol` parameter
/// </summary>
/// <param name="symbol">Token symbols.</param>
/// <returns>
/// The PricingUriParameterBuilder object.
/// </returns>
        public PricingUriParametersBuilder WithSymbol(string symbol)
        {
            _symbol = symbol;
            return this;
        }


/// <summary>
/// `WithBurned` sets the `_burned` field to the value of the `burned` parameter
/// </summary>
/// <param name="burned">It filters for burned assets.</param>
/// <returns>
/// The PricingUriParameterBuilder object.
/// </returns>
        public PricingUriParametersBuilder WithBurned(bool burned)
        {
            _burned = burned;
            return this;
        }


/// <summary>
/// `WithCollectionName` is a function that takes a string as a parameter and returns an
/// `PricingUriParameterBuilder` object
/// </summary>
/// <param name="collectionName">The name of the collection you want to query.</param>
/// <returns>
/// The PricingUriParameterBuilder object.
/// </returns>
        public PricingUriParametersBuilder WithCollectionName(string collectionName)
        {
            _collectionName = collectionName;
            return this;
        }


/// <summary>
/// > This function sets the schema name for the query
/// </summary>
/// <param name="schemaName">The name of the schema to use.</param>
/// <returns>
/// The PricingUriParameterBuilder object.
/// </returns>
        public PricingUriParametersBuilder WithSchemaName(string schemaName)
        {
            _schemaName = schemaName;
            return this;
        }


/// <summary>
/// `WithTemplateId` sets the `_templateId` variable to the value of the `templateId` parameter
/// </summary>
/// <param name="templateId">The templateId of the results to return.</param>
/// <returns>
/// The PricingUriParameterBuilder object.
/// </returns>
        public PricingUriParametersBuilder WithTemplateId(string templateId)
        {
            _templateId = templateId;
            return this;
        }


/// <summary>
/// `WithIsTransferable` sets the `_isTransferable` field to the value of the `isTransferable` parameter
/// </summary>
/// <param name="isTransferable">The isTransferable parameter filters results.</param>
/// <returns>
/// The PricingUriParameterBuilder object.
/// </returns>
        public PricingUriParametersBuilder WithIsTransferable(bool isTransferable)
        {
            _isTransferable = isTransferable;
            return this;
        }


/// <summary>
/// `WithIsTransferable` sets the `_isTransferable` field to the value of the `isTransferable` parameter
/// </summary>
/// <param name="isTransferable">The isTransferable parameter filters for transferable assets.</param>
/// <returns>
/// The PricingUriParameterBuilder object.
/// </returns>
        public PricingUriParametersBuilder WithIsBurnable(bool isBurnable)
        {
            _isBurnable = isBurnable;
            return this;
        }


/// <summary>
/// It builds a query string based on the parameters that have been set
/// </summary>
/// <returns>
/// A string that contains the parameters for the query.
/// </returns>
        public string Build()
        {
            var parameterString = new StringBuilder("?");
            if (!string.IsNullOrEmpty(_symbol))
            {
                parameterString.Append($"&symbol={_symbol}");
            }
            if (_burned.HasValue)
            {
                parameterString.Append($"&burned={_burned}");
            }
            if (!string.IsNullOrEmpty(_collectionName))
            {
                parameterString.Append($"&collection_name={_collectionName}");
            }
            if (!string.IsNullOrEmpty(_schemaName))
            {
                parameterString.Append($"&schema_name={_schemaName}");
            }
            if (!string.IsNullOrEmpty(_templateId))
            {
                parameterString.Append($"&template_id={_templateId}");
            }
            if (_isTransferable.HasValue)
            {
                parameterString.Append($"&is_transferable={_isTransferable}");
            }
            if (_isBurnable.HasValue)
            {
                parameterString.Append($"&is_burnable={_isBurnable}");
            }
            return parameterString.ToString();
        }
    }
}
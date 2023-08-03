using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AtomicAssetsApiClient
{
    public class HttpRequestBuilder
    {
        private readonly HttpRequestMessage _request;
        private string _authenticationToken;
        private string _content;
        private string _authenticationType;

        private HttpRequestBuilder(HttpRequestMessage request) => _request = request;

        public static HttpRequestBuilder GetRequest(Uri uri)
            => new HttpRequestBuilder(new HttpRequestMessage
            {
                Method = HttpMethod.Get, 
                RequestUri = uri
            });

        public static HttpRequestBuilder PostRequest(Uri uri) 
            => new HttpRequestBuilder(new HttpRequestMessage
            {
                Method = HttpMethod.Post, 
                RequestUri = uri
            });

        public HttpRequestBuilder WithContent(string content)
        {
            _content = content;
            return this;
        }

        public HttpRequestBuilder WithAuthentication(string authenticationToken, string type = "bearer")
        {
            _authenticationToken = authenticationToken;
            _authenticationType = type;
            return this;
        }

        public HttpRequestMessage Build()
        {
            if (!string.IsNullOrEmpty(_content))
                _request.Content = new StringContent(_content, Encoding.UTF8, "application/json");
            if (!string.IsNullOrEmpty(_authenticationToken))
                _request.Headers.Authorization = new AuthenticationHeaderValue(_authenticationType, _authenticationToken);

            return _request;
        }
    }
}
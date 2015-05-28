using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Xml.Linq;
using RestSharp;

namespace Dilemma.Business.WebPurify
{
    internal class WebPurifyResponder : IWebPurifyResponder
    {
        private readonly string _apiKey;

        private readonly RestClient _client;

        public WebPurifyResponder(string apiKey)
        {
            _client = new RestClient("http://api1.webpurify.com/services/rest/");

            _apiKey = apiKey;
        }

        public WebPurifyStatus Return(string text, out IEnumerable<string> result)
        {
            var request = GetRequest(WebPurifyMethod.Return, text);
            var response = _client.Execute(request);

            var status = GetResponseStatus(response);

            if (status == WebPurifyStatus.Ok)
            {
                result = XDocument.Parse(response.Content).Elements("rsp").Elements("expletive").Select(x => x.Value);
            }
            else
            {
                result = Enumerable.Empty<string>();
            }

            return status;
        }

        private WebPurifyStatus GetResponseStatus(IRestResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return WebPurifyStatus.BadResponseStatusCode;
            }

            var doc = XDocument.Parse(response.Content);

            var rspNode = doc.Element("rsp");

            if (rspNode == null)
            {
                return WebPurifyStatus.ResponseNodeNotFound;
            }

            var statAttrib = rspNode.Attribute("stat");

            if (statAttrib == null)
            {
                return WebPurifyStatus.StatusAttributeNotFound;
            }

            if (statAttrib.Value == "ok")
            {
                return WebPurifyStatus.Ok;
            }
            else if (statAttrib.Value != "fail")
            {
                return WebPurifyStatus.UnknownStatusAttributeValue;
            }

            var errNode = rspNode.Element("err");

            if (errNode == null)
            {
                return WebPurifyStatus.ErrorNodeNotFound;
            }

            var codeAttribute = errNode.Attribute("code");

            if (codeAttribute == null)
            {
                return WebPurifyStatus.CodeAttributeNodeFound;
            }

            var errorCode = codeAttribute.Value;

            if (errorCode == "100")
            {
                return WebPurifyStatus.InvalidApiKey;
            }

            if (errorCode == "101")
            {
                return WebPurifyStatus.InactiveApiKey;
            }

            if (errorCode == "102")
            {
                return WebPurifyStatus.ApiKeyNotSent;
            }

            if (errorCode == "103")
            {
                return WebPurifyStatus.ServiceUnavailable;
            }

            if (errorCode == "104")
            {
                return WebPurifyStatus.RateLimitExceeded;
            }

            return WebPurifyStatus.UnknownCodeAttributeValue;
        }

        private RestRequest GetRequest(WebPurifyMethod method, string text)
        {
            var request = new RestRequest();

            request.AddParameter("api_key", _apiKey);
            request.AddParameter("method", GetWebPurifyMethodKey(method));
            request.AddParameter("text", text);

            return request;
        }

        private static string GetWebPurifyMethodKey(WebPurifyMethod method)
        {
            switch (method)
            {
                case WebPurifyMethod.Return:
                    return "webpurify.live.return";
                default:
                    throw new ArgumentOutOfRangeException("method", method, null);
            }
        }
    }
}

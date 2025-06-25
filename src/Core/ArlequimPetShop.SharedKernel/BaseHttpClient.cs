using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SrShut.Common;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ArlequimPetShop.SharedKernel
{
    public class BaseHttpClient
    {
        private readonly ILogger<BaseHttpClient> _logger;
        private readonly HttpClient _client;

        public BaseHttpClient(ILogger<BaseHttpClient> logger)
        {
            Throw.ArgumentIsNull(logger);

            _logger = logger;
            _client = new HttpClient { Timeout = TimeSpan.FromMilliseconds(Date.TimeOut()) };
        }

        public async Task<HttpResponse<T>> Send<T>(HttpMethod method, string url, bool saveLog, object? headers = null, object? requestData = null, Dictionary<string, string>? formUrlencoded = null, string? bearer = null)
        {
            if (saveLog)
                _logger.LogWarning(string.Format("Solicitação: {0} | {1}", method, url));

            var request = new HttpRequestMessage(method, url);

            if (bearer != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
            }

            if (headers != null)
            {
                foreach (var item in headers?.ToDictionary())
                    request.Headers.Add(item.Key, item.Value);
            }

            if (requestData != null)
            {
                var body = JsonConvert.SerializeObject(requestData);
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");

                if (saveLog)
                    _logger.LogWarning(string.Format("Solicitação Content: {0}", body));
            }


            if (formUrlencoded != null)
            {
                var content = new FormUrlEncodedContent(formUrlencoded);
                request.Content = content;

                if (saveLog)
                    _logger.LogWarning(string.Format("Solicitação Content: {0}", content));
            }

            using (var response = await _client.SendAsync(request))
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                    case HttpStatusCode.Accepted:
                        {
                            var data = await response.Content.ReadAsStringAsync();

                            if (saveLog)
                                _logger.LogWarning(string.Format("Retorno Sucesso: {0}", data));

                            return new HttpResponse<T>
                            {
                                StatusCode = response.StatusCode,
                                Messages = "Success",
                                Data = JsonConvert.DeserializeObject<T>(data ?? "")
                            };
                        }
                    default:
                        {
                            var data = await response.Content.ReadAsStringAsync();

                            if (saveLog)
                                _logger.LogWarning($"Retorno Erro: {response.RequestMessage}");

                            return new HttpResponse<T>
                            {
                                StatusCode = response.StatusCode,
                                Data = JsonConvert.DeserializeObject<T>(data ?? "")
                            };
                        }
                }
            }
        }

        public class HttpResponse<T>
        {
            public HttpStatusCode StatusCode { get; set; }

            public string Messages { get; set; }

            public T? Data { get; set; }
        }
    }
}
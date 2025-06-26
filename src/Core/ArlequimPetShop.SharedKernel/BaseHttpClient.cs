using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SrShut.Common;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ArlequimPetShop.SharedKernel;

/// <summary>
/// Cliente HTTP base para envio de requisições com suporte a log, headers customizados e autenticação via bearer token.
/// </summary>
public class BaseHttpClient
{
    private readonly ILogger<BaseHttpClient> _logger;
    private readonly HttpClient _client;

    /// <summary>
    /// Construtor principal. Aceita injeção de um HttpClient externo (para facilitar testes unitários).
    /// </summary>
    /// <param name="logger">Instância do logger.</param>
    /// <param name="client">HttpClient opcional para injeção (default: novo HttpClient).</param>
    public BaseHttpClient(ILogger<BaseHttpClient> logger, HttpClient? client = null)
    {
        Throw.ArgumentIsNull(logger);
        _logger = logger;
        _client = client ?? new HttpClient { Timeout = TimeSpan.FromMilliseconds(Date.TimeOut()) };
    }

    /// <summary>
    /// Envia uma requisição HTTP com suporte a log, headers, conteúdo JSON ou x-www-form-urlencoded, e autenticação via token.
    /// </summary>
    /// <typeparam name="T">Tipo esperado no retorno da resposta (JSON).</typeparam>
    /// <param name="method">Verbo HTTP (GET, POST, etc).</param>
    /// <param name="url">URL do endpoint.</param>
    /// <param name="saveLog">Se deve registrar log da requisição.</param>
    /// <param name="headers">Headers adicionais (opcional).</param>
    /// <param name="requestData">Objeto para envio como JSON (opcional).</param>
    /// <param name="formUrlencoded">Dados para envio como formulário (opcional).</param>
    /// <param name="bearer">Token de autenticação (opcional).</param>
    /// <returns>Resposta HTTP contendo status, mensagem e dados desserializados.</returns>
    public async Task<HttpResponse<T>> Send<T>(HttpMethod method, string url, bool saveLog, object? headers = null, object? requestData = null, Dictionary<string, string>? formUrlencoded = null, string? bearer = null)
    {
        if (saveLog)
            _logger.LogWarning($"Solicitação: {method} | {url}");

        var request = new HttpRequestMessage(method, url);

        if (bearer != null)
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

        if (headers != null)
        {
            foreach (var item in headers.ToDictionary())
                request.Headers.Add(item.Key, item.Value);
        }

        if (requestData != null)
        {
            var body = JsonConvert.SerializeObject(requestData);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            if (saveLog)
                _logger.LogWarning($"Solicitação Content: {body}");
        }

        if (formUrlencoded != null)
        {
            var content = new FormUrlEncodedContent(formUrlencoded);
            request.Content = content;

            if (saveLog)
                _logger.LogWarning($"Solicitação Content: {content}");
        }

        using var response = await _client.SendAsync(request);

        var responseBody = await response.Content.ReadAsStringAsync();

        if (saveLog)
            _logger.LogWarning($"Retorno {(response.IsSuccessStatusCode ? "Sucesso" : "Erro")}: {responseBody}");

        return new HttpResponse<T>
        {
            StatusCode = response.StatusCode,
            Messages = response.IsSuccessStatusCode ? "Success" : "Error",
            Data = JsonConvert.DeserializeObject<T>(responseBody ?? string.Empty)
        };
    }

    /// <summary>
    /// Modelo de resposta HTTP unificada.
    /// </summary>
    /// <typeparam name="T">Tipo dos dados retornados.</typeparam>
    public class HttpResponse<T>
    {
        /// <summary>
        /// Código HTTP retornado pela requisição.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Mensagem de status (Success/Error).
        /// </summary>
        public string Messages { get; set; } = string.Empty;

        /// <summary>
        /// Dados retornados da resposta, desserializados.
        /// </summary>
        public T? Data { get; set; }
    }
}
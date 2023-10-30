﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.EndpointProviders;
using OpenAI.Interfaces;

namespace OpenAI.Managers;

public partial class OpenAIService : IOpenAIService, IDisposable
{
    private readonly bool _disposeHttpClient;
    private readonly IOpenAIEndpointProvider _endpointProvider;
    private readonly HttpClient _httpClient;
    private string? _defaultModelId;

    [ActivatorUtilitiesConstructor]
    public OpenAIService(IOptions<OpenAIOptions> settings, HttpClient httpClient)
        : this(settings.Value, httpClient)
    {
    }

    public OpenAIService(OpenAIOptions settings, HttpClient? httpClient = null)
    {
        settings.Validate();

        if (httpClient == null)
        {
            _disposeHttpClient = true;
            _httpClient = new HttpClient();
        }
        else
        {
            _httpClient = httpClient;
        }

        _httpClient.BaseAddress = new Uri(settings.BaseDomain);

        // switch (settings.ProviderType)
        // {
        //     case ProviderType.Azure:
        //         _httpClient.DefaultRequestHeaders.Add("api-key", settings.ApiKey);
        //         break;
        //     case ProviderType.OpenAI:
        //     default:
        //         _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.ApiKey}");
        //         break;
        // }
        _httpClient.DefaultRequestHeaders.Add("X-Access-Code", $"Bearer {settings.AccessCode}");

        if (!string.IsNullOrEmpty(settings.Organization))
        {
            _httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", $"{settings.Organization}");
        }

        _endpointProvider = settings.ProviderType switch
        {
            ProviderType.Azure => new AzureOpenAIEndpointProvider(settings.ApiVersion, settings.DeploymentId!, settings.ProxyPath),
            _ => new OpenAIEndpointProvider(settings.ApiVersion, settings.ProxyPath)
        };

        _defaultModelId = settings.DefaultModelId;
    }

    /// <summary>
    ///     Method to dispose the HttpContext if created internally.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    /// <inheritdoc />
    public IModelService Models => this;

    /// <inheritdoc />
    public ICompletionService Completions => this;

    /// <inheritdoc />
    public IEmbeddingService Embeddings => this;

    /// <inheritdoc />
    public IFileService Files => this;

    /// <inheritdoc />
    public IFineTuneService FineTunes => this;

    /// <inheritdoc />
    public IFineTuningJobService FineTuningJob => this;

    /// <inheritdoc />
    public IModerationService Moderation => this;

    /// <inheritdoc />
    public IImageService Image => this;

    /// <inheritdoc />
    public IEditService Edit => this;

    /// <inheritdoc />
    public IChatCompletionService ChatCompletion => this;

    /// <inheritdoc />
    public IAudioService Audio => this;

    /// <summary>
    ///     Sets default Model Id
    /// </summary>
    /// <param name="modelId"></param>
    public void SetDefaultModelId(string modelId)
    {
        _defaultModelId = modelId;
    }

    /// <summary>
    ///     Get default Model Id
    /// </summary>
    /// <returns></returns>
    public string? GetDefaultModelId()
    {
        return _defaultModelId;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_disposeHttpClient && _httpClient != null)
            {
                _httpClient.Dispose();
            }
        }
    }
}
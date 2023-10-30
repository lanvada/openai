using System.Net;

namespace OpenAI.EndpointProviders;

internal class OpenAIEndpointProvider : IOpenAIEndpointProvider
{
    private readonly string _apiVersion;
    private readonly string _proxyPath;

    public OpenAIEndpointProvider(string apiVersion, string? proxyPath = null)
    {
        _apiVersion = apiVersion;
        _proxyPath = proxyPath ?? "";
    }

    private string Prefix => $"/{_proxyPath}/{_apiVersion}";

    public string ModelRetrieve(string model)
    {
        return $"{Prefix}/models/{model}";
    }

    public string FileDelete(string fileId)
    {
        return $"{Prefix}/files/{fileId}";
    }

    public string CompletionCreate()
    {
        return $"{Prefix}/completions";
    }

    public string ChatCompletionCreate()
    {
        return $"{Prefix}/chat/completions";
    }

    public string AudioCreateTranscription()
    {
        return $"{Prefix}/audio/transcriptions";
    }

    public string AudioCreateTranslation()
    {
        return $"{Prefix}/audio/translations";
    }

    public string EditCreate()
    {
        return $"{Prefix}/edits";
    }

    public string ModelsList()
    {
        return $"{Prefix}/models";
    }

    public string FilesList()
    {
        return Files();
    }

    public string FilesUpload()
    {
        return Files();
    }

    public string FileRetrieve(string fileId)
    {
        return $"{Prefix}/files/{fileId}";
    }

    public string FileRetrieveContent(string fileId)
    {
        return $"{Prefix}/files/{fileId}/content";
    }

    public string FineTuneCreate()
    {
        return $"{Prefix}/fine-tunes";
    }

    public string FineTuneList()
    {
        return $"{Prefix}/fine-tunes";
    }

    public string FineTuneRetrieve(string fineTuneId)
    {
        return $"{Prefix}/fine-tunes/{fineTuneId}";
    }

    public string FineTuneCancel(string fineTuneId)
    {
        return $"{Prefix}/fine-tunes/{fineTuneId}/cancel";
    }

    public string FineTuneListEvents(string fineTuneId)
    {
        return $"{Prefix}/fine-tunes/{fineTuneId}/events";
    }

    public string FineTuneDelete(string fineTuneId)
    {
        return $"{Prefix}/models/{fineTuneId}";
    }

    public string EmbeddingCreate()
    {
        return $"{Prefix}/embeddings";
    }

    public string ModerationCreate()
    {
        return $"{Prefix}/moderations";
    }

    public string ImageCreate()
    {
        return $"{Prefix}/images/generations";
    }

    public string ImageEditCreate()
    {
        return $"{Prefix}/images/edits";
    }

    public string ImageVariationCreate()
    {
        return $"{Prefix}/images/variations";
    }

    private string Files()
    {
        return $"{Prefix}/files";
    }
}
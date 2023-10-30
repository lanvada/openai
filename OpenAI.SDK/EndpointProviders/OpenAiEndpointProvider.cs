using System.Net;
using OpenAI.ObjectModels.RequestModels;

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

    public string FineTuningJobCreate()
    {
        return $"{Prefix}/fine_tuning/jobs";
    }

    public string FineTuningJobList(FineTuningJobListRequest? fineTuningJobListRequest)
    {
        var url = $"{Prefix}/fine_tuning/jobs";
        if (fineTuningJobListRequest != null)
        {
            var queryParams = new List<string>();
            if (fineTuningJobListRequest.After != null)
                queryParams.Add($"after={WebUtility.UrlEncode(fineTuningJobListRequest.After)}");
            if (fineTuningJobListRequest.Limit.HasValue)
                queryParams.Add($"limit={fineTuningJobListRequest.Limit.Value}");
        
            if (queryParams.Any())
                url = $"{url}?{string.Join("&", queryParams)}";
        }
        return url;
    }

    public string FineTuningJobRetrieve(string fineTuningJobId)
    {
        return $"{Prefix}/fine_tuning/jobs/{fineTuningJobId}";
    }

    public string FineTuningJobCancel(string fineTuningJobId)
    {
        return $"{Prefix}/fine_tuning/jobs/{fineTuningJobId}/cancel";
    }

    public string FineTuningJobListEvents(string fineTuningJobId)
    {
        return $"{Prefix}/fine_tuning/jobs/{fineTuningJobId}/events";
    }

    public string ModelsDelete(string modelId)
    {
        return $"{Prefix}/models/{modelId}";
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
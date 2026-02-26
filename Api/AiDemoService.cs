using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OptiDemoCms.Api;

/// <summary>
/// Demo AI service: Groq (chat completions) and Hugging Face (summarization, sentiment).
/// When API keys are not set, returns demo responses so Postman always works.
/// </summary>
public class AiDemoService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    private string? GroqApiKey => _configuration["Ai:GroqApiKey"];
    private string? HuggingFaceToken => _configuration["Ai:HuggingFaceToken"];

    public AiDemoService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public bool IsGroqConfigured => !string.IsNullOrWhiteSpace(GroqApiKey);
    public bool IsHuggingFaceConfigured => !string.IsNullOrWhiteSpace(HuggingFaceToken);

    public async Task<string> CompleteAsync(string prompt, CancellationToken cancellationToken = default)
    {
        if (!IsGroqConfigured)
        {
            return $"[Demo mode – set Ai:GroqApiKey for real AI] A thoughtful answer to: \"{prompt}\" might be: Testing and automation help deliver reliable software. Try adding GROQ_API_KEY or Ai:GroqApiKey for live completions with Groq (Llama).";
        }

        var client = _httpClientFactory.CreateClient("Groq");
        var body = new { model = "llama-3.3-70b-versatile", messages = new[] { new { role = "user", content = prompt } }, max_tokens = 150 };
        var response = await client.PostAsJsonAsync("https://api.groq.com/openai/v1/chat/completions", body, cancellationToken);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        var content = json.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        return content ?? "";
    }

    public async Task<string> SummarizeAsync(string text, CancellationToken cancellationToken = default)
    {
        if (!IsHuggingFaceConfigured)
        {
            var preview = text.Length > 120 ? text[..120] + "…" : text;
            return $"[Demo mode – set Ai:HuggingFaceToken for real summarization] Summary of: \"{preview}\" — Use Hugging Face token for live summarization with BART.";
        }

        var client = _httpClientFactory.CreateClient("HuggingFace");
        var body = new { inputs = text };
        var response = await client.PostAsJsonAsync("https://api-inference.huggingface.co/models/facebook/bart-large-cnn", body, cancellationToken);
        response.EnsureSuccessStatusCode();
        var list = await response.Content.ReadFromJsonAsync<List<HfSummaryResult>>(cancellationToken);
        return list?.FirstOrDefault()?.SummaryText ?? text[..Math.Min(200, text.Length)] + "…";
    }

    public async Task<(string Label, double Score)> SentimentAsync(string text, CancellationToken cancellationToken = default)
    {
        if (!IsHuggingFaceConfigured)
        {
            return ("DEMO", 0.95);
        }

        var client = _httpClientFactory.CreateClient("HuggingFace");
        var body = new { inputs = text };
        var response = await client.PostAsJsonAsync("https://api-inference.huggingface.co/models/distilbert-base-uncased-finetuned-sst-2-english", body, cancellationToken);
        response.EnsureSuccessStatusCode();
        var list = await response.Content.ReadFromJsonAsync<List<HfClassificationResult>>(cancellationToken);
        var first = list?.FirstOrDefault();
        return (first?.Label ?? "NEUTRAL", first?.Score ?? 0.5);
    }

    private class HfSummaryResult
    {
        [JsonPropertyName("summary_text")]
        public string? SummaryText { get; set; }
    }

    private class HfClassificationResult
    {
        public string? Label { get; set; }
        public double Score { get; set; }
    }
}

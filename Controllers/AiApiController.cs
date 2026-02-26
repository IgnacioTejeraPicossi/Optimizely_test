using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Api;

namespace OptiDemoCms.Controllers;

/// <summary>
/// Demo AI API for Postman: completions (Groq/Llama), summarization and sentiment (Hugging Face).
/// Works in demo mode without API keys; set Ai:GroqApiKey and/or Ai:HuggingFaceToken for live AI.
/// </summary>
[ApiController]
[Route("api/ai")]
public class AiApiController : ControllerBase
{
    private readonly AiDemoService _aiService;

    public AiApiController(AiDemoService aiService)
    {
        _aiService = aiService;
    }

    /// <summary>
    /// Health and capability check. Shows whether Groq and Hugging Face are configured.
    /// </summary>
    [HttpGet("health")]
    [Produces("application/json")]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "ok",
            mode = _aiService.IsGroqConfigured || _aiService.IsHuggingFaceConfigured ? "live" : "demo",
            providers = new
            {
                groq = _aiService.IsGroqConfigured,
                huggingface = _aiService.IsHuggingFaceConfigured
            },
            endpoints = new[] { "POST /api/ai/complete", "POST /api/ai/summarize", "POST /api/ai/sentiment" }
        });
    }

    /// <summary>
    /// Chat completion (Groq/Llama). In demo mode returns a sample response without calling Groq.
    /// </summary>
    [HttpPost("complete")]
    [Produces("application/json")]
    public async Task<IActionResult> Complete([FromBody] CompleteRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request?.Prompt))
            return BadRequest(new { error = "Prompt is required." });

        var text = await _aiService.CompleteAsync(request.Prompt, cancellationToken);
        return Ok(new { prompt = request.Prompt, completion = text });
    }

    /// <summary>
    /// Summarize text (Hugging Face BART). In demo mode returns a placeholder.
    /// </summary>
    [HttpPost("summarize")]
    [Produces("application/json")]
    public async Task<IActionResult> Summarize([FromBody] SummarizeRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request?.Text))
            return BadRequest(new { error = "Text is required." });

        var summary = await _aiService.SummarizeAsync(request.Text, cancellationToken);
        return Ok(new { originalLength = request.Text.Length, summary });
    }

    /// <summary>
    /// Sentiment analysis (Hugging Face). In demo mode returns label DEMO and score 0.95.
    /// </summary>
    [HttpPost("sentiment")]
    [Produces("application/json")]
    public async Task<IActionResult> Sentiment([FromBody] SentimentRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request?.Text))
            return BadRequest(new { error = "Text is required." });

        var (label, score) = await _aiService.SentimentAsync(request.Text, cancellationToken);
        return Ok(new { text = request.Text, label, score });
    }

    public class CompleteRequest
    {
        public string? Prompt { get; set; }
    }

    public class SummarizeRequest
    {
        public string? Text { get; set; }
    }

    public class SentimentRequest
    {
        public string? Text { get; set; }
    }
}

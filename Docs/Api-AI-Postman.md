# AI Demo API (Postman)

Demo API to showcase **Postman** and **AI integrations**: chat completions (Groq/Llama), summarization and sentiment (Hugging Face). All endpoints work in **demo mode** without API keys; when keys are set, live AI is used.

## Base URL

- **Local:** `https://localhost:5000` (or `http://localhost:5000` if not using HTTPS)

## Endpoints

| Method | Path | Description |
|--------|------|-------------|
| GET | `/api/ai/health` | Health and capability check (demo vs live, which providers are configured) |
| POST | `/api/ai/complete` | Chat completion (Groq/Llama). Body: `{ "prompt": "Why is testing important?" }` |
| POST | `/api/ai/summarize` | Summarize text (Hugging Face BART). Body: `{ "text": "Long paragraph..." }` |
| POST | `/api/ai/sentiment` | Sentiment analysis (Hugging Face). Body: `{ "text": "I love this product!" }` |

## Demo mode vs live AI

- **No API keys:** All endpoints return demo/sample responses so you can try them in Postman immediately.
- **With API keys:** Set `Ai:GroqApiKey` and/or `Ai:HuggingFaceToken` (or env vars `Ai__GroqApiKey`, `Ai__HuggingFaceToken`) to use:
  - **Groq** — Llama 3.3 70B for `/api/ai/complete` (fast, free tier).
  - **Hugging Face** — BART for summarization, DistilBERT for sentiment.

## Postman

1. Import the collection: **`postman/Optimizely-Demo-AI-API.postman_collection.json`**
2. Set the collection variable **`baseUrl`** to `https://localhost:5000` (or your app URL).
3. For HTTPS with self-signed cert: in Postman Settings → turn off "SSL certificate verification" for this environment if needed.
4. Run **Get AI health** first, then **Complete**, **Summarize**, **Sentiment**.

## Example requests (Postman / cURL)

**Health:**
```http
GET {{baseUrl}}/api/ai/health
```

**Complete (prompt for Llama):**
```http
POST {{baseUrl}}/api/ai/complete
Content-Type: application/json

{ "prompt": "In one sentence, why is software testing important?" }
```

**Summarize:**
```http
POST {{baseUrl}}/api/ai/summarize
Content-Type: application/json

{ "text": "Optimizely CMS is a .NET-based content management system. It supports blocks, pages, and multi-site. Many enterprises use it for their websites." }
```

**Sentiment:**
```http
POST {{baseUrl}}/api/ai/sentiment
Content-Type: application/json

{ "text": "This demo is really useful for learning APIs and AI." }
```

## Getting API keys (optional)

- **Groq:** [console.groq.com](https://console.groq.com) — free tier, very fast Llama inference.
- **Hugging Face:** [huggingface.co/settings/tokens](https://huggingface.co/settings/tokens) — create a token with “Inference API” access.

Set in `appsettings.Development.json` under `Ai` or via environment variables `Ai__GroqApiKey` and `Ai__HuggingFaceToken`.

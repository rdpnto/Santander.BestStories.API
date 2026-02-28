# Santander – Hacker News Best Stories API

## Overview

This project implements a RESTful API using **ASP.NET Core** to retrieve the top **N best stories** from the Hacker News API, ordered by score in descending order.

The API is designed with:

- Clean Architecture
- Concurrency Control
- Caching
- Resilience policies

The solution is organized by separating responsibilities into independent layers:

```
Santander.BestStories.API
│
├── Presentation        → Controllers and configuration
├── Application         → Use cases
├── Domain              → Entities and contracts
├── Infrastructure      → External integrations
└── Tests               → Unit tests
```

## How to Run

### Requirements

- .NET 8 SDK

### Instructions

1. Restore: `dotnet restore`
2. Build: `dotnet build`
3. Run: `dotnet run`
4. Access `https://localhost:{port}/swagger`

## Endpoints

### 1. Get N Best Stories

```
GET /api/stories/best-stories?n={number}
```

### Response

```json
[
  {
    "title": "Example Story",
    "uri": "https://example.com",
    "postedBy": "author",
    "time": "2025-02-27T12:00:00+00:00",
    "score": 12345,
    "commentCount": 10
  }
]
```

### 2. Health Check

```
GET /health-check
```

### Response

```json
healthy
```

## Assumptions

- Only the number of requested stories (`n`) is considered, and the resultant items are stored in memory. Due to such nature, while cache isn't refreshed the results might not reflect the latest data from HackerNewsAPI. This behaviour should be configurable by adjusting the Ttl. An evolution to this API might be designing a subscription to Firebase, which will help keep track of data changes and therefore reflect real-time updates.
- In-Memory cache mechanism is used to avoid overloading the HackerNewsAPI, meaning that an instance of the API will have it's data shared throughout it's entire lifetime.
- The [HackerNews API docs](`https://github.com/HackerNews/API`) was used to determine the matching fields for the data schema proposed by the Code Challenge.

## Author

Rodrigo Pinto de Souza
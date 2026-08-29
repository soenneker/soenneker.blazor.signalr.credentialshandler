[![](https://img.shields.io/nuget/v/soenneker.blazor.signalr.credentialshandler.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.signalr.credentialshandler/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.signalr.credentialshandler/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.signalr.credentialshandler/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.signalr.credentialshandler.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.signalr.credentialshandler/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.signalr.credentialshandler/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.signalr.credentialshandler/actions/workflows/codeql.yml)

# Soenneker.Blazor.SignalR.CredentialsHandler

A `DelegatingHandler` for Blazor WebAssembly that sets browser request credentials to `Include`, allowing matching cookies to accompany SignalR HTTP requests.

## Installation

```bash
dotnet add package Soenneker.Blazor.SignalR.CredentialsHandler
```

## Usage

Create the handler inside the SignalR connection's `HttpMessageHandlerFactory` and retain the inner handler supplied by SignalR:

```csharp
using Microsoft.AspNetCore.SignalR.Client;
using Soenneker.Blazor.SignalR.CredentialsHandler;

HubConnection connection = new HubConnectionBuilder()
    .WithUrl("https://api.example.com/hubs/notifications", options =>
    {
        options.HttpMessageHandlerFactory = innerHandler =>
            new BlazorSignalRCredentialsHandler
            {
                InnerHandler = innerHandler
            };
    })
    .WithAutomaticReconnect()
    .Build();

await connection.StartAsync();
```

Use the handler when the hub authenticates browser clients with cookies and the relevant SignalR HTTP requests must include them. It changes the browser fetch credentials mode; it does not create cookies, add bearer tokens, refresh authentication, or configure the hub.

## Cross-origin connections

For a hub on another origin, the server must explicitly allow the Blazor application's origin and credentials:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy => policy
        .WithOrigins("https://app.example.com")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});
```

The server must apply that policy to the request pipeline, and cross-site cookies generally need appropriate `SameSite=None` and `Secure` attributes. A wildcard CORS origin cannot be combined with credentialed requests.

## Scope and security

- Use a dedicated handler instance for the hub connection. Every HTTP request passing through it is marked to include matching browser credentials.
- The browser still enforces cookie domain, path, `SameSite`, security, and CORS rules.
- The handler affects SignalR's HTTP requests, including negotiation and HTTP-based transports. Browser WebSocket credential behavior remains controlled by the browser.
- This package is for Blazor WebAssembly browser HTTP handling; it is not an authentication handler for Blazor Server or other server-side .NET clients.

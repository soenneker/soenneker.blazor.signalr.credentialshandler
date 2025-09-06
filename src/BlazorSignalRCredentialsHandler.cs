using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Soenneker.Blazor.SignalR.CredentialsHandler.Abstract;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.SignalR.CredentialsHandler;

/// <inheritdoc cref="IBlazorSignalRCredentialsHandler"/>
public sealed class BlazorSignalRCredentialsHandler : DelegatingHandler, IBlazorSignalRCredentialsHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        return base.SendAsync(request, cancellationToken);
    }
}

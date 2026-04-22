using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.SignalR.CredentialsHandler.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class BlazorSignalRCredentialsHandlerTests : HostedUnitTest
{
    public BlazorSignalRCredentialsHandlerTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}

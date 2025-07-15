using Octokit.Webhooks.Events;
using Octokit.Webhooks;
using Octokit.Webhooks.Events.RegistryPackage;

namespace pefi.githublistener.Services;

public sealed class ProcessRegistryPackageWebhookProcessor(ServiceManagerClient svcmgr, ILogger<ProcessRegistryPackageWebhookProcessor> logger) : WebhookEventProcessor
{
    protected async override Task ProcessRegistryPackageWebhookAsync(WebhookHeaders headers, RegistryPackageEvent evt, RegistryPackageAction action)
    {
        var packageUrl = evt.Package.PackageVersion!.PackageUrl;

        var purl = packageUrl[..packageUrl.LastIndexOf(":")];

        var services = await svcmgr.Get_All_ServicesAsync();

        foreach (var service in services.Where(x => x.dockerImageUrl == purl))
        {
           await svcmgr.Update_ServiceAsync(service.serviceName);
        }

    }

}

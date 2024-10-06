using Asp.Versioning.Builder;
using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Evently.Common.Application.Extensions;

public static class ApplicationApiVersionSet
{
    public static ApiVersionSet VersionSets(this IEndpointRouteBuilder app)
    {
        return app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();
    }
}
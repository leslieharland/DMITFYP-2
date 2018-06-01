using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.FileProviders;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseStaticResources(this IApplicationBuilder app, HostingEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "dist"); // this allows for serving up contents in a folder named 'static'
        var provider = new PhysicalFileProvider(path);

        var options = new StaticFileOptions();
        options.RequestPath = ""; // an empty string will give the *appearance* of it being served up from the root
        //options.RequestPath = "/content"; // this will use the URL path named content, but could be any made-up name you want
        options.FileProvider = provider;

        app.UseStaticFiles(options);
        return app;
    }
}
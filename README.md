# StaticSiteServer

StaticSiteServer is a small library to assist in serving static files from your Azure Functions instance.

## How to use
First add a folder to your solution called `wwwroot` (this can be overriden with the enviroment variable `CONTENT_ROOT`). All the files in 
this folder must be set to the build action `Content` and copy to output directory `Copy if newer`.

Add the dependencies to your DI container `builder.Services.AddStaticSiteServing()`

(Optional) Remove the default `api` route by altering your `host.json` file: 
```
{
  "version": "2.0",
  "extensions": {
    "http": {
      "routePrefix": ""
    }
  }
}
```
Add your static server function(s):
```
public class Function1
{
    private readonly ISiteService _siteService;
    public Function1(ISiteService siteService)
    {
        _siteService = siteService;
    }
    [FunctionName(nameof(ServeRoot))]
    public async Task<IActionResult> ServeRoot(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{file?}")]
        HttpRequest req
    )
    {
        return await _serveStatic.Run(req, file);
    }
}
```

Now if you run your Azure Functions instance, the base path will response with `index.html`.

## Route collision with wwwroot and subfolders
Because we have overriden the default route with the `ServeRoot` function, we need to prefix the route of every other function manually. 
So any api calls should have the Route `Route = "api/some/route"` else you risk `ServeRoot` gobbling it up. For `wwwroot` subfolders you can 
set up a function like this:
```
[FunctionName(nameof(StaticContent))]
public async Task<IActionResult> StaticContent(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "content/{folder}/{file}")]
    HttpRequest req,
    string file,string folder) 
{ 
    await _serveStatic.Run(req, file, folder);
}
```
This will capture `/content/css/site.css` and route it to `wwwroot/css/site.css` so your folder structure can remain sane.

Alternatively you can define a function for each subfolder manually such as `Route = "css/{file}"` and call `_serveStatic.Run(req,file,"css");`

## Response headers
Response headers are super simple to add to specific file types using the DI setup options:
```
builder.Services.AddStaticSiteServing(options => {
    options.AddResponseHeader(new SiteHeaderFilter(".html", "Cache-Control", "no-cache, no-store"));
    options.AddResponseHeader(new SiteHeaderFilter(".css", "Cache-Control", "public, max-age=15552000"));
});
```
Url.Handler
===========

Url handler library for .net

This component works using filters to modify the original url provided, so in order to get this working you will need
identify whats filters do you need.

### Quick start
```csharp
Resolve urlHandler = new Resolve("/assets/media/image.jpg");

//create a filter to change the relative URL to resolve the assets from a CDN
urlHandler.AddFilter(delegate(string _url, Resolve holder) {
    string host = "mycdnbucket";

    if (holder.IsRelative)
        return new Uri(new Uri(host), _url).ToString();

    Uri uri = new Uri(_url);

    return uri.ToString().Replace(uri.Host, host);
});

//get te URL after executing all the filters (in this case just one): http://mycdnbucket/assets/media/image.jpg
var resultUrl = urlHandler.ApplyFilters();
```
### Aditional configuration
There are three events related to the execution flow.

#### BeforeStart
Event that occurs before all the process begins.
```csharp
Resolve urlHandler = new Resolve("/assets/media/image.jpg");

urlHandler.BeforeStart += handler_BeforeStart;

static void handler_BeforeStart(Filter currentFilter, Resolve myArgs)
{
    Trace.WriteLine("custom event fired - BeforeStart");
}
```
#### BeforeFilterApply
Event that occurs before every filter is executed.
```csharp
Resolve urlHandler = new Resolve("/assets/media/image.jpg");

urlHandler.BeforeFilterApply += handler_BeforeFilterApply;

static void handler_BeforeFilterApply(Filter currentFilter, Resolve myArgs)
{
    Trace.WriteLine("custom event fired - BeforeFilterApply");
}
```
#### AfterFilterApply
Event that occurs after every filter is executed, the base class executes a validation input string to check if is relative or absolute url.
```csharp
Resolve urlHandler = new Resolve("/assets/media/image.jpg");

urlHandler.AfterFilterApply += handler_AfterFilterApply;

static void handler_AfterFilterApply(Filter currentFilter, Resolve myArgs)
{
    Trace.WriteLine("custom event fired - AfterFilterApply");
}
```


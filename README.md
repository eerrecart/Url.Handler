Url.Handler
===========

Url handler library for .net

This component works using filters to modify the original url.

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

//get te URL after executing all filters (in this case just one): http://mycdnbucket/assets/media/image.jpg
var resultUrl = urlHandler.ApplyFilters();
```
### Aditional configuration
There are three events related to the execution flow.

#### BeforeStart
Occurs before all the process begins.
```csharp
Resolve urlHandler = new Resolve("/assets/media/image.jpg");

urlHandler.BeforeStart += handler_BeforeStart;

static void handler_BeforeStart(Filter currentFilter, Resolve myArgs)
{
    Trace.WriteLine("custom event fired - BeforeStart");
}
```
#### BeforeFilterApply
Occurs before each filter is executed.
```csharp
Resolve urlHandler = new Resolve("/assets/media/image.jpg");

urlHandler.BeforeFilterApply += handler_BeforeFilterApply;

static void handler_BeforeFilterApply(Filter currentFilter, Resolve myArgs)
{
    Trace.WriteLine("custom event fired - BeforeFilterApply");
}
```
#### AfterFilterApply
Occurs after each filter is executed, the base class run a validation input string to check if is relative or absolute url.
```csharp
Resolve urlHandler = new Resolve("/assets/media/image.jpg");

urlHandler.AfterFilterApply += handler_AfterFilterApply;

static void handler_AfterFilterApply(Filter currentFilter, Resolve myArgs)
{
    Trace.WriteLine("custom event fired - AfterFilterApply");
}
```

License
-------

The MIT License (MIT)

Copyright (c) 2013 Elias Errecart

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

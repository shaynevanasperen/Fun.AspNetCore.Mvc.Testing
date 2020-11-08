<img src="GreenTick.png" align="right" />

[![Build status](https://ci.appveyor.com/api/projects/status/x7v9xktorso5yidp?svg=true)](https://ci.appveyor.com/project/shaynevanasperen/fun-aspnetcore-mvc-testing)
[![Join the chat at https://gitter.im/Fun-AspNetCore-Mvc-Testing](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Fun-AspNetCore-Mvc-Testing)
![License](https://img.shields.io/github/license/shaynevanasperen/Fun.AspNetCore.Mvc.Testing.svg)

[![NuGet](https://img.shields.io/nuget/v/Fun.AspNetCore.Mvc.Testing.svg)](https://www.nuget.org/packages/Fun.AspNetCore.Mvc.Testing)
[![NuGet](https://img.shields.io/nuget/dt/Fun.AspNetCore.Mvc.Testing.svg)](https://www.nuget.org/packages/Fun.AspNetCore.Mvc.Testing)

## Fun.AspNetCore.Mvc.Testing

A set of libraries for eliminating boilerplate code in web application tests. The main innovation here is that we provide an _extensible_ `WebApplicationFactory<TEntryPoint>`.
The problem with the default one is that it requires us to subclass it for each way that we'd like to configure our web application (for each fixture, or suite of similar tests).
This is not practical, because we often need to configure our application in different ways, and we'd like to be able to define that configuration composably from within the test
classes themselves. We can't just make our test classes derive from `WebApplicationFactory<TEntryPoint>`, as then we'd be creating a new instance of our web application for each test!

We want to share a single instance of our web application per test suite rather than per test, but allow the test class to define how that web application is configured. The
included `ExtensibleWebApplicationFactory<TEntryPoint>` class allows us to do just that, and in a beautifully composable way that eliminates boilerplate code and keeps our tests
clean and DRY.

Create your base class similar to this:
```cs
public abstract class WebApplicationTests : XunitWebApplicationTests<Startup> // implements IClassFixture<ExtensibleWebApplicationFactory<TEntryPoint>>
{
    protected WebApplicationTests(ExtensibleWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        : base(factory, testOutputHelper)
    {
        factory.AfterConfigureAppConfiguration((context, builder) => builder.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "Some.Setting", "FakeValue" }
        }));
        factory.AfterConfigureServices(services =>
        {
            services.AddHttpRequestInterceptor();
            services.ReplaceLoggerFactoryWithXUnit(testOutputHelper);
        });
    }

    public HttpClientInterceptorOptions Interceptor => App.Services.GetRequiredService<HttpClientInterceptorOptions>();
}
```
And then add further customization in a test class:
```cs
public class FooTests : WebApplicationTests
{
    protected WebApplicationTests(ExtensibleWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        : base(factory, testOutputHelper)
    {
        // The configurations we apply here are composed on top of those performed by the base class
        factory.AfterConfigureAppConfiguration((context, builder) => builder.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "Other.Setting", "OtherFakeValue" }
        }));
        factory.AfterConfigureServices(services =>
        {
            services.ReplaceDatabaseWithAFake();
            services.ReplaceFooServiceWithAFake();
            services.AddTransient<ITestScopeProducer>(provider =>
                new TestScopeProducer<IDisposable>(provider.GetRequiredService<FakeDbScope>().BeginScope, x => x.Dispose));
        });
    }

    [Fact]
    public async void FooWorks()
    {
        using var scope = App.BeginTestScope();
        
        HttpRequests.Intercept
            .For(x => x.RequestUri.Host == "some.web.api/external-dependency")
            .Responds()
            .WithStatus(HttpStatusCode.NotModified)
            .RegisterWith(Interceptor);

        var response = await App.Client.GetAsync("foo/check");
        response.IsSuccessStatusCode.Should().BeTrue();
    }
}
```

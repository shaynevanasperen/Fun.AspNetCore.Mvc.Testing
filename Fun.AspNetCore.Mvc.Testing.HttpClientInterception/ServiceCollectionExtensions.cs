using System;
using System.ComponentModel;
using System.Net.Http;
using Fun.AspNetCore.Mvc.Testing;
using Fun.AspNetCore.Mvc.Testing.HttpClientInterception;
using JustEat.HttpClientInterception;
using Microsoft.Extensions.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// A <see cref="IServiceCollection"/> extension class for adding HTTP request interception capability.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds a singleton <see cref="IHttpMessageHandlerBuilderFilter"/> which registers an intercepting HTTP message handler at the end (innermost) of
		/// the message handler pipeline whenever an <see cref="HttpClient"/> is created. Also adds a singleton <see cref="HttpClientInterceptorOptions"/>
		/// for configuring the interceptions, and a transient <see cref="ITestScopeProducer"/> for aggregating test scopes wherein configured interceptions
		/// are isolated.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
		/// <param name="optionsFactory">The factory for creating the <see cref="HttpClientInterceptorOptions"/> to use for configuring the interceptions.</param>
		/// <returns>A reference to this instance after the operation has completed.</returns>
		public static IServiceCollection AddHttpRequestInterceptor(this IServiceCollection services, Func<IServiceProvider, HttpClientInterceptorOptions> optionsFactory)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			services.AddSingleton(optionsFactory);
			services.AddSingleton<IHttpMessageHandlerBuilderFilter, HttpInterceptionFilter>();
			services.AddTransient<ITestScopeProducer>(provider =>
				new TestScopeProducer<IDisposable>(provider.GetRequiredService<HttpClientInterceptorOptions>().BeginScope, x => x.Dispose));

			return services;
		}
	}
}

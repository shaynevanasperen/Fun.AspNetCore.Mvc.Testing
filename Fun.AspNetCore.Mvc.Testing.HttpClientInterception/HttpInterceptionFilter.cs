using System;
using System.Net.Http;
using JustEat.HttpClientInterception;
using Microsoft.Extensions.Http;

namespace Fun.AspNetCore.Mvc.Testing.HttpClientInterception
{
	/// <summary>
	/// Registers an intercepting HTTP message handler at the end (innermost) of
	/// the message handler pipeline when an <see cref="HttpClient"/> is created.
	/// </summary>
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	class HttpInterceptionFilter : IHttpMessageHandlerBuilderFilter
	{
		readonly HttpClientInterceptorOptions _options;

		public HttpInterceptionFilter(HttpClientInterceptorOptions options) => _options = options;

		/// <inheritdoc/>
		public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
		{
			return builder =>
			{
				// Run any actions the application has configured for itself
				next(builder);

				// Add the interceptor as the last (innermost) message handler
				builder.AdditionalHandlers.Add(_options.CreateHttpMessageHandler());
			};
		}
	}
}

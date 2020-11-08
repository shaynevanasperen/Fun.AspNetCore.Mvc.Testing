using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fun.AspNetCore.Mvc.Testing
{
	/// <summary>
	/// Extensible factory for bootstrapping an application in memory for functional end to end tests.
	/// </summary>
	/// <typeparam name="TEntryPoint">A type in the entry point assembly of the application.
	/// Typically the Startup or Program classes can be used.</typeparam>
	public sealed class ExtensibleWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
	{
		Action<WebHostBuilderContext, IConfigurationBuilder>? _afterConfigureAppConfiguration;
		Action<IServiceCollection>? _afterConfigureServices;

		/// <summary>
		/// Configures an additional action to be performed after the application configuration has been configured.
		/// </summary>
		/// <param name="configure">The action to perform.</param>
		public void AfterConfigureAppConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configure) =>
			_afterConfigureAppConfiguration += configure;

		/// <summary>
		/// Configures an additional action to be performed after the application service collection has been configured.
		/// </summary>
		/// <param name="configure">The action to perform.</param>
		public void AfterConfigureServices(Action<IServiceCollection> configure) => _afterConfigureServices += configure;

		/// <summary>
		/// Creates a new instance of <see cref="WebApplication"/> representing the running web application.
		/// </summary>
		/// <returns>A new instance of <see cref="WebApplication"/>.</returns>
		public WebApplication GetWebApplication() => new WebApplication(Services, CreateClient());

		/// <inheritdoc/>
		protected override IHost CreateHost(IHostBuilder builder)
		{
			builder.UseContentRoot(Directory.GetCurrentDirectory());
			return base.CreateHost(builder);
		}

		/// <inheritdoc/>
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));

			builder.ConfigureAppConfiguration((context, config) => _afterConfigureAppConfiguration?.Invoke(context, config));
			builder.ConfigureTestServices(services => _afterConfigureServices?.Invoke(services));
		}
	}
}

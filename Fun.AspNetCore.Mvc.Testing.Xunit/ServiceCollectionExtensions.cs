using System;
using System.ComponentModel;
using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// A <see cref="IServiceCollection"/> extension class for redirecting logging output to the test runner.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Replaces the singleton <see cref="ILoggerFactory"/> with one backed by <see cref="ITestOutputHelper"/>.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
		/// <param name="testOutputHelper">The instance of <see cref="ITestOutputHelper"/> to write log messages to.</param>
		/// <param name="configure">An optional callback for configuring the options for logging to Xunit.</param>
		/// <returns>A reference to this instance after the operation has completed.</returns>
		public static IServiceCollection ReplaceLoggerFactoryWithXUnit(this IServiceCollection services, ITestOutputHelper testOutputHelper,
			Action<XUnitLoggerOptions>? configure = null)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));
			if (testOutputHelper == null) throw new ArgumentNullException(nameof(testOutputHelper));

			services.Replace(ServiceDescriptor.Singleton(testOutputHelper.ToLoggerFactory(configure)));
			return services;
		}
	}
}

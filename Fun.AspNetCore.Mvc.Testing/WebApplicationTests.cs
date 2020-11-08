using System;
using AutoFixture;

namespace Fun.AspNetCore.Mvc.Testing
{
	/// <summary>
	/// Fixture class for functional end to end tests.
	/// </summary>
	/// <typeparam name="TEntryPoint">A type in the entry point assembly of the application.
	/// Typically the Startup or Program classes can be used.</typeparam>
#pragma warning disable CA1710 // Identifiers should have correct suffix
	public abstract class WebApplicationTests<TEntryPoint> : Fixture where TEntryPoint : class
#pragma warning restore CA1710 // Identifiers should have correct suffix
	{
		readonly Lazy<WebApplication> _app;

		/// <summary>
		/// Creates a new instance of the fixture.
		/// </summary>
		/// <param name="factory">The factory for creating the web application.</param>
		protected WebApplicationTests(ExtensibleWebApplicationFactory<TEntryPoint> factory)
		{
			if (factory == null) throw new ArgumentNullException(nameof(factory));

			_app = new Lazy<WebApplication>(factory.GetWebApplication);
		}

		/// <summary>
		/// Represents the running web application.
		/// </summary>
		protected WebApplication App => _app.Value;
	}
}

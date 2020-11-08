using Fun.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc.Testing
{
	/// <summary>
	/// Fixture class for functional end to end tests using Xunit.
	/// </summary>
	/// <typeparam name="TEntryPoint">A type in the entry point assembly of the application.
	/// Typically the Startup or Program classes can be used.</typeparam>
#pragma warning disable CA1710 // Identifiers should have correct suffix
	public abstract class XunitWebApplicationTests<TEntryPoint> : WebApplicationTests<TEntryPoint>,
		IClassFixture<ExtensibleWebApplicationFactory<TEntryPoint>> where TEntryPoint : class
#pragma warning restore CA1710 // Identifiers should have correct suffix
	{
		/// <summary>
		/// Creates a new instance of the fixture.
		/// </summary>
		/// <param name="factory">The factory for creating the web application.</param>
		/// <param name="testOutputHelper">The <see cref="ITestOutputHelper"/> for logging test output.</param>
		protected XunitWebApplicationTests(ExtensibleWebApplicationFactory<TEntryPoint> factory, ITestOutputHelper testOutputHelper)
			: base(factory)
		{
			TestOutputHelper = testOutputHelper ?? throw new System.ArgumentNullException(nameof(testOutputHelper));
		}

		/// <summary>
		/// The <see cref="ITestOutputHelper"/> for logging test output.
		/// </summary>
		protected ITestOutputHelper TestOutputHelper { get; }
	}
}

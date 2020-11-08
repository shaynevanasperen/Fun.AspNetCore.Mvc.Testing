using System;
using System.ComponentModel;
using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
// ReSharper disable once IdentifierTypo
namespace Xunit.Abstractions
{
	/// <summary>
	/// A <see cref="ITestOutputHelper"/> extension class for wrapping the given instance with a <see cref="ILoggerFactory"/>.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class TestOutputHelperExtensions
	{
		/// <summary>
		/// Returns a new <see cref="ILoggerFactory"/> backed by <paramref name="testOutputHelper"/>.
		/// </summary>
		/// <param name="testOutputHelper">The instance of <see cref="ITestOutputHelper"/> to write log messages to.</param>
		/// <param name="configure">An optional callback for configuring the options for logging to Xunit.</param>
		/// <returns>The new <see cref="ILoggerFactory"/>.</returns>
		public static ILoggerFactory ToLoggerFactory(this ITestOutputHelper testOutputHelper, Action<XUnitLoggerOptions>? configure = null)
		{
			if (testOutputHelper == null) throw new ArgumentNullException(nameof(testOutputHelper));

			configure ??= options => { };
#pragma warning disable CA2000 // Dispose objects before losing scope
			return new LoggerFactory().AddXUnit(testOutputHelper, configure);
#pragma warning restore CA2000 // Dispose objects before losing scope
		}
	}
}

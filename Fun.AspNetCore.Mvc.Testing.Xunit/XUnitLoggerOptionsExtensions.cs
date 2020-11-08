using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace MartinCostello.Logging.XUnit
{
	/// <summary>
	/// A <see cref="XUnitLoggerOptions"/> extension class for filtering log output based on levels and category prefixes.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class XUnitLoggerOptionsExtensions
	{
		/// <summary>
		/// Sets the <see cref="XUnitLoggerOptions.Filter" /> to only include log messages with level <paramref name="minimumLevel"/> and above,
		/// with optional <paramref name="categoryPrefixesForAboveOnly"/> where we only want to include log messages with level <b>above</b>
		/// <paramref name="minimumLevel"/>.
		/// </summary>
		/// <param name="options">The <see cref="XUnitLoggerOptions" /> to configure.</param>
		/// <param name="minimumLevel">The minimum log level for messages to be included.</param>
		/// <param name="categoryPrefixesForAboveOnly">Optional category prefixes where we only want to include log messages with level <b>above</b>
		/// <paramref name="minimumLevel"/>.</param>
		/// <returns>A reference to this instance after the operation has completed.</returns>
		public static XUnitLoggerOptions ForLevelAndAbove(this XUnitLoggerOptions options, LogLevel minimumLevel, params string[] categoryPrefixesForAboveOnly)
		{
			if (options == null) throw new ArgumentNullException(nameof(options));
			if (categoryPrefixesForAboveOnly == null) throw new ArgumentNullException(nameof(categoryPrefixesForAboveOnly));

			options.Filter = (category, level) => categoryPrefixesForAboveOnly.Any(category.StartsWith)
				? level > minimumLevel
				: level >= minimumLevel;

			return options;
		}
	}
}

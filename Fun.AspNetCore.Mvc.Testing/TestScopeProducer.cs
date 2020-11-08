using System;

namespace Fun.AspNetCore.Mvc.Testing
{
	/// <summary>
	/// An interface for abstracting the aggregation of test scopes.
	/// </summary>
	public interface ITestScopeProducer
	{
		/// <summary>Starts a new test scope.</summary>
		/// <returns>An action to invoke when the scope should end.</returns>
		Action Invoke();
	}

	/// <summary>
	/// An implementation of <see cref="ITestScopeProducer"/> which takes in a begin and end function.
	/// </summary>
	/// <typeparam name="T">The type returned by the begin function and supplied to the end function.</typeparam>
	public class TestScopeProducer<T> : ITestScopeProducer
	{
		readonly Func<T> _begin;
		readonly Func<T, Action> _end;

		/// <summary>
		/// Creates a new instance, accepting a begin and end function.
		/// </summary>
		/// <param name="begin">The function to call when starting the scope.</param>
		/// <param name="end">The function to call when ending the scope.</param>
		public TestScopeProducer(Func<T> begin, Func<T, Action> end)
		{
			_begin = begin ?? throw new ArgumentNullException(nameof(begin));
			_end = end ?? throw new ArgumentNullException(nameof(end));
		}

		/// <inheritdoc/>
		public Action Invoke() => _end(_begin());
	}
}

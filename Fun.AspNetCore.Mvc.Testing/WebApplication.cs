using System;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fun.AspNetCore.Mvc.Testing
{
	/// <summary>
	/// An abstraction representing a running web application, for use by test fixtures.
	/// </summary>
	public sealed class WebApplication
	{
		internal WebApplication(IServiceProvider services, HttpClient client)
		{
			Services = services;
			Client = client;
		}

		/// <summary>
		/// The service provider in use by the web application.
		/// </summary>
		public IServiceProvider Services { get; }

		/// <summary>
		/// The HTTP client to be used by consumers of the web application.
		/// </summary>
		public HttpClient Client { get; }

		/// <summary>
		/// Starts a new test scope.
		/// </summary>
		/// <returns>An <see cref="IDisposable"/> to be invoked to end the test scope.</returns>
		public IDisposable BeginTestScope()
		{
			var testScopes = Services.GetServices<ITestScopeProducer>().Select(x => x.Invoke());
			return new TestScope(testScopes.ToArray());
		}

		class TestScope : IDisposable
		{
			readonly Action[] _endActions;

			public TestScope(params Action[] endActions) => _endActions = endActions;

			public void Dispose()
			{
				foreach (var endAction in _endActions)
					endAction.Invoke();
			}
		}
	}
}

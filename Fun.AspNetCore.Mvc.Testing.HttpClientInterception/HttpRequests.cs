using JustEat.HttpClientInterception;

namespace Fun.AspNetCore.Mvc.Testing.HttpClientInterception
{
	/// <summary>
	/// An entry point class for configuring HTTP request interceptions.
	/// </summary>
	public static class HttpRequests
	{
		/// <summary>
		/// Returns a new <see cref="HttpRequestInterceptionBuilder"/>.
		/// </summary>
		public static HttpRequestInterceptionBuilder Intercept => new();
	}
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RequestGenerators.Internal
{
	public interface IRequest
	{
		/// <summary>
		/// Gets the RequestUri
		/// </summary>
		string RequestUri { get; }

		/// <summary>
		/// Gets the HTTP Method
		/// </summary>
		string Method { get; }

		/// <summary>
		/// Gets the Headers for the request
		/// </summary>
		WebHeaderCollection Headers { get; }

		/// <summary>
		/// Gets a count of headers for the request
		/// </summary>
		/// <remarks>IRequest implementors can save from having to generate a collection by returning zero normally.</remarks>
		int HeadersCount { get; }

		/// <summary>
		/// Will attempt to get a NetworkCredential instance.  BaseWrapper implementation will return null if there's no credentials.
		/// </summary>
		/// <returns></returns>
		NetworkCredential GetNetworkCredentials();
		
	}


}

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RequestGenerators.Internal
{
	public class WebRequestWrapper : BaseWrapper, IRequest
	{
		public WebRequestWrapper(WebRequest request)
		{
			if(request == null) ExceptionHelper.ArgumentNull(nameof(request));
			this._request = request;
		}
		private WebRequest _request;

		public string RequestUri
		{
			get => this._request.RequestUri.ToString();
		}

		public string Method
		{
			get => this._request.Method;
		}

		public int HeadersCount
		{
			get => this._request.Headers.Count;
		}

		public WebHeaderCollection Headers => this._request.Headers;

		public NetworkCredential GetNetworkCredentials()
		{
			return base.GetNetworkCredentials(this._request.Credentials);
		}
	}




}

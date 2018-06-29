
using RequestGenerators.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace RequestGenerators
{
	public static class CurlRequests
	{
		private static string[] ignoredHeaders = new string[] { "Content-Length", "Transfer-Encoding" };

		/// <summary>
		/// Will generate a curl commandline that can be pasted into a bash shell.
		/// </summary>
		/// <param name="request">The WebRequest instance to generate the curl command from</param>
		/// <param name="incomingRequestStream">Normally you need to assign a variable to return of WebRequest.GetIncomingRequestStream().  See remarks.</param>
		/// <remarks>The second parameter is a ref parameter due to stream handling.  Since we have to retrieve data off the request stream, we copy all that data 
		/// into a MemoryStream instance and then assigns the incomingRequestStream variable to that MemoryStream properly seeked back to the beginning upon return.
		/// This enables us to generate the necessary content for curl's --data parameter and also be able to actually send that data out once we return.
		/// </remarks>
		/// <returns></returns>
		public static string CurlBash(this WebRequest request, ref Stream incomingRequestStream)
		{
			if(request == null) ExceptionHelper.ArgumentNull(nameof(request));
			return CurlBash(new WebRequestWrapper(request), ref incomingRequestStream);
		}


		public static string CurlBash(IRequest request, ref Stream incomingRequestStream)
		{
			try
			{
				List<string> parts = new List<string>();

				parts.Add("curl");

				parts.Add(string.Format("\"{0}\"", request.RequestUri));

				if(request.Method != "GET")
					parts.Add(string.Format("-X {0}", request.Method));

				NetworkCredential nwcred;
				if((nwcred = request.GetNetworkCredentials()) != null)
				{
					parts.Add(string.Format("-u \"{0}:{1}\"", nwcred.UserName, nwcred.Password));
				}

				if(request.HeadersCount > 0)
				{
					foreach(string key in request.Headers.AllKeys)
					{
						if(ignoredHeaders.Any(_ => _.Equals(key, StringComparison.OrdinalIgnoreCase))) continue;

						string value = request.Headers[key];
						parts.Add(string.Format("-H \"{0}: {1}\"", key, value));
					}
				}

				if(request.Method != "GET" && incomingRequestStream != null)
				{
					MemoryStream ms = incomingRequestStream as MemoryStream;
					if(ms == null)
					{
						ms = new MemoryStream();
						incomingRequestStream.CopyTo(ms);
					}

					//reset to beginning
					ms.Seek(0, SeekOrigin.Begin);

					using(StreamReader sr = new StreamReader(ms, Encoding.UTF8, true, 64 * 1024, leaveOpen:true))
					{
						string data = sr.ReadToEnd();
						parts.Add(string.Format("--data '{0}'", data));
					}

					ms.Seek(0, SeekOrigin.Begin);
				}

				return String.Join(" ", parts);
			}
			catch(Exception ex)
			{
				return "failed to generate curl cmd: " + ex.Message;
			}
		}

	}
}
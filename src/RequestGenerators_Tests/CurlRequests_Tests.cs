using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using FluentAssertions;
using RequestGenerators;
using System.IO;
using System.Net.Http.Headers;

namespace RequestGenerators_Tests
{
	[TestFixture]
	public class CurlRequests_Tests
	{

		[Test]
		[TestCase("https://www.microsoft.com", "curl \"https://www.microsoft.com/\"")]
		public void WebRequest_SimpleGets(string url, string expected)
		{
			//arrange
			WebRequest request = System.Net.WebRequest.Create(url);

			//act
			Stream stream = null;
			string actual = CurlRequests.CurlBash(request, ref stream);

			//assert
			actual.Should().Be(expected);
		}

		[Test]
		[TestCase("https://www.microsoft.com", "{\"a\":1}", "curl \"https://www.microsoft.com/\" -X POST --data '{\"a\":1}'")]
		public void WebRequest_SimplePosts(string url, string data, string expected)
		{
			//arrange
			WebRequest request = System.Net.WebRequest.Create(url);
			request.Method = "POST";

			//act
			MemoryStream ms = new MemoryStream();
			var sw = new StreamWriter(ms, Encoding.UTF8);
			sw.Write(data);
			sw.Flush();

			ms.Position = 0;
			Stream stream = ms;

			string actual = CurlRequests.CurlBash(request, ref stream);

			//assert
			actual.Should().Be(expected);
		}

		[Test]
		[TestCase("https://postman-echo.com/POST", "{\"a\":1}", "curl \"https://postman-echo.com/POST\" -X POST -H \"Content-Type: application/json\" -H \"Accept: application/json\" --data '{\"a\":1}'")]
		public void WebRequest_Posts_Headers(string url, string data, string expected)
		{
			//arrange
			HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(url);
			request.Method = "POST";
			request.ContentType = "application/json";
			request.Accept = "application/json";

			//act
			MemoryStream ms = new MemoryStream();
			var sw = new StreamWriter(ms, Encoding.UTF8);
			sw.Write(data);
			sw.Flush();

			ms.Position = 0;
			Stream stream = ms;

			string actual = CurlRequests.CurlBash(request, ref stream);

			//assert
			actual.Should().Be(expected);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace RequestGenerators
{
	internal static class ExceptionHelper
	{
		public struct ResMsg
		{
			public ResMsg(string resourceKey, params object[] args)
			{
				this.ResourceKey = resourceKey;
				this.Args = args;
			}
			public string ResourceKey { get; private set; }
			public object[] Args { get; private set; }
		}
		public static ResMsg Msg(string resourceKey, params object[] args)
		{
			return new ResMsg(resourceKey, args);
		}

		public static void ArgumentNull(string paramName)
		{
			throw new ArgumentNullException(paramName);
		}
		public static void ArgumentNull(string paramName, ResMsg msg)
		{
			throw new ArgumentNullException(paramName, CreateMessageFromResourceMessage(msg));
		}
		public static void ArgumentNull(string paramName, Exception inner)
		{
			throw new ArgumentNullException(paramName, inner);
		}

		public static void ArgumentException(string paramName, ResMsg msg)
		{
			throw new ArgumentException(CreateMessageFromResourceMessage(msg), paramName);
		}
		public static void ArgumentException(ResMsg msg)
		{
			throw new ArgumentException(CreateMessageFromResourceMessage(msg));
		}

		public static string CreateMessageFromResourceMessage(ResMsg m)
		{
			string format = m.ResourceKey;
			return string.Format(format, m.Args);
		}

	}
}

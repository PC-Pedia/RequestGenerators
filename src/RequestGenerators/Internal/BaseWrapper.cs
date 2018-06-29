using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RequestGenerators.Internal
{
	public abstract class BaseWrapper
	{
		public virtual NetworkCredential GetNetworkCredentials(ICredentials credentials)
		{
			if(credentials != null)
			{
				NetworkCredential nwcred;
				if((nwcred = credentials as NetworkCredential) != null)
				{
					return nwcred;
				}
			}
			return null;
		}

	}
}

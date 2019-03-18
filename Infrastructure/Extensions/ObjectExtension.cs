using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
	public static class ObjectExtensions
	{
		public static bool IsNull(this object obj)
		{
			var response = false;

			if (obj == null)
			{
				response = true;
			}
			return response;
		}
	}
}

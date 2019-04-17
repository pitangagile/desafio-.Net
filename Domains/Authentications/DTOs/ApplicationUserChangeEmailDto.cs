using System;
using System.Collections.Generic;
using System.Text;

namespace Domains
{
	public class ApplicationUserChangeEmailDto: BaseDto
	{
		public string CurrentEmail { get; set; }
		public string NewEmail { get; set; }
	}
}

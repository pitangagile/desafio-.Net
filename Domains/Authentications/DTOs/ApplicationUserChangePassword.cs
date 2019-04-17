using System;
using System.Collections.Generic;
using System.Text;

namespace Domains
{
	public class ApplicationUserChangePassword : BaseDto
	{
		public string Email { get; set; }
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
	}
}

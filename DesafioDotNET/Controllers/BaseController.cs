using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace DesafioDotNET
{
	[AllowAnonymous]
	[EnableCors("FrontEnd")]
	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	public class BaseController : Controller
    {
    }
}
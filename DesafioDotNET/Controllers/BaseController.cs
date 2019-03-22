using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DesafioDotNET
{
	//https://imasters.com.br/back-end/padroes-de-web-api-parte-01-documentacao-requests-responses
	[AllowAnonymous]
	[EnableCors("FrontEnd")]
	[Produces("application/json")]
	[Route("api/v1/[controller]")]
	[ApiController]
	public class BaseController : Controller
    {
    }
}
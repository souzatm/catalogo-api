using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [Route("api/v{version:apiVersion}/teste")]
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TesteV2Controller : ControllerBase
    {
        [HttpGet]
        public string GetVersion()
        {
            return "Teste - V2";
        }
    }
}

//Controlador de teste para versionamento V2

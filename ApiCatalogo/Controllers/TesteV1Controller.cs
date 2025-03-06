using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [Route("api/v{version:apiVersion}/teste")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    //Inibe a exibição da documentação para os endpoints
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TesteV1Controller : ControllerBase
    {
        [HttpGet]
        public string GetVersion()
        {
            return "Teste - V1";
        }
    }
}

//Controlador de teste para versionamento V1
using Faith.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faith.Server.Controllers
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly FaithContext _faithContext;
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;

        public PostController(FaithContext faithContext, ILogger<UserController> logger, IConfiguration configuration)
        {
            _faithContext = faithContext;
            _logger = logger;
            _configuration = configuration;
        }


    }
}

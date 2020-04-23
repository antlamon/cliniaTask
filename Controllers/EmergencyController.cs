using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CliniaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmergencyController : ControllerBase
    {
        private readonly ILogger<EmergencyController> _logger;
        private readonly IEmergencyService _service;

        public EmergencyController(IEmergencyService service, ILogger<EmergencyController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public List<IResource> Get(string query="", string aroundLatLng="", string aroundRadius="")
        {
            return _service.Search(query, aroundLatLng, aroundRadius);
        }
    }
}

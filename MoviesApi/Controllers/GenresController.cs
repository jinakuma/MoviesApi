using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using MoviesApi.Entities;
using MoviesApi.Filters;


namespace MoviesApi.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController :ControllerBase
    {
        
        private readonly ILogger _logger;

        public GenresController( ILogger<GenresController> logger)
        {
            
            _logger = logger;
        }

        [HttpGet]
        
        public async Task<ActionResult<List<Genre>>> Get()
        {
            _logger.LogInformation("Executing GetAllGenres");
            return new List<Genre>
            {
                new Genre { Id = 1, Name = "Comedy" },
                new Genre { Id = 2, Name = "Action" }
            }; 
        }

        [HttpGet("{id:int}")]
        public ActionResult<Genre> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genre genre)
        {

            throw new NotImplementedException();
        }

        [HttpPut]
        public ActionResult Put()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using System.Linq; 
using System.Threading.Tasks;

namespace MoviesApi.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    public class RatingsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public RatingsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult> Post([FromBody] RatingDTO ratingDto)
        {
            
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
            var user = await userManager.FindByEmailAsync(email);
            var userId = user.Id;

            var currentRate = await context.Ratings.FirstOrDefaultAsync(x => x.MovieId == ratingDto.MovieId && x.UserId == userId);

            if (currentRate == null)
            {
                Rating rating = new()
                {
                    UserId = userId,
                    MovieId = ratingDto.MovieId,
                    Rate = ratingDto.Rating
                };
                context.Add(rating);
            }
            else
            {
                currentRate.Rate = ratingDto.Rating;
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

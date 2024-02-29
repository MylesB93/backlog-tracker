using BacklogTracker.Data;
using Microsoft.AspNetCore.Mvc;

namespace BacklogTracker.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class BacklogController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public BacklogController(ApplicationDbContext dbContext)
        { 
            _dbContext = dbContext; 
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostData([FromBody] string gameIDs, [FromBody] string email)
        {
            if (string.IsNullOrEmpty(gameIDs) || string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }

            var user = _dbContext.Users.Where(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            user.GameIDs?.Add(gameIDs);
            _dbContext.SaveChanges();

            return Ok("Data saved successfully.");
        }
    }
}

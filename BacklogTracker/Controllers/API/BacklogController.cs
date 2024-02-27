using BacklogTracker.Data;
using Microsoft.AspNetCore.Mvc;

namespace BacklogTracker.Controllers.API
{
    public class BacklogController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public BacklogController(ApplicationDbContext dbContext)
        { 
            _dbContext = dbContext; 
        }

        [HttpPost]
        public IActionResult PostData([FromBody] string gameIDs, [FromBody] string email)
        {
            if (string.IsNullOrEmpty(gameIDs) || string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }

            // WIP
            //_dbContext.Users.Where(u => u.Email == email)

            return Ok("Data saved successfully.");
        }
    }
}

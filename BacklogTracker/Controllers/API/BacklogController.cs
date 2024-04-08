using BacklogTracker.Data;
using BacklogTracker.Data.DTOs;
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

        [HttpGet]
        public IActionResult GetTest()
        {
            return Ok("Great Success!");
        }

        [HttpPatch("add-game-to-backlog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddToBacklog([FromBody] UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.GameID) || string.IsNullOrEmpty(userDto.Email))
            {
                return BadRequest();
            }

            var user = _dbContext.Users.Where(u => u.Email == userDto.Email).FirstOrDefault();
            if (user == null)
            {
				return BadRequest(new { ErrorMessage = "User not found." });
			}

			var gameIDs = user.GameIDs;

			if (gameIDs == null)
            {
                gameIDs = new List<string>();
            }

			if (gameIDs.Contains(userDto.GameID))
			{
				return BadRequest(new { ErrorMessage = "GameID already exists in the user's backlog." });
			}

			gameIDs.Add(userDto.GameID);
            _dbContext.SaveChanges();

            return Ok(new { Message = "Data saved successfully." });
        }

		[HttpPatch("remove-game-from-backlog")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult RemoveFromBacklog([FromBody] UserDto userDto)
		{
			if (string.IsNullOrEmpty(userDto.GameID) || string.IsNullOrEmpty(userDto.Email))
			{
				return BadRequest();
			}

			var user = _dbContext.Users.Where(u => u.Email == userDto.Email).FirstOrDefault();
			if (user == null)
			{
				return BadRequest(new { ErrorMessage = "User not found." });
			}

			var gameIDs = user.GameIDs;

			if (gameIDs != null && gameIDs.Contains(userDto.GameID))
			{
				gameIDs.Remove(userDto.GameID);
				_dbContext.SaveChanges();

				return Ok(new { Message = "Data saved successfully." });
			}

			return BadRequest(new { Message = "Game does not exist in user's backlog." });
		}
	}
}

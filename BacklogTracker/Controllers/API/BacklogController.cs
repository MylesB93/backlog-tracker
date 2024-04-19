using BacklogTracker.Data;
using BacklogTracker.Data.DTOs;
using BacklogTracker.Interfaces;
using BacklogTracker.Models.UserBacklog;
using Microsoft.AspNetCore.Mvc;

namespace BacklogTracker.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class BacklogController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
		private readonly IBacklogService _backlogService;

        public BacklogController(ApplicationDbContext dbContext, IBacklogService backlogService)
        { 
            _dbContext = dbContext; 
			_backlogService = backlogService;
        }

        [HttpPatch("add-game-to-backlog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddToBacklog([FromBody] UserDto userDto)
        {
			try
			{
				_backlogService.AddToBacklog(userDto);
                return Ok(new { Message = "Data saved successfully." });
            }
			catch(ArgumentException ex)
			{
				return BadRequest(new { ErrorMessage = ex.Message });
			}
			catch (Exception ex)
			{
                return StatusCode(500, "An error occurred while processing the request.");
            }            
        }

		[HttpPatch("remove-game-from-backlog")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult RemoveFromBacklog([FromBody] UserDto userDto)
		{
			try
			{
				_backlogService.RemoveFromBacklog(userDto);
                return Ok(new { Message = "Data saved successfully." });
            }
			catch(ArgumentException ex)
			{
                return BadRequest(new { ErrorMessage = ex.Message });
            }
			catch(Exception ex)
			{
                return StatusCode(500, "An error occurred while processing the request.");
            }
		}

		[HttpGet("get-users-games")]
		public IActionResult GetUsersBacklog(string email)
		{
			try
			{
				var gameIds = _backlogService.GetBacklog(email) ?? new List<string>();
				return Ok(gameIds);
			}
			catch(Exception ex)
			{
				return BadRequest(new {ErrorMessage =  ex.Message});
			}
		}
	}
}

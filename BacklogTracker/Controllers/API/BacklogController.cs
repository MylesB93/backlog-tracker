﻿using BacklogTracker.Data.DTOs;
using BacklogTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BacklogTracker.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class BacklogController : ControllerBase
    {
		private readonly IBacklogService _backlogService;

        public BacklogController(IBacklogService backlogService)
        { 
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
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred while processing the request.");
			}
		}

		[HttpPatch("add-game-to-completed-games")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult AddToCompleted([FromBody] UserDto userDto)
		{
			try
			{
				_backlogService.AddToCompleted(userDto);
				return Ok(new { Message = "Data saved successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred while processing the request.");
			}
		}

		[HttpPatch("move-from-completed-games-to-backlog")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult MoveFromCompletedToBacklog([FromBody] UserDto userDto)
		{
			try
			{
				// remove from completed
				_backlogService.RemoveFromCompleted(userDto);

				// add to backlog
				_backlogService.AddToBacklog(userDto);
				return Ok(new { Message = "Data saved successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred while processing the request.");
			}
		}
	}
}

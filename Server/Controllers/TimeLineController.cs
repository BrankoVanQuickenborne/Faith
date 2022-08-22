using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Auth0.ManagementApi;
using Faith.Server.Data;
using Faith.Shared.Domain;
using Faith.Shared.DTOs;
using Faith.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Faith.Server.Controllers
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeLineController : Controller
    {
        private readonly FaithContext _faithContext;
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;

        public TimeLineController(FaithContext faithContext, ILogger<UserController> logger, IConfiguration configuration)
        {
            _faithContext = faithContext;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("get-by-id/{id}")]
        [AllowAnonymous]
        public ActionResult<TimeLineDTO> GetById(Guid id)
        {
            try
            {
                TimeLine timeline = _faithContext.FTimeLines
                    .Include(t => t.Posts)
                    .ThenInclude(p => p.Reactions)
                    .SingleOrDefault(t => t.TimeLineID == id);
                TimeLineDTO timelineDTO = new TimeLineDTO(timeline);
                return timelineDTO != null ? Ok(timelineDTO)
                    : BadRequest(new { message = $"Timeline with id {id} does not exist." });
            }
            catch (Exception e)
            {
                var methodName = this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.LogError($"[INTERNAL ERROR] Something broke in {nameof(TimeLineController)}/{methodName}: {e.Message}");
                return BadRequest(new { message = "Internal server error: " + e.Message });
            }
        }

        [HttpGet("get-all-posts-for-monitor/{monitorID}")]
        [Authorize(Roles = "Administrator,Monitor")]
        public ActionResult<List<UserDTO>> GetAllPostsForMonitor(Guid monitorID)
        {
            try
            {
                Shared.Domain.User monitor = _faithContext.FUsers
                    .Include(m => m.Youngsters)
                    .ThenInclude(y => y.TimeLine)
                    .ThenInclude(t => t.Posts)
                    .ThenInclude(p => p.Reactions)
                    .SingleOrDefault(u => u.UserID == monitorID);
                List<UserDTO> youngsters = new List<UserDTO>();
                foreach (var youngster in monitor.Youngsters)
                {
                    youngsters.Add(new UserDTO(youngster));
                }
                return Ok(youngsters);
            }
            catch (Exception e)
            {
                var methodName = this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.LogError($"[INTERNAL ERROR] Something broke in {nameof(TimeLineController)}/{methodName}: {e.Message}");
                return BadRequest(new { message = "Internal server error: " + e.Message });
            }
        }

        [HttpPost("add-reaction-to-post/{postID}")]
        [Authorize(Roles = "Administrator,Monitor")]
        public async Task<ActionResult<Guid>> AddReactionToPost(Guid postID, ReactionDTO reactionDTO)
        {
            try
            {
                Post post = _faithContext.FPosts.SingleOrDefault(u => u.PostID == postID);
                Reaction reaction = new Reaction(reactionDTO)
                {
                    ReactionID = Guid.NewGuid(),
                    Date = DateTime.Now
                };
                post.Reactions.Add(reaction);
                _faithContext.FPosts.Update(post);
                await _faithContext.FReactions.AddAsync(reaction);
                var is_created = await _faithContext.SaveChangesAsync() > 0;

                return is_created ? Ok(reaction.ReactionID)
                    : BadRequest(new { message = $"Reaction could not be created." });
            }
            catch (Exception e)
            {
                var methodName = this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.LogError($"[INTERNAL ERROR] Something broke in {nameof(UserController)}/{methodName}: {e.Message}");
                return BadRequest(new { message = "Internal server error: " + e.Message });
            }
        }
    }
}

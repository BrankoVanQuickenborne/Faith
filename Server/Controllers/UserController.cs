using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Faith.Server.Data;
using Faith.Shared.Domain;
using Faith.Shared.DTOs;
using Faith.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly FaithContext _faithContext;
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ManagementApiClient _managementApiClient;

        public UserController(FaithContext faithContext, ILogger<UserController> logger, IConfiguration configuration, ManagementApiClient managementApiClient)
        {
            _faithContext = faithContext;
            _logger = logger;
            _configuration = configuration;
            _managementApiClient = managementApiClient;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await _managementApiClient.Users.GetAllAsync(new GetUsersRequest(), new PaginationInfo());
            return users.Select(x => new UserDTO
            {
                Email = x.Email,
                FirstName = x.FirstName,
                FamilyName = x.LastName
            });
        }

        [HttpGet("get-logged-in-user")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> GetLoggedInUser()
        {
            try
            {
                var loggedInUser = (await _managementApiClient.Users.GetAllAsync(new GetUsersRequest(), new PaginationInfo()))[0];
                UserDTO userDTO = new UserDTO(_faithContext.FUsers.Include(m => m.Youngsters).SingleOrDefault(u => u.Email == loggedInUser.Email));

                return userDTO != null ? Ok(userDTO)
                    : BadRequest(new { message = $"There is no logged in user." });
            }
            catch (Exception e)
            {
                var methodName = this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.LogError($"[INTERNAL ERROR] Something broke in {nameof(UserController)}/{methodName}: {e.Message}");
                return BadRequest(new { message = "Internal server error: " + e.Message });
            }
        }

        [HttpGet("get-by-id/{id}")]
        [AllowAnonymous]
        public ActionResult<UserDTO> GetById(Guid id)
        {
            try
            {
                Shared.Domain.User user = _faithContext.FUsers.SingleOrDefault(u => u.UserID == id);
                UserDTO userDTO = null;
                switch (user.Role)
                {
                    case UserRole.ADMIN:
                    case UserRole.MONITOR:
                        Shared.Domain.User monitor = _faithContext.FUsers
                            .Include(u => u.Youngsters)
                            .SingleOrDefault(m => m.UserID == id);
                        userDTO = new UserDTO(monitor);
                        break;
                    case UserRole.YOUNGSTER:
                        userDTO = new UserDTO(user);
                        break;
                }
                return userDTO != null ? Ok(userDTO)
                    : BadRequest(new { message = $"User with id {id} does not exist." });
            }
            catch (Exception e)
            {
                var methodName = this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.LogError($"[INTERNAL ERROR] Something broke in {nameof(UserController)}/{methodName}: {e.Message}");
                return BadRequest(new { message = "Internal server error: " + e.Message });
            }
        }

        [HttpGet("get-all-users-for-monitor/{monitorID}")]
        [Authorize(Roles = "Administrator,Monitor")]
        public ActionResult<List<UserDTO>> GetAllUsersForMonitor(Guid monitorID)
        {
            try
            {
                Shared.Domain.User monitor = _faithContext.FUsers
                    .Include(m => m.Youngsters)
                    .SingleOrDefault(m => m.UserID == monitorID);
                List<Shared.Domain.User> youngsters = monitor.Youngsters.ToList();

                return Ok(youngsters);
            }
            catch (Exception e)
            {
                var methodName = this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.LogError($"[INTERNAL ERROR] Something broke in {nameof(UserController)}/{methodName}: {e.Message}");
                return BadRequest(new { message = "Internal server error: " + e.Message });
            }
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<UserDTO>> GetAll()
        {
            try
            {
                List<Shared.Domain.User> users = _faithContext.FUsers.ToList();

                return Ok(users);
            }
            catch (Exception e)
            {
                var methodName = this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.LogError($"[INTERNAL ERROR] Something broke in {nameof(UserController)}/{methodName}: {e.Message}");
                return BadRequest(new { message = "Internal server error: " + e.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Monitor")]
        public async Task<ActionResult<Guid>> Create(UserDTO userDTO)
        {
            try
            {
                var loggedInUser = (await _managementApiClient.Users.GetAllAsync(new GetUsersRequest(), new PaginationInfo()))[0];
                Shared.Domain.User monitor = _faithContext.FUsers.SingleOrDefault(u => u.Email == loggedInUser.Email);
                Shared.Domain.User user = new Shared.Domain.User(userDTO)
                {
                    UserID = Guid.NewGuid(),
                    Role = UserRole.YOUNGSTER,
                    TimeLine = new TimeLine()
                };
                monitor.Youngsters.Add(user);
                _faithContext.FUsers.Update(monitor);
                await _faithContext.FUsers.AddAsync(user);
                var is_created = await _faithContext.SaveChangesAsync() > 0;

                //_managementApiClient.Users.CreateAsync(new UserCreateRequest()).Wait()

                return is_created ? Ok(user.UserID)
                    : BadRequest(new { message = $"User could not be created." });
            }
            catch (Exception e)
            {
                var methodName = this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.LogError($"[INTERNAL ERROR] Something broke in {nameof(UserController)}/{methodName}: {e.Message}");
                return BadRequest(new { message = "Internal server error: " + e.Message });
            }
        }

        [HttpPatch]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Update(UserDTO userDTO)
        {
            try
            {
                bool user_exists = _faithContext.FUsers
                    .Any(u => u.UserID == userDTO.UserID);

                if (user_exists)
                {
                    _faithContext.FUsers.Update(new Shared.Domain.User(userDTO));
                    await _faithContext.SaveChangesAsync();

                    return Ok();
                }

                return BadRequest(new { message = $"User with id {userDTO.UserID} does not exist." });
            }
            catch (Exception e)
            {
                var methodName = this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.LogError($"[INTERNAL ERROR] Something broke in {nameof(UserController)}/{methodName}: {e.Message}");
                return BadRequest(new { message = "Internal server error: " + e.Message });
            }
        }

        [HttpPost("connect")]
        [Authorize(Roles = "Administrator,Monitor")]
        public async Task<ActionResult<Guid>> Connect(UserDTO userDTO)
        {
            try
            {
                Shared.Domain.User monitor = _faithContext.FUsers.SingleOrDefault(u => u.UserID == userDTO.UserID);
                Shared.Domain.User youngster = _faithContext.FUsers.SingleOrDefault(u => u.UserID == userDTO.Youngsters.First().UserID);

                monitor.Youngsters.Add(youngster);
                _faithContext.FUsers.Update(monitor);
                _faithContext.FUsers.Update(youngster);
                var is_disconnected = await _faithContext.SaveChangesAsync() > 0;

                return is_disconnected ? Ok(youngster.UserID)
                    : BadRequest(new { message = $"User could not be created." });
            }
            catch (Exception e)
            {
                var methodName = this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.LogError($"[INTERNAL ERROR] Something broke in {nameof(UserController)}/{methodName}: {e.Message}");
                return BadRequest(new { message = "Internal server error: " + e.Message });
            }
        }

        [HttpPost("disconnect")]
        [Authorize(Roles = "Administrator,Monitor")]
        public async Task<ActionResult<Guid>> Disconnect(UserDTO userDTO)
        {
            try
            {
                Shared.Domain.User monitor = _faithContext.FUsers.SingleOrDefault(u => u.UserID == userDTO.UserID);
                Shared.Domain.User youngster = _faithContext.FUsers.SingleOrDefault(u => u.UserID == userDTO.Youngsters.First().UserID);

                monitor.Youngsters.Remove(youngster);
                _faithContext.FUsers.Update(monitor);
                _faithContext.FUsers.Update(youngster);
                var is_disconnected = await _faithContext.SaveChangesAsync() > 0;

                return is_disconnected ? Ok(youngster.UserID)
                    : BadRequest(new { message = $"User could not be created." });
            }
            catch (Exception e)
            {
                var methodName = this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.LogError($"[INTERNAL ERROR] Something broke in {nameof(UserController)}/{methodName}: {e.Message}");
                return BadRequest(new { message = "Internal server error: " + e.Message });
            }
        }

        [HttpDelete("delete/{userId}")]
        [Authorize(Roles = "Administrator,Monitor")]
        public async Task<ActionResult> Remove(Guid userID)
        {
            try
            {
                Shared.Domain.User user = _faithContext.FUsers
                    .SingleOrDefault(e => e.UserID == userID);

                if (user != null)
                {
                    _faithContext.FUsers.Remove(user);
                    await _faithContext.SaveChangesAsync();

                    return Ok();
                }

                return BadRequest(new { message = $"User with id {userID} does not exist." });
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

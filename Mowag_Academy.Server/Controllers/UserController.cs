using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mowag_Academy.Server.Services;
using Mowag_Academy.Shared.DTOs;

namespace Mowag_Academy.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        // Injecting the UsersService for handling user operations
        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        // GET: api/users
        // Returns a list of all users
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> Get()
        {
            var users = await _usersService.GetAsync();
            if (users == null || users.Count == 0)
            {
                return NoContent(); // Returns 204 if no users found
            }
            return Ok(users); // Returns 200 with the list of users
        }

        // GET: api/users/{id}
        // Returns a single user by ID
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<UserDto>> Get(string id)
        {
            var user = await _usersService.GetAsync(id);

            if (user is null)
            {
                return NotFound(); // Returns 404 if user not found
            }

            return Ok(user); // Returns 200 with the user data
        }

        // POST: api/users
        // Creates a new user
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(UserDto newUser)
        {
            await _usersService.CreateAsync(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser); // Returns 201 with location of new user
        }

        // PUT: api/users/{id}
        // Updates an existing user
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, UserDto updatedUser)
        {
            var existingUser = await _usersService.GetAsync(id);

            if (existingUser is null)
            {
                return NotFound(); // Returns 404 if user not found
            }

            updatedUser.Id = existingUser.Id; // Ensure the ID is consistent
            await _usersService.UpdateAsync(id, updatedUser);

            return NoContent(); // Returns 204 when the update is successful
        }

        // DELETE: api/users/{id}
        // Deletes a user by ID
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _usersService.GetAsync(id);

            if (user is null)
            {
                return NotFound(); // Returns 404 if user not found
            }

            await _usersService.RemoveAsync(id);
            return NoContent(); // Returns 204 when the deletion is successful
        }
    }
}

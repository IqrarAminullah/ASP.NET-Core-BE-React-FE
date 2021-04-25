using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApi.Models;
using TestApi.Helpers;
using TestApi.Wrappers;
using BC = BCrypt.Net.BCrypt;

namespace TestApi.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TicketContext _context;

        public UserController(TicketContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var userList = await _context.Users.Include(u => u.auth).ToListAsync();
            var user = userList.Where(i => i.userId == id).Single();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.userId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> RegisterUser(UserRegisterRequest request)
        {
            var user = new User();
            user.username = request.username;
            user.email = request.email;

            bool validEmail = UserAuthHelper.IsValidEmail(request.email);
            if(!validEmail){
                return BadRequest();
            }
            bool validPassword = UserAuthHelper.isValidPassword(request.password);
            if(!validPassword){
                return BadRequest();
            }
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            CreatedAtAction("GetUser", new { id = user.userId }, user);
            var userAuth = new UserAuth();
            userAuth.userId = user.userId;
            userAuth.password = BC.HashPassword(request.password);
            _context.UserAuths.Add(userAuth);
            await _context.SaveChangesAsync();
            

            return CreatedAtAction("GetUser", new { id = user.userId }, user);
        }

        [HttpPost("auth")]
        public async Task<ActionResult<UserAuthResponse>> AuthUser(UserAuthRequest request)
        {
            var user = await _context.Users.Include(u => u.auth).Where(u => u.email == request.email).SingleOrDefaultAsync();
            if(user == null || !BC.Verify(request.password, user.auth.password))
            {
                return null;
            }else
            {
                var token = UserAuthHelper.generateJwtToken(user);
                return new UserAuthResponse(user,token);
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.userId == id);
        }
    }
}

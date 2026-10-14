using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Text;
using SharedLibrary.DTOs;
using System.IdentityModel.Tokens.Jwt;
using SharedLibrary.Model;
using SharedLibrary.Mappers;
using System.Text.Encodings.Web;
using SharedLibrary.Services;

namespace AuthProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomEmailSender _emailSender;


        public LoginController(IConfiguration configuration, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ICustomEmailSender emailService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            //Console.WriteLine($"Login attempt for user: {userLogin.FullName}"); // Debug logging

            var user = await Authenticate(userLogin);
            if (user == null)
            {
                Console.WriteLine("Authentication failed");
                return Unauthorized();
            }

            Console.WriteLine("Authentication successful, generating token...");
            var token = await GenerateToken(user);
            Console.WriteLine($"Token generated successfully");

            return Ok(new { Token = token});
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO newUser)
        {
            var user = UserMapper.DtoToEntity(newUser);
            var existingUser= await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null) {
                return Ok("User already exists in the system");
            }

            // Store user data in AspNetUsers database table
            var result = await _userManager.CreateAsync(user, newUser.password);
            await _userManager.AddToRoleAsync(user, "User");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await SendConfirmationEmail(newUser.EmailAddress, user);
            //if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
            //{
            //    return RedirectToAction("ListUsers", "Administration");
            //}

            // If there are any errors, add them to the ModelState object
            // which will be displayed by the validation summary tag helper
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Ok("User registered sucessfully");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string Token)
        {
            if(string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(Token))
            {
                return BadRequest("The link is invalid or has expired. Please request a new one if needed");
            }

            //Find the User by Id
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound("We could not find a user associated with the given link");
            }

            //Attempt to confirm the email
            var result = await _userManager.ConfirmEmailAsync(user, Token);
            if (!result.Succeeded) 
            {
                return BadRequest("We were unable to confirm your email address. Please try again or request a new link.");
            }

            return Ok("Thank you for confirming your email address. Your account is now verified!");
        }




        //Helper Methods
        private async Task SendConfirmationEmail(string email, ApplicationUser user)
        {
            //Generate the email confirmation token
            //var user=UserMapper.DtoToEntity(newUser);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            //Build the confirmation callback URL
            var confirmationLink = Url.Action("ConfirmEmail", "Login", new { UserId = user.Id, Token = token },
                protocol: HttpContext.Request.Scheme);

            //Encode the link to prevent XSS and other injection attacks
            var safeLink = HtmlEncoder.Default.Encode(confirmationLink);

            var subject = "Welcome to IT HelpDesk, Please Confirm Your Email";

            //Email to be sent
            var messageBody = $@"
                                <div style=""font-family:Arial,Helvetica,sans-serif;font-size:16px;line-height:1.6;color:#333;"">
                                            <p>Hi {user.UserName},</p>
                                            <p>Thank you for creating an account at <strong>Dot Net Tutorials</strong>.
                                            To start enjoying all of our features, please confirm your email address by clicking the button below:</p>
                                            <p>
                                                <a href=""{safeLink}"" 
                                                   style=""background-color:#007bff;color:#fff;padding:10px 20px;text-decoration:none;
                                                          font-weight:bold;border-radius:5px;display:inline-block;"">
                                                    Confirm Email
                                                </a>
                                            </p>
                                            <p>If the button doesn’t work for you, copy and paste the following URL into your browser:
                                                <br />
                                                <a href=""{safeLink}"" style=""color:#007bff;text-decoration:none;"">{safeLink}</a>
                                            </p>
                                            <p>If you did not sign up for this account, please ignore this email.</p>
                                            <p>Thanks,<br />
                                            IT HelpDesk</p>
                                        </div>
                              ";

            await _emailSender.SendEmailAsync(email, subject, messageBody);
        }

        private async Task<ApplicationUser> Authenticate(UserLoginDto userLogin)
        {
            var currentUser = await _userManager.FindByEmailAsync(userLogin.username);
            if (currentUser == null)
            {
                //Console.WriteLine($"User not found: {userLogin.FullName}");
                return null;
            }

            var authenticated = await _signInManager.CheckPasswordSignInAsync(currentUser, userLogin.password, false);

            if (authenticated.Succeeded)
            {
                Console.WriteLine("Password verification successful");
                return currentUser;
            }

            Console.WriteLine("Password verification failed");
            return null;
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("username", user.UserName ?? ""),
                new Claim("email", user.Email ?? "")
                
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
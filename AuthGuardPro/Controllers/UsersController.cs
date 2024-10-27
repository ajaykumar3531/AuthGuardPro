using AuthGuardPro_Application.DTO_s.Requests;
using AuthGuardPro_Application.DTO_s.Responses;
using AuthGuardPro_Application.Repos.Contracts;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user creation request containing username, email, and password.</param>
    /// <returns>A response containing the created user's details.</returns>
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        CreateUserResponse response = null;
        try
        {
            response = await _userService.CreateUser(request);

            if (response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.StatusMessage))
                return Ok(response);
            else if (response.StatusCode != StatusCodes.Status200OK || string.IsNullOrEmpty(response.StatusMessage))
                return BadRequest(response);
            else
                return BadRequest(response);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser(LoginUserRequest request)
    {
        LoginUserResponse response = null;
        try
        {
            response = await _userService.LoginUser(request);

            if (response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.StatusMessage))
                return Ok(response);
            else if (response.StatusCode != StatusCodes.Status200OK || string.IsNullOrEmpty(response.StatusMessage))
                return BadRequest(response);
            else
                return BadRequest(response);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}

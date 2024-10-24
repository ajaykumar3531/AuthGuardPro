using AuthGuardPro_Application.DTO_s.DTO;
using AuthGuardPro_Application.DTO_s.Requests;
using AuthGuardPro_Application.DTO_s.Responses;
using AuthGuardPro_Application.Repos.Contracts;
using AuthGuardPro_Domain.Entities;
using AuthGuardPro_Infrastucture.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthGuardPro_Application.Repos.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userContext;
        public UserService(IBaseRepository<User> userContext)
        {
            _userContext = userContext;
        }
        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            try
            {

                CreateUserResponse response = new CreateUserResponse();

                if (request == null)
                {
                    response.StatusCode = StatusCodes.Status204NoContent;
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                }

                if (request != null)
                {
                    var existedUser = (await _userContext.GetAllAsync())?.ToList()?.FirstOrDefault(x => x.Username.ToLower() == request.Username.ToLower() || x.Email.ToLower() == request.Email);
                    if (existedUser != null)
                    {
                        response.StatusMessage = Constants.MSG_DATA_FOUND;
                        response.StatusCode = StatusCodes.Status302Found;
                    }
                    else
                    {
                        // Generate a valid BCrypt salt
                        string salt = BCrypt.Net.BCrypt.GenerateSalt(12);

                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

                        var userData = new User()
                        {
                            Username = request.Username,
                            Email = request.Email,
                            IsDeleted = false,
                            PasswordHash = hashedPassword,
                            Salt = salt,
                        };
                        await _userContext.AddAsync(userData);
                        if (await _userContext.SaveChangesAsync() > 0)
                        {
                            response.UserId = userData.UserId;  // Assuming UserID is a UNIQUEIDENTIFIER
                            response.Username = userData.Username;
                            response.Email = userData.Email;
                            response.DateCreated = userData.DateCreated;
                            response.StatusMessage = Constants.MSG_USER_ADD;
                            response.StatusCode = StatusCodes.Status200OK;
                        }
                        else
                        {
                            response.StatusCode = StatusCodes.Status204NoContent;
                            response.StatusMessage = Constants.MSG_REQ_NULL;
                        }
                    }
                }


                return response; // Return the populated response
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw; // Re-throwing the exception can be useful for higher-level error handling
            }
        }


    }
}

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
        private readonly IJWTTokenGeneration _JWTTokenGeneration;
        public UserService(IBaseRepository<User> userContext, IJWTTokenGeneration jWTTokenGeneration)
        {
            _userContext = userContext;
            _JWTTokenGeneration = jWTTokenGeneration;
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

                        string salt = BCrypt.Net.BCrypt.GenerateSalt(12);

                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password + salt);

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

        public async Task<LoginUserResponse> LoginUser(LoginUserRequest request)
        {
            LoginUserResponse response = new LoginUserResponse();
            try
            {
                if (request == null)
                {
                    response.StatusCode = StatusCodes.Status204NoContent;
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                }
                else
                {
                    User existedUserData = (await _userContext.GetAllAsync())?.FirstOrDefault(x => x.Username.ToLower() == request.Username.ToLower() && x.Email.ToLower() == request.Email);

                    if (existedUserData != null)
                    {
                        string salt = existedUserData.Salt;
                        bool verifyPassword = BCrypt.Net.BCrypt.Verify(request.Password + salt, existedUserData.PasswordHash);

                        if (verifyPassword)
                        {
                            TokenRequest tokenRequest = new TokenRequest()
                            {
                                Email = existedUserData.Email,
                                UserID = existedUserData.UserId.ToString(),
                                UserName = existedUserData.Username 
                            };


                            response.JWTToken = await _JWTTokenGeneration.TokenGeneration(tokenRequest);
                            response.Username = request.Username;
                            response.Email = request.Email;
                            response.StatusMessage = Constants.MSG_LOGIN_SUCC;
                            response.StatusCode = StatusCodes.Status200OK;
                        }
                        else
                        {
                            response.Username = request.Username;
                            response.Email = request.Email;
                            response.StatusCode = StatusCodes.Status400BadRequest;
                            response.StatusMessage = Constants.MSG_LOGIN_FAIL;
                        }
                    }
                    else
                    {
                        response.Username = request.Username;
                        response.Email = request.Email;
                        response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                        response.StatusCode = StatusCodes.Status404NotFound;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

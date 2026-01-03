using Logex.API.Common;
using Logex.API.Dtos.IdentityDtos;
using Logex.API.Models;
using Logex.API.Services.Interfaces;

namespace Logex.API.Services.Implementations
{
    public class AuthService(ITokenManagement tokenManagement, IUserManagement userManagement)
        : IAuthService
    {
        public async Task<ServiceResponse> Register(RegisterDto user)
        {
            var newUser = new User
            {
                UserName = user.Username,
                Email = user.Email,
                PasswordHash = user.Password,
            };

            var result = await userManagement.CreateUser(newUser);

            if (!result)
            {
                return new ServiceResponse
                {
                    Message = "Email might be already in use or unknown error occured",
                };
            }

            return new ServiceResponse { Success = true, Message = "Account created" };
        }

        public async Task<LoginResponse> Login(LoginDto user)
        {
            var mappedModel = new User { Email = user.Email, PasswordHash = user.Password };

            bool loginResult = await userManagement.LoginUser(mappedModel);
            if (!loginResult)
            {
                return new LoginResponse(Message: "Email not found or invalid credentials");
            }

            var _user = await userManagement.GetUserByEmail(user.Email);
            var claims = await userManagement.GetUserClaims(_user!.Email!);

            string jwtToken = tokenManagement.GenerateToken(claims);
            string refreshToken = tokenManagement.GetRefreshToken();

            int saveTokenResult = 0;

            bool userTokenCheck = await tokenManagement.ValidateRefreshToken(refreshToken);
            if (userTokenCheck)
            {
                saveTokenResult = await tokenManagement.UpdateRefreshToken(refreshToken);
            }
            else
            {
                saveTokenResult = await tokenManagement.AddRefreshToken(_user.Id, refreshToken);
            }

            if (saveTokenResult <= 0)
            {
                return new LoginResponse(Message: "Internal error occurred while authenticating");
            }

            return new LoginResponse(Success: true, Token: jwtToken, RefreshToken: refreshToken);
        }

        public async Task<LoginResponse> ReviveToken(string refreshToken)
        {
            bool validateTokenResult = await tokenManagement.ValidateRefreshToken(refreshToken);

            if (validateTokenResult == false)
            {
                return new LoginResponse(Message: "Invalid token");
            }

            int userId = await tokenManagement.GetUserIdByRefreshToken(refreshToken);
            User? user = await userManagement.GetUserById(userId);
            var claims = await userManagement.GetUserClaims(user.Email!);
            string newJwtToken = tokenManagement.GenerateToken(claims);
            string newRefreshToken = tokenManagement.GetRefreshToken();
            await tokenManagement.UpdateRefreshToken(newRefreshToken);

            return new LoginResponse(
                Success: true,
                Token: newJwtToken,
                RefreshToken: newRefreshToken
            );
        }
    }
}

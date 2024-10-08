using Application.Services;
using Infrastructure;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class BalanceCommandHandler : IRequestHandler<BalanceCommand, BalanceResponse>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserRepository _userRepository;

        public BalanceCommandHandler(IJwtTokenService jwtTokenService, IUserRepository userRepository)
        {
            _jwtTokenService = jwtTokenService;
            _userRepository = userRepository;
        }

        public async Task<BalanceResponse> Handle(BalanceCommand request, CancellationToken cancellationToken)
        {
            // Validate the JWT token
            var isValidToken = _jwtTokenService.ValidateToken(request.Token);
            if (!isValidToken)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }

            // Retrieve user by token's username (could be from claims or through database)
            var username = GetUsernameFromToken(request.Token); // Implement this helper to extract username from token
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Return the user's balance
            return new BalanceResponse
            {
                Balance = user.Balance
            };
        }

        //private string GetUsernameFromToken(string token)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var jwtToken = tokenHandler.ReadJwtToken(token);
        //    var username = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
        //    return username;
        //}
        public ClaimsPrincipal GetClaimsFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims));
        }

        public string GetUsernameFromToken(string token)
        {
            var claimsPrincipal = GetClaimsFromToken(token);
            return claimsPrincipal?.FindFirst("name")?.Value; // Assuming username is stored in the 'name' claim
        }
    }

}

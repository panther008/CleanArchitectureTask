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
    /// <summary>
    /// Handles balance-related commands, specifically fetching the user's balance.
    /// Implements the IRequestHandler interface from MediatR.
    /// </summary>
    public class BalanceCommandHandler : IRequestHandler<BalanceCommand, BalanceResponse>
    {
        private readonly IJwtTokenService _jwtTokenService; // Service for validating JWT tokens.
        private readonly IUserRepository _userRepository; // Repository for user-related operations.

        /// <summary>
        /// Initializes a new instance of the <see cref="BalanceCommandHandler"/> class.
        /// </summary>
        /// <param name="jwtTokenService">Service used for validating JWT tokens.</param>
        /// <param name="userRepository">Repository used to interact with user data.</param>
        public BalanceCommandHandler(IJwtTokenService jwtTokenService, IUserRepository userRepository)
        {
            _jwtTokenService = jwtTokenService;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Handles the retrieval of the user's balance by validating the token and fetching user data.
        /// </summary>
        /// <param name="request">The command containing the token for validation.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the request if needed.</param>
        /// <returns>A task representing the completion of the command with the user's balance response.</returns>
        public async Task<BalanceResponse> Handle(BalanceCommand request, CancellationToken cancellationToken)
        {
            // Validate the JWT token to ensure the request is authorized.
            var isValidToken = _jwtTokenService.ValidateToken(request.Token);
            if (!isValidToken)
            {
                // Throw an exception if the token is invalid.
                throw new UnauthorizedAccessException("Invalid token");
            }

            // Retrieve the username from the token (this could be done via claims or a direct database query).
            var username = GetUsernameFromToken(request.Token); // Calls helper method to extract the username.
            var user = await _userRepository.GetUserByUsernameAsync(username);

            // Check if the user exists in the repository.
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Return the user's balance in the response.
            return new BalanceResponse
            {
                Balance = user.Balance
            };
        }

        /// <summary>
        /// Extracts claims from the provided JWT token.
        /// </summary>
        /// <param name="token">The JWT token to extract claims from.</param>
        /// <returns>A ClaimsPrincipal containing the claims from the token.</returns>
        public ClaimsPrincipal GetClaimsFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Validate the token structure and throw an exception if invalid.
            if (jwtToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            // Return the ClaimsPrincipal with the extracted claims.
            return new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims));
        }

        /// <summary>
        /// Retrieves the username from the JWT token.
        /// </summary>
        /// <param name="token">The JWT token containing the username claim.</param>
        /// <returns>The username extracted from the token.</returns>
        public string GetUsernameFromToken(string token)
        {
            var claimsPrincipal = GetClaimsFromToken(token);
            // Assuming the username is stored in the 'name' claim.
            return claimsPrincipal?.FindFirst("name")?.Value;
        }
    }
}

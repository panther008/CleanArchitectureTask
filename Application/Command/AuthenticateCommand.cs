using Application.Services;
using Infrastructure;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    /// <summary>
    /// Represents a command to handle user authentication. Implements the IRequest interface from MediatR.
    /// </summary>
    public class AuthenticateCommand : IRequest<AuthenticateResponse>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string IpAddress { get; set; }

      
        public string Device { get; set; }

       
        public string Browser { get; set; }
    }

    /// <summary>
    /// Handles the execution of the AuthenticateCommand by implementing the IRequestHandler interface from MediatR.
    /// </summary>
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponse>
    {
        private readonly IUserRepository _userRepository; // Repository for user-related operations.
        private readonly IJwtTokenService _jwtTokenService; // Service for generating JWT tokens.
        private readonly IPasswordHasher _passwordHasher; // Service for verifying hashed passwords.

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">Repository used to interact with user data.</param>
        /// <param name="jwtTokenService">Service used for generating JWT tokens.</param>
        /// <param name="passwordHasher">Service used to hash and verify passwords.</param>
        public AuthenticateCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Handles the authentication process by validating user credentials and generating a JWT token.
        /// </summary>
        /// <param name="request">The command containing user authentication details.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the request if needed.</param>
        /// <returns>A task representing the completion of the command with an authentication response.</returns>
        public async Task<AuthenticateResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            // Retrieves the user from the repository based on the provided username.
            var user = await _userRepository.GetUserByUsernameAsync(request.Username);

            // Validates the user's existence and checks the password using the password hasher.
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                // Throws an exception if the credentials are invalid.
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            // If the user's balance is zero, initializes it with a default amount of 5.
            if (user.Balance == 0)
            {
                user.Balance = 5;
                await _userRepository.UpdateUserAsync(user); // Updates the user balance in the repository.
            }

            // Generates a JWT token for the authenticated user.
            var token = _jwtTokenService.GenerateToken(user);

            // Returns the authentication response containing user details and the generated token.
            return new AuthenticateResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token
            };
        }
    }
}

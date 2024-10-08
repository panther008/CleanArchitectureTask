using Application.Services;
using Domain.Entities;
using Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    /// <summary>
    /// Represents a command to handle user sign-up. Implements the IRequest interface from MediatR.
    /// </summary>
    public class SignUpCommand : IRequest<Unit>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Device { get; set; }
        public string IpAddress { get; set; }
    }

    /// <summary>
    /// Handles the execution of the SignUpCommand by implementing the IRequestHandler interface from MediatR.
    /// </summary>
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Unit>
    {
        private readonly IUserRepository _userRepository; // Repository for user-related operations.
        private readonly IPasswordHasher _passwordHasher; // Service for hashing passwords.

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">Repository used to interact with user data.</param>
        /// <param name="passwordHasher">Service used to hash user passwords.</param>
        public SignUpCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Handles the sign-up process by creating a new user and storing it in the repository.
        /// </summary>
        /// <param name="request">The command containing user sign-up details.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the request if needed.</param>
        /// <returns>A task representing the completion of the command with Unit as a result.</returns>
        public async Task<Unit> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            // Hashes the user's password using the provided IPasswordHasher service.
            var passwordHash = _passwordHasher.HashPassword(request.Password);

            // Creates a new User entity with the sign-up information.
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Device = request.Device,
                IpAddress = request.IpAddress
            };

            // Adds the new user to the repository asynchronously.
            await _userRepository.AddUserAsync(user);

            // Returns Unit.Value, indicating successful completion of the command with no return value.
            return Unit.Value;
        }
    }
}

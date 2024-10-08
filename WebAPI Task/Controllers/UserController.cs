using Application.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebAPI_Task.Controllers
{
    /// <summary>
    /// Handles all user-related actions, such as registration, authentication, and balance retrieval.
    /// Utilizes the Mediator pattern to decouple request handling from business logic.
    /// </summary>
    [ApiController]
    [Route("api/user")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR mediator instance for handling commands.</param>
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="command">Command containing user registration details.</param>
        /// <returns>Returns a 200 OK response on successful registration.</returns>
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            // Sends the sign-up command to the mediator for handling user registration logic.
            await _mediator.Send(command);
            return Ok(); // Returns HTTP 200 OK if the operation is successful.
        }

        /// <summary>
        /// Authenticates a user based on provided credentials.
        /// </summary>
        /// <param name="command">Command containing user authentication details.</param>
        /// <returns>Returns authentication token or user details upon successful login.</returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateCommand command)
        {
            // Sends the authentication command to the mediator for login verification.
            var response = await _mediator.Send(command);
            return Ok(response); // Returns the authentication response, typically including a token.
        }

        /// <summary>
        /// Retrieves the current balance of a user.
        /// </summary>
        /// <param name="command">Command containing user identification details.</param>
        /// <returns>Returns the user's balance as part of the response.</returns>
        [HttpPost("balance")]
        public async Task<IActionResult> GetBalance([FromBody] BalanceCommand command)
        {
            // Sends the balance retrieval command to the mediator.
            var balance = await _mediator.Send(command);
            // Returns the balance in the response.
            return Ok(new { balance });
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;
using VerticalSliceArchitecture.CodeSample.API.Features.Employees.Commands;
using VerticalSliceArchitecture.CodeSample.API.Features.Employees.Queries;

namespace VerticalSliceArchitecture.CodeSample.API.Features.Employees
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IMediator mediator, ILogger<EmployeeController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [SwaggerOperation(summary: "Create Employee", description: "Create a new Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddEmployee([FromBody] CreateEmployeeCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ex switch
                {
                    CompanyNotFoundException => NotFound(ex.Message),
                    _ => Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError)
                };
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(summary: "Update Employee", description: "Update an existing Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, [FromBody] UpdateEmployeeCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                command.Id = id;
                var result = await _mediator.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ex switch
                {
                    EmployeeNotFoundException => NotFound(ex.Message),
                    CompanyNotFoundException => NotFound(ex.Message),
                    _ => Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError)
                };
            }
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(summary: "Get Employee", description: "Get a specific Employee by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetEmployeeQuery() { Id = id };
                var result = await _mediator.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ex switch
                {
                    EmployeeNotFoundException => NotFound(ex.Message),
                    _ => Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError)
                };
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(summary: "Delete Employee", description: "Delete a specific Employee by Id")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var command = new DeleteEmployeeCommand() { Id = id };
                var result = await _mediator.Send(command, cancellationToken);
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ex switch
                {
                    EmployeeNotFoundException => NotFound(ex.Message),
                    _ => Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError)
                };
            }
        }
    }
}
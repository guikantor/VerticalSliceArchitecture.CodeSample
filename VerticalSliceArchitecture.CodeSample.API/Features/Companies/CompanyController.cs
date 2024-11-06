using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;
using VerticalSliceArchitecture.CodeSample.API.Features.Companies.Commands;
using VerticalSliceArchitecture.CodeSample.API.Features.Companies.Queries;

namespace VerticalSliceArchitecture.CodeSample.API.Features.Companies
{
    [ApiController]
    [Route("api/company")]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(IMediator mediator, ILogger<CompanyController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [SwaggerOperation(summary: "Create Company", description: "Create a new Company")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCompany([FromBody] CreateCompanyCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(summary: "Update Company", description: "Update an existing Company")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCompany([FromRoute] Guid id, [FromBody] UpdateCompanyCommand command, CancellationToken cancellationToken = default)
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
                    CompanyNotFoundException => NotFound(ex.Message),
                    _ => Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError)
                };
            }
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(summary: "Get Company", description: "Get a Company by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCompany([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetCompanyQuery() { Id = id };
                var result = await _mediator.Send(query, cancellationToken);
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

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(summary: "Delete Company", description: "Delete a Company by Id")]
        public async Task<IActionResult> DeleteCompany([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var command = new DeleteCompanyCommand() { Id = id };
                var result = await _mediator.Send(command, cancellationToken);
                return Ok();

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
    }
}

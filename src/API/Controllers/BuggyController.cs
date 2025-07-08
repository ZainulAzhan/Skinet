using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("notfound")]
    public IActionResult GetNotFoundRequest()
    {
        return NotFound();
    }

    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorized()
    {
        return Unauthorized();
    }

    [HttpGet("internalerror")]
    public IActionResult GetInternalError()
    {
        throw new Exception("This is a test exception.");
    }

    [HttpPost("validationerror")]
    public IActionResult GetValidationError(CreateProductDto product)
    {
        return Ok();
    }

    [HttpGet("badrequest")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("Not a good request");
    }
}
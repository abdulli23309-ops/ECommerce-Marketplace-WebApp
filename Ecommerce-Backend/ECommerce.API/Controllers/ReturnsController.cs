using ECommerce.Application.DTOs.Returns;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReturnsController : ControllerBase
    {
        private readonly IReturnService _returnService;
        public ReturnsController(IReturnService returnService) => _returnService = returnService;

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> CreateReturnRequest(CreateReturnRequestDto dto)
            => Ok(await _returnService.CreateReturnRequestAsync(GetUserId(), dto));

        [HttpGet("my")]
        public async Task<IActionResult> GetMyReturns()
            => Ok(await _returnService.GetMyReturnRequestsAsync(GetUserId()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReturnRequest(Guid id)
        {
            var result = await _returnService.GetReturnRequestByIdAsync(GetUserId(), id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
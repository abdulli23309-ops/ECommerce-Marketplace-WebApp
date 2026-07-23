using ECommerce.Application.DTOs.Reviews;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>
        /// Create a review for a delivered order item.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview(CreateReviewDto dto)
        {
            var result = await _reviewService.CreateReviewAsync(GetUserId(), dto);
            return Ok(result);
        }

        /// <summary>
        /// Get all reviews written by the current user.
        /// </summary>
        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyReviews()
        {
            var result = await _reviewService.GetMyReviewsAsync(GetUserId());
            return Ok(result);
        }

        /// <summary>
        /// Get all reviews for a product (public).
        /// </summary>
        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductReviews(Guid productId)
        {
            var result = await _reviewService.GetProductReviewsAsync(productId);
            return Ok(result);
        }
    }
}
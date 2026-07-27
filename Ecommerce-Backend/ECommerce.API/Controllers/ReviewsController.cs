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
        private readonly IFileStorageService _fileStorageService;

        public ReviewsController(IReviewService reviewService, IFileStorageService fileStorageService)
        {
            _reviewService = reviewService;
            _fileStorageService = fileStorageService;
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
        [HttpPost("upload-image")]
        [Authorize]
        public async Task<IActionResult> UploadReviewImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file provided." });

            try
            {
                var imageUrl = await _fileStorageService.SaveFileAsync(file.OpenReadStream(), file.FileName, "reviews");
                return Ok(new { imageUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetReview(Guid id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null) return NotFound();
            return Ok(review);
        }
    }
}
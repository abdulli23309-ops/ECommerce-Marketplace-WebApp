using ECommerce.Application.DTOs.Returns;

namespace ECommerce.Application.Interfaces
{
    public interface IReturnService
    {
        Task<ReturnRequestDto> CreateReturnRequestAsync(Guid userId, CreateReturnRequestDto dto);
        Task<IEnumerable<ReturnRequestDto>> GetMyReturnRequestsAsync(Guid userId);
        Task<ReturnRequestDto?> GetReturnRequestByIdAsync(Guid userId, Guid returnRequestId);
    }
}
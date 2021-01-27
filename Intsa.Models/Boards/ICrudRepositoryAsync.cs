using Dul.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Intsa.Models.Boards
{
    /// <summary>
    /// [3] Generic Repository Interface 
    /// </summary>
    public interface ICrudRepositoryAsync<T>
    {
        Task<T> AddAsync(T model);          // 입력
        Task<List<T>> GetAllAsync();        // 출력
        Task<T> GetByIdAsync(int id);            // 상세
        Task<bool> EditAsync(T model);      // 수정
        Task<bool> DeleteAsync(int id);     // 삭제 
        Task<PagingResult<T>> GetAllAsync(int pageIndex, int pageSize);  // 페이징 
        Task<PagingResult<T>> GetAllByParentIdAsync(int pageIndex, int pageSize, int parentId);   // 부모 
    }
}

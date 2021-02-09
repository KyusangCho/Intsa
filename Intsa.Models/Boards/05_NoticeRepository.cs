using Dul.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intsa.Models.Boards
{
    /// <summary>
    /// [6] Repository Class: 
    /// </summary>
    public class NoticeRepository : INoticeRepository
    {
        private readonly NoticeAppDbContext _context;
        private readonly ILogger _logger;

        public NoticeRepository(NoticeAppDbContext context, ILoggerFactory loggerFactory)
        {
            this._context = context;
            this._logger = loggerFactory.CreateLogger(nameof(NoticeRepository));
        }

        // 입력
        public async Task<BoardNotices> AddAsync(BoardNotices model)
        {
            _context.BoardNotices.Add(model);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"에러 발생({nameof(AddAsync)}): {e.Message}");
            }
            return model;
        }
        // 출력
        public async Task<List<BoardNotices>> GetAllAsync()
        {
            return await _context.BoardNotices.OrderByDescending(m => m.Id)
                //.Include(m => m.NoticesComments)
                .ToListAsync();
        }

        // 상세
        public async Task<BoardNotices> GetByIdAsync(int id)
        {
            return await _context.BoardNotices
                    //.Include(m => m.NoticesComments)
                    .SingleOrDefaultAsync(m => m.Id == id);
        }

        // 수정
        public async Task<bool> EditAsync(BoardNotices model)
        {
            _context.BoardNotices.Attach(model);
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception e)
            {
                _logger.LogError($"에러발생({nameof(EditAsync)}): {e.Message}");
            }
            return false;
        }
        // 삭제 
        public async Task<bool> DeleteAsync(int id)
        {
            var model = await _context.BoardNotices
                                .SingleOrDefaultAsync(m => m.Id == id);
            _context.Remove(model);

            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception e)
            {
                _logger.LogError($"에러발생({nameof(DeleteAsync)}): {e.Message}");
            }
            return false;
        }
        // 페이징
        public async Task<PagingResult<BoardNotices>> GetAllAsync(int pageIndex, int pageSize)
        {
            var totalRecords = await _context.BoardNotices.CountAsync();
            var models = await _context.BoardNotices
                                .OrderByDescending(m => m.Id)
                                //.Include(m => m.NoticesComments)
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            return new PagingResult<BoardNotices>(models, totalRecords);
        }

        // 부모 
        public async Task<PagingResult<BoardNotices>> GetAllByParentIdAsync(int pageIndex, int pageSize, int parentId)
        {
            var totalRecords = await _context.BoardNotices.Where(m => m.ParentId == parentId).CountAsync();

            var models = await _context.BoardNotices
                    .Where(m => m.ParentId == parentId)
                    .OrderByDescending(m => m.Id)
                    //.Include(m => m.NoticesComments)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return new PagingResult<BoardNotices>(models, totalRecords);

        }

        // 고정상태 (전체중에 몇개) 
        public async Task<Tuple<int, int>> GetStatus(int parentId)
        {
            var totalRecords = await _context.BoardNotices.Where(m => m.ParentId == parentId).CountAsync();
            var pinnedRecords = await _context.BoardNotices.Where(m => m.ParentId == parentId && m.IsPinned == true).CountAsync();

            // 새로운 클래스 생성없이 튜플로 반환 
            return new Tuple<int, int>(pinnedRecords, totalRecords);    // (2, 10) 10개중 2개 고정 
        }

        // DeleteAll by Parent
        public async Task<bool> DeleteAllByParentId(int parentId)
        {

            try
            {
                var models = await _context.BoardNotices.Where(m => m.ParentId == parentId).ToListAsync();

                foreach (var model in models)
                {
                    _context.BoardNotices.Remove(model);
                }
                return (await _context.SaveChangesAsync() > 0);
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR({nameof(DeleteAllByParentId)}): {e.Message}");
            }
            return false;
        }

        // 검색 
        public async Task<PagingResult<BoardNotices>> SearchAllAsync(int pageIndex, int pageSize, string searchQuery)
        {
            var totalRecords = await _context.BoardNotices
                .Where(m => m.Name.Contains(searchQuery) || m.Title.Contains(searchQuery) || m.Content.Contains(searchQuery))
                .CountAsync();
            var models = await _context.BoardNotices
                .Where(m => m.Name.Contains(searchQuery) || m.Title.Contains(searchQuery) || m.Content.Contains(searchQuery))
                .OrderByDescending(m => m.Id)
                //.Include(m => m.NoticesComments)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingResult<BoardNotices>(models, totalRecords);
        }

        public async Task<PagingResult<BoardNotices>> SearchAllByParentIdAsync(int pageIndex, int pageSize, string searchQuery, int parentId)
        {
            var totalRecords = await _context.BoardNotices
                .Where(m => m.ParentId == parentId)
                .Where(m => m.Name.Contains(searchQuery) || m.Title.Contains(searchQuery) || m.Content.Contains(searchQuery))
                .CountAsync();
            var models = await _context.BoardNotices
                .Where(m => m.ParentId == parentId)
                .Where(m => m.Name.Contains(searchQuery) || m.Title.Contains(searchQuery) || m.Content.Contains(searchQuery))
                .OrderByDescending(m => m.Id)
                //.Include(m => m.NoticesComments)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingResult<BoardNotices>(models, totalRecords);
        }
    }
}

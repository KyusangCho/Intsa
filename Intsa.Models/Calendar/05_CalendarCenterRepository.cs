using Dul.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intsa.Models.Calendar
{
    /// <summary>
    /// [6] Repository Class: 
    /// </summary>
    public class CalendarCenterRepository : ICalendarCenterRepository
    {
        private readonly CalendarAppDbContext _context;
        private readonly ILogger _logger;

        public CalendarCenterRepository(CalendarAppDbContext context, ILoggerFactory loggerFactory)
        {
            this._context = context;
            this._logger = loggerFactory.CreateLogger(nameof(CalendarCenterRepository));
        }

        // 입력
        public async Task<CalendarCenter> AddAsync(CalendarCenter model)
        {
            _context.CalendarCenter.Add(model);

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
        public async Task<List<CalendarCenter>> GetAllAsync()
        {
            return await _context.CalendarCenter.OrderByDescending(m => m.Id)
                //.Include(m => m.NoticesComments)
                .ToListAsync();
        }

        // 상세
        public async Task<CalendarCenter> GetByIdAsync(int id)
        {
            return await _context.CalendarCenter
                    //.Include(m => m.NoticesComments)
                    .SingleOrDefaultAsync(m => m.Id == id);
        }

        // 수정
        public async Task<bool> EditAsync(CalendarCenter model)
        {
            _context.CalendarCenter.Attach(model);
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
            var model = await _context.CalendarCenter
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
        public async Task<PagingResult<CalendarCenter>> GetAllAsync(int pageIndex, int pageSize)
        {
            var totalRecords = await _context.CalendarCenter.CountAsync();
            var models = await _context.CalendarCenter
                                .OrderByDescending(m => m.Id)
                                //.Include(m => m.NoticesComments)
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            return new PagingResult<CalendarCenter>(models, totalRecords);
        }
                
    }
}

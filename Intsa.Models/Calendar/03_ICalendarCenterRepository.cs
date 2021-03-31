using System;
using System.Threading.Tasks;

namespace Intsa.Models.Calendar
{
    /// <summary>
    /// [3] Repository Interface 
    /// </summary>
    public interface ICalendarCenterRepository : ICrudRepository<CalendarCenter>
    {
        //Task<Tuple<int, int>> GetStatus(int parentId);
        //Task<bool> DeleteAllByParentId(int parentId); 
    }
}

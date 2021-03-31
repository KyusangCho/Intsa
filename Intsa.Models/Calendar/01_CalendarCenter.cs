using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intsa.Models.Calendar
{
    /// <summary>
    /// [2] Model Class: Notice 모델 클래스 == Notices 테이블과 일대일 매핑 
    /// </summary>
    [Table("CalendarCenter")]
    public class CalendarCenter
    {
		/// <summary>
		/// ID
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool? IsAllDay { get; set; } = true;
        public int? CenterId { get; set; }
        [Required(ErrorMessage = "제목을 입력하세요.")]
        [MaxLength(255)]
        public string Subject { get; set; }
        public string Location { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public int? RecurrenceID { get; set; }
    }
}

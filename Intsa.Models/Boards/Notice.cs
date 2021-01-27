using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intsa.Models.Boards
{
    /// <summary>
    /// [2] Model Class: Notice 모델 클래스 == Notices 테이블과 일대일 매핑 
    /// </summary>
    [Table("BoardNotices")]
    public class BoardNotices
	{
		/// <summary>
		/// Serial Number 
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
		/// <summary>
		/// 외래키 
		/// </summary>
        public int? ParentId { get; set; }

		/// <summary>
		/// 이름
		/// </summary>
		[Required(ErrorMessage = "이름을 입력하세요.")]
		[MaxLength(255)]
        public string Name { get; set; }
		/// <summary>
		/// 제목
		/// </summary>
		[MaxLength(255)]
        public string Title { get; set; }
		/// <summary>
		/// 내용
		/// </summary>
        public string Content { get; set; }
		/// <summary>
		/// 상단고정: 공지글로 올리기 
		/// </summary>
		public bool? IsPinned { get; set; } = false; 
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
		
	}
}

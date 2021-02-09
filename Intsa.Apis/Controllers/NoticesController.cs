using Intsa.Models.Boards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Intsa.Apis.Controllers
{
    [Produces("application/json")]
    [Route("api/Boards/Notices")]
    public class NoticesController : ControllerBase
    {
        private readonly INoticeRepository _repository;
        private readonly ILogger _logger; 

        public NoticesController(INoticeRepository repository, ILoggerFactory loggerFactory)
        {
            this._repository = repository;
            this._logger = loggerFactory.CreateLogger(nameof(NoticesController)); 
        }


        // 삭제
        // DELETE api/Boards/Notices/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]int id)
        {
            try
            {
                var result = await _repository.DeleteAsync(id);
                if (!result)
                {
                    BadRequest(); 
                }
                return Ok(); 
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); 
            }
        }


        // 수정
        // PUT api/Boards/Notices/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditAsync(int id, [FromBody] BoardNotices model)
        {
            model.Id = id;
            if (!ModelState.IsValid)
            {
                return BadRequest(); 
            }

            try
            {
                var result = await _repository.EditAsync(model);
                if (!result)
                {
                    return BadRequest(); 
                }
                return Ok(); 
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); 
            }
        }


        // 상세
        // GET api/Boards/Notices/1
        [HttpGet("{id:int}", Name = "GetBoardNoticeById")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                var model = await _repository.GetByIdAsync(id);
                return Ok(model); 
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); 
            }
        }

        // 입력
        // POST api/Boards/Notices
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody]BoardNotices model)
        {
            model.Created = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest(); 
            }

            try
            {
                var newModel = await _repository.AddAsync(model);

                if (newModel == null)
                {
                    return BadRequest(); 
                }

                //return Ok(newModel);    // 200 OK
                var uri = Url.Link("GetBoardNoticeById", new { id = newModel.Id });
                return Created(uri, newModel);      // 201 Created
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); 
            }
        }

        // 출력 
        // GET api/Boards/Notices
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var notices = await _repository.GetAllAsync();
                return Ok(notices); 
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); 
            }
        }
        
        // 페이징
        // GET api/Boards/Notices/Page/0/10 
        [HttpGet("Page/{pageIndex:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAll(int pageIndex, int pageSize)
        {
            try
            {
                var results = await _repository.GetAllAsync(pageIndex, pageSize);

                // 응답 헤더에 총 레코드수를 담아 출력
                Response.Headers.Add("X-TotalRecordCount", results.TotalRecords.ToString());
                Response.Headers.Add("Access-Control-Expose-Headers", "X-TotalRecordCount");

                return Ok(results.Records); 
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); 
            }
        }
    }
}

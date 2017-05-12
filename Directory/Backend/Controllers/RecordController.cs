using Auth;
using Common;
using BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Backend.Controllers
{
    [Authorize]
    [Route("api/record")]
    public class RecordController : Controller
    {
        private IAuthService _authService;
        private int _userId;
        private IRecordOperations _recordOperaions;

        public RecordController(IRecordOperations recordOperations, IAuthService authService)
        {
            _authService = authService;
            _recordOperaions = recordOperations;
            Result<int> resultId = authService.GetId(User);
            _userId = resultId.Data;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id) =>
            new JsonResult(await _recordOperaions.Get(id, _userId));

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Get([FromBody]Fetch fetch) =>
            new JsonResult(await _recordOperaions.Get(fetch, _userId));

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromBody]RecordModel record) =>
            new JsonResult(await _recordOperaions.Save(record, _userId));

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Put([FromBody]RecordModel record) =>
            new JsonResult(await _recordOperaions.Update(record, _userId));

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id) =>
            new JsonResult(await _recordOperaions.Delete(id, _userId));

        [HttpPost]
        [Route("{id:int}/photo")]
        public async Task<IActionResult> UploadImage(IFormFile file, int id) =>
            new JsonResult(await _recordOperaions.UploadImage(id, file, _userId));

        [HttpGet]
        [Route("{id:int}/photo")]
        public async Task<IActionResult> GetImage(int id)
        {
            var result = await _recordOperaions.GetImage(id, _userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return File(result.Data.File, result.Data.ContentType);
        }
    }
}

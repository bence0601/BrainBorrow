using BrainBorrowAPI.Models;
using BrainBorrowAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrainBorrowAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        public INoteService _NoteService;
        public NoteController(INoteService notesService)
        {
            _NoteService = notesService;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<List<NoteModel>>> Register(NoteModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _NoteService.CreateNoteAsync(model);
            return Ok(result);
        }


    }
}


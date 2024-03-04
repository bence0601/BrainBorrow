using BrainBorrowAPI.Models;
using BrainBorrowAPI.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("GetNoteById")]
        public async Task<ActionResult<NoteModel>> GetSinlgeNoteById(int id)
        {
            var result = await _NoteService.FindNoteById(id);
            return Ok(result);
        }

        [HttpGet("GetAllNotes")]
        public async Task<ActionResult<List<NoteModel>>> GetAllNotes()
        {
            var result = await _NoteService.GetAllNotes();
            return Ok(result);
        }

        [HttpGet("DeleteNoteById"), Authorize]
        public async Task<ActionResult<List<NoteModel>>> DeleteNoteById(int id)
        {
            var result = await _NoteService.DeleteModelById(id);
            return Ok(result);
        }


    }
}


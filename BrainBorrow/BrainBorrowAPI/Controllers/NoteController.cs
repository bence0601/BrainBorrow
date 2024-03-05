using BrainBorrowAPI.Models;
using BrainBorrowAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrainBorrowAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
        }

        [HttpPost("Register")]
        [Authorize]
        public async Task<ActionResult<List<NoteModel>>> AddNote(NoteModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _noteService.CreateNoteAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and possibly notify administrators
                return StatusCode(500, $"An error occurred while adding the note: {ex.Message}");
            }
        }

        [HttpGet("GetNoteById")]
        public async Task<ActionResult<NoteModel>> GetSingleNoteById(int id)
        {
            try
            {
                var result = await _noteService.FindNoteById(id);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and possibly notify administrators
                return StatusCode(500, $"An error occurred while retrieving the note: {ex.Message}");
            }
        }

        [HttpGet("GetAllNotes")]
        public async Task<ActionResult<List<NoteModel>>> GetAllNotes()
        {
            try
            {
                var result = await _noteService.GetAllNotes();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and possibly notify administrators
                return StatusCode(500, $"An error occurred while retrieving all notes: {ex.Message}");
            }
        }

        [HttpGet("DeleteNoteById")]
        [Authorize]
        public async Task<ActionResult<List<NoteModel>>> DeleteNoteById(int id)
        {
            try
            {
                var result = await _noteService.DeleteModelById(id);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and possibly notify administrators
                return StatusCode(500, $"An error occurred while deleting the note: {ex.Message}");
            }
        }
    }
}

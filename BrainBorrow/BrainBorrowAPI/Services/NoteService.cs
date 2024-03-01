using BrainBorrowAPI.Data;
using BrainBorrowAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BrainBorrowAPI.Services
{
    public class NotesService : INoteService
    {
        private readonly NoteContext _noteContext;
        public NotesService(NoteContext noteDataContext)
        {
            _noteContext = noteDataContext;
        }
        public async Task<List<NoteModel>> CreateNoteAsync(NoteModel model)
        {
            _noteContext.Add(model);
            await _noteContext.SaveChangesAsync();
            return await _noteContext.Notes.ToListAsync();
        }
    }
}
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

        public async Task<NoteModel> FindNoteById(int id)
        {
            var NoteToReturn = await _noteContext.Notes.FindAsync(id);
            return NoteToReturn;
        }

        public Task<List<NoteModel>> GetAllNotes()
        {
            return _noteContext.Notes.ToListAsync();
        }

        public async Task<List<NoteModel>> DeleteModelById(int id)
        {
            var NoteToDelete = await _noteContext.Notes.FirstOrDefaultAsync(p => p.Id == id);

            if (NoteToDelete != null)
            {
               _noteContext.Notes.Remove(NoteToDelete);
                await _noteContext.SaveChangesAsync();
            }

            return await _noteContext.Notes.ToListAsync();

        }

        
    }
}
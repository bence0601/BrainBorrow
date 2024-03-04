using BrainBorrowAPI.Models;

namespace BrainBorrowAPI.Services
{
    public interface INoteService
    {
        public Task<List<NoteModel>> CreateNoteAsync(NoteModel model);

        public Task<NoteModel> FindNoteById(int id);

        public Task<List<NoteModel>> GetAllNotes();
        public Task<List<NoteModel>> DeleteModelById(int id);
    }
}

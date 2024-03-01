using BrainBorrowAPI.Models;

namespace BrainBorrowAPI.Services
{
    public interface INoteService
    {
        public Task<List<NoteModel>> CreateNoteAsync(NoteModel model);
    }
}

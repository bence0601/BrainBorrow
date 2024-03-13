using BrainBorrowAPI.Data;
using BrainBorrowAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrainBorrowAPI.Services
{
    public class NotesService : INoteService
    {
        private readonly NoteContext _noteContext;

        public NotesService(NoteContext noteDataContext)
        {
            _noteContext = noteDataContext ?? throw new ArgumentNullException(nameof(noteDataContext));
        }

        public async Task<List<NoteModel>> CreateNoteAsync(NoteModel model)
        {
            try
            {
                _noteContext.Add(model);
                await _noteContext.SaveChangesAsync();
                return await _noteContext.Notes.ToListAsync();
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while creating a note.", ex);
            }
        }

        public async Task<NoteModel> FindNoteById(int id)
        {
            try
            {
                var noteToReturn = await _noteContext.Notes.FindAsync(id);
                return noteToReturn;
            }
            catch (Exception ex)
            {
                
                throw new Exception($"An error occurred while finding a note with ID {id}.", ex);
            }
        }

        public async Task<List<NoteModel>> GetAllNotes()
        {
            try
            {
                return await _noteContext.Notes.ToListAsync();
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while retrieving all notes.", ex);
            }
        }

        public async Task<List<NoteModel>> DeleteModelById(int id)
        {
            try
            {
                var noteToDelete = await _noteContext.Notes.FindAsync(id);

                if (noteToDelete != null)
                {
                    _noteContext.Notes.Remove(noteToDelete);
                    await _noteContext.SaveChangesAsync();
                }

                return await _noteContext.Notes.ToListAsync();
            }
            catch (Exception ex)
            {
                
                throw new Exception($"An error occurred while deleting a note with ID {id}.", ex);
            }
        }
    }
}

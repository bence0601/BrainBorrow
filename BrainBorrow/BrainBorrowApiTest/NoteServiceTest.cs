using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainBorrowAPI.Data;
using BrainBorrowAPI.Models;
using BrainBorrowAPI.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BrainBorrowApiTest
{
    [TestFixture]
    public class NotesServiceTest
    {
        private DbContextOptions<NoteContext> _options;

        [OneTimeSetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<NoteContext>()
                .UseInMemoryDatabase(databaseName: "Test_Note_Database")
                .Options;
        }

        [Test]
        public async Task CreateNoteAsync_ValidNote_ReturnsAllNotes()
        {
            // Arrange
            using var context = new NoteContext(_options);
            var service = new NotesService(context);
            var note = new NoteModel { Id = 1, Description = "Test Note", University = "Test University" };

            try
            {
                // Act
                var result = await service.CreateNoteAsync(note);

                // Assert
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(note.Description, result[0].Description);
                Assert.AreEqual(note.University, result[0].University);
            }
            finally
            {
                // Clean up: Remove the created note from the context
                context.Notes.Remove(note);
                await context.SaveChangesAsync();
            }
        }


        [Test]
        public async Task FindNoteById_ExistingId_ReturnsNote()
        {
            // Arrange
            using var context = new NoteContext(_options);
            var service = new NotesService(context);
            var note = new NoteModel { Id = 2, Description = "Test Note", University = "Test University" };
            await context.Notes.AddAsync(note);
            await context.SaveChangesAsync();

            // Act
            var result = await service.FindNoteById(2);
            try
            {
                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(note.Description, result.Description);
                Assert.AreEqual(note.University, result.University);
            }
            finally
            {
                context.Notes.Remove(note);
                await context.SaveChangesAsync();
            }

        }

        [Test]
        public async Task GetAllNotes_ReturnsAllNotes()
        {
            // Arrange
            using var context = new NoteContext(_options);
            var service = new NotesService(context);
            var notes = new List<NoteModel>
            {
                new NoteModel { Id = 3, Description = "Note 1", University = "University 1" },
                new NoteModel { Id = 4, Description = "Note 2", University = "University 2" }
            };
            await context.Notes.AddRangeAsync(notes);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetAllNotes();

            // Assert
            Assert.AreEqual(notes.Count, result.Count);
            Assert.IsTrue(notes.All(n => result.Any(r => r.Id == n.Id && r.Description == n.Description && r.University == n.University)));
        }

        [Test]
        public async Task DeleteModelById_ExistingId_RemovesNote()
        {
            // Arrange
            using var context = new NoteContext(_options);
            var service = new NotesService(context);
            var note = new NoteModel { Id = 5, Description = "Test Note", University = "Test University" };
            await context.Notes.AddAsync(note);
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteModelById(5);

            // Assert
            Assert.AreEqual(0, context.Notes.Count()); 
            Assert.IsEmpty(result); 
        }

    }
}

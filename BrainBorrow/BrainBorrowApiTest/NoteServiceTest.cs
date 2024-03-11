using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainBorrowApiTest
{
    internal class NoteServiceTest
    {
        private NotesService _notesService;
        private Mock<NoteContext> _noteContextMock;

        [SetUp]
        public void Setup()
        {
            _noteContextMock = new Mock<NoteContext>();
            _notesService = new NotesService(_noteContextMock.Object);
        }

        [Test]
        public async Task CreateNoteAsync_ValidNote_ReturnsListOfNotes()
        {
            // Arrange
            var noteModel = new NoteModel { Id = 1, Title = "Test Note", Content = "Test Content" };

            // Act
            _noteContextMock.Setup(x => x.Add(noteModel)).Verifiable();
            _noteContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _noteContextMock.Setup(x => x.Notes.ToListAsync()).ReturnsAsync(new List<NoteModel> { noteModel });
            var result = await _notesService.CreateNoteAsync(noteModel);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(noteModel.Id, result[0].Id);
            Assert.AreEqual(noteModel.Title, result[0].Title);
            Assert.AreEqual(noteModel.Content, result[0].Content);
        }

        [Test]
        public async Task FindNoteById_ValidId_ReturnsNoteModel()
        {
            // Arrange
            var noteModel = new NoteModel { Id = 1, Title = "Test Note", Content = "Test Content" };

            // Act
            _noteContextMock.Setup(x => x.Notes.FindAsync(1)).ReturnsAsync(noteModel);
            var result = await _notesService.FindNoteById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(noteModel.Id, result.Id);
            Assert.AreEqual(noteModel.Title, result.Title);
            Assert.AreEqual(noteModel.Content, result.Content);
        }

        [Test]
        public async Task GetAllNotes_ReturnsListOfNotes()
        {
            // Arrange
            var noteModels = new List<NoteModel>
            {
                new NoteModel { Id = 1, Title = "Note 1", Content = "Content 1" },
                new NoteModel { Id = 2, Title = "Note 2", Content = "Content 2" }
            };

            // Act
            _noteContextMock.Setup(x => x.Notes.ToListAsync()).ReturnsAsync(noteModels);
            var result = await _notesService.GetAllNotes();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(x => x.Id == 1 && x.Title == "Note 1" && x.Content == "Content 1"));
            Assert.IsTrue(result.Any(x => x.Id == 2 && x.Title == "Note 2" && x.Content == "Content 2"));
        }

        [Test]
        public async Task DeleteModelById_ValidId_RemovesNoteAndReturnsListOfNotes()
        {
            // Arrange
            var noteModels = new List<NoteModel>
            {
                new NoteModel { Id = 1, Title = "Note 1", Content = "Content 1" },
                new NoteModel { Id = 2, Title = "Note 2", Content = "Content 2" }
            };

            // Act
            _noteContextMock.Setup(x => x.Notes.FirstOrDefaultAsync(p => p.Id == 1)).ReturnsAsync(noteModels[0]);
            _noteContextMock.Setup(x => x.Notes.Remove(noteModels[0])).Verifiable();
            _noteContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _noteContextMock.Setup(x => x.Notes.ToListAsync()).ReturnsAsync(noteModels.Skip(1).ToList());
            var result = await _notesService.DeleteModelById(1);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Any(x => x.Id == 2 && x.Title == "Note 2" && x.Content == "Content 2"));
        }
    }
}

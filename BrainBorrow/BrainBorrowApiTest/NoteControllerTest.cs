using BrainBorrowAPI.Controllers;
using BrainBorrowAPI.Models;
using BrainBorrowAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainBorrowApiTest
{
    internal class NoteControllerTest
    {
        private Mock<INoteService> _mockNoteService;
        private NoteController _controller;

        [SetUp]
        public void Setup()
        {
            _mockNoteService = new Mock<INoteService>();
            _controller = new NoteController(_mockNoteService.Object);
        }

        [Test]
        public async Task AddNote_ValidNote_ReturnsOkResult()
        {
            // Arrange
            var noteModel = new NoteModel { /* Initialize with valid properties */ };
            _mockNoteService.Setup(service => service.CreateNoteAsync(noteModel)).ReturnsAsync(new List<NoteModel>());

            // Act
            var result = await _controller.AddNote(noteModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task AddNote_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("PropertyName", "Error Message");

            // Act
            var result = await _controller.AddNote(new NoteModel());

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task GetSingleNoteById_ValidId_ReturnsOkResult()
        {
            // Arrange
            int id = 1;
            _mockNoteService.Setup(service => service.FindNoteById(id)).ReturnsAsync(new NoteModel());

            // Act
            var result = await _controller.GetSingleNoteById(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetSingleNoteById_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int id = 1;
            _mockNoteService.Setup(service => service.FindNoteById(id)).ReturnsAsync((NoteModel)null);

            // Act
            var result = await _controller.GetSingleNoteById(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task GetAllNotes_ReturnsOkResult()
        {
            // Arrange
            _mockNoteService.Setup(service => service.GetAllNotes()).ReturnsAsync(new List<NoteModel>());

            // Act
            var result = await _controller.GetAllNotes();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task DeleteNoteById_ValidId_ReturnsOkResult()
        {
            // Arrange
            int id = 1;
            _mockNoteService.Setup(service => service.DeleteModelById(id)).ReturnsAsync(new List<NoteModel>());

            // Act
            var result = await _controller.DeleteNoteById(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task DeleteNoteById_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int id = 1;
            _mockNoteService.Setup(service => service.DeleteModelById(id)).ReturnsAsync((List<NoteModel>)null);

            // Act
            var result = await _controller.DeleteNoteById(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}

using BrainBorrowAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BrainBorrowAPI.Data
{
    public class NoteContext : DbContext
    {
        public NoteContext(DbContextOptions<NoteContext> options) : base(options)
        {
        }
        public DbSet<NoteModel> Notes { get; set; }

    }
}
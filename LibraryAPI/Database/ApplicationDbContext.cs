using LibraryAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Book> Book { get; set; } = default!;
    public DbSet<Author> Author { get; set; } = default!;
}
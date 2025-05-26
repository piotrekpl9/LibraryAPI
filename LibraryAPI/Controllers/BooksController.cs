using LibraryAPI.Data;
using LibraryAPI.DTO;
using LibraryAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI.Controllers;
[Route("[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BooksController(ApplicationDbContext context)
    {
        _context = context;
    }
    
       [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook([FromQuery] int? authorId)
        {
            if (authorId != null)
            {
                var books = await _context.Book.Where(book => book.AuthorId == authorId).Include(book => book.Author).ToListAsync();

           

                return Ok(books);
            }

            return Ok(await _context.Book
                .Include(e => e.Author)
                .ToListAsync());
        }
        
     
        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Book.Include(book1 => book1.Author)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutBook(int id, CreateBook inputBook)
        {
            if (inputBook.Title.IsNullOrEmpty() || inputBook.AuthorId == null ||inputBook.Year < 0)
            {
                return BadRequest();
            }

            if (!AuthorExists(inputBook.AuthorId))
            {
                return BadRequest();
            }

            var book = new Book()
            {
                Id = id,
                Title  = inputBook.Title,
                Year= inputBook.Year,
                AuthorId = inputBook.AuthorId,
            };

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ShowBook>> PostBook(CreateBook inputBook)
        {
            if (inputBook.Title.IsNullOrEmpty() || inputBook.AuthorId == null ||inputBook.Year < 0)
            {
                return BadRequest();
            }

            if (!AuthorExists(inputBook.AuthorId))
            {
                return BadRequest();
            }
            
            var book = new Book
            {
                Title = inputBook.Title,
                Year = inputBook.Year,
                AuthorId = inputBook.AuthorId,
            };

            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            var finalBook = await _context.Book.Where(book1 => book1.Id == book.Id).Include(book1 => book1.Author).FirstOrDefaultAsync();
            return CreatedAtAction("GetBook", new { id = book.Id }, finalBook);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
     
        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
        
        private bool AuthorExists(int id)
        {
            return _context.Author.Any(e => e.Id == id);
        }
}
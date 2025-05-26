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
public class AuthorsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthor()
    {
        return  Ok(await _context.Author.ToListAsync());
    }
    
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Author>> GetAuthor(int id)
    {
        var author = await _context.Author.Where(author => author.Id == id).FirstOrDefaultAsync();
        if (author == null)
        {
            return NotFound();
        }

        return author;
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutAuthor(int id, CreateAuthor inputAuthor)
    {
        if (!AuthorExists(id))
        {
            return NotFound();
        }
        if (inputAuthor.FirstName.IsNullOrEmpty() || inputAuthor.LastName.IsNullOrEmpty())
        {
            return BadRequest();
        }
       

        Author author = new Author()
        {
            Id = id,
            FirstName = inputAuthor.FirstName,
            LastName= inputAuthor.LastName,
        };

        _context.Entry(author).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuthorExists(id))
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
    public async Task<ActionResult<Author>> PostAuthor(CreateAuthor inputAuthor)
    {
        if (inputAuthor.FirstName.IsNullOrEmpty() || inputAuthor.LastName.IsNullOrEmpty())
        {
            return BadRequest();
        }
        var author = new Author()
        {
            FirstName = inputAuthor.FirstName,
            LastName = inputAuthor.LastName,
        };
        _context.Author.Add(author);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var category = await _context.Author.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Author.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    private bool AuthorExists(int id)
    {
        return _context.Author.Any(e => e.Id == id);
    }
}
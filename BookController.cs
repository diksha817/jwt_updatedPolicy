using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1;

namespace jwt_token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private static List<Book> _books = new();


        [Authorize(Policy ="Admin")]
        [Route("GetBooks")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_books);
        }
        [Authorize(Policy ="User")] 
        [Route("PostBooks")]
        [HttpPost]
        public IActionResult Post([FromBody] Book book)
        {
            try
            {
                int maxISBN = _books.Count()!=0?_books.Max(b => b.ISBN):0;
                book.ISBN = maxISBN + 1;
                _books.Add(book);
                return CreatedAtAction(nameof(Get), new { isbn = book.ISBN }, book);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "An error occured");
            }

        }

        [Authorize(Policy ="Admin")]
        [Route("PutBooks/{isbn}")]
        [HttpPut]
        public IActionResult Put(int isbn, [FromBody] Book book)
        {
            var ebook = _books.FirstOrDefault(b => b.ISBN == isbn);
            if (ebook == null)
            {
                return NotFound();
            }
            ebook.Title = book.Title;
            ebook.Author = book.Author;
            ebook.Price = book.Price;
            return Ok(ebook);
        }
        [Authorize(Policy = "Admin")]
        [Route("DeleteBook")]
        [HttpDelete]
        public IActionResult Delete(int isbn)
        {
            var ebook = _books.FirstOrDefault(b => b.ISBN == isbn);
            if (ebook == null)
            {
                return NotFound();
            }
            _books.Remove(ebook);
            return Ok("Deleted book Successfully");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMAService.Business;
using CMAService.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMAService.API.Controllers
{
    //sample controller to check the db
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IBusinessAccess _businessObj;
        public AuthorController(IBusinessAccess businessobj)
        {
            _businessObj = businessobj ??
                throw new ArgumentNullException(nameof(businessobj));
        }

        // #if (AddSql || AddMongo || AddCouch)

        [HttpGet()]

        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var authorsFromRepo = await _businessObj.GetAuthors();
            if (authorsFromRepo != null)
            {
                return Ok(authorsFromRepo);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public async Task<IActionResult> GetAuthor(Guid authorId)
        {
            var authorFromRepo = await _businessObj.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(authorFromRepo);
        }
        [HttpPut("{authorId}")]
        public async Task<IActionResult>UpdateAuthor(Guid authorId, Author author)
        {
            if (authorId != author.Id)
            {
                return BadRequest();
            }
            var authorFromRepo =await _businessObj.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

           await  _businessObj.UpdateAuthor(authorId, author);
            return NoContent();
        }
        [HttpDelete("{authorId}")]
        public async Task<ActionResult> DeleteAuthor(Guid authorId)
        {
            var authorFromRepo =await _businessObj.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            await _businessObj.DeleteAuthor(authorFromRepo);

            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(Author author)
        {

            await _businessObj.AddAuthor(author);



            return CreatedAtRoute("GetAuthor",
                new { authorId = author.Id },
                author);
        }
        //#endif
        //mongo

        [HttpGet("GetBooks")]

        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var booksFromRepo = await _businessObj.GetBooks();
            if (booksFromRepo != null)
            {
                return Ok(booksFromRepo);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("book/{bookId}", Name = "GetBook")]
        public async Task<IActionResult> GetBook(Guid bookId)
        {
            var bookFromRepo = await _businessObj.GetBook(bookId);

            if (bookFromRepo == null)
            {
                return NotFound();
            }

            return Ok(bookFromRepo);
        }
        [HttpPut("book/{bookId}")]
        public async Task<IActionResult> UpdateBook(Guid bookId, Book book)
        {
            if (bookId != book.BookId)
            {
                return BadRequest();
            }
            var bookFromRepo = await _businessObj.GetBook(bookId);

            if (bookFromRepo == null)
            {
                return NotFound();
            }

            await _businessObj.UpdateBook(bookId, book);
            return NoContent();
        }
        [HttpDelete("book/{bookId}")]
        public async Task<ActionResult> DeleteBook(Guid bookId)
        {
            var bookFromRepo = await _businessObj.GetBook(bookId);

            if (bookFromRepo == null)
            {
                return NotFound();
            }

            await _businessObj.DeleteBook(bookFromRepo);

            return NoContent();
        }
        [HttpPost("CreateBook")]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {

            await _businessObj.AddBook(book);



            return CreatedAtRoute("GetBook",
                new { BookId = book.BookId },
                book);
        }

        //mongo ends

        //couch starts


        [HttpGet("GetPublishers")]

        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            var publishersFromRepo = await _businessObj.GetPublishers();
            if (publishersFromRepo != null)
            {
                return Ok(publishersFromRepo);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("publisher/{Id}", Name = "GetPublisher")]
        public async Task<IActionResult> GetPublisher(string Id)
        {
            var publisherFromRepo = await _businessObj.GetPublisher(Id);

            if (publisherFromRepo == null)
            {
                return NotFound();
            }

            return Ok(publisherFromRepo);
        }
        [HttpPut("publisher/{Id}")]
        public async Task<IActionResult> UpdatePublisher(string Id, Publisher publisher)
        {
            if (Id != publisher.Id)
            {
                return BadRequest();
            }
            var publisherFromRepo = await _businessObj.GetPublisher(Id);

            if (publisherFromRepo == null)
            {
                return NotFound();
            }

            await _businessObj.UpdatePublisher(Id, publisher);
            return NoContent();
        }
        [HttpDelete("publisher/{Id}")]
        public async Task<ActionResult> DeletePublisher(string Id)
        {
            var publisherFromRepo = await _businessObj.GetPublisher(Id);

            if (publisherFromRepo == null)
            {
                return NotFound();
            }

            await _businessObj.DeletePublisher(publisherFromRepo);

            return NoContent();
        }
        [HttpPost("CreatePublisher")]
        public async Task<ActionResult<Publisher>> CreatePublisher(Publisher publisher)
        {

            await _businessObj.AddPublisher(publisher);



            return CreatedAtRoute("GetPublisher",
                new { Id = publisher.Id },
                publisher);
        }
        //couch ends
    }
}
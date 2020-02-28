using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using CouchDB.Driver;
using CouchDB;
namespace CMAService.Repository
{
    public class DataAccess : IDataAccess
    {
        private readonly AuthorContext _context;

        private readonly IMongoCollection<Book> _books;

        private readonly CouchClient _couchclient;
       private readonly CouchDatabase<Publisher> _publisherdatabase;
        public DataAccess(AuthorContext context , IMongoDbSettings settings, CouchClient couchclient)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));


            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.CollectionName);

            _couchclient = couchclient;
            _publisherdatabase = _couchclient.GetDatabase<Publisher>();
        }
        public async Task<IEnumerable<Author>> GetAuthors()
        {
            return await _context.Authors.ToListAsync<Author>();
        }
        public async Task<Author> GetAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return await _context.Authors.FirstOrDefaultAsync(a => a.Id == authorId);
        }

        public async Task AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            // the repository fills the id (instead of using identity columns)
            author.Id = Guid.NewGuid();

            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAuthor(Guid authorId, Author author)
        {
            var authorfromRepo = _context.Authors.Where(x => x.Id == authorId).FirstOrDefault();

            if (authorfromRepo != null)
            {
                authorfromRepo.FirstName = author.FirstName;
                authorfromRepo.LastName = author.LastName;
                //_context.Entry(author).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                throw new Exception("Author id does not exists in db");
            }
        }
        private bool AuthorExists(Guid id) =>
         _context.Authors.Any(e => e.Id == id);



        public async Task DeleteAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
       
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
        //mongodb starts
        public async Task AddBook(Book book)
        {
            await _books.InsertOneAsync(book);
        }

        public async Task DeleteBook(Book book)
        {
            await _books.DeleteOneAsync(b => b.BookId == book.BookId);
        }

        public async Task<Book> GetBook(Guid bookId)
        {
            return await _books.Find<Book>(book => book.BookId == bookId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _books.Find<Book>(Book => true).ToListAsync();
        }

        public async Task UpdateBook(Guid bookId, Book book)
        {
            await _books.ReplaceOneAsync(book => book.BookId == bookId, book);
        }
        //mongodb ends

        //couch db starts

        public async Task AddPublisher(Publisher publisher)
        {

            await _publisherdatabase.CreateAsync(publisher);
        }

        public async Task DeletePublisher(Publisher publisher)
        {

            await _publisherdatabase.DeleteAsync(publisher);
        }

        public async Task<Publisher> GetPublisher(string publisherId)
        {
            var publisher = await _publisherdatabase.FindAsync(publisherId.ToString());
            return publisher;
        }

        public async Task<IEnumerable<Publisher>> GetPublishers()
        {

            return await _publisherdatabase.ToListAsync();
        }

        public async Task UpdatePublisher(string publisherId, Publisher publisher)
        {
            await _publisherdatabase.CreateOrUpdateAsync(publisher);
        }

        //couch db ends
    }

}

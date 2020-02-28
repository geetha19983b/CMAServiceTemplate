using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CMAService.Repository
{
    public interface IDataAccess
    {
        //#if(AddSql || AddMongo || AddCouch)
        //IEnumerable<Author> GetAuthors();

        //void AddAuthor(Author author);
        //Author GetAuthor(Guid authorId);
        //void UpdateAuthor(Guid authorId, Author author);
        //void DeleteAuthor(Author author);
        // #endif

        Task<IEnumerable<Author>> GetAuthors();

        Task AddAuthor(Author author);
        Task<Author> GetAuthor(Guid authorId);
        Task UpdateAuthor(Guid authorId, Author author);
        Task DeleteAuthor(Author author);
        //mongo starts
        Task AddBook(Book book);
        Task DeleteBook(Book book);
        Task<Book> GetBook(Guid bookId);
        Task<IEnumerable<Book>> GetBooks();
        Task UpdateBook(Guid bookId, Book book);

        //mongo ends

        //couch starts
        Task AddPublisher(Publisher publisher);
        Task DeletePublisher(Publisher publisher);
        Task<Publisher> GetPublisher(string publisherId);
        Task<IEnumerable<Publisher>> GetPublishers();
        Task UpdatePublisher(string publisherId, Publisher publisher);
        //couch ends

    }
}

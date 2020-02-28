using CMAService.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMAService.Business
{
    public class BusinessAccess : IBusinessAccess
    {

        private readonly IDataAccess _dataAccessObj;
        public BusinessAccess(IDataAccess dataAccessObj)
        {
            _dataAccessObj = dataAccessObj ??
                throw new ArgumentNullException(nameof(dataAccessObj));
        }

        //#if(AddSql || AddMongo)
        public async Task AddAuthor(Author author)
        {
            await _dataAccessObj.AddAuthor(author);


        }
        
        
        public async Task DeleteAuthor(Author author)
        {
            await _dataAccessObj.DeleteAuthor(author);
        }
       
        public async Task<Author> GetAuthor(Guid authorId)
        {
            return await _dataAccessObj.GetAuthor(authorId);
        }
       
        public async Task<IEnumerable<Author>> GetAuthors()
        {
            return await _dataAccessObj.GetAuthors();
        }
       
        public async Task UpdateAuthor(Guid authorId, Author author)
        {
            await _dataAccessObj.UpdateAuthor(authorId, author);
        }
        // #endif
        //mongodb starts
        public async Task AddBook(Book book)
        {
            await _dataAccessObj.AddBook(book);
        }

        public async Task DeleteBook(Book book)
        {
            await _dataAccessObj.DeleteBook(book);
        }

        public async Task<Book> GetBook(Guid bookId)
        {
            return await _dataAccessObj.GetBook(bookId);
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _dataAccessObj.GetBooks();
        }

        public async Task UpdateBook(Guid bookId, Book book)
        {
            await _dataAccessObj.UpdateBook(bookId, book);
        }
        //mongodb ends

        //couch starts
        public async Task AddPublisher(Publisher publisher)
        {

            await _dataAccessObj.AddPublisher(publisher);
        }

        public async Task DeletePublisher(Publisher publisher)
        {

            await _dataAccessObj.DeletePublisher(publisher);
        }

        public async Task<Publisher> GetPublisher(string publisherId)
        {
           return await _dataAccessObj.GetPublisher(publisherId);
        }

        public async Task<IEnumerable<Publisher>> GetPublishers()
        {

            return await _dataAccessObj.GetPublishers();
        }

        public async Task UpdatePublisher(string publisherId, Publisher publisher)
        {
           await  _dataAccessObj.UpdatePublisher(publisherId, publisher);
        }

        //couch ends
    }

}

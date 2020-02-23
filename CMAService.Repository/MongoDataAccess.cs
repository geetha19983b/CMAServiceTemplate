using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace CMAService.Repository
{
    public class MongoDataAccess : IDataAccess
    {
        private readonly IMongoCollection<Author> _authors;

        public MongoDataAccess(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _authors = database.GetCollection<Author>(settings.CollectionName);
        }
        public void AddAuthor(Author author)
        {
            _authors.InsertOne(author);
        }

        public void DeleteAuthor(Author author)
        {
            _authors.DeleteOne(auth => auth.Id == author.Id);
        }

        public Author GetAuthor(Guid authorId)
        {
            return _authors.Find<Author>(author => author.Id == authorId).FirstOrDefault();
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _authors.Find<Author>(author => true).ToList();
        }




        public void UpdateAuthor(Guid authorId, Author author)
        {
            _authors.ReplaceOne(auth => auth.Id == authorId, author);
        }
    }
}

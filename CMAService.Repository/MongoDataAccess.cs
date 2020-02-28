//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using MongoDB.Driver;

//namespace CMAService.Repository
//{
//    public class MongoDataAccess : IDataAccess
//    {
//        private readonly IMongoCollection<Author> _authors;

//        public MongoDataAccess(IMongoDbSettings settings)
//        {
//            var client = new MongoClient(settings.ConnectionString);
//            var database = client.GetDatabase(settings.DatabaseName);

//            _authors = database.GetCollection<Author>(settings.CollectionName);
//        }
//        public async Task AddAuthor(Author author)
//        {
//            await _authors.InsertOneAsync(author);
//        }

//        public async void DeleteAuthor(Author author)
//        {
//           await _authors.DeleteOneAsync(auth => auth.Id == author.Id);
//        }

//        public async Task<Author> GetAuthor(Guid authorId)
//        {
//            return await _authors.Find<Author>(author => author.Id == authorId).FirstOrDefaultAsync();
//        }

//        public async Task<IEnumerable<Author>> GetAuthors()
//        {
//            return await _authors.Find<Author>(author => true).ToListAsync();
//        }




//        public async void UpdateAuthor(Guid authorId, Author author)
//        {
//            await _authors.ReplaceOneAsync(auth => auth.Id == authorId, author);
//        }
//    }
//}

//using CouchDB.Driver;
//using CouchDB;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace CMAService.Repository
//{
//    public class CouchDataAccess : IDataAccess
//    {
//        private readonly CouchClient _client;
//        private readonly CouchDatabase<Author> _authordatabase;
//        public CouchDataAccess(CouchClient client)
//        {
//            _client = client;
//            _authordatabase = _client.GetDatabase<Author>();
//        }

//public async Task AddAuthor(Author author)
//{

//    await _authordatabase.CreateAsync(author);
//}

//public async void DeleteAuthor(Author author)
//{

//    await _authordatabase.DeleteAsync(author);
//}

//public async Task<Author> GetAuthor(string authorId)
//{
//    var author = await _authordatabase.FindAsync(authorId.ToString());
//    return author;
//}

//public async Task<IEnumerable<Author>> GetAuthors()
//{

//    return await _authordatabase.ToListAsync();
//}

//public async void UpdateAuthor(string authorId, Author author)
//{
//    await _authordatabase.CreateOrUpdateAsync(author);
//}


//    }
//}

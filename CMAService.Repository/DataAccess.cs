using System;
using System.Collections.Generic;

namespace CMAService.Repository
{
    public class DataAccess : IDataAccess
    {
        #if(AddSql || AddMongo)
        public void AddAuthor(Author author)
        {
            throw new NotImplementedException();
        }
        #endif
        #if (AddSql || AddMongo)
        public void DeleteAuthor(Author author)
        {
            throw new NotImplementedException();
        }
        #endif
        #if (AddSql || AddMongo)
        public Author GetAuthor(Guid authorId)
        {
            throw new NotImplementedException();
        }
        #endif
        #if (AddSql || AddMongo)
        public IEnumerable<Author> GetAuthors()
        {
            throw new NotImplementedException();
        }
        #endif
        #if (AddSql || AddMongo)
        public void UpdateAuthor(Guid authorId, Author author)
        {
            throw new NotImplementedException();
        }
        #endif
    }
}

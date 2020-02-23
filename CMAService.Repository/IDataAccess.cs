using System;
using System.Collections.Generic;
using System.Text;

namespace CMAService.Repository
{
    public interface IDataAccess
    {
        #if(AddSql || AddMongo)
        IEnumerable<Author> GetAuthors();
            
        void AddAuthor(Author author);
        Author GetAuthor(Guid authorId);
        void UpdateAuthor(Guid authorId, Author author);
        void DeleteAuthor(Author author);
        #endif

    }
}

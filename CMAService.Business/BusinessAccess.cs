using CMAService.Repository;
using System;
using System.Collections.Generic;

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

        #if(AddSql || AddMongo)
        public void AddAuthor(Author author)
        {
            _dataAccessObj.AddAuthor(author);


        }
        #endif
        #if(AddSql || AddMongo)
        public void DeleteAuthor(Author author)
        {
            _dataAccessObj.DeleteAuthor(author);
        }
        #endif
        #if(AddSql || AddMongo)
        public Author GetAuthor(Guid authorId)
        {
            return _dataAccessObj.GetAuthor(authorId);
        }
        #endif
        #if(AddSql || AddMongo)
        public IEnumerable<Author> GetAuthors()
        {
            return _dataAccessObj.GetAuthors();
        }
        #endif
        #if(AddSql || AddMongo)
        public void UpdateAuthor(Guid authorId, Author author)
        {
            _dataAccessObj.UpdateAuthor(authorId, author);
        }
        #endif

    }

}

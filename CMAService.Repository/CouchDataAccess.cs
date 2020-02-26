using CouchDB.Driver;
using CouchDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMAService.Repository
{
    public class CouchDataAccess : IDataAccess
    {
        private readonly CouchClient _client;
        public CouchDataAccess(CouchClient client)
        {
            _client = client;
        }
        public IEnumerable<Author> GetAuthors()
        {
            var authors = _client.GetDatabase<Author>();
            return authors.ToList();
        }

    }
}

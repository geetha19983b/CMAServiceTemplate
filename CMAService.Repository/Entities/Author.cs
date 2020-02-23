using System;
using System.Collections.Generic;
using System.Text;
//#if (AddMongo)
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//#endif

namespace CMAService.Repository
{
    public class Author
    {
        //#if (AddMongo)
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        //#endif
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}

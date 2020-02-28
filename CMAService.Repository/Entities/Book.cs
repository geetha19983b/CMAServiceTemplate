using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CMAService.Repository
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid BookId { get; set; }
        public string BookName { get; set; }
    }
}

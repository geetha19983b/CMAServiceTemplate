using System;
using System.Collections.Generic;
using System.Text;
using CouchDB.Driver;
using CouchDB.Driver.Types;

namespace CMAService.Repository
{
    public class Publisher : CouchDocument
    {
        //public override string Id { get; set; }
        public string PublisherName { get; set; }
        public int PublisherRating { get; set; }
    }
}

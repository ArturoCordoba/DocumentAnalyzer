﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using DataHandlerMongoDB.Repository;

namespace DataHandlerMongoDB.Model
{
    [BsonCollection("documents")]
    public class File : Document
    {
        [BsonElement(elementName: "title")]
        public string Title { get; set; }

        [BsonElement(elementName: "url")]
        public string Url { get; set; }

        [BsonElement(elementName: "status")]
        public bool Status { get; set; }

        [BsonElement(elementName: "references")]
        public Reference[] References { get; set; }

        [BsonElement(elementName: "owner")]
        public int Owner { get; set; }
    }

}

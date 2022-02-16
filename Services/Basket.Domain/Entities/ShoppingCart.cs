using System;
using System.Collections.Generic;
using Basket.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Basket.Domain.Entities
{
    public class ShoppingCart:IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int? UserId { get; set; }
        public List<ShoppingCardItem> ShoppingCartItems { get; set; } = new List<ShoppingCardItem>();
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? CreatedTime { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? UpdatedTime { get; set; }
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeChat.Data.Documents
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        string Id { get; set; }
    }
}

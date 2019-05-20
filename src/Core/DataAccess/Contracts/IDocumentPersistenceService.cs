using LiteDB;

namespace DataAccess.Contracts
{
    public interface IDocumentPersistenceService
    {
        BsonValue SaveDocument(string data);

        BsonDocument GetDocument(string bsonId);
    }
}
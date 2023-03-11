using LiteDB;

namespace Infrastructure.Persistence;
public interface IDocumentPersistenceService
{
    BsonValue SaveDocument(string data);

    BsonDocument GetDocument(string bsonId);
}
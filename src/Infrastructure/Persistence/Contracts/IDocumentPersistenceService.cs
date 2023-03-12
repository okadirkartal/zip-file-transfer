using Domain.Entities;
using LiteDB;

namespace Infrastructure.Persistence;
public interface IDocumentPersistenceService
{
    BsonValue SaveDocument(string data);

    TransferModel GetDocument(string bsonId);
}
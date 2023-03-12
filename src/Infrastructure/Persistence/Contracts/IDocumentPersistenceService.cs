using Domain.Entities;
using LiteDB;

namespace Infrastructure.Persistence.Contracts;
public interface IDocumentPersistenceService
{
    BsonValue SaveDocument(DirectoryModel data);

    TransferModel GetDocument(string bsonId);
}
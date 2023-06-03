using Domain.Entities;
using Infrastructure.Persistence.Contracts;
using LiteDB;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence;

public class DocumentPersistenceService : IDocumentPersistenceService
{
   
    private readonly string _liteDbConnectionString;

    public DocumentPersistenceService(IOptions<ApplicationOptions> options)
    {
        _liteDbConnectionString = options.Value.Storage.ConnectionString;
    }

    public BsonValue SaveDocument(DirectoryModel data)
    {
        using var db = new LiteDatabase(_liteDbConnectionString);
        var col = db.GetCollection<TransferModel>(nameof(TransferModel));

        var model = new TransferModel { Id = Guid.NewGuid().ToString().Replace("-", ""), JsonData = data };

        col.EnsureIndex(x => x.Id, true);

        return col.Insert(model);
    }

    public TransferModel GetDocument(string bsonId)
    {
        using var db = new LiteDatabase(_liteDbConnectionString);
        var collection = db.GetCollection<TransferModel>();
        return collection.FindById(bsonId);
    }
}
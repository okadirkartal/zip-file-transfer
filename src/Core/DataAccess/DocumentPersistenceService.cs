using System;
using Core.Entities;
using DataAccess.Contracts;
using LiteDB;
using Microsoft.Extensions.Configuration;

namespace DataAccess
{
    public class DocumentPersistenceService : IDocumentPersistenceService
    {
        private readonly IConfiguration _configuration;

        private LiteDatabase db;

        private string _liteDbConnectionString;


        public DocumentPersistenceService(IConfiguration configuration)
        {
            this._configuration = configuration;
            _liteDbConnectionString = _configuration[Core.Common.Constants.StorageConnectionString];
        }

        public BsonValue SaveDocument(string data)
        {
            using (var db = new LiteDatabase(_liteDbConnectionString))
            {
                var col = db.GetCollection<TransferModel>(nameof(TransferModel));

                var model = new TransferModel
                {
                    Id = Guid.NewGuid().ToString().Replace("-", ""),
                    JsonData = data
                };

                col.EnsureIndex(x => x.Id, true);

                return col.Insert(model);
            }
        }

        public BsonDocument GetDocument(string bsonId)
        {
            using (var db = new LiteDatabase(_liteDbConnectionString))
            {
                return db.Engine.FindById(nameof(TransferModel), bsonId);
            }
        }
    }
}

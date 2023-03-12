using System.Collections.Generic;
using System.IO;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Contracts;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace tests.ZipFileTests;

public class DocumentPersistenceTests
{
    private IConfiguration _configuration;
    private IDocumentPersistenceService _documentPersistenceService;

    private string bsonDocumentId;

    [SetUp]
    public void SetUp()
    {
        _configuration = Substitute.For<IConfiguration>();

        var dbPath = "../../../Data/Documents.db";

        _configuration["Storage:ConnectionString"]
            .Returns("FileName=../../../Data/Documents.db;Timeout=10; Journal=false;Mode=Exclusive");


        if (!File.Exists(dbPath))
        {
            Directory.CreateDirectory("../../../Data");
            File.Create(dbPath);
        }

        _documentPersistenceService = new DocumentPersistenceService(_configuration);
    }

    [Test]
    public void SaveJsonData_WhenInserted_ReturnsTrue()
    {
        bsonDocumentId = _documentPersistenceService.SaveDocument(GetDirectoryModel());

        Assert.NotNull(bsonDocumentId);

        GetDocument_WhenIsValid_ReturnsTrue();
    }

    [Test]
    public void GetDocument_WhenIsValid_ReturnsTrue()
    {
        if (string.IsNullOrWhiteSpace(bsonDocumentId))
            SaveJsonData_WhenInserted_ReturnsTrue();

        var bsonDocument = _documentPersistenceService.GetDocument(bsonDocumentId);

        Assert.NotNull(bsonDocument);
    }

    private DirectoryModel GetDirectoryModel()
    {
        var _model = new DirectoryModel();
        _model._itemNameFlat = "root";
        _model.subItems = new List<DirectoryModel>
        {
            new()
            {
                _itemNameFlat = "first item",
                item = "data 2",
                subItems = new List<DirectoryModel>
                {
                    new() { _itemNameFlat = "third item", item = "data 3" }
                }
            }
        };
        return _model;
    }
}
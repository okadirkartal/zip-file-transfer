using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Services;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Security.Contracts;
using LiteDB;

namespace Recipient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecipientController : ControllerBase
    {
        private readonly IDocumentPersistenceService _documentPersistenceService;

        private readonly IDecrypter _decrypter;

        public RecipientController(IDocumentPersistenceService documentPersistenceService, IDecrypter decrypter)
        {
            this._documentPersistenceService = documentPersistenceService;
            this._decrypter = decrypter;
        }

        // GET api/values
        [HttpPost, Route("Post")]
        public async Task<ActionResult<ResultViewModel>> Post([FromBody] string jsonData)
        {
            ResultViewModel model = new ResultViewModel();

            if (string.IsNullOrEmpty(jsonData))
            {
                model.Errors.Add(new ErrorModel
                { StatusCode = (int)HttpStatusCode.NoContent, ErrorMessage = nameof(HttpStatusCode.NoContent) });
                return model;
            }

            try
            {
                var decryptedData = await DirectoryService.GetDecryptedDirectoryNode(jsonData, _decrypter);

                var bsonDocument = _documentPersistenceService.SaveDocument(decryptedData);

                var savedData = _documentPersistenceService.GetDocument(bsonDocument);

                model.Data = System.Text.Json.JsonSerializer.Serialize(savedData);
            }
            catch (Exception ex)
            {
                model.Errors.Add(new ErrorModel { ErrorMessage = ex.Message });
            }

            return model;
        }
    }
}
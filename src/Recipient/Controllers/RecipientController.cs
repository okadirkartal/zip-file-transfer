using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence.Contracts;
using Infrastructure.Services; 
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Security.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace Recipient.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
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
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DirectoryModel jsonData)
        {
            ResultViewModel model = new ResultViewModel();

            if (jsonData == null)
            {
                model.Errors.Add(new ErrorModel
                { StatusCode = (int)HttpStatusCode.NoContent, ErrorMessage = nameof(HttpStatusCode.NoContent) });
                return NoContent();
            }

            try
            {
                await DirectoryService.GetDecryptedDirectoryNode(jsonData, _decrypter);

                var bsonDocument = _documentPersistenceService.SaveDocument(jsonData);

                var savedData = _documentPersistenceService.GetDocument(bsonDocument);

                model.Data = System.Text.Json.JsonSerializer.Serialize(savedData);
                
                return Ok(model);
            }
            catch (Exception ex)
            {
                model.Errors.Add(new ErrorModel { ErrorMessage = ex.Message });
                return BadRequest();
            }

        }
    }
}
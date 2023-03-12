using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence.Contracts;
using Infrastructure.Security.Contracts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Recipient.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class RecipientController : ControllerBase
{
    private readonly IDecrypter _decrypter;
    private readonly IDocumentPersistenceService _documentPersistenceService;

    public RecipientController(IDocumentPersistenceService documentPersistenceService, IDecrypter decrypter)
    {
        _documentPersistenceService = documentPersistenceService;
        _decrypter = decrypter;
    }

    // GET api/values
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DirectoryModel jsonData)
    {
        var model = new ResultViewModel();

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

            model.Data = JsonSerializer.Serialize(savedData);

            return Ok(model);
        }
        catch (Exception ex)
        {
            model.Errors.Add(new ErrorModel { ErrorMessage = ex.Message });
            return BadRequest();
        }
    }
}
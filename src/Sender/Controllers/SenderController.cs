using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Security.Contracts;
using Infrastructure.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Sender.Models;
using Sender.Services.Contracts;

namespace Sender.Controllers;

public class SenderController : Controller
{
    private readonly IEncrypter _encrypter;
    private readonly IFileManagementService _fileManagementService;

    private readonly ITransferService _transferService;

    private readonly IZipManagementService _zipManagementService;

    public SenderController(IFileManagementService fileManagementService,
        IZipManagementService zipManagementService,
        IEncrypter encrypter, ITransferService transferService)
    {
        _transferService = transferService ?? throw new ArgumentNullException(nameof(transferService));
        _encrypter = encrypter ?? throw new ArgumentNullException(nameof(encrypter));
        _fileManagementService =
            fileManagementService ?? throw new ArgumentNullException(nameof(fileManagementService));
        _zipManagementService =
            zipManagementService ?? throw new ArgumentNullException(nameof(zipManagementService));
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index([Required] UploadViewModel model)
    {
        if (model == null) return BadRequest();

        if (ModelState.IsValid)
        {
            var savedResult = await _fileManagementService.SaveZipFileToLocalFolder(model.File);

            if (savedResult.result)
            {
                var result = await _zipManagementService.GetSerializedDirectoryStructure(savedResult.savedFilePath);

                // var encryptedData = await _encrypter.EncryptAsync(result);

                var response = await _transferService.PostToRecipientAsync(
                    new TransferModel
                    {
                        UserName = await _encrypter.EncryptAsync(model.UserName),
                        Password = await _encrypter.EncryptAsync(model.Password),
                        JsonData = result
                    });

                if (response == null)
                    return View("Result", new ResultViewModel
                    {
                        Errors = new List<ErrorModel>
                        {
                            new()
                            {
                                ErrorMessage = "Authentication Error : Invalid username or password"
                            }
                        }
                    });
                return View("Result", response);
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Result(ResultViewModel model)
    {
        return View(model);
    }
}